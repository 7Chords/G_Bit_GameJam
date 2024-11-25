using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaderEnemy : BaseEnemy
{
    protected override LogicTile FindBestNextTile()
    {        
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player == null) return null;
        
        if (IsPlayerFar())
        {
            return FindWanderingTile();
        }
        
        LogicTile bestTile = null;
        float farthestDistance = 0;

        foreach (var neighbor in currentStandTile.NeighborLogicTileList)
        {
            float distanceToPlayer = Vector3.Distance(neighbor.transform.position, player.currentStandTile.transform.position);
            if (distanceToPlayer > farthestDistance)
            {
                farthestDistance = distanceToPlayer;
                bestTile = neighbor;
            }
        }

        return bestTile;
    }
}

