using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelPreview : MonoBehaviour
{
    [SerializeField] private GameObject targetBoard;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject[] star;
    [SerializeField] private Text levelTxt;

    private void OnDisable()
    {
        foreach (Transform child in targetBoard.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < star.Length; i++)
        {
            star[i].SetActive(false);
        }
    }

    public IEnumerator LoadUI(LevelData data)
    {
        levelTxt.text = data.levelNumber.ToString();
        foreach (TargetStat item in data.targets)
        {
            CreateTargetCard(item);
        }
        yield return new WaitForSeconds(.5f);
        for (int i = 0; i < data.starCnt; i++)
        {
            star[i].SetActive(true);
        }
    }

    private void CreateTargetCard(TargetStat sample)
    {
        GameObject card = Instantiate(cardPrefab, targetBoard.transform);
        card.GetComponent<UITargetCard>().SetInfo(sample);
    }
}