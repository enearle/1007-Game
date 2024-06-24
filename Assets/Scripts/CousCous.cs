using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CousCous : MonoBehaviour
{
    
    
    // Init
    private Camera c;
    private GameObject o;
    private float speed = 200;
    private float rotSpeed = 100;
    private Rigidbody2D rb;
    private Vector2 target;
    private Transform enemyPos;
    
    void Awake()
    {
        if (WorldManager.Instance.aliveEnemies.Count > 0)
        {
            int closestEnemyIndex = 0;
            for(int i = 0; i < WorldManager.Instance.aliveEnemies.Count; i++)
            {
                if ((WorldManager.Instance.aliveEnemies[i].transform.position - transform.position).magnitude
                    < (WorldManager.Instance.aliveEnemies[closestEnemyIndex].transform.position - transform.position)
                    .magnitude)
                    closestEnemyIndex = i;
            }

            enemyPos = WorldManager.Instance.aliveEnemies[closestEnemyIndex].transform;
        }
        
        c = Camera.main;
        o = gameObject;
        rb = o.GetComponent<Rigidbody2D>();
    }
    

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraSpace = c.WorldToScreenPoint(o.transform.position);
        
        if (cameraSpace.y < 0 || cameraSpace.y > c.pixelHeight || cameraSpace.x < 0 || cameraSpace.x > c.pixelWidth)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (enemyPos != null)
            target = enemyPos.position;
        else
            target = transform.position + (Vector3.forward * 20);
        
        SeekForward();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Entity e = other.gameObject.GetComponent<Entity>();
        if (e as Enemy != null)
        {
            Collider2D[] hitList;
            hitList = Physics2D.OverlapCircleAll(transform.position, 3);

            foreach (var hit in hitList)
            {
                Enemy enemy = hit.gameObject.GetComponent<Enemy>();
                if (enemy as Enemy != null)
                {
                    enemy.DoDamage(100);
                }
            }
            //e.DoDamage(100);
            WorldManager.Instance.CreateExplosion(transform.position, 10f);
            Destroy(gameObject);
        }
    }
    
    public void SeekForward()
    {
        Vector2 directionToTarget = ((Vector3)target - rb.transform.position).normalized;

        float targetAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg + 90f;

        float angleDifference = Mathf.DeltaAngle(targetAngle, rb.transform.eulerAngles.z);

        float rotationStep = rotSpeed* Time.fixedDeltaTime;

        float rotationAmount = Mathf.Clamp(angleDifference, -rotationStep, rotationStep);
        
        rb.transform.Rotate(Vector3.forward, rotationAmount);

        rb.velocity = rb.transform.up * (speed * Time.fixedDeltaTime);
    }
}
