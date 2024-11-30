using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserEnemy : BaseEnemy
{
    [SerializeField] private float teleportDistance; // �����˾��봥������
    [SerializeField] private float minRangeAroundPlayer; // ����λ�õ���С����뾶
    [SerializeField] private float maxRangeAroundPlayer; // ����λ�õ��������뾶

    protected override LogicTile FindBestNextTile()
    {
        return base.FindBestNextTile();
    }

    protected override void OnPlayerMove()
    {
        if(IsPlayerFar(teleportDistance))
            TeleportToPlayer();
        else
            base.OnPlayerMove();
        
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

                Debug.Log("Enemy teleported to player");
            }
        
    }
    
    private Vector3 GetRandomOffsetAroundPlayer()
    {
        float randomAngle = Random.Range(0, Mathf.PI * 2);

        // ��������뾶���� minRange �� maxRange ֮�䣩
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


