using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    private LogicTile flagStandTile;
    
    public LogicTile currentStandTile;
    public float alertDistance = 5f;
    public bool isMoving;
    
    // 动画处理
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        FindNearestTile();
         
        flagStandTile = currentStandTile;

        EventManager.OnPlayerMove += OnPlayerMove;
    }

    private void OnDestroy()
    {
        EventManager.OnPlayerMove -= OnPlayerMove;
    }

    protected virtual void OnPlayerMove()
    {
        if (isMoving) return;
        
                    
        if (!StealthManager.Instance.IsInvisible && currentStandTile == PlayerController.Instance.currentStandTile)
        {
            EncounterWithPlayer();
            Debug.Log("encounter with enemy");
        }

        if (StealthManager.Instance.IsInvisible)
        {
            // 玩家隐形时随机移动
            LogicTile randomTile = FindWanderingTile();
            if (randomTile != null && randomTile.LogicWalkable)
            {
                MoveToTile(randomTile);
            }
        }
        else
        {
            // 玩家可见时根据特定行为移动
            LogicTile targetTile = FindBestNextTile();
            if (targetTile != null && targetTile.LogicWalkable)
            {
                MoveToTile(targetTile);
            }
        }
    }

    protected virtual LogicTile FindBestNextTile()
    {
        // 默认追踪玩家
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
        // 与玩家相遇
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
    /// 计算与玩家的距离，判断是否进入警戒状态
    /// </summary>
    protected virtual bool IsPlayerFar(float distance)
    {
        float distanceToPlayer = Vector3.Distance(transform.position, PlayerController.Instance.transform.position);
        return distanceToPlayer > distance;
    }

    private void MoveToTile(LogicTile targetTile)
    {
        isMoving = true;

        Vector3 endPosition = targetTile.transform.position;
        
        ChangeAnim(endPosition.y);
        FlipCharacter(endPosition.x);
        
        transform.DOMove(endPosition, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
        {
            CompleteTileMove(targetTile);
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
    /// 找到敌人当前所站的逻辑瓦片，并令位置到那里，消除偏差，Start调用
    /// </summary>
    protected void FindNearestTile()
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
    
    private void FlipCharacter(float targetX)
    {
        if (targetX > transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else if (targetX < transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
    }
    
    private void ChangeAnim(float targetY)
    {
        if (targetY > transform.position.y)
        {
            animator.SetBool("IsDown",false);
        }
        else if (targetY < transform.position.y)
        {
            animator.SetBool("IsDown",true);
        }
    }

    public void SetEnemyToFlag()
    {
        transform.position = flagStandTile.transform.position;
        currentStandTile = flagStandTile;
    }
}


