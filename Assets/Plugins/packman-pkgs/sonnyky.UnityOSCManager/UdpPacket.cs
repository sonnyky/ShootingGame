using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class UdpPacket
{
    private UdpClient Sender;
    private UdpClient Receiver;
    private bool socketsOpen;
    private string remoteHostName;
    private int remotePort;
    private int localPort;

    public string RemoteHostName
    {
        get
        {
            return remoteHostName;
        }
        set
        {
            remoteHostName = value;
        }
    }

    public int RemotePort
    {
        get
        {
            return remotePort;
        }
        set
        {
            remotePort = value;
        }
    }

    public int LocalPort
    {
        get
        {
            return localPort;
        }
        set
        {
            localPort = value;
        }
    }

    public UdpPacket(string hostIP, int remotePort, int localPort)
    {
        RemoteHostName = hostIP;
        RemotePort = remotePort;
        LocalPort = localPort;
        socketsOpen = false;
    }


    ~UdpPacket()
    {
        // latest time for this socket to be closed
        if (IsOpen())
        {
            Debug.Log("closing udpclient listener on port " + localPort);
            Close();
        }

    }

    public bool IsOpen()
    {
        return socketsOpen;
    }

    public void Close()
    {
        if (Sender != null)
            Sender.Close();

        if (Receiver != null)
        {
            Receiver.Close();
            // Debug.Log("UDP receiver closed");
        }
        Receiver = null;
        socketsOpen = false;

    }

    public void OnDisable()
    {
        Close();
    }

    public bool Open()
    {
        try
        {
            Sender = new UdpClient();
            Debug.Log("Opening OSC listener on port " + localPort);

            IPEndPoint listenerIp = new IPEndPoint(IPAddress.Any, localPort);
            Receiver = new UdpClient(listenerIp);


            socketsOpen = true;

            return true;
        }
        catch (Exception e)
        {
            Debug.LogWarning("cannot open udp client interface at port " + localPort);
            Debug.LogWarning(e);
        }

        return false;
    }

    public void SendPacket(byte[] packet, int length)
    {
        if (!IsOpen())
            Open();
        if (!IsOpen())
            return;

        Sender.Send(packet, length, remoteHostName, remotePort);
        //Debug.Log("osc message sent to "+remoteHostName+" port "+remotePort+" len="+length);
    }

    public int ReceivePacket(byte[] buffer)
    {
        if (!IsOpen())
            Open();
        if (!IsOpen())
            return 0;


        IPEndPoint iep = new IPEndPoint(IPAddress.Any, localPort);
        byte[] incoming = Receiver.Receive(ref iep);
        int count = Math.Min(buffer.Length, incoming.Length);
        System.Array.Copy(incoming, buffer, count);
        return count;
    }
}
