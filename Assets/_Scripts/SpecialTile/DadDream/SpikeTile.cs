using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpikeTile : MonoBehaviour, IEnterTileSpecial
{
    public SpikeTile _anotherSpikeTile;

    public TileBase spikeTileBase;

    public TileBase emptyTileBase;

    public Tilemap tilemap;

    private void OnEnable()
    {
        EventManager.OnBeforePlayerMove += ChangeSpikeTile;
    }

    private void OnDisable()
    {
        EventManager.OnBeforePlayerMove -= ChangeSpikeTile;
    }


    public void Apply()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f);

        foreach (var collider in colliders)
        {
            if (collider.GetComponent<PlayerController>() != null)
            {
                if (enabled)
                {
                    PlayerController.Instance.Dead();
                }
            
            }
        }
    }

    private void ChangeSpikeTile()
    {
        _anotherSpikeTile.enabled = true;

        enabled = false;

        tilemap.SetTile(GetComponent<LogicTile>().CellPosition,emptyTileBase);

        tilemap.SetTile(_anotherSpikeTile.GetComponent<LogicTile>().CellPosition, spikeTileBase);
    }
}