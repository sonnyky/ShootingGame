using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour {

    public PowerUpScriptableObject m_PowerUpValues;

    public GameObject m_Image;

    internal Collider2D m_Collider;

    private void Awake()
    {
        m_Collider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        m_Collider.enabled = true;
        m_Image.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
