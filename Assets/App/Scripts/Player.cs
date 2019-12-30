using UnityEngine;
using System.Collections;

public enum Weapons
{
    Normal,
    Scatter
}

public class Player : Ship {

    Vector2 direction;

    Bounds m_ShipBoundary;

    [SerializeField]
    string[] m_BulletNames;

    public Weapon m_WeaponManager;

    Weapons currentWeapon = Weapons.Normal;

    // Use this for initialization
    void Start () {

        direction = new Vector2();
        m_ShipBoundary = GetComponent<Renderer>().bounds;

        m_DestroyedState = new Destroyed(this);
        m_MovingState = new Moving(this);

        m_OwnerId = 0;

        StartCoroutine(Fire());
	}

    IEnumerator Fire()
    {
        InstantiateBullets(currentWeapon);
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(Fire());
    }

    public override void Move()
    {
        // Keyboard input
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        if(x != 0 || y != 0)
        {
            if (!MoveAreaLimit(x, y))
            {
                direction = new Vector2(x, y).normalized;
            }
            else
            {
                direction = Vector2.zero;
            }
        }
        else
        {
            direction = Vector2.zero;
        }

        // Mouse input. This will cause the keyboard commands to stop working because direction is zeroed here
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePoint = Input.mousePosition;
            mousePoint.z = 1f;
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mousePoint);
            x = worldPoint.x;
            y = worldPoint.y;
         
            direction.x = x - transform.position.x;
            direction.y = y - transform.position.y;
        }
        else
        {
            direction = Vector2.zero;
        }

        if(Tinker.Input.touchCount > 0)
        {
            Tinker.Touch touchInstance = Tinker.Input.GetTouch(0);
            Vector3 touchPoint = touchInstance.position;
            touchPoint.z = 1f;
            Vector3 pos = Camera.main.ScreenToWorldPoint(touchPoint);

            direction.x = pos.x - transform.position.x;
            direction.y = pos.y - transform.position.y;
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
        m_WeaponManager.ChangeSe(currentWeapon);

        m_WeaponManager.PlaySe();
        switch (selectedWeapon) {
            case Weapons.Normal:
                ShootNormalBullets();
                break;
            case Weapons.Scatter:
                ShootScatterBullets();
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
        bullet.GetComponent<Shot>().m_ShotOwner = 0;
        bullet.SetActive(true);

        GameObject another_bullet = ObjectPooler.SharedInstance.GetPooledObject(m_BulletNames[0]);

        // Instantiate on the other
        bullet_pos.x -= 0.14f;

        another_bullet.transform.position = bullet_pos;
        another_bullet.transform.rotation = transform.rotation;
        another_bullet.GetComponent<Shot>().m_ShotOwner = 0;
        another_bullet.SetActive(true);

    }

    void ShootScatterBullets()
    {
        GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject(m_BulletNames[1]);
        bullet.transform.position = transform.position;
        bullet.SetActive(true);
        bullet.GetComponent<ScatterBullet>().m_DirectionId = 0;

        GameObject leftBullet = ObjectPooler.SharedInstance.GetPooledObject(m_BulletNames[1]);
        leftBullet.transform.position = transform.position;
        leftBullet.SetActive(true);
        leftBullet.GetComponent<ScatterBullet>().m_DirectionId = 1;

        GameObject rightBullet = ObjectPooler.SharedInstance.GetPooledObject(m_BulletNames[1]);
        rightBullet.transform.position = transform.position;
        rightBullet.SetActive(true);
        rightBullet.GetComponent<ScatterBullet>().m_DirectionId = 2;
    }

    /// <summary>
    /// Called by the power up items and handles the behavior change due to the power up. Target being weapons or ship armor, further divided into types.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="type"></param>
    public void PowerUp(string target, string type)
    {
        switch (target)
        {
            case "Weapons":
                switch (type)
                {
                    case "Random":
                        //Weapons newWeapon = Tinker.Utilites.RandomEnumValue<Weapons>();
                        currentWeapon = Weapons.Scatter;
                        break;
                    default:
                        break;

                }

                break;

            default:
                break;
        }

    }

    public override void OnCollisionWithAnotherUnit(GameObject collidingObject)
    {
        base.OnCollisionWithAnotherUnit(collidingObject);
        string incomingTag = collidingObject.tag;

        string[] tagSplit = incomingTag.Split('_');
        switch (tagSplit[0])
        {
            case "Enemy":
                int damage = collidingObject.GetComponent<Enemy>().m_DamageToPlayerWhenHit;
                ReceiveDamage(damage);
                break;
        }

    }


}
