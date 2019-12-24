using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorCalibration : MonoBehaviour
{

    Calibration m_Calibration;
    Tinker.Input m_SensorInput;

    bool m_IsCalibrating = false;

    // Start is called before the first frame update
    void Start()
    {
        m_Calibration = FindObjectOfType<Calibration>();
        m_SensorInput = FindObjectOfType<Tinker.Input>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Tinker.Input.touchCount > 0)
        {
            Tinker.Touch touchInstance = Tinker.Input.GetTouch(0);
            if(touchInstance.touchPhase == TouchPhase.Began)
            {
                m_IsCalibrating = true;
            }else if(touchInstance.touchPhase == TouchPhase.Ended && m_IsCalibrating)
            {

            }
        }
    }
}
