using UnityEngine;
using System.Collections;

public class Player : Ship {

    Vector2 direction;

    Bounds m_ShipBoundary;

    [SerializeField]
    string[] m_BulletNames;

    enum Weapons
    {
        Normal,
        Scatter
    }

    // Use this for initialization
    IEnumerator Start () {

        direction = new Vector2();
        m_ShipBoundary = GetComponent<Renderer>().bounds;

        m_DestroyedState = new Destroyed(this);
        m_MovingState = new Moving(this);

        InitializeParameters();

		while (true) {
            InstantiateBullets();
			yield return new WaitForSeconds (0.05f);
		}
	}

    public override void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

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

    void InstantiateBullets()
    {
        GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject(m_BulletNames[0]);
        Vector2 bullet_pos = transform.position;

        // Instantiate on one side
        bullet_pos.x += 0.07f;
        bullet.transform.position = bullet_pos ;
        bullet.transform.rotation = transform.rotation;

        bullet.SetActive(true);

        GameObject another_bullet = ObjectPooler.SharedInstance.GetPooledObject(m_BulletNames[0]);

        // Instantiate on the other
        bullet_pos.x -= 0.14f;

        another_bullet.transform.position = bullet_pos;
        another_bullet.transform.rotation = transform.rotation;
 
        another_bullet.SetActive(true);
    }
    

}
