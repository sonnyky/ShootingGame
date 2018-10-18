using UnityEngine;
using System.Collections;

public class Enemy : Ship {
    float speed;

	// Use this for initialization
	void Start () {
        speed = 5f;
	}
	
	// Update is called once per frame
	void Update () {
      
      // transform.position += transform.up * -Time.deltaTime;
        //if (!IsInViewport())
        //{
        //    gameObject.SetActive(false);
        //}
	}
}
