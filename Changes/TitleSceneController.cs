using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneController : MonoBehaviour
{
    public Button startGameButton;
    public Button pauseMenuButton;
    public AudioSource backgroundMusic;

    void Start()
    {
        startGameButton.onClick.AddListener(StartGame);
        pauseMenuButton.onClick.AddListener(OpenPauseMenu);
        if (backgroundMusic != null)
        {
            backgroundMusic.Play();
        }
    }

    void StartGame()
    {
        SceneManager.LoadScene("Level1"); // Replace with your main game scene name
    }

    void OpenPauseMenu()
    {
        // Implement pause menu functionality
        Debug.Log("Pause Menu button clicked!");
    }
}
