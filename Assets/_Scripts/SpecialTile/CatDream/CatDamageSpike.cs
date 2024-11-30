using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CatDamageSpike : MonoBehaviour, IEnterTileSpecial
{
    public CatDamageSpike _anotherSpikeTile;

    public TileBase spikeTileBase;

    public TileBase emptyTileBase;

    public Tilemap tilemap;
    public void Apply()
    {
        if(enabled)
        {
            PlayerController.Instance.Dead();
        }
    }

    private void OnEnable()
    {
        EventManager.OnBeforePlayerMove += ChangeSpikeTile;

        EventManager.OnPlayerLoadData += DisableSelf;
    }

    private void DisableSelf()
    {
        this.enabled = false;
    }

    private void OnDisable()
    {
        EventManager.OnBeforePlayerMove -= ChangeSpikeTile;

        EventManager.OnPlayerLoadData -= DisableSelf;

    }

    private void ChangeSpikeTile()
    {
        _anotherSpikeTile.enabled = true;

        this.enabled = false;

        tilemap.SetTile(GetComponent<LogicTile>().CellPosition,emptyTileBase);

        tilemap.SetTile(_anotherSpikeTile.GetComponent<LogicTile>().CellPosition, spikeTileBase);



    }
}
