using UnityEngine;
using UnityEngine.UI;

public class LevelNode: MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Text levelTxt;
    [SerializeField] private GameObject[] stars;

    public void LoadUI(int levelNumber, int starCnt)
    {
        levelTxt.text = levelNumber.ToString();
        for(int i = 0; i < stars.Length; i++)
        {
            if(i<starCnt)
            stars[i].SetActive(true);
            else
            stars[i].SetActive(false);
        }
    }
}