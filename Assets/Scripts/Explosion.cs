using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private Sprite[] explosionFrames;

    private SpriteRenderer sr;
    
    private int frame = 0;
    private float animTimer = 0;
    private float frameDuration = 0.05f;

    void Awake()
    {
        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = explosionFrames[frame];
        sr.sortingOrder = 5;
    }

    void Update()
    {
        animTimer += Time.deltaTime;
        if (animTimer >= frameDuration)
        {
            frame += (int)(animTimer / frameDuration);
            animTimer -= frameDuration * (int)(animTimer / frameDuration);
            
            if (frame >= explosionFrames.Length)
            {
                Destroy(gameObject);
                return;
            }
                
            sr.sprite = explosionFrames[frame];
        }
    }
}
