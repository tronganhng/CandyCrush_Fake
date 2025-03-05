using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject targetBoard;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private StarBar starBar;
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
}