using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    Animator m_ExplosionAnimator;

	// Use this for initialization
	void Start () {
        m_ExplosionAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (m_ExplosionAnimator.GetCurrentAnimatorStateInfo(0).IsName("explosion") && m_ExplosionAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
           //Animation is playing, first run.
        }
        else
        {
            //Animation is finished
            gameObject.SetActive(false);
        }
	}
}
