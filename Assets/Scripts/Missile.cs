using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public bool playerOwned = false;
    
    
    // Init
    private Camera c;
    private GameObject o;
    private float speed = 20;
    private Rigidbody2D rb;
    void Awake()
    {
        c = Camera.main;
        o = gameObject;
        rb = o.GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        
        Vector3 cameraSpace = c.WorldToScreenPoint(o.transform.position);
        
        if (cameraSpace.y < 0 || cameraSpace.y > c.pixelHeight || cameraSpace.x < 0 || cameraSpace.x > c.pixelWidth)
        {
            WorldManager.Instance.DestroyMissile(this);
        }
    }

    private void FixedUpdate()
    {

        rb.velocity = o.transform.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Entity e = other.gameObject.GetComponent<Entity>();
        if (e as Enemy != null && playerOwned)
        {
            e.DoDamage(34);
            WorldManager.Instance.CreateExplosion(transform.position, 1f);
            WorldManager.Instance.DestroyMissile(this);
        }
        else if (e as Player != null && !playerOwned)
        {
            e.DoDamage(10);
            WorldManager.Instance.CreateExplosion(transform.position, 1f);
            WorldManager.Instance.DestroyMissile(this);
        }
    }
}
