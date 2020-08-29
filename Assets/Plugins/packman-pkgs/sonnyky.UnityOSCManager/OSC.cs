using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;

public delegate void OscMessageHandler(OSCMessage oscM);

public class OSC : MonoBehaviour
{
    [SerializeField]
    private int m_InPort = 5001;

    [SerializeField]
    private int m_OutPort = 3001;

    [SerializeField]
    private string m_OutIp = "127.0.0.1";

    Hashtable m_AddressTable;

    private UdpPacket m_OscPacket;

    Thread m_ReadThread;

    private bool ReaderRunning;
    private OscMessageHandler AllMessageHandler;

    ArrayList messagesReceived;

    private object ReadThreadLock = new object();

    byte[] buffer;

    bool paused = false;

#if UNITY_EDITOR

    private void HandleOnPlayModeChanged(UnityEditor.PlayModeStateChange state) //FIX FOR UNITY POST 2017
    {
        // This method is run whenever the playmode state is changed.


        paused = UnityEditor.EditorApplication.isPaused;
        //print ("editor paused "+paused);
        // do stuff when the editor is paused.

    }
#endif

    void Awake()
    {
        //print("Opening OSC listener on port " + inPort);

        m_OscPacket = new UdpPacket(m_OutIp, m_OutPort, m_InPort);
        m_AddressTable = new Hashtable();

        messagesReceived = new ArrayList();

        buffer = new byte[1000];


        m_ReadThread = new Thread(Read);
        ReaderRunning = true;
        m_ReadThread.IsBackground = true;
        m_ReadThread.Start();

#if UNITY_EDITOR
        //UnityEditor.EditorApplication.playmodeStateChanged = HandleOnPlayModeChanged;
        UnityEditor.EditorApplication.playModeStateChanged += HandleOnPlayModeChanged;  //FIX FOR UNITY POST 2017
#endif

    }

    void Update()
    {


        if (messagesReceived.Count > 0)
        {
            //Debug.Log("received " + messagesReceived.Count + " messages");
            lock (ReadThreadLock)
            {
                foreach (OSCMessage om in messagesReceived)
                {

                    if (AllMessageHandler != null)
                        AllMessageHandler(om);

                    ArrayList al = (ArrayList)Hashtable.Synchronized(m_AddressTable)[om.address];
                    if (al != null)
                    {
                        foreach (OscMessageHandler h in al)
                        {
                            h(om);
                        }
                    }

                }
                messagesReceived.Clear();
            }
        }
    }

    public void Close()
    {
        if (ReaderRunning)
        {
            ReaderRunning = false;
            m_ReadThread.Abort();

        }

        if (m_OscPacket != null && m_OscPacket.IsOpen())
        {
            m_OscPacket.Close();
            m_OscPacket = null;
            print("Closed OSC listener");
        }

    }

    private void Read()
    {
        try
        {
            while (ReaderRunning)
            {


                int length = m_OscPacket.ReceivePacket(buffer);

                if (length > 0)
                {
                    lock (ReadThreadLock)
                    {

                        if (paused == false)
                        {
                            ArrayList newMessages = OSC.PacketToOscMessages(buffer, length);
                            messagesReceived.AddRange(newMessages);
                        }

                    }
                }
                else
                    Thread.Sleep(5);
            }
        }

        catch (Exception e)
        {
            Debug.Log("ThreadAbortException" + e);
        }
        finally
        {

        }

    }


    public void Send(OSCMessage oscMessage)
    {
        byte[] packet = new byte[1000];
        int length = OSC.OscMessageToPacket(oscMessage, packet, 1000);
        m_OscPacket.SendPacket(packet, length);
    }

    public void Send(ArrayList oms)
    {
        byte[] packet = new byte[1000];
        int length = OSC.OscMessagesToPacket(oms, packet, 1000);
        m_OscPacket.SendPacket(packet, length);
    }

    public void SetAllMessageHandler(OscMessageHandler amh)
    {
        AllMessageHandler = amh;
    }

