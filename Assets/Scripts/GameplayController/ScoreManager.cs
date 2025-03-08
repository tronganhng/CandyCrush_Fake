using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [SerializeField] private GameplayUIManager uIManager;
    [SerializeField] private InputController inputController;
    [SerializeField] private int targetPoint;
    [SerializeField] private int turnLeft;
    [SerializeField] private TargetStat[] targets;
    private int totalCandyToHit = 0;
    private int candyLeftToHit = 0;
    private int currentPoint = 0;
    void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;
    }
    private void Start()
    {
        Controller.Instance.OnCandyMatched += UpdateTarget;
        inputController.OnTurnComplete += DecreaseTurn;
        Controller.Instance.OnWaveEnd += CheckWinLevel;
        if(LevelManager.Instance != null)
        {
            targets = LevelManager.Instance.playingLevel.targets;
            turnLeft = LevelManager.Instance.playingLevel.totalTurn;
        }
        foreach (TargetStat stat in targets)
        {
            uIManager.CreateTargetCard(stat);
            totalCandyToHit += stat.amount;
        }
        candyLeftToHit = totalCandyToHit;
        uIManager.SetTurnText(turnLeft);
        UpdateTarget();
        uIManager.SetStarFillBar(0);
    }

    private void OnDisable()
    {
        Controller.Instance.OnCandyMatched -= UpdateTarget;
        inputController.OnTurnComplete -= DecreaseTurn;
        Controller.Instance.OnWaveEnd -= CheckWinLevel;
    }

    public void OnCandyHitted(CandyColor color, HitType hitType)
    {
        uIManager.DecreaseTargetAmount(color, hitType);
    }

    public void IncreasePoint(int amount)
    {
        currentPoint += amount;
        uIManager.SetStarFillBar((float)currentPoint / targetPoint);
    }

    private void UpdateTarget()
    {
        candyLeftToHit = 0;
        foreach (UITargetCard item in uIManager.cards)
        {
            candyLeftToHit += item.stat.amount;
        }
        float targetFill = (float)(totalCandyToHit - candyLeftToHit) / totalCandyToHit;
    }

    private void DecreaseTurn()
    {
        turnLeft--;
        if (turnLeft < 0) turnLeft = 0;
        uIManager.SetTurnText(turnLeft);
    }

    private void CheckWinLevel()
    {
        if (turnLeft > 0)
        {
            if (candyLeftToHit == 0 && uIManager.starBar.GetActiveStar() == 3)
            {
                inputController.SetLockRayCast(true);
                StartCoroutine(uIManager.WinLevelShow());
            }
        }
        else
        {
            inputController.SetLockRayCast(true);
            if (candyLeftToHit > 0)
            {
                uIManager.loseBoard.SetActive(true);
            }
            else
            {
                if (uIManager.starBar.GetActiveStar() == 0) uIManager.loseBoard.SetActive(true);
                else StartCoroutine(uIManager.WinLevelShow());
            }
        }
    }
}