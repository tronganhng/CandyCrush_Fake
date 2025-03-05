using System;
using UnityEngine;

[Serializable]
public struct TargetStat
{
    public CandyColor color;
    public HitType hitType;
    public int amount; // số kẹo phải ăn
}

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [SerializeField] private UIManager uIManager;
    [SerializeField] private int turnLeft;
    [SerializeField] private TargetStat[] targets;
    private int totalCandyToHit = 0;
    private int candyLeftToHit = 0;
    void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;
    }
    private void Start()
    {
        Controller.Instance.OnCandyMatched += UpdateTarget;
        foreach (TargetStat stat in targets)
        {
            uIManager.CreateTargetCard(stat);
            totalCandyToHit += stat.amount;
        }
        candyLeftToHit = totalCandyToHit;
        UpdateTarget();
    }

    private void OnDisable()
    {
        Controller.Instance.OnCandyMatched -= UpdateTarget;
    }

    public void OnCandyHitted(CandyColor color, HitType hitType)
    {
        uIManager.DecreaseTargetAmount(color, hitType);
    }

    private void UpdateTarget()
    {
        candyLeftToHit = 0;
        foreach (UITargetCard item in uIManager.cards)
        {
            candyLeftToHit += item.stat.amount;
        }
        float targetFill = (float)(totalCandyToHit - candyLeftToHit) / totalCandyToHit;
        uIManager.SetStarFillBar(targetFill);
    }
}