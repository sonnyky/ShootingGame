using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolvingBackground : MonoBehaviour {

    public GameObject m_FirstBackground;
    public GameObject m_SecondBackground;

    private Renderer m_BackgroundRenderer;
    private float m_HeightOfVisibleBackground;
    private Vector2 m_RevolvingPosition;

	// Use this for initialization
	void Start () {
        m_BackgroundRenderer = m_SecondBackground.GetComponent<Renderer>();
        m_HeightOfVisibleBackground = 14f;
        m_RevolvingPosition = m_FirstBackground.transform.localPosition;

	}
	
	// Update is called once per frame
	void Update () {
		m_FirstBackground.transform.localPosition += transform.up * -Time.deltaTime;
        m_SecondBackground.transform.localPosition += transform.up * -Time.deltaTime;
        if (IsFarEnoughBehindCamera(m_SecondBackground.transform.localPosition))
        {
            m_SecondBackground.transform.localPosition = m_RevolvingPosition;
        }
        if (IsFarEnoughBehindCamera(m_FirstBackground.transform.localPosition))
        {
            m_FirstBackground.transform.localPosition = m_RevolvingPosition;
        }
    }

    bool IsFarEnoughBehindCamera(Vector2 pos)
    {
        return pos.y < 0 && pos.y < -m_HeightOfVisibleBackground ? true:false;
    }

}
