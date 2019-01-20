using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {

    public float m_MaxHealth;
    public float m_CurrentHealth;

    private HealthBar m_HealthBar;

    private StateManager<Ship> currentState;
    public StateManager<Ship> previousState;

    internal float speed = 5f;

    internal Destroyed m_DestroyedState;
    internal Moving m_MovingState;

    private void Awake()
    {
        m_HealthBar = transform.Find("HealthBar").GetComponent<HealthBar>();
      
        m_DestroyedState = new Destroyed(this);
        m_MovingState = new Moving(this);
    }

    public void InitializeParameters()
    {
        m_CurrentHealth = m_MaxHealth;
        m_HealthBar.ResetToMax();
        //SetState(m_MovingState);
    }

    public void MoveShip(float speed)
    {
        transform.position += transform.up * Time.deltaTime *speed;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string incomingTag = collision.gameObject.tag;
        if (collision.gameObject.CompareTag("Shot"))
        {
            float damage = collision.gameObject.GetComponent<Shot>().GetDamage();

            m_CurrentHealth -= damage;
            m_HealthBar.HitPointReduceTo(m_CurrentHealth/m_MaxHealth);
            

            if (m_CurrentHealth <= 0f)
            {
                ExplosionEffect();
                SetState(m_DestroyedState);
            }

        }
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
