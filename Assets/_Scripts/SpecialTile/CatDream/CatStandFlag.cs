using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CatStandFlag : MonoBehaviour,IEnterTileSpecial
{

    public CatBlockTile blockTile;

    public bool hasStanded;

    public List<CatStandFlag> connectStandTiles;

    public TileBase standedTileBase;

    public Tilemap tileMap;
    private void OnEnable()
    {
        EventManager.OnPlayerLoadData += OnPlayerLoadData;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerLoadData -= OnPlayerLoadData;
    }
    public void Apply()
    {
        if(!hasStanded)
        {
            AudioManager.Instance.PlaySfx("OccupiedFlag");

            hasStanded = true;

            Instantiate(Resources.Load<GameObject>("Effect/SparkYellow"), transform.position, Quaternion.identity);

            tileMap.SetTile(GetComponent<LogicTile>().CellPosition, standedTileBase);

            if (CheckAllTileStandState())
            {
                blockTile.MakingWalkable();
            }
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

    private void OnPlayerLoadData()
    {
        if(!blockTile.hasBroken)
        {
            hasStanded = false;
        }
    }
}
