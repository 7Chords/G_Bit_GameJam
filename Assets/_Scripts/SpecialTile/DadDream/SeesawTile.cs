using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    up,
    down,
    left,
    right
}

public class SeesawTile : MonoBehaviour, IEnterTileSpecial
{
    public SeesawTile anotherSeesawTile;
    public Direction teleportDirection;

    public void Apply()
    {
        if (anotherSeesawTile == null)
        {
            Debug.LogError($"{name} 没有设置另一块SeesawTile");
            return;
        }

        // 检测当前地块上是否有敌人
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f);
        foreach (var collider in colliders)
        {
            var enemy = collider.GetComponent<BaseEnemy>();
            if (enemy != null)
            {
                TeleportEnemy(enemy);
            }
        }
    }

    private void TeleportEnemy(BaseEnemy enemy)
    {
        if (anotherSeesawTile == null) return;
        
        LogicTile anotherTile = anotherSeesawTile.GetComponent<LogicTile>();
        if (anotherTile == null)
        {
            return;
        }

        LogicTile targetTile = GetDirectionalNeighbor(anotherTile);
        if (targetTile == null)
        {
            Debug.LogError($"{anotherTile.name} 在方向 {teleportDirection} 上没有临近地块");
            return;
        }
        
        enemy.transform.position = targetTile.transform.position;
        
        enemy.currentStandTile = targetTile;
    }

    private LogicTile GetDirectionalNeighbor(LogicTile tile)
    {
        switch (teleportDirection)
        {
            case Direction.up:
                return anotherSeesawTile.GetComponent<LogicTile>().NeighborLogicTileList[3];
            case Direction.down:
                return anotherSeesawTile.GetComponent<LogicTile>().NeighborLogicTileList[0];
            case Direction.left:
                return anotherSeesawTile.GetComponent<LogicTile>().NeighborLogicTileList[2];
            case Direction.right:
                return anotherSeesawTile.GetComponent<LogicTile>().NeighborLogicTileList[1];
            default:
                return null;
        }
    }
}
