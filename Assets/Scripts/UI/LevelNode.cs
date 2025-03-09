using UnityEngine;
using UnityEngine.UI;

public class LevelNode : MonoBehaviour
{
    [SerializeField] public Button button;
    [SerializeField] private Text levelTxt;
    [SerializeField] private GameObject[] stars;

    public void LoadUI(LevelData data)
    {
        levelTxt.text = data.levelNumber.ToString();
        for (int i = 0; i < stars.Length; i++)
        {
            if (i < data.starCnt)
                stars[i].SetActive(true);
            else
                stars[i].SetActive(false);
        }
    }
}