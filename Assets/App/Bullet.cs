using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public float speed = 5f;
	// Use this for initialization
	void Start () {
		
	}
    

    // Update is called once per frame
    void Update () {
        transform.position += transform.up * Time.deltaTime * speed;
        if (!IsInViewport())
        {
            gameObject.SetActive(false);
        }
	}

    bool IsInViewport()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.SetActive(false);
    }

}
