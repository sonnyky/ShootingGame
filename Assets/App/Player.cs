using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public float speed = 5;

    Vector2 direction;

    Bounds m_ShipBoundary;

	// PlayerBulletプレハブ
	public GameObject bullet;

    private void Awake()
    {
        direction = new Vector2();
        m_ShipBoundary = GetComponent<Renderer>().bounds;
    }

    // Use this for initialization
    IEnumerator Start () {
		while (true) {
            GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("Shot");
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;
            bullet.SetActive(true);
			yield return new WaitForSeconds (0.05f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		float x = Input.GetAxisRaw ("Horizontal");
		float y = Input.GetAxisRaw ("Vertical");

        if (!MoveAreaLimit(x, y))
        {
            direction = new Vector2(x, y).normalized;            
        }
        else
        {
            direction = Vector2.zero;
        }
        GetComponent<Rigidbody2D>().velocity = direction * speed;
    }

    public bool MoveAreaLimit(float xDir, float yDir)
    {
       
        Vector2 min = Camera.main.WorldToViewportPoint(transform.position + m_ShipBoundary.min);
        Vector2 max = Camera.main.WorldToViewportPoint(transform.position + m_ShipBoundary.max);

        return (min.x < 0f && xDir < 0) || (min.y < 0f && yDir < 0) || (max.x > 1f && xDir > 0) || (max.y > 1f && yDir > 0) ? true : false;
    }
    

}
