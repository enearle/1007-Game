using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonCamera : MonoBehaviour
{

    private static SingletonCamera instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
}
