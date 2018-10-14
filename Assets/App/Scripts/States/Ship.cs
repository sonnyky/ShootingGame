using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    bool IsInViewport()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string incomingTag = collision.gameObject.tag;
        if (collision.gameObject.CompareTag())
        {

        }
    }
}
