using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserEnemy : BaseEnemy
{
    [SerializeField] private float teleportDistance; // 超过此距离触发传送
    [SerializeField] private float minRangeAroundPlayer; // 传送位置的最小随机半径
    [SerializeField] private float maxRangeAroundPlayer; // 传送位置的最大随机半径
    
    protected override void OnPlayerMove()
    {
        if(IsPlayerFar(teleportDistance))
            TeleportToPlayer();
        else
            base.OnPlayerMove();
        
    }

    protected override void EncounterWithPlayer()
    {
        if(!StealthManager.Instance.IsInvisible)
            PlayerController.Instance.Dead();
    }

    private void TeleportToPlayer()
    {

            Vector3 playerPosition = PlayerController.Instance.currentStandTile.transform.position;
            
            Vector3 randomOffset = GetRandomOffsetAroundPlayer();
            Vector3 targetPosition = playerPosition + randomOffset;
            
            LogicTile nearestTile = FindNearestTileToPosition(targetPosition);

            if (nearestTile != null)
            {
                transform.position = nearestTile.transform.position;
                currentStandTile = nearestTile;
                
                Instantiate(Resources.Load<GameObject>("Effect/StoneDust"),transform.position, Quaternion.identity);
            }
        
    }
    
    private Vector3 GetRandomOffsetAroundPlayer()
    {
        float randomAngle = Random.Range(0, Mathf.PI * 2);

        // 生成随机半径（在 minRange 和 maxRange 之间）
        float randomRadius = Random.Range(minRangeAroundPlayer, maxRangeAroundPlayer);
        
        float offsetX = Mathf.Cos(randomAngle) * randomRadius;
        float offsetY = Mathf.Sin(randomAngle) * randomRadius;

        return new Vector3(offsetX, offsetY, 0);
    }
    
    private LogicTile FindNearestTileToPosition(Vector3 targetPosition)
    {
        LogicTile nearestTile = null;
        float nearestDistance = 9999;

        foreach (var tile in MapGenerator.Instance.logicTileList)
        {
            float distance = Vector3.Distance(tile.transform.position, targetPosition);
            if (distance < nearestDistance)
            {
                nearestTile = tile;
                nearestDistance = distance;
            }
        }

        return nearestTile;
    }
}