    public static OSCMessage StringToOscMessage(string message)
    {
        OSCMessage oM = new OSCMessage
();
        Console.WriteLine("Splitting " + message);
        string[] ss = message.Split(new char[] { ' ' });
        IEnumerator sE = ss.GetEnumerator();
        if (sE.MoveNext())
            oM.address = (string)sE.Current;
        while (sE.MoveNext())
        {
            string s = (string)sE.Current;
            // Console.WriteLine("  <" + s + ">");
            if (s.StartsWith("\""))
            {
                StringBuilder quoted = new StringBuilder();
                bool looped = false;
                if (s.Length > 1)
                    quoted.Append(s.Substring(1));
                else
                    looped = true;
                while (sE.MoveNext())
                {
                    string a = (string)sE.Current;
                    // Console.WriteLine("    q:<" + a + ">");
                    if (looped)
                        quoted.Append(" ");
                    if (a.EndsWith("\""))
                    {
                        quoted.Append(a.Substring(0, a.Length - 1));
                        break;
                    }
                    else
                    {
                        if (a.Length == 0)
                            quoted.Append(" ");
                        else
                            quoted.Append(a);
                    }
                    looped = true;
                }
                oM.values.Add(quoted.ToString());
            }
            else
            {
                if (s.Length > 0)
                {
                    try
                    {
                        int i = int.Parse(s);
                        // Console.WriteLine("  i:" + i);
                        oM.values.Add(i);
                    }
                    catch
                    {
                        try
                        {
                            float f = float.Parse(s);
                            // Console.WriteLine("  f:" + f);
                            oM.values.Add(f);
                        }
                        catch
                        {
                            // Console.WriteLine("  s:" + s);
                            oM.values.Add(s);
                        }
                    }

                }
            }
        }
        return oM;
    }

    public void SetAddressHandler(string key, OscMessageHandler ah)

    {
        ArrayList al = (ArrayList)Hashtable.Synchronized(m_AddressTable)[key];
        if (al == null)
        {
            al = new ArrayList();
            al.Add(ah);
            Hashtable.Synchronized(m_AddressTable).Add(key, al);
        }
        else
        {
            al.Add(ah);
        }
        /*
		OscMessageHandler h = (OscMessageHandler)Hashtable.Synchronized(AddressTable)[key];
		if (h == null)  Hashtable.Synchronized(AddressTable).Add(key, ah);
		else print ("there");
		*/
    }

    public static ArrayList PacketToOscMessages(byte[] packet, int length)
    {
        ArrayList messages = new ArrayList();
        ExtractMessages(messages, packet, 0, length);
        return messages;
    }

    public static int OscMessageToPacket(OSCMessage oscM, byte[] packet, int length)
    {
        return OscMessageToPacket(oscM, packet, 0, length);
    }

    public static int OscMessagesToPacket(ArrayList messages, byte[] packet, int length)
    {
        int index = 0;
        if (messages.Count == 1)
            index = OscMessageToPacket((OSCMessage)messages[0], packet, 0, length);
        else
        {
            // Write the first bundle bit
            index = InsertString("#bundle", packet, index, length);
            // Write a null timestamp (another 8bytes)
            int c = 8;
            while ((c--) > 0)
                packet[index++]++;
            // Now, put each message preceded by it's length
            foreach (OSCMessage oscM in messages)
            {
                int lengthIndex = index;
                index += 4;
                int packetStart = index;
                index = OscMessageToPacket(oscM, packet, index, length);
                int packetSize = index - packetStart;
                packet[lengthIndex++] = (byte)((packetSize >> 24) & 0xFF);
                packet[lengthIndex++] = (byte)((packetSize >> 16) & 0xFF);
                packet[lengthIndex++] = (byte)((packetSize >> 8) & 0xFF);
                packet[lengthIndex++] = (byte)((packetSize) & 0xFF);
            }
        }
        return index;
    }

