using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;

public class GameplayUIManager : MonoBehaviour
{
    public GameObject winBoard, loseBoard;
    public StarBar starBar;
    [SerializeField] private GameObject[] stars = new GameObject[3];
    [SerializeField] private GameObject targetBoard;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Text turnTxt;
    [HideInInspector] public List<UITargetCard> cards = new List<UITargetCard>();

    private void Start()
    {
        Controller.Instance.OnCandyMatched += SetTargetsText;
    }
    private void OnDisable()
    {
        Controller.Instance.OnCandyMatched -= SetTargetsText;
    }

    public void SetStarFillBar(float targetFill)
    {
        starBar.fillBar.DOFillAmount(targetFill, 0.6f).SetEase(Ease.OutQuad).OnComplete(() => starBar.CheckFill());
    }

    public void DecreaseTargetAmount(CandyColor color, HitType hitType)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].stat.color == color && cards[i].stat.hitType == hitType)
            {
                cards[i].DecreaseAmount(1);
            }
        }
    }

    private void SetTargetsText()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].UpdateText();
        }
    }

    public void CreateTargetCard(TargetStat sample)
    {
        GameObject card = Instantiate(cardPrefab, targetBoard.transform);
        card.GetComponent<UITargetCard>().SetInfo(sample);
        cards.Add(card.GetComponent<UITargetCard>());
    }

    public void SetTurnText(int turnLeft)
    {
        turnTxt.text = turnLeft.ToString();
    }

    public IEnumerator WinLevelShow()
    {
        yield return new WaitForSeconds(.7f);
        winBoard.SetActive(true);
        yield return new WaitForSeconds(.5f);
        int totalStars = starBar.GetActiveStar();
        for (int i = 0; i < totalStars; i++)
        {
            yield return new WaitForSeconds(.3f);
            stars[i].SetActive(true);
        }
    }

}