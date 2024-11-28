using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CatBlockTile : MonoBehaviour,IEnterTileSpecial
{
    private LogicTile _ownerLogicTile;

    public Tilemap tilemap;

    public TileBase brokenTileBase;

    public bool hasBroken;
    private void Awake()
    {
        _ownerLogicTile = GetComponent<LogicTile>();
    }

    private void Start()
    {
        _ownerLogicTile.SetLogicWalkable(false);
    }
    public void Apply()
    {
    }

    public void MakingWalkable()
    {
        hasBroken = true;

        _ownerLogicTile.SetLogicWalkable(true);

        tilemap.SetTile(GetComponent<LogicTile>().CellPosition,brokenTileBase);
    }






}
