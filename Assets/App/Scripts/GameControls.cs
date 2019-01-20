using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControls : MonoBehaviour {

    private MenuManager m_MenuManager;
    private Player m_PlayerShip;

    float timeToReachTarget, t;
    Vector3 target;

    // Use this for initialization
    void Start () {
      m_MenuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
        m_PlayerShip = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
       if(IsDoubleTap())
        {
            m_MenuManager.TogglePause();
        }

        if (IsPressed())
        {
            t += Time.deltaTime / timeToReachTarget;
            Debug.Log("position : " + Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position));
            m_PlayerShip.transform.position = Vector2.Lerp(m_PlayerShip.transform.position, target, t);
        }

    }

    public bool IsPressed()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            t = 0;
            timeToReachTarget = 2f;
            target = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        }
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            target = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            return true;
        }
        else return false;
    }

    public static bool IsDoubleTap()
    {
        bool result = false;
        float MaxTimeWait = 1;
        float VariancePosition = 1;

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Debug.Log("Tap detected");
            float DeltaTime = Input.GetTouch(0).deltaTime;
            float DeltaPositionLenght = Input.GetTouch(0).deltaPosition.magnitude;

            if (DeltaTime > 0 && DeltaTime < MaxTimeWait && DeltaPositionLenght < VariancePosition)
                result = true;
        }
        return result;
    }
}
