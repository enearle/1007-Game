using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : Entity
{
    public bool moveUp = true;
    [SerializeField] private Vector3 moveSpeed = new (-0.5f,2);
    [SerializeField] private Transform missileSpawnTransform;
    [SerializeField] private float fireRate = 0.54321f;
    private float fireTimer = 0;
    private bool canFire = false;
    private void Update()
    {
        // Move
        if (moveUp)
        {
            moveSpeed.y = 2;
            Vector3 newPos = gameObject.transform.position + moveSpeed * Time.deltaTime;
            newPos.z = 0;
            if (newPos.y >= 6) 
                newPos.y = -6;
            gameObject.transform.position = newPos;
        }
        else
        {
            moveSpeed.y = -2;
            Vector3 newPos = gameObject.transform.position + moveSpeed * Time.deltaTime;
            newPos.z = 0;
            if (newPos.y <= -6) 
                newPos.y = 6;
            gameObject.transform.position = newPos;
        }

        // Fire
        if (canFire)
        {
            Missile m = worldManager.SpawnMissile(false);
            m.transform.position = missileSpawnTransform.position;
            m.transform.rotation = missileSpawnTransform.rotation;
            canFire = false;
        }
        else
        {
            if (fireTimer >= fireRate)
            {
                fireTimer = 0;
                canFire = true;
            }
            else
            {
                fireTimer += Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Entity e = other.gameObject.GetComponent<Entity>();
        if (e as Player != null)
        {
            e.DoDamage(100);
        }
    }
}
