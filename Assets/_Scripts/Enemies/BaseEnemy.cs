using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    public LogicTile currentStandTile;
    public float alertDistance = 5f;// ����ĳɸ����߼���Ƭ�ж�
    
    public bool isMoving;

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

        if (StealthManager.Instance.IsInvisible)
        {
            // �������ʱ����ƶ�
            LogicTile randomTile = FindWanderingTile();
            if (randomTile != null)
            {
                MoveToTile(randomTile);
            }
        }
        else
        {
            // ��ҿɼ�ʱ�����ض���Ϊ�ƶ�
            LogicTile targetTile = FindBestNextTile();
            if (targetTile != null)
            {
                MoveToTile(targetTile);
            }
        }
    }

    protected virtual LogicTile FindBestNextTile()
    {
        // Ĭ��׷�����
        //PlayerController player = FindObjectOfType<PlayerController>();
        //if (player == null) return null;

        LogicTile bestTile = null;
        float nearestDistance = 9999;

        foreach (var neighbor in currentStandTile.NeighborLogicTileList)
        {
            float distanceToPlayer = Vector3.Distance(neighbor.transform.position, PlayerController.Instance.currentStandTile.transform.position);
            if (distanceToPlayer < nearestDistance)
            {
                nearestDistance = distanceToPlayer;
                bestTile = neighbor;
            }
        }

        return bestTile;
    }
    
    protected virtual void EncounterWithPlayer()
    {
        // ���������
    }
    
    protected virtual LogicTile FindWanderingTile()
    {
        if (currentStandTile == null || currentStandTile.NeighborLogicTileList.Count == 0)
        {
            return null;
        }
        
        int randomIndex = Random.Range(0, currentStandTile.NeighborLogicTileList.Count);
        return currentStandTile.NeighborLogicTileList[randomIndex];
    }  
    
    /// <summary>
    /// ��������ҵľ��룬�ж��Ƿ���뾯��״̬
    /// </summary>
    protected bool IsPlayerFar()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player == null) return true;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        return distanceToPlayer > alertDistance;
    }

    private void MoveToTile(LogicTile targetTile)
    {
        isMoving = true;

        Vector3 endPosition = targetTile.transform.position;
        
        transform.DOMove(endPosition, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
        {
            CompleteTileMove(targetTile);
            
            if (!FindObjectOfType<StealthManager>().IsInvisible && currentStandTile == FindObjectOfType<PlayerController>().currentStandTile)
            {
                EncounterWithPlayer();
            }
        });
    }
    
    private void CompleteTileMove(LogicTile targetTile)
    {
        isMoving = false;

        if (currentStandTile.GetComponent<InvisibleEnterTile>() == null)
            currentStandTile?.GetComponent<IExitTileSpecial>()?.OnExit();

        currentStandTile = targetTile;

        currentStandTile?.GetComponent<IEnterTileSpecial>()?.Apply();
    }


    /// <summary>
    /// �ҵ����˵�ǰ��վ���߼���Ƭ������λ�õ��������ƫ�Start����
    /// </summary>
    private void FindNearestTile()
    {
        float nearestDis = 9999;

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


