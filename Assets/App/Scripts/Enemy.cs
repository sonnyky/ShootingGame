using UnityEngine;
using System.Collections;

public class Enemy : Ship {

    private int m_ScoreWhenDestroyed = 10;

	// Use this for initialization
	void Start () {
        speed = -1f;
        m_OwnerId = 1;
	}

    /// <summary>
    /// Called when the enemy state is moving.
    /// </summary>
    public override void Move()
    {
        
    }

    public override void OnDestroyed()
    {
        base.OnDestroyed();
        AddPlayerScore();
    }

    public void AddPlayerScore()
    {
        m_ScoreBoard.AddNewScore(m_ScoreWhenDestroyed);
    }
}
