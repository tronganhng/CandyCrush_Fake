using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public LevelDataList levelDataList;
    public LevelData playingLevel;
    private string jsonPath = "/Users/ngocanh/Documents/Unity Project/Work project1/LevelsData.json";

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        MenuEvent.OnEnterLevel += SetPlayingLevel;
        LoadJsonData();
        MenuEvent.OnLoadLevelNodes?.Invoke(levelDataList);
    }

    private void SetPlayingLevel(LevelData levelData)
    {
        playingLevel = levelData;
    }

    private void LoadJsonData()
    {
        string data = File.ReadAllText(jsonPath);
        levelDataList = JsonConvert.DeserializeObject<LevelDataList>(data);
    }
}