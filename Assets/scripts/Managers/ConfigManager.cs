using UnityEngine;
using System.Collections;
using System.IO;

public class ConfigManager : MonoBehaviour
{
    private string gameDataFileName = "data.json";
    public GameData gameData;

    // Use this for initialization
    void Start()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            
            gameData = JsonUtility.FromJson<GameData>(dataAsJson);
            

        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public class GameData : MonoBehaviour
    {

    }
}
