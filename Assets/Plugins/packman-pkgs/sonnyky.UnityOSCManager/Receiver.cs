using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receiver : MonoBehaviour
{

    public OSC osc;

    public System.Action<string> OnReceiveInputData;

    // Start is called before the first frame update
    void Start()
    {
        osc.SetAddressHandler("/rangefinder", OnReceiveTouchData);
    }

    void OnReceiveTouchData(OSCMessage message)
    {
        OnReceiveInputData.Invoke(message.ToString());
    }
}
