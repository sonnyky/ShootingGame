using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour {

    public PowerUpScriptableObject m_PowerUpValues;

    public GameObject m_Image;

    private void OnEnable()
    {
        m_Image.SetActive(true);
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
