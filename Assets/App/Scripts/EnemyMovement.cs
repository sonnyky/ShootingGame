using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {
    float speed;

	// Use this for initialization
	void Start () {
        speed = 5f;
	}
	
	// Update is called once per frame
	void Update () {
      
       transform.position += transform.up * -Time.deltaTime;
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
        //Debug.Log("collision with : " + collision.gameObject.name);
    }

}
