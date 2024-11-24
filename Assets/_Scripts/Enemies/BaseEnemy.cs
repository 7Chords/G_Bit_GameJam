using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public LogicTile currentStandTile;

    private bool isMoving; // 是否正在移动
    private StealthManager stealthManager;

    private void Start()
    {
        FindNearestTile();

        stealthManager = FindObjectOfType<StealthManager>();
        if (stealthManager == null)
        {
            Debug.LogError("StealthManager 未找到！");
        }

        EventManager.OnPlayerMove += OnPlayerMove;
    }

    private void OnDestroy()
    {
        EventManager.OnPlayerMove -= OnPlayerMove;
    }

    private void OnPlayerMove()
    {
        if (isMoving || (stealthManager != null && stealthManager.IsInvisible)) return;

        LogicTile targetTile = FindBestNextTile();

        if (targetTile != null)
        {
            isMoving = true;

            Vector3 endPosition = targetTile.transform.position;

            transform.DOMove(endPosition, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
            {
                currentStandTile = targetTile;
                isMoving = false;

                if (currentStandTile == FindObjectOfType<PlayerController>().currentStandTile)
                {
                    Debug.Log("玩家被抓住！游戏结束！");
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

