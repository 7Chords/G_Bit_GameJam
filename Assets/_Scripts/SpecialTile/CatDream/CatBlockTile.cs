using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CatBlockTile : MonoBehaviour,IEnterTileSpecial
{
    private LogicTile _ownerLogicTile;

    public Tilemap tilemap;

    public TileBase brokenTileBase;

    public TileBase noBrokenTileBase;

    public bool hasBroken;
    private void Awake()
    {
        _ownerLogicTile = GetComponent<LogicTile>();
    }

    private void OnEnable()
    {
        EventManager.OnPlayerLoadData += OnPlayerLoadData;
    }

    private void OnPlayerLoadData()
    {
        hasBroken = false;

        _ownerLogicTile.SetLogicWalkable(false);

        tilemap.SetTile(GetComponent<LogicTile>().CellPosition,noBrokenTileBase);

    }

    private void OnDisable()
    {
        EventManager.OnPlayerLoadData -= OnPlayerLoadData;
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
        Instantiate(Resources.Load<GameObject>("Effect/StoneDust"),transform.position, Quaternion.identity);
        
        hasBroken = true;

        _ownerLogicTile.SetLogicWalkable(true);

        tilemap.SetTile(GetComponent<LogicTile>().CellPosition,brokenTileBase);
    }






}
