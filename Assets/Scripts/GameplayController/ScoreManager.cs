using System;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private TargetStat[] targets;
    private List<UITargetCard> cards = new List<UITargetCard>();

    void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;
    }
    private void Start()
    {
        foreach (TargetStat item in targets)
        {
            GameObject card = Instantiate(cardPrefab, targetBoard.transform);
            card.GetComponent<UITargetCard>().SetInfo(item);
            cards.Add(card.GetComponent<UITargetCard>());
        }
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
}