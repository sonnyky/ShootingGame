using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {

    public float m_MaxHealth;
    public float m_CurrentHealth;

    public HealthBar m_HealthBar;

    private StateManager<Ship> currentState;
    public StateManager<Ship> previousState;

    internal float speed = 5f;

    internal Destroyed m_DestroyedState;
    internal Moving m_MovingState;

    internal Scoreboard m_ScoreBoard;

    public int m_OwnerId;

    private void Awake()
    {
        m_DestroyedState = new Destroyed(this);
        m_MovingState = new Moving(this);
        m_ScoreBoard = GameObject.Find("Scoreboard").GetComponent<Scoreboard>();
        SetState(m_MovingState);
    }

    public void ReinitializeParameters()
    {
        m_CurrentHealth = m_MaxHealth;
        m_HealthBar.ResetToMax();
    }

    public void SetState(StateManager<Ship> nextState)
    {
        if (currentState != null) currentState.OnStateExit();
        previousState = currentState;
        currentState = nextState;
        if (currentState != null) currentState.OnStateEnter();
    }

    // Update is called once per frame
    void Update () {
        if (currentState != null)
        {
            currentState.Tick();
        }
	}

    internal bool IsInViewport()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        string incomingTag = collider.gameObject.tag;

        string[] tagSplit = incomingTag.Split('_');

        // Colliding with a weapon
        if (tagSplit[0].Equals("Shot"))
        {
            if (collider.gameObject.GetComponent<Shot>().m_ShotOwner != m_OwnerId)
            {
                float damage = collider.transform.GetComponent<Shot>().GetDamage();
                ReceiveDamage(damage);
               
                if (m_CurrentHealth <= 0f)
                {
                    SetState(m_DestroyedState);
                }
            }
        }
        else
        {
            //Debug.Log("collided with an enemy unit");
            OnCollisionWithAnotherUnit(collider.gameObject);
        }
    }

    public virtual void OnDestroyed()
    {

    }

    public virtual void OnCollisionWithAnotherUnit(GameObject collidingObject)
    {

    }

    public virtual void Move()
    {
        
    }

    public void ReceiveDamage(float damage)
    {
        m_CurrentHealth -= damage;
        m_HealthBar.HitPointReduceTo(m_CurrentHealth / m_MaxHealth);
    }

    public void ExplosionEffect()
    {
        GameObject explosion = ObjectPooler.SharedInstance.GetPooledObject("Eff_Explosion");
        explosion.transform.position = transform.position;
        explosion.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
