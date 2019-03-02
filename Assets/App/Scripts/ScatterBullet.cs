using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterBullet : Shot {

    // 0 : straight, 1: left diagonal, 2: right diagonal
    public int m_DirectionId { get; set; }

    private Vector3 m_LeftDirection;
    private Vector3 m_RightDirection;

    private void Start()
    {
       m_LeftDirection = new Vector3(1f,Mathf.Cos(0.35f), Mathf.Sin(0.35f));
    }

    // Update is called once per frame
    void Update () {

        switch(m_DirectionId){
            case 0:
                transform.position += transform.up * Time.deltaTime * speed;
                break;
            case 1:
                transform.position += m_LeftDirection * Time.deltaTime * speed;
                break;
            default:
                break;
        }

        if (!IsInViewport())
        {
            gameObject.SetActive(false);
        }
    }
}
