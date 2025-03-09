using System.IO;
using Newtonsoft.Json;
using System.Collections;
using UnityEngine;

public class PlayerDataManager: MonoBehaviour
{
    public static PlayerDataManager Instance;

    public PlayerData playerData;

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
        }
        Instance.LoadData();
    }

    private void LoadData()
    {
        string data = File.ReadAllText(JsonPath.playerData);
        playerData = JsonConvert.DeserializeObject<PlayerData>(data);
    }

    public void SaveData()
    {
        string data = JsonConvert.SerializeObject(playerData, Formatting.Indented);
        File.WriteAllText(JsonPath.playerData, data);
    }

    public bool CanEnterLevel()
    {
        if(playerData.currentHealth > 0)
        {
            StartCoroutine(LoadSceneCoroutine());
            return true;
        }
        return false;
    }

    private IEnumerator LoadSceneCoroutine()
    {
        yield return new WaitForSeconds(.7f);
        SceneLoader.LoadScene(SceneName.Gameplay);
    }
}