using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public static Controller Instance;

    public bool candyMoving;
    private Dictionary<int, int> spawnY = new Dictionary<int, int>();
    public Candy[,] candyGrid;
    [SerializeField] private int candyHitCnt = 0;
    void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;
    }

    void Start()
    {
        candyGrid = CandyCreator.Instance.candyGrid;
    }

    // Detect match
    public void BFS(int startX, int startY)
    {
        int row = CandyCreator.Instance.matrixSize.x;
        int col = CandyCreator.Instance.matrixSize.y;
        CandyColor color = candyGrid[startX, startY].color;

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
                    !visited[newX, newY] && candyGrid[newX, newY] != null)
                {
                    if (candyGrid[newX, newY].color == color)
                    {
                        queue.Enqueue((newX, newY));
                        visited[newX, newY] = true;
                    }
                }
            }
        }

        if (cluster.Count >= 2) // Nếu có ít nhất 3 kẹo cùng loại
        {
            candyHitCnt = cluster.Count;
            foreach (var (x, y) in cluster)
            {
                HitCandy(x, y, color);
            }
            //Delay
            StartCoroutine(DropCandies());
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
        candyHitCnt--;
        candyGrid[x, y].Explode();
        candyGrid[x, y] = null;
        SpawnY(x);
    }
    private IEnumerator HitStripeHor(int x, int y)
    {
        int sizeX = CandyCreator.Instance.matrixSize.x;
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
        int sizeY = CandyCreator.Instance.matrixSize.y;
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
        int sizeX = CandyCreator.Instance.matrixSize.x;
        int sizeY = CandyCreator.Instance.matrixSize.y;
        if (x < 0 || y < 0 || x > sizeX - 1 || y > sizeY - 1)
        {
            return false;
        }
        return true;
    }
    public void ColorBomb(int x, int y, CandyColor color)
    {
        //spawnY.Clear();
        Vector2Int matrixSize = CandyCreator.Instance.matrixSize;
        HitCell(x, y);
        for(int i = 0; i < matrixSize.x; i++)
        {
            for(int j = 0; j < matrixSize.y; j++)
            {
                if(candyGrid[i, j] == null) continue;
                if(candyGrid[i, j].color == color)
                {
                    candyHitCnt++;
                    HitCandy(i, j, color);
                }
            }
        }
    }
    public IEnumerator ClearBoad()
    {
        Vector2Int matrixSize = CandyCreator.Instance.matrixSize;
        for(int x = 0; x < matrixSize.x; x++)
        {
            for(int y = 0; y < matrixSize.y; y++)
            {
                candyHitCnt++;
                HitCell(x, y);
            }
            if(x < matrixSize.x - 1) yield return new WaitForSeconds(.05f);
        }
        StartCoroutine(DropCandies());
    }
    public IEnumerator DropCandies()
    {
        candyMoving = true;
        while (candyHitCnt > 0) yield return null;
        yield return new WaitForSeconds(.3f);
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
        spawnY.Clear();
    }
    private void SpawnY(int x)
    {
        if (spawnY.ContainsKey(x))
        {
            spawnY[x]++;
        }
        else
        {
            spawnY[x] = CandyCreator.Instance.matrixSize.y;
        }
        CandyCreator.Instance.CreateCandy(new Vector2Int(x, spawnY[x]));
    }
}