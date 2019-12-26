using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorCalibration : MonoBehaviour
{

    Calibration m_Calibration;
    Tinker.Input m_SensorInput;

    bool m_IsCalibrating = false;

    bool m_IsCalibratingPoint = false;

    [SerializeField]
    GameObject m_Cursor;

    // Start is called before the first frame update
    void Start()
    {
        m_Calibration = FindObjectOfType<Calibration>();
        m_SensorInput = FindObjectOfType<Tinker.Input>();

        m_Calibration.OnCalibrationFinished += CalibrationFinished;
        m_Calibration.gameObject.SetActive(false);

        m_Cursor = Instantiate(m_Cursor);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && !m_IsCalibrating)
        {
            m_Calibration.gameObject.SetActive(true);
            m_IsCalibrating = true;
        }

        if(Tinker.Input.touchCount > 0)
        {
            Tinker.Touch touchInstance = Tinker.Input.GetTouch(0);
            if(touchInstance.touchPhase == TouchPhase.Began && m_IsCalibrating && !m_IsCalibratingPoint)
            {
                m_IsCalibratingPoint = true;
                
            }else if(touchInstance.touchPhase == TouchPhase.Ended && m_IsCalibrating && m_IsCalibratingPoint)
            {
                m_Calibration.SetSensorInputPoint(touchInstance.rawPosition);
                m_IsCalibratingPoint = false;
            }
            else
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(touchInstance.position);
                pos.z = 0f;
                m_Cursor.transform.position = pos;
              
            }
        }
    }

    void CalibrationFinished()
    {
        m_IsCalibrating = false;
        m_SensorInput.LoadHomographyMatrix();
        m_Calibration.gameObject.SetActive(false);
    }
}
