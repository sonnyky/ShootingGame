using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

namespace Tinker
{
    public class Input : MonoBehaviour
    {

        public const int NO_SENSOR_TOUCH = 5;

        public const int MAX_TOUCH_INPUT = 4;

        [SerializeField]
        private Receiver m_Receiver;

        [SerializeField]
        string m_PathToHomographyFile;

        public static List<Touch> touches { get; set; }
        public static int touchCount { get; set; }

        public List<Vector2> previousTouchPositions;

        Matrix4x4 m_HomographyMatrix;

        public static Touch GetTouch(int index)
        {
            return touches[index];
        }

        private void Start()
        {
            m_Receiver.OnReceiveInputData += ProcessInput;

            touchCount = 0;

            m_PathToHomographyFile = Application.persistentDataPath + m_PathToHomographyFile;

            m_HomographyMatrix = Matrix4x4.identity;
            LoadHomographyMatrix(m_PathToHomographyFile);

            // Currently only support 4 touch inputs
            touches = new List<Touch>();
            previousTouchPositions = new List<Vector2>();
        }

        void ClearTouches()
        {
            touches.Clear();
        }

        public void LoadHomographyMatrix(string path)
        {
            string prefix = Application.persistentDataPath;
            string fullPath = prefix + path;

            if (File.Exists(fullPath))
            {
                string text = "init";

                StreamReader reader = new StreamReader(path);
                List<float> homographyFromFile = new List<float>();
                while (text != null)
                {
                    try
                    {
                        text = reader.ReadLine();
                        if (text != null)
                        {
                            float val = float.Parse(text, CultureInfo.InvariantCulture);
                            homographyFromFile.Add(val);
                            Debug.Log("val : " + val);
                        }
                    }
                    catch (FormatException exc)
                    {
                        Debug.Log("FormatException : " + exc);
                    }
                }
                reader.Close();
                CreateHomographyMatrix(homographyFromFile);
            }
        }

        void CreateHomographyMatrix(List<float> values)
        {
            for(int i=0 ; i<4; i++)
            {
                for (int j=0; j<4; j++)
                {
                    float valueToInsert = 0f;
                    if( (i==3 || j==3) && i != j)
                    {
                        valueToInsert = 0f;
                    }else if (i==j && i==3)
                    {
                        valueToInsert = 1;
                    }
                    else
                    {
                        valueToInsert = values[(i * 3) + j];
                    }
                    m_HomographyMatrix[i,j] = valueToInsert;
                }
            }
        }

        void ProcessInput(string input)
        {
            string[] splitInput = input.Split(' ');

            touchCount = 0;
            int id = 0;

            touches.Clear();

            // message comes in a predefined format. make sure the format matches
            for(int i=1; i <= 13 ; i = i + 4)
            {

                // parse to get touch phase
                int phase = int.Parse(splitInput[i]);
                if(phase != NO_SENSOR_TOUCH)
                {
                    Touch newTouch = new Touch();

                    // Get touch info
                    newTouch.touchPhase = (TouchPhase) phase;
                    Vector2 rawPos = new Vector2(float.Parse(splitInput[i+1]), float.Parse(splitInput[i+2]));
                    newTouch.rawPosition = rawPos;
                    newTouch.fingerId = id;
                    newTouch.deltaPositionMagnitude = float.Parse(splitInput[i + 3]);

                    Vector3 rawPos3 = rawPos;
                    rawPos3.z = 1;
                    newTouch.position = m_HomographyMatrix.MultiplyPoint3x4(rawPos3);
                    touchCount++;

                    touches.Add(newTouch);
                }
                id++;
            }
        }
    }
}