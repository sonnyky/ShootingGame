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
    private int m_MaxDigits;

    private void Awake()
    {
        m_IdleState = new Idle(this);
        m_UpdateState = new Update(this);
        m_MaxDigits = m_DigitsPosition.Length;
        m_CurrentScore = 0;
        SetState(m_IdleState);
    }

    public void SetState(StateManager<Scoreboard> nextState)
    {
        if (m_CurrentState != null) m_CurrentState.OnStateExit();
        m_PreviousState = m_CurrentState;
        m_CurrentState = nextState;
        if (m_CurrentState != null) m_CurrentState.OnStateEnter();
    }

    public void AddNewScore(int score)
    {
        m_CurrentScore += score;

        string scoreString = m_CurrentScore.ToString();

        Debug.Log("score digits : "  + scoreString.Length);
        int digitId = 0;
        for(int i = scoreString.Length - 1; i >= 0; i--)
        {
            Debug.Log("digit : " + scoreString[i] + " at position : " + (m_MaxDigits - 1 - digitId));
            m_DigitsPosition[digitId].sprite = m_NumberSprites[int.Parse(scoreString[i].ToString())];
            digitId++;
        }
    }
}
