using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class CandyPool : MonoBehaviour
{
    public static CandyPool Instance;
    private List<GameObject> pool;
    [SerializeField] private GameObject candyPrefab;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;
    }

    private void Start()
    {
        Vector2Int matrixSize = CandyCreator.Instance.matrixSize;
        CreatePool(candyPrefab, matrixSize.x * matrixSize.y * 2);
        // set pos to center screen
        Vector2 camPos = Camera.main.transform.position;
        transform.position = camPos - new Vector2(matrixSize.x / 2 - .5f, matrixSize.y / 2 - .5f);
    }

    private void CreatePool(GameObject preFab, int size)
    {
        pool = new List<GameObject>();
        for (int i = 0; i < size; i++)
        {
            GameObject candy = Instantiate(preFab, transform);
            candy.SetActive(false);
            pool.Add(candy);
        }
    }

    public GameObject GetCandy()
    {
        foreach (GameObject candy in pool)
        {
            if (!candy.activeSelf)
            {
                candy.SetActive(true);
                return candy;
            }
        }
        return null;

    }
}
