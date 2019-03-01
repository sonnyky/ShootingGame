using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PowerUpBonus : Bonus, ISoundObject {

    // Interfaces to control sounds such as detecting when a clip has finished playing.
    private SoundObject m_SoundObject;
    public bool i_ClipFinished { get; set; }

    private AudioClipManager m_ClipManager;

	// Use this for initialization
	void Start () {
        m_ClipManager = GetComponentInChildren<AudioClipManager>();
        m_ClipManager.m_OnSeClipFinished += ClipFinished;
	}

    public void ClipFinished()
    {
        DisableObject();
    }
  
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            m_Collider.enabled = false;
            collider.gameObject.GetComponent<Player>().PowerUp(m_PowerUpValues.powerUpTarget, m_PowerUpValues.powerUpType);

            m_Image.SetActive(false);
            m_ClipManager.PlayAudio();
        }
    }

    private void DisableObject()
    {
        gameObject.SetActive(false);
    }


}
