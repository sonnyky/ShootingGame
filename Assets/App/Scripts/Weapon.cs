using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public AudioClip[] m_WeaponSe;
    private AudioSource m_AudioSource;
    private int m_SeId = 0;

	// Use this for initialization
	void Start () {
        m_AudioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlaySe()
    {
        m_AudioSource.Play();
    }

    public void ChangeSe(Weapons weapon)
    {
        m_SeId = (int)weapon;
        m_AudioSource.clip = m_WeaponSe[(int)weapon];
    }

}
