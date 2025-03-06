using System.Collections.Generic;
using UnityEngine;

public class JsonMatrix
{
    public int[,] matrix;

    public int[,] ConvertArr(int width, int height)
    {
        if (matrix == null) Debug.Log("null");

        int[,] result = new int[width, height];

        for (int y = 0; y < height; y++)
        {

            for (int x = 0; x < width; x++)
            {
                result[x, y] = matrix[height - y - 1,x];
            }
        }
        return result;
    }
}