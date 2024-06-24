using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Xml.Serialization;

[XmlRoot("GameData")]
public class GameData
{
    public int saveScore;

    public GameData()
    {
        saveScore = 0;
    }
}
public class MainController : MonoBehaviour
{
    public static MainController Instance;
    private int currentScore = 0;
    private GameData gameData;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        GameData gd = DeserializeFromXml();

        if (gd != null)
        {
            gameData = DeserializeFromXml();
        }
        else
        {
            gameData = new GameData();
            SerializeToXml(gameData);
        }
            
    }
    
    void SerializeToXml(GameData data)
    {
        if (currentScore > gameData.saveScore)
        {
            gameData.saveScore = currentScore;
            Debug.Log("New top score saved.");
        }

        XmlSerializer serializer = new XmlSerializer(typeof(GameData));
        using (var streamWriter = new FileStream("Assets/gamedata.xml", FileMode.Create))
        {
            serializer.Serialize(streamWriter, gameData);
        }
    }

    private GameData DeserializeFromXml()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(GameData));
        
        using (var streamReader = new FileStream("Assets/gamedata.xml", FileMode.Open))
        {
            return serializer.Deserialize(streamReader) as GameData;
        }
    }

    public int GetScore()
    {
        return currentScore;
    }

    public int GetTopScore()
    {
        gameData = DeserializeFromXml();
        SerializeToXml(gameData);
        return gameData.saveScore;
    }
    
    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void EndScene(int score)
    {
        currentScore = score;
        SceneManager.LoadScene("GameOver");
    }
    
}
