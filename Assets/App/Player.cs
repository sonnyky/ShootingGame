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

    Weapons currentWeapon = Weapons.Normal;

    // Use this for initialization
    IEnumerator Start () {

        direction = new Vector2();
        m_ShipBoundary = GetComponent<Renderer>().bounds;

        m_DestroyedState = new Destroyed(this);
        m_MovingState = new Moving(this);

		while (true) {
            InstantiateBullets(currentWeapon);
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

    void InstantiateBullets(Weapons selectedWeapon)
    {

        switch (selectedWeapon) {
            case Weapons.Normal:
                ShootNormalBullets();
                break;
            default:
                ShootNormalBullets();
                break;
        }
    }

    void ShootNormalBullets()
    {
        GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject(m_BulletNames[0]);
        Vector2 bullet_pos = transform.position;

        // Instantiate on one side
        bullet_pos.x += 0.07f;
        bullet.transform.position = bullet_pos;
        bullet.transform.rotation = transform.rotation;

        bullet.SetActive(true);

        GameObject another_bullet = ObjectPooler.SharedInstance.GetPooledObject(m_BulletNames[0]);

        // Instantiate on the other
        bullet_pos.x -= 0.14f;

        another_bullet.transform.position = bullet_pos;
        another_bullet.transform.rotation = transform.rotation;

        another_bullet.SetActive(true);
    }

    /// <summary>
    /// Called by the power up items and handles the behavior change due to the power up. Target being weapons or ship armor, further divided into types.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="type"></param>
    public void PowerUp(string target, string type)
    {
        Debug.Log("target and type : " + target + ", " + type);
    }
    

}
