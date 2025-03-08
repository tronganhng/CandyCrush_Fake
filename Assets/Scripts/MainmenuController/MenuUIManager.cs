using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{
    [SerializeField] private LevelPreview levelPreview;
    [SerializeField] private LevelNode[] levelNodes;
    [SerializeField] private GameObject loadingUI;

    private void OnEnable()
    {
        MenuEvent.OnLoadLevelNodes += LoadLevelNode;
        MenuEvent.OnEnterLevel += TurnOnLoadingUI;
    }

    private void LoadLevelPreview(int index)
    {
        if (index >= LevelManager.Instance.levelDataList.levelList.Count) return;
        StartCoroutine(levelPreview.LoadUI(LevelManager.Instance.levelDataList.levelList[index]));
    }

    public void LoadLevelNode(LevelDataList levelCollection)
    {
        for (int i = 0; i < levelCollection.currentLevel; i++)
        {
            if (i >= LevelManager.Instance.levelDataList.levelList.Count) return;
            int levelNumber = levelCollection.levelList[i].levelNumber;
            int starCnt = levelCollection.levelList[i].starCnt;
            levelNodes[i].LoadUI(levelNumber, starCnt);
            levelNodes[i].button.onClick.AddListener(() => LoadLevelPreview(levelNumber - 1));
            levelNodes[i].button.onClick.AddListener(TurnOnPreviewLevel);
        }
    }

    private void TurnOnPreviewLevel()
    {
        levelPreview.gameObject.SetActive(true);
    }

    private void TurnOnLoadingUI(LevelData levelData)
    {
        loadingUI.gameObject.SetActive(true);
    }
}