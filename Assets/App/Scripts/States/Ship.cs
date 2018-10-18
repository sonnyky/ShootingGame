using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {

    public float m_MaxHealth;
    public float m_CurrentHealth;

    private HealthBar m_HealthBar;

    private StateManager<Ship> currentState;
    public StateManager<Ship> previousState;


    internal Destroyed m_DestroyedState;
    internal Initialize m_InitializeState;

    private void Awake()
    {
        m_HealthBar = transform.Find("HealthBar").GetComponent<HealthBar>();
      
        m_DestroyedState = new Destroyed(this);
        m_InitializeState = new Initialize(this);
    }

    public void InitializeParameters()
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
        currentState.Tick();
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
                SetState(m_DestroyedState);
            }

        }
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
