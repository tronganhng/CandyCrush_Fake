using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class EffecPool : MonoBehaviour
{
    public static EffecPool Instance;
    [SerializeField] private GameObject explodePrefab;
    private int size;
    private List<GameObject> pool;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;
    }

    private void Start()
    {
        size = CandyCreator.Instance.matrixSize.x * CandyCreator.Instance.matrixSize.y;
        CreatePool(explodePrefab, size);
    }

    private void CreatePool(GameObject preFab, int size)
    {
        pool = new List<GameObject>();
        for (int i = 0; i < size; i++)
        {
            GameObject eff = Instantiate(preFab, transform);
            eff.SetActive(false);
            pool.Add(eff);
        }
    }

    public GameObject GetExplodeEffAt(Vector3 position)
    {
        foreach (GameObject eff in pool)
        {
            if (!eff.activeSelf)
            {
                eff.SetActive(true);
                eff.transform.position = position;
                return eff;
            }
        }
        return null;
    }
}
