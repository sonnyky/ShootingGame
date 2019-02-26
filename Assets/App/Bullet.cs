using UnityEngine;
using System.Collections;

public class Bullet : Shot {
	public float speed = 5f;
	
    void Update () {
        transform.position += transform.up * Time.deltaTime * speed;
        if (!IsInViewport())
        {
            gameObject.SetActive(false);
        }
	}
}