    private static int OscMessageToPacket(OSCMessage oscM, byte[] packet, int start, int length)
    {
        int index = start;
        index = InsertString(oscM.address, packet, index, length);
        //if (oscM.values.Count > 0)
        {
            StringBuilder tag = new StringBuilder();
            tag.Append(",");
            int tagIndex = index;
            index += PadSize(2 + oscM.values.Count);

            foreach (object o in oscM.values)
            {
                if (o is int)
                {
                    int i = (int)o;
                    tag.Append("i");
                    packet[index++] = (byte)((i >> 24) & 0xFF);
                    packet[index++] = (byte)((i >> 16) & 0xFF);
                    packet[index++] = (byte)((i >> 8) & 0xFF);
                    packet[index++] = (byte)((i) & 0xFF);
                }
                else
                {
                    if (o is float)
                    {
                        float f = (float)o;
                        tag.Append("f");
                        byte[] buffer = new byte[4];
                        MemoryStream ms = new MemoryStream(buffer);
                        BinaryWriter bw = new BinaryWriter(ms);
                        bw.Write(f);
                        packet[index++] = buffer[3];
                        packet[index++] = buffer[2];
                        packet[index++] = buffer[1];
                        packet[index++] = buffer[0];
                    }
                    else
                    {
                        if (o is string)
                        {
                            tag.Append("s");
                            index = InsertString(o.ToString(), packet, index, length);
                        }
                        else
                        {
                            tag.Append("?");
                        }
                    }
                }
            }
            InsertString(tag.ToString(), packet, tagIndex, length);
        }
        return index;
    }

    private static int ExtractMessages(ArrayList messages, byte[] packet, int start, int length)
    {
        int index = start;
        switch ((char)packet[start])
        {
            case '/':
                index = ExtractMessage(messages, packet, index, length);
                break;
            case '#':
                string bundleString = ExtractString(packet, start, length);
                if (bundleString == "#bundle")
                {
                    // skip the "bundle" and the timestamp
                    index += 16;
                    while (index < length)
                    {
                        int messageSize = (packet[index++] << 24) + (packet[index++] << 16) + (packet[index++] << 8) + packet[index++];
                        /*int newIndex = */
                        ExtractMessages(messages, packet, index, length);
                        index += messageSize;
                    }
                }
                break;
        }
        return index;
    }

    private static int ExtractMessage(ArrayList messages, byte[] packet, int start, int length)
    {
        OSCMessage oscM = new OSCMessage();
        oscM.address = ExtractString(packet, start, length);
        int index = start + PadSize(oscM.address.Length + 1);
        string typeTag = ExtractString(packet, index, length);
        index += PadSize(typeTag.Length + 1);
        //oscM.values.Add(typeTag);
        foreach (char c in typeTag)
        {
            switch (c)
            {
                case ',':
                    break;
                case 's':
                    {
                        string s = ExtractString(packet, index, length);
                        index += PadSize(s.Length + 1);
                        oscM.values.Add(s);
                        break;
                    }
                case 'i':
                    {
                        int i = (packet[index++] << 24) + (packet[index++] << 16) + (packet[index++] << 8) + packet[index++];
                        oscM.values.Add(i);
                        break;
                    }
                case 'f':
                    {
                        byte[] buffer = new byte[4];
                        buffer[3] = packet[index++];
                        buffer[2] = packet[index++];
                        buffer[1] = packet[index++];
                        buffer[0] = packet[index++];
                        MemoryStream ms = new MemoryStream(buffer);
                        BinaryReader br = new BinaryReader(ms);
                        float f = br.ReadSingle();
                        oscM.values.Add(f);
                        break;
                    }
            }
        }
        messages.Add(oscM);
        return index;
    }

    private static string ExtractString(byte[] packet, int start, int length)
    {
        StringBuilder sb = new StringBuilder();
        int index = start;
        while (packet[index] != 0 && index < length)
            sb.Append((char)packet[index++]);
        return sb.ToString();
    }

    private static string Dump(byte[] packet, int start, int length)
    {
        StringBuilder sb = new StringBuilder();
        int index = start;
        while (index < length)
            sb.Append(packet[index++] + "|");
        return sb.ToString();
    }

    private static int InsertString(string s, byte[] packet, int start, int length)
    {
        int index = start;
        foreach (char c in s)
        {
            packet[index++] = (byte)c;
            if (index == length)
                return index;
        }
        packet[index++] = 0;
        int pad = (s.Length + 1) % 4;
        if (pad != 0)
        {
            pad = 4 - pad;
            while (pad-- > 0)
                packet[index++] = 0;
        }
        return index;
    }

    private static int PadSize(int rawSize)
    {
        int pad = rawSize % 4;
        if (pad == 0)
            return rawSize;
        else
            return rawSize + (4 - pad);
    }
}
