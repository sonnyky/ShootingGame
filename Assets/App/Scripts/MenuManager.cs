using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void PauseGame()
    {
        Time.timeScale = 0;
    }

    void RestartGameAfterPause()
    {
        Time.timeScale = 1;
    }

    public void TogglePause()
    {
        if(Time.timeScale <= 0.1f)
        {
            RestartGameAfterPause();
        }
        else
        {
            PauseGame();
        }
    }
}
