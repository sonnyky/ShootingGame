using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControls : MonoBehaviour {

    MenuManager m_MenuManager;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        foreach (Touch touch in Input.touches)
        {
            if (touch.tapCount == 2)
            {
                //Double tap
                m_MenuManager.TogglePause();
            }
                
        }
    }
}
