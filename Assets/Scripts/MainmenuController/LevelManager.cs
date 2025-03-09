using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public LevelDataList levelDataList;
    public LevelData playingLevel;
    private string jsonPath = JsonPath.levelData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Nếu đã có instance, hủy GameObject mới để tránh trùng lặp
        }
        LoadData();
    }

    private void Start()
    {
        MenuEvent.OnEnterLevel += SetPlayingLevel;
    }

    private void SetPlayingLevel(LevelData levelData)
    {
        playingLevel = levelData;
    }

    private void LoadData()
    {
        string data = File.ReadAllText(jsonPath);
        levelDataList = JsonConvert.DeserializeObject<LevelDataList>(data);
        for (int i = 0; i < levelDataList.levelList.Count; i++)
        {
            levelDataList.levelList[i].iD = i;
        }
    }

    private void SaveData()
    {
        string data = JsonConvert.SerializeObject(levelDataList, Formatting.Indented);
        File.WriteAllText(jsonPath, data);
    }

    public void UpdatePlayingLevel(int starCnt)
    {
        if (starCnt > playingLevel.starCnt) playingLevel.starCnt = starCnt;
        if (playingLevel.levelNumber == levelDataList.currentLevel)
        {
            levelDataList.currentLevel++;
        }
        SaveData();
    }
}