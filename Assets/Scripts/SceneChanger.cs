using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private void Awake()
    {
        Destroy(WorldManager.Instance.gameObject);
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
