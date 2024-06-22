using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverSceneController : MonoBehaviour
{
    public Button mainMenuButton;

    void Start()
    {
        mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    void GoToMainMenu()
    {
        SceneManager.LoadScene("TitleScene"); // Replace with your title scene name
    }
}
