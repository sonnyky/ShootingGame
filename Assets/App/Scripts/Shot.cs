﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour {

    public float m_damage;

    public float speed = 5f;

    public int m_ShotOwner { get; set; } // 0:Player, 1:Enemy

    protected virtual void Start()
    {

    }

    public float GetDamage()
    {
        return m_damage;
    }

    internal bool IsInViewport()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        int ownerId = collider.GetComponent<Ship>().m_OwnerId;
        if (m_ShotOwner != ownerId)
        {
            gameObject.SetActive(false);
        }
    }


}
