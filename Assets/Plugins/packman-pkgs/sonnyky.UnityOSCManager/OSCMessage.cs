using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class OSCMessage
{
    public string address;

    public ArrayList values;

    public OSCMessage()
    {
        values = new ArrayList();
    }

    public override string ToString()
    {
        StringBuilder s = new StringBuilder();
        s.Append(address);
        foreach (object o in values)
        {
            s.Append(" ");
            s.Append(o.ToString());
        }
        return s.ToString();

    }


    public int GetInt(int index)
    {

        if (values[index].GetType() == typeof(int))
        {
            int data = (int)values[index];
            if (Double.IsNaN(data)) return 0;
            return data;
        }
        else if (values[index].GetType() == typeof(float))
        {
            int data = (int)((float)values[index]);
            if (Double.IsNaN(data)) return 0;
            return data;
        }
        else
        {
            Debug.Log("Wrong type");
            return 0;
        }
    }

    public float GetFloat(int index)
    {

        if (values[index].GetType() == typeof(int))
        {
            float data = (int)values[index];
            if (Double.IsNaN(data)) return 0f;
            return data;
        }
        else if (values[index].GetType() == typeof(float))
        {
            float data = (float)values[index];
            if (Double.IsNaN(data)) return 0f;
            return data;
        }
        else
        {
            Debug.Log("Wrong type");
            return 0f;
        }
    }
}
