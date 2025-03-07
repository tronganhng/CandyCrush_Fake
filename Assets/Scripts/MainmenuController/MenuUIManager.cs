using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager: MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private LevelPreview levelPreview;
    [SerializeField] private LevelNode[] levelNodes;


    public void LoadLevelPreview(int index)
    {
        StartCoroutine(levelPreview.LoadUI(levelManager.levelCollection.levelList[index]));
    }

    public void LoadLevelNode(LevelCollection levelCollection)
    {
        for (int i = 0; i < levelNodes.Length; i++)
        {
            int levelNumber = levelCollection.levelList[i].levelNumber;
            int starCnt = levelCollection.levelList[i].starCnt;
            Debug.Log(levelNumber+""+ starCnt);
            levelNodes[i].LoadUI(levelNumber, starCnt);
        }
    }
}