using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class LevelManager: MonoBehaviour
{
    public LevelCollection levelCollection;
    [SerializeField] private MenuUIManager menuUIManager;
    private string jsonPath = "/Users/ngocanh/Documents/Unity Project/Work project1/LevelsData.json";

    private void Start()
    {
        string data = File.ReadAllText(jsonPath);
        levelCollection = JsonConvert.DeserializeObject<LevelCollection>(data);
        menuUIManager.LoadLevelNode(levelCollection);
    }
}