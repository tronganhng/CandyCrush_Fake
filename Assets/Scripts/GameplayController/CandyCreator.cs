using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class CandyCreator : MonoBehaviour
{
    public static CandyCreator Instance;

    [SerializeField] private CandyOS[] candyOs;
    public Vector2Int matrixSize;
    public Candy[,] candyGrid;
    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
    }

    void Start()
    {
        candyGrid = new Candy[matrixSize.x, matrixSize.y * 2];
        SpawnCandy();
    }

    void SpawnCandy()
    {
        for (int i = 0; i < matrixSize.x; i++)
        {
            for (int j = 0; j < matrixSize.y; j++)
            {
                GameObject candy = CandyPool.Instance.GetCandy();
                candy.GetComponent<Candy>().SetInfo(candyOs[(int)HitType.Normal].candies[Random.Range(0, 5)]);
                candy.transform.localPosition = new Vector2(i, j);
                candyGrid[i, j] = candy.GetComponent<Candy>();
                candyGrid[i, j].matrixPos = new Vector2Int(i, j);
            }
        }
    }

    public void CreateCandy(Vector2Int position)
    {
        int random = Random.Range(0, 100);
        GameObject candyObj = CandyPool.Instance.GetCandy();
        CandyDataOS data;
        if (random > 95)
        {
            data = candyOs[(int)HitType.StripeVer].candies[Random.Range(0, 5)];
            candyObj.GetComponent<Candy>().SetInfo(data);
        }
        else if (random > 90 && random <= 95)
        {
            data = candyOs[(int)HitType.StripeHor].candies[Random.Range(0, 5)];
            candyObj.GetComponent<Candy>().SetInfo(data);
        }
        else if (random >= 86 && random <= 90)
        {
            data = candyOs[(int)HitType.Area].candies[Random.Range(0, 5)];
            candyObj.GetComponent<Candy>().SetInfo(data);
        }
        else if (random >= 81 && random < 86)
        {
            data = candyOs[(int)HitType.ColorBomb].candies[0];
            candyObj.GetComponent<Candy>().SetInfo(data);
        }
        else
        {
            data = candyOs[(int)HitType.Normal].candies[Random.Range(0, 5)];
            candyObj.GetComponent<Candy>().SetInfo(data);
        }
        candyObj.transform.localPosition = (Vector2)position;
        candyGrid[position.x, position.y] = candyObj.GetComponent<Candy>();
        candyGrid[position.x, position.y].matrixPos = new Vector2Int(position.x, position.y);
    }

    public void RefreshGrid()
    {
        Candy[,] testGrid = new Candy[matrixSize.x, matrixSize.y];
        List<Candy> candies = new List<Candy>();
        // Check matchable
        if (Matchable(candyGrid)) return;
        // Add all grid item to list
        for (int x = 0; x < matrixSize.x; x++)
        {
            for (int y = 0; y < matrixSize.y; y++)
            {
                candies.Add(candyGrid[x, y]);
            }
        }
        if (candies.Count != matrixSize.x * matrixSize.y) { Debug.LogError("Clone Grid Fail!"); return; }

        // check match able and apply on real grid
        bool matchable = false;
        while (!matchable)
        {
            // Reset candy on testGrid
            RandomGridReset(testGrid, candies);

            matchable = Matchable(testGrid);
        }
        SetGridEqualTo(testGrid);
    }
    private void RandomGridReset(Candy[,] testGrid, List<Candy> candies)
    {
        System.Random random = new System.Random();
        for (int x = 0; x < matrixSize.x; x++)
        {
            for (int y = 0; y < matrixSize.y; y++)
            {
                int index = random.Next(candies.Count);
                testGrid[x, y] = candies[index];
                testGrid[x, y].matrixPos = new Vector2Int(x, y);
                candies.RemoveAt(index);
            }
        }
    }
    private void SetGridEqualTo(Candy[,] testGrid)
    {
        for (int x = 0; x < matrixSize.x; x++)
        {
            for (int y = 0; y < matrixSize.y; y++)
            {
                candyGrid[x, y] = testGrid[x, y];
                candyGrid[x, y].transform.localPosition = (Vector2)testGrid[x, y].matrixPos;
                candyGrid[x, y].gameObject.SetActive(false);
                candyGrid[x, y].gameObject.SetActive(true);
            }
        }
    }
    private bool Matchable(Candy[,] matrix)
    {
        List<(int, int)> cluster;
        for (int x = 0; x < matrixSize.x; x++)
        {
            for (int y = 0; y < matrixSize.y; y++)
            {
                if (matrix[x, y].color == CandyColor.RainBow) return true;
                cluster = Controller.Instance.BFS(matrix, x, y);
                if (cluster == null) continue;
                if (cluster.Count >= Controller.Instance.matchCnt)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
