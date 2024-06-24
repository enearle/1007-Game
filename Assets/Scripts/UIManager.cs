using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject soundMenu;

    private bool pause = false;

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape)) && pauseMenu != null)
        {
            ToggleMenuVisibility();
        }
    }

    public void ToggleMenuVisibility()
    {
        pause = !pause;
        Time.timeScale = pause ? 0 : 1; 
        pauseMenu.SetActive(pause);
    }
    
    public void ToggleSoundMenuVisibility()
    {
        soundMenu.SetActive(!soundMenu.activeSelf);
    }

    public void LoadScene(int sceneIndex)
    {
        string[] scenes = { "TitleScene", "Level1", "GameOver" };
        Time.timeScale = 1;
        MainController.Instance.ChangeScene(scenes[sceneIndex]);
    }
}
