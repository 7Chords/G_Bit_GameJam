using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaderEnemy : BaseEnemy
{
    protected override LogicTile FindBestNextTile()
    {        
        if (IsPlayerFar())
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
}

