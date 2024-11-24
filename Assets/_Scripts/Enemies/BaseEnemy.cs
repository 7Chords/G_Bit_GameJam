using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BaseEnemy : MonoBehaviour
{
    public LogicTile currentStandTile;

    private bool isMoving;

    private void Start()
    {
        FindNearestTile();
        
        EventManager.OnPlayerMove += OnPlayerMove;
    }

    private void OnDestroy()
    {
        EventManager.OnPlayerMove -= OnPlayerMove;
    }

    private void OnPlayerMove()
    {
        if (isMoving) return;

        LogicTile targetTile = FindBestNextTile();

        if (targetTile != null)
        {
            isMoving = true;

            Vector3 endPosition = targetTile.transform.position;

            transform.DOMove(endPosition, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
            {
                currentStandTile = targetTile;
                isMoving = false;

                // 检查是否与玩家重合
                if (currentStandTile == FindObjectOfType<PlayerController>().currentStandTile)
                {
                    Debug.Log("玩家被抓住");
                }
            });
        }
    }

    private LogicTile FindBestNextTile()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player == null) return null;

        LogicTile bestTile = null;
        float nearestDistance = Mathf.Infinity;

        foreach (var neighbor in currentStandTile.NeighborLogicTileList)
        {
            float distanceToPlayer = Vector3.Distance(neighbor.transform.position, player.currentStandTile.transform.position);
            if (distanceToPlayer < nearestDistance)
            {
                nearestDistance = distanceToPlayer;
                bestTile = neighbor;
            }
        }

        return bestTile;
    }

    private void FindNearestTile()
    {
        float nearestDis = Mathf.Infinity;

        foreach (var logicTile in MapGenerator.Instance.logicTileList)
        {
            float currentDis = Vector3.Distance(logicTile.transform.position, transform.position);
            if (currentDis < nearestDis)
            {
                nearestDis = currentDis;
                currentStandTile = logicTile;
            }
        }

        transform.position = currentStandTile.transform.position;
    }
}

