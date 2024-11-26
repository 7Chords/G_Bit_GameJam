using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatStandFlag : MonoBehaviour,IEnterTileSpecial
{

    public CatBlockTile blockTile;

    public bool hasStanded;

    public List<CatStandFlag> connectStandTiles;
    public void Apply()
    {
        hasStanded = true;

        if(CheckAllTileStandState())
        {
            blockTile.MakingWalkable();
        }
    }

    private bool CheckAllTileStandState()
    {
        foreach (var tile in connectStandTiles)
        {
            if(!tile.hasStanded)
            {
                return false;
            }
        }
        return true;
    }
}
