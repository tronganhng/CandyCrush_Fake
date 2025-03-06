using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public static Controller Instance;

    public event Action OnCandyMatched, OnWaveEnd;
    [HideInInspector] public const int MATCH_CNT = 3; // số kẹo tối thiểu khi match
    public bool candyMoving;
    public Candy[,] candyGrid;
    private Vector2Int matrixSize;
    private int candyHitCnt = 0;
    void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;
    }

    void Start()
    {
        candyGrid = CandyCreator.Instance.candyGrid;
        matrixSize = CandyCreator.Instance.matrixSize;
        OnCandyMatched += SpawnCandies;
    }

    // Detect match
    public List<(int, int)> BFS(Candy[,] matrix, int startX, int startY)
    {
        int row = matrixSize.x;
        int col = matrixSize.y;
        CandyColor color = matrix[startX, startY].color;
        if (color == CandyColor.RainBow) return null;

        Queue<(int, int)> queue = new Queue<(int, int)>();
        List<(int, int)> cluster = new List<(int, int)>(); // các ô đã đi qua quêu
        bool[,] visited = new bool[row, col];

        queue.Enqueue((startX, startY));
        visited[startX, startY] = true;

        int[] dx = { 0, 0, -1, 1 }; // Duyệt dưới, trên, trái, phải
        int[] dy = { -1, 1, 0, 0 };

        while (queue.Count > 0)
        {
            var (x, y) = queue.Dequeue();
            cluster.Add((x, y));

            for (int d = 0; d < 4; d++)
            {
                int newX = x + dx[d];
                int newY = y + dy[d];

                if (newX >= 0 && newX < row && newY >= 0 && newY < col &&
                    !visited[newX, newY] && matrix[newX, newY] != null)
                {
                    if (matrix[newX, newY].color == color)
                    {
                        queue.Enqueue((newX, newY));
                        visited[newX, newY] = true;
                    }
                }
            }
        }

        return cluster;
    }
    public void ScoreBy(List<(int, int)> cluster, int matchCnt)
    {
        if (cluster == null) return;
        CandyColor targetColor = candyGrid[cluster[0].Item1, cluster[0].Item2].color;
        if (cluster.Count >= matchCnt) // Nếu có ít nhất 3 kẹo cùng loại
        {
            candyHitCnt += cluster.Count;
            foreach (var (x, y) in cluster)
            {
                HitCandy(x, y, targetColor);
            }
            // spawn skill candy
            Vector2Int spawnPos = new Vector2Int(cluster[0].Item1, cluster[0].Item2);
            switch (cluster.Count)
            {
                case 3: break;
                case 4:
                    if(UnityEngine.Random.Range(0,2) == 0) CandyCreator.Instance.CreateCandyBy(spawnPos, targetColor, HitType.StripeHor);
                    else CandyCreator.Instance.CreateCandyBy(spawnPos, targetColor, HitType.StripeVer);
                    break;
                case 5:
                    CandyCreator.Instance.CreateCandyBy(spawnPos, targetColor, HitType.Area);
                    break;
                default:
                    CandyCreator.Instance.CreateCandyBy(spawnPos, CandyColor.RainBow, HitType.ColorBomb);
                    break;
            }
            // StartCoroutine(DropCandies());
        }
    }
    private void HitCandy(int x, int y, CandyColor color)
    {
        if (candyGrid[x, y] == null)
        {
            candyHitCnt--;
            return;
        }
        HitType hitType = candyGrid[x, y].hitType;
        switch (hitType)
        {
            case HitType.Normal:
                HitCell(x, y);
                break;
            case HitType.StripeHor:
                StartCoroutine(HitStripeHor(x, y));
                break;
            case HitType.StripeVer:
                StartCoroutine(HitStripeVer(x, y));
                break;
            case HitType.Area:
                HitArea(x, y);
                break;
            case HitType.ColorBomb:
                ColorBomb(x, y, color);
                break;
        }
    }
    private void HitCell(int x, int y)
    {
        ScoreManager.Instance.OnCandyHitted(candyGrid[x, y].color, candyGrid[x, y].hitType);
        candyHitCnt--;
        candyGrid[x, y].Explode();
        candyGrid[x, y] = null;
    }
    private IEnumerator HitStripeHor(int x, int y)
    {
        int sizeX = matrixSize.x;
        candyHitCnt += sizeX - 1;
        CandyColor color = candyGrid[x, y].color;
        HitCell(x, y);
        int i = x - 1, j = x + 1;
        while (true)
        {
            yield return new WaitForSeconds(.05f);
            if (i >= 0) HitCandy(i, y, color);
            if (j < sizeX) HitCandy(j, y, color);
            i--;
            j++;
            if (i < 0 && j >= sizeX)
            {
                break;
            }
        }
    }
    private IEnumerator HitStripeVer(int x, int y)
    {
        int sizeY = matrixSize.y;
        candyHitCnt += sizeY - 1;
        CandyColor color = candyGrid[x, y].color;
        HitCell(x, y);
        int i = y - 1, j = y + 1;
        while (true)
        {
            yield return new WaitForSeconds(.05f);
            if (i >= 0) HitCandy(x, i, color);
            if (j < sizeY) HitCandy(x, j, color);
            i--;
            j++;
            if (i < 0 && j >= sizeY)
            {
                break;
            }
        }
    }
    private void HitArea(int x, int y)
    {
        CandyColor color = candyGrid[x, y].color;
        HitCell(x, y);
        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if (ValidatePos(i, j) && (i != x || j != y))
                {
                    candyHitCnt++;
                    HitCandy(i, j, color);
                }
            }
        }
    }
    private bool ValidatePos(int x, int y)
    {
        if (x < 0 || y < 0 || x > matrixSize.x - 1 || y > matrixSize.y - 1)
        {
            return false;
        }
        return true;
    }
    public void ColorBomb(int x, int y, CandyColor color)
    {
        HitCell(x, y);
        for (int i = 0; i < matrixSize.x; i++)
        {
            for (int j = 0; j < matrixSize.y; j++)
            {
                if (candyGrid[i, j] == null) continue;
                if (candyGrid[i, j].color == color)
                {
                    candyHitCnt++;
                    HitCandy(i, j, color);
                }
            }
        }
    }
    public IEnumerator ClearBoad()
    {
        for (int x = 0; x < matrixSize.x; x++)
        {
            for (int y = 0; y < matrixSize.y; y++)
            {
                candyHitCnt++;
                HitCell(x, y);
            }
            if (x < matrixSize.x - 1) yield return new WaitForSeconds(.05f);
        }
        StartCoroutine(DropCandies());
    }
    public IEnumerator DropCandies()
    {
        candyMoving = true;
        while (candyHitCnt > 0) yield return null;
        yield return new WaitForSeconds(.3f);
        OnCandyMatched?.Invoke();
        candyHitCnt = 0;
        for (int x = 0; x < candyGrid.GetLength(0); x++)
        {
            for (int y = 1; y < candyGrid.GetLength(1); y++)
            {
                if (candyGrid[x, y] == null || candyGrid[x, y - 1] != null) continue;
                int nullCount = 0;
                for (int j = 0; j < y; j++)
                {
                    if (candyGrid[x, j] == null)
                    {
                        nullCount++;
                    }
                }
                candyGrid[x, y].Move(new Vector2Int(x, y - nullCount));
                candyGrid[x, y - nullCount] = candyGrid[x, y];
                candyGrid[x, y].matrixPos = new Vector2Int(x, y - nullCount);
                candyGrid[x, y] = null;
            }
        }
        while (candyMoving) yield return null;
        if(!CanCombo())
        {
            CandyCreator.Instance.RefreshGrid();
            OnWaveEnd?.Invoke();
        }
        else 
        {
            MakeCombo();
            StartCoroutine(DropCandies());
        }
    }
    private void SpawnCandies()
    {
        Dictionary<int, int> spawnY = new Dictionary<int, int>();
        int nullCnt = 0;
        for (int x = 0; x < matrixSize.x; x++)
        {
            for (int y = 0; y < matrixSize.y; y++)
            {
                if (candyGrid[x, y] == null)
                {
                    nullCnt++;
                    if (spawnY.ContainsKey(x))
                    {
                        spawnY[x]++;
                    }
                    else
                    {
                        spawnY[x] = matrixSize.y;
                    }
                    CandyCreator.Instance.CreateCandyByJson(new Vector2Int(x, spawnY[x]));
                }
            }
        }
        ScoreManager.Instance.IncreasePoint(nullCnt);
    }
    private void MakeCombo()
    {
        candyHitCnt += 1;
        List<(int, int)> cluster;
        for (int x = 0; x < matrixSize.x; x++)
        {
            for (int y = 0; y < matrixSize.y; y++)
            {
                if (candyGrid[x, y] == null) continue;
                cluster = BFS(candyGrid, x, y);
                if(cluster == null) continue;
                ScoreBy(cluster, MATCH_CNT + 1);
            }
        }
        candyHitCnt -= 1;
    }
    private bool CanCombo()
    {
        bool canCombo = false;
        List<(int, int)> cluster;
        for (int x = 0; x < matrixSize.x; x++)
        {
            for (int y = 0; y < matrixSize.y; y++)
            {
                cluster = BFS(candyGrid, x, y);
                if(cluster == null) continue;
                if(cluster.Count >= MATCH_CNT + 1) canCombo = true;
            }
        }
        return canCombo;
    }
}