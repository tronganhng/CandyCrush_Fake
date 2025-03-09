using System.Collections.Generic;
using UnityEngine;

public class MatrixCollection
{
    public List<JsonMatrix> matrixList;
}

public class JsonMatrix
{
    public int levelNumber;
    public List<List<int[]>> pairMatrix; // ma trận chứa các cặp (color, hitType)

    public void InvertMatrix() // đảo ngược trục y ma trận
    {
        List<List<int[]>> clone = new List<List<int[]>>();
        for (int y = pairMatrix.Count - 1; y >= 0; y--)
        {
            clone.Add(pairMatrix[y]);
        }
        pairMatrix = clone;
    }
}