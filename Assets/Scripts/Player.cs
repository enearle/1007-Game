using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : Entity
{
    // Declaration
    private GameObject o;
    private Camera c;
    private bool fireCoolDown = false;
    [SerializeField] private float fireCDDuration = 0.08f;
    private float fireTimer = 0;
    [SerializeField] private Transform missileSpawnTransform;
    [SerializeField] private float playerMoveSpeed = 10;
    
    
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        c = Camera.main;
        o = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        
        FireWeapon();
    }
    
    void FireWeapon()
    {
        if (fireCoolDown)
            fireTimer += Time.deltaTime;
        
        if (fireTimer >= fireCDDuration)
        {
            fireTimer = 0;
            fireCoolDown = false;
        }
        
        if ((Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Space)) && !fireCoolDown)
        {
            fireCoolDown = true;
            Missile m = worldManager.SpawnMissile(true);
            m.transform.position = missileSpawnTransform.position;
            m.transform.rotation = missileSpawnTransform.rotation;
        }
    }
    void MovePlayer()
    {
        Vector3 move = new Vector3((Input.GetKey(KeyCode.D) ? 1 : 0) + (Input.GetKey(KeyCode.A) ? -1 : 0),
            (Input.GetKey(KeyCode.W) ? 1 : 0) + (Input.GetKey(KeyCode.S) ? -1 : 0));
        
        Vector3 movement = move.normalized * (Time.deltaTime * playerMoveSpeed);
        
        Vector3 cameraSpace = c.WorldToScreenPoint(o.transform.position + movement);
        
        if (cameraSpace.y < 0 || cameraSpace.y > c.pixelHeight)
        {
            movement.y = 0;
        }

        if (cameraSpace.x < 0 || cameraSpace.x > c.pixelWidth / 2f)
        {
            movement.x = 0;
        }
        
        o.transform.position += movement;
    }
}
