using UnityEngine;
using System.Collections;

public class Bullet : Shot {
	
    void Update () {
        transform.position += transform.up * Time.deltaTime * speed;
        if (!IsInViewport())
        {
            gameObject.SetActive(false);
        }
	}
}
