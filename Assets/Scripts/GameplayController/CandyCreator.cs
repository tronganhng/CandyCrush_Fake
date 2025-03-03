using UnityEngine;
public class CandyCreator : MonoBehaviour
{
    public static CandyCreator Instance;

    [SerializeField] CandyOS[] candyOs;
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
        GameObject candyObj = CandyPool.Instance.GetCandy();;
        if (random > 95)
        {
            candyObj.GetComponent<Candy>().SetInfo(candyOs[(int)HitType.StripeVer].candies[Random.Range(0, 5)]);
        }
        else if (random > 90 && random <= 95)
        {
            candyObj.GetComponent<Candy>().SetInfo(candyOs[(int)HitType.StripeHor].candies[Random.Range(0, 5)]);
        }
        else if (random >= 86 && random <= 90)
        {
            candyObj.GetComponent<Candy>().SetInfo(candyOs[(int)HitType.Area].candies[Random.Range(0, 5)]);
        }
        else if (random >= 81 && random < 86)
        {
            candyObj.GetComponent<Candy>().SetInfo(candyOs[(int)HitType.ColorBomb].candies[0]);
        }
        else
        {
            candyObj.GetComponent<Candy>().SetInfo(candyOs[(int)HitType.Normal].candies[Random.Range(0, 5)]);
        }
        candyObj.transform.localPosition = (Vector2)position;
        candyGrid[position.x, position.y] = candyObj.GetComponent<Candy>();
        candyGrid[position.x, position.y].matrixPos = new Vector2Int(position.x, position.y);
    }
}
