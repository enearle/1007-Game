using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected WorldManager worldManager;
    
    protected float health = 100;
    [SerializeField] private SpriteRenderer HealthBar;
    private GameObject healthSprite;
    private Entity entity;
    [SerializeField] private GameObject healthDrop;
    [SerializeField] private GameObject cousCousDrop;
    protected virtual void Awake()
    {
        entity = this;
    }

    public void SetReferenceToWorld(WorldManager world)
    {
        worldManager = world;
    }
    protected virtual void OnEnable()
    {
        health = 100;
        UpdateHealthUI();
    }

    public void DoDamage(float Damage)
    {
        SoundManager.Instance.PlaySoundFX(2);
        if (worldManager == null)
        {
            Debug.Log("Null worldManager reference");
            return;
        }
        
        health -= Damage;
        if (health <= 0)
            health = 0;
        
        if (this as Player != null)
            worldManager.UpdateHealth(health);
        
        UpdateHealthUI();
        Debug.Log($"Did {Damage} to enemy health = {health}");
        
        if (health <= 0)
        {
            SoundManager.Instance.PlaySoundFX(1);
            worldManager.CreateExplosion(transform.position, 3f);
            if (this as Player != null)
            {
                worldManager.EndGame();
            }
            else
            {
                if (Random.Range(0, 10) % 9 == 0)
                {
                    if (Random.Range(1, 3) % 2 == 0)
                        Instantiate(healthDrop, transform.position, transform.rotation);
                    else
                        Instantiate(cousCousDrop, transform.position, transform.rotation);
                }
                worldManager.DestroyEnemy((Enemy)this);
                worldManager.UpdateScore();
            } 
        }
    }

    protected void UpdateHealthUI()
    {
        HealthBar.color = new Color(1 - health / 100, health / 100, 0, 1);
        HealthBar.transform.localScale = new Vector3(health / 100, 0.1f, 1);
    }
}