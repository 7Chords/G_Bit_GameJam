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
        // ��������׶β�������һ����
        SceneLoader.Instance.LoadScene("Map1 1","");
    }
}

