using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EvaderEnemy : BaseEnemy
{
    protected override LogicTile FindBestNextTile()
    {        
        if (IsPlayerFar(alertDistance))
        {
            return FindWanderingTile();
        }
        
        LogicTile bestTile = null;
        float farthestDistance = 0;

        foreach (var neighbor in currentStandTile.NeighborLogicTileList)
        {
            float distanceToPlayer = Vector3.Distance(neighbor.transform.position, PlayerController.Instance.currentStandTile.transform.position);
            if (distanceToPlayer > farthestDistance)
            {
                farthestDistance = distanceToPlayer;
                bestTile = neighbor;
            }
        }

        return bestTile;
    }

    protected override void EncounterWithPlayer()
    {
        // 保存任务阶段并进入下一场景
        SceneLoader.Instance.LoadScene("Map1 1","");
    }
}

