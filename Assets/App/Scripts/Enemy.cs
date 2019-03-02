using UnityEngine;
using System.Collections;

public class Enemy : Ship {

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
}
