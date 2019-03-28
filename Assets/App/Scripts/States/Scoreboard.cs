using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour {

    private StateManager<Scoreboard> m_CurrentState;
    public StateManager<Scoreboard> m_PreviousState;

    internal Idle m_IdleState;
    internal Update m_UpdateState;

    private int m_CurrentScore;

    [SerializeField]
    private Sprite[] m_NumberSprites;

    [SerializeField]
    private Image[] m_DigitsPosition;

    private void Awake()
    {
        m_IdleState = new Idle(this);
        m_UpdateState = new Update(this);
        SetState(m_IdleState);
    }

    public void SetState(StateManager<Scoreboard> nextState)
    {
        if (m_CurrentState != null) m_CurrentState.OnStateExit();
        m_PreviousState = m_CurrentState;
        m_CurrentState = nextState;
        if (m_CurrentState != null) m_CurrentState.OnStateEnter();
    }

    public void SetNewScore(int score)
    {

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
