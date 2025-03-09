using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{
    [SerializeField] private LevelPreview levelPreview;
    [SerializeField] private LevelNode[] levelNodes;
    [SerializeField] private GameObject loadingUI;
    [SerializeField] private Text healthTxt;

    private void Start()
    {
        StartCoroutine(LoadLevelNode(LevelManager.Instance.levelDataList));
        healthTxt.text = string.Format("{0}/{1}", PlayerDataManager.Instance.playerData.currentHealth, PlayerDataManager.Instance.playerData.maxHealth);
        MenuEvent.OnEnterLevel += TurnOnLoadingUI;
    }

    private void LoadLevelPreview(int levelID)
    {
        if (levelID >= LevelManager.Instance.levelDataList.levelList.Count) return;
        StartCoroutine(levelPreview.LoadUI(LevelManager.Instance.levelDataList.levelList[levelID]));
    }

    public IEnumerator LoadLevelNode(LevelDataList levelCollection)
    {
        while(LevelManager.Instance.levelDataList == null) yield return null;
        for (int i = 0; i < levelCollection.currentLevel; i++)
        {
            if (i >= LevelManager.Instance.levelDataList.levelList.Count) yield break;
            int levelID = levelCollection.levelList[i].iD;
            levelNodes[i].LoadUI(levelCollection.levelList[i]);
            levelNodes[i].button.onClick.AddListener(() => LoadLevelPreview(levelID));
            levelNodes[i].button.onClick.AddListener(TurnOnPreviewLevel);
        }
    }

    private void TurnOnPreviewLevel()
    {
        levelPreview.gameObject.SetActive(true);
    }

    private void TurnOnLoadingUI(LevelData levelData)
    {
        if(loadingUI == null) return;
        loadingUI.SetActive(true);
    }
}