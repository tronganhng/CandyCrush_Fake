using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private LayerMask candyLayer;
    [SerializeField] private GameObject bg;
    [SerializeField] private Controller controller;
    [HideInInspector] public Candy currentBomb;
    [HideInInspector] public bool lockRaycast = false;
    public event Action OnTurnComplete;
    private void Update()
    {
        if (controller.candyMoving || lockRaycast) return;
        if (!Input.GetMouseButtonDown(0)) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        
        if (currentBomb == null)
        {
            if (hit.collider == null || 1 << hit.collider.gameObject.layer != candyLayer) return;
            Vector2Int pos = hit.collider.GetComponent<Candy>().matrixPos;
            if (controller.candyGrid[pos.x, pos.y].hitType != HitType.ColorBomb)
            {
                List<(int, int)> cluster = controller.BFS(controller.candyGrid, pos.x, pos.y);
                controller.ScoreBy(cluster, Controller.MATCH_CNT);
                if(cluster.Count >= Controller.MATCH_CNT) 
                {
                    OnTurnComplete?.Invoke();
                    StartCoroutine(controller.DropCandies());
                }
            }
            else
            {
                currentBomb = controller.candyGrid[pos.x, pos.y];
                SelectBomb(pos, true, 2);
            }
        }
        else
        {
            if (hit.collider == null || 1 << hit.collider.gameObject.layer != candyLayer)
            {
                SelectBomb(currentBomb.matrixPos, false, 0);
                currentBomb = null;
                return;
            }
            Vector2Int pos = hit.collider.GetComponent<Candy>().matrixPos;
            Vector2Int bombPos = currentBomb.matrixPos;
            SelectBomb(bombPos, false, 0);
            if ((pos.y == bombPos.y && Mathf.Abs(pos.x - bombPos.x) == 1) || (pos.x == bombPos.x && Mathf.Abs(pos.y - bombPos.y) == 1))
            {
                if (controller.candyGrid[pos.x, pos.y].hitType == HitType.ColorBomb)
                {
                    StartCoroutine(controller.ClearBoad());
                    OnTurnComplete?.Invoke();
                }
                else
                {
                    CandyColor targetColor = controller.candyGrid[pos.x, pos.y].color;
                    currentBomb.explodeController = controller.candyGrid[pos.x, pos.y].explodeController;
                    controller.ColorBomb(currentBomb.matrixPos.x, currentBomb.matrixPos.y, targetColor);
                    StartCoroutine(controller.DropCandies());
                    OnTurnComplete?.Invoke();
                }
            }
            currentBomb = null;
        }
    }

    private void SelectBomb(Vector2Int pos, bool select, int sortOrder)
    {
        currentBomb.GetComponent<Animator>().SetTrigger("select");
        bg.SetActive(select);
        controller.candyGrid[pos.x, pos.y].spriteRenderer.sortingOrder = sortOrder;
        if (pos.x > 0) controller.candyGrid[pos.x - 1, pos.y].spriteRenderer.sortingOrder = sortOrder;
        if (pos.x < CandyCreator.Instance.matrixSize.x - 1) controller.candyGrid[pos.x + 1, pos.y].spriteRenderer.sortingOrder = sortOrder;
        if (pos.y > 0) controller.candyGrid[pos.x, pos.y - 1].spriteRenderer.sortingOrder = sortOrder;
        if (pos.y < CandyCreator.Instance.matrixSize.y - 1) controller.candyGrid[pos.x, pos.y + 1].spriteRenderer.sortingOrder = sortOrder;
    }

    public void SetLockRayCast(bool state)
    {
        lockRaycast = state;
    }
}