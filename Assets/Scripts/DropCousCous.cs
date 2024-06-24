using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropCousCous : Drop
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Player p = other.GetComponent<Player>();
        if (p as Player != null)
        {
            p.CousCousUp();
            Destroy(gameObject);
        }
    }
}
