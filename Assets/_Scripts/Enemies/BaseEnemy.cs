using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public LogicTile currentStandTile;

    private bool isMoving; // �Ƿ������ƶ�
    private StealthManager stealthManager;

    private void Start()
    {
        FindNearestTile();

        stealthManager = FindObjectOfType<StealthManager>();
        if (stealthManager == null)
        {
            Debug.LogError("StealthManagerδ�ҵ�");
        }

        EventManager.OnPlayerMove += OnPlayerMove;
    }

    private void OnDestroy()
    {
        EventManager.OnPlayerMove -= OnPlayerMove;
    }

    private void OnPlayerMove()
    {
        if (isMoving) return;

        if (stealthManager != null && stealthManager.IsInvisible)
        {
            // �������ʱ����ƶ�
            LogicTile randomTile = GetRandomNeighborTile();
            if (randomTile != null)
            {
                MoveToTile(randomTile);
            }
        }
        else
        {
            // ��ҿɼ�ʱ׷�����
            LogicTile targetTile = FindBestNextTile();
            if (targetTile != null)
            {
                MoveToTile(targetTile);
            }
        }
    }

    private LogicTile GetRandomNeighborTile()
    {
        if (currentStandTile == null || currentStandTile.NeighborLogicTileList.Count == 0)
        {
            return null;
        }
        
        int randomIndex = Random.Range(0, currentStandTile.NeighborLogicTileList.Count);
        return currentStandTile.NeighborLogicTileList[randomIndex];
    }

    private void MoveToTile(LogicTile targetTile)
    {
        isMoving = true;

        Vector3 endPosition = targetTile.transform.position;
        
        transform.DOMove(endPosition, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
        {
            currentStandTile = targetTile;
            isMoving = false;
            
            if (!stealthManager.IsInvisible && currentStandTile == FindObjectOfType<PlayerController>().currentStandTile)
            {
                Debug.Log("��ұ�ץס");
            }
        });
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


