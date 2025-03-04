using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[Serializable]
public struct TargetStat
{
    public CandyColor color;
    public HitType hitType;
    public int amount;
}

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [SerializeField] private GameObject targetBoard;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private StarBar starBar;
    [SerializeField] private TargetStat[] targets;
    private List<UITargetCard> cards = new List<UITargetCard>();
    private int totalCandyToHit = 0;
    private int candyLeftToHit = 0;
    void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;
    }
    private void Start()
    {
        Controller.Instance.OnCandyMatched += UpdateFillBar;
        foreach (TargetStat item in targets)
        {
            GameObject card = Instantiate(cardPrefab, targetBoard.transform);
            card.GetComponent<UITargetCard>().SetInfo(item);
            cards.Add(card.GetComponent<UITargetCard>());
            totalCandyToHit += item.amount;
        }
        candyLeftToHit = totalCandyToHit;
        UpdateFillBar();
    }

    private void OnDisable()
    {
        Controller.Instance.OnCandyMatched -= UpdateFillBar;
    }

    public void OnCandyHitted(CandyColor color, HitType hitType)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].stat.color == color && cards[i].stat.hitType == hitType)
            {
                cards[i].SetAmount(1);
            }
        }
    }

    private void UpdateFillBar()
    {
        candyLeftToHit = 0;
        foreach (UITargetCard item in cards)
        {
            candyLeftToHit += item.stat.amount;
        }
        float targetFill = (float)(totalCandyToHit - candyLeftToHit) / totalCandyToHit;
        starBar.fillBar.DOFillAmount(targetFill, 0.6f).SetEase(Ease.OutQuad).OnComplete(() => starBar.CheckFill());
    }
}