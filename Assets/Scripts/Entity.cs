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

    protected virtual void Awake()
    {
        entity = this;
    }

    protected virtual void Start()
    {
        
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
        worldManager.sound.Play(2);
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
            worldManager.sound.Play(1);
            worldManager.CreateExplosion(transform.position, 3f);
            if (this as Player != null)
            {
                worldManager.LoseGame();
            }
            else
            {
                worldManager.DestroyEnemy((Enemy)this);
                worldManager.UpdateScore();
            } 
        }
    }

    private void UpdateHealthUI()
    {
        HealthBar.color = new Color(1 - health / 100, health / 100, 0, 1);
        HealthBar.transform.localScale = new Vector3(health / 100, 0.1f, 1);
    }
}