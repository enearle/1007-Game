using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Vector3 Move = new Vector3(-5, 0, 0);
    
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += Move * Time.deltaTime;
        if(gameObject.transform.position.x <= -8.5)
            Destroy(gameObject);
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
