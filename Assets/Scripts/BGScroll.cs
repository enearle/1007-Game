using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroll : MonoBehaviour
{
    // Background Sprite Object
    [SerializeField] private GameObject rSpriteObject;
    private List<GameObject> spriteObjects = new List<GameObject>();
    private float spriteScale = 1.5f;
    [SerializeField] private float scrollSpeed = 5;
    
    // Start is called before the first frame update
    private void Awake()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject so = Instantiate(rSpriteObject);
            so.transform.position = new Vector3((i - 1) * 32.4f, 0);
            so.transform.localScale = Vector3.one * spriteScale;
            spriteObjects.Add(so);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 3; i++)
        {
            spriteObjects[i].transform.position += new Vector3(-scrollSpeed * Time.deltaTime, 0);
            if (spriteObjects[i].transform.position.x <= (- 46f))
                spriteObjects[i].transform.position += new Vector3(97.2f,0); 
        }
        
    }
}
