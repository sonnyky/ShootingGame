using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioClipManager : MonoBehaviour {

    private AudioSource m_AudioSource;

    private bool m_AudioStarted;

    public event Action m_OnSeClipFinished;

    // Use this for initialization
    void Start () {
        m_AudioStarted = false;
        m_AudioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!m_AudioSource.isPlaying && m_AudioStarted)
        {
            m_AudioStarted = false;
            ClipFinished();
        }
    }

    private void ClipFinished()
    {
        m_OnSeClipFinished?.Invoke();
    }

    public void PlayAudio()
    {
        m_AudioStarted = true;
        m_AudioSource.Play();
    }
}
