using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]

public class TileAssets
{
    public Direction direction;
    public List<TileBase> tileBasesArrays; // ͼ���б�
}

public class TileUpdater : Singleton<TileUpdater>
{
    public Tilemap tilemap;
    public List<TileAssets> TileAssetsArray; // �������ͼ���б������

    /// <summary>
    /// ��ָ��λ����ָ�������𲽸���ͼ��
    /// </summary>
    /// <param name="startPosition">��ʼλ��</param>
    /// <param name="direction">���·���</param>
    public void UpdateTilesInDirection(LogicTile currentTile, Direction direction)
    {
        if (TileAssetsArray == null || TileAssetsArray.Count == 0)
        {
            return;
        }

        BoundsInt bounds = tilemap.cellBounds;
        Vector3Int currentPosition = currentTile.CellPosition;
        
        foreach (var tileAsset in TileAssetsArray)
        {
            if (tileAsset.direction == direction)
            {
                foreach (var tileBase in tileAsset.tileBasesArrays)
                {
                    if (!bounds.Contains(currentPosition))
                    {
                        return;
                    }
                                
                    TileBase originalTile = tilemap.GetTile(currentPosition); 
                    if (originalTile != null)
                    {
                        tilemap.SetTile(currentPosition, tileBase);
                    }
                    
                    currentTile = GetDirectionalNeighbor(currentTile, direction);
                    currentPosition = currentTile.CellPosition;
                }
            }

        }
        
        tilemap.RefreshAllTiles();
    }
    
    private LogicTile GetDirectionalNeighbor(LogicTile targetTile,Direction direction)
    {
        switch (direction)
        {
            case Direction.down:
                return targetTile.NeighborLogicTileList[3];
            case Direction.up:
                return targetTile.NeighborLogicTileList[0];
            case Direction.right:
                return targetTile.NeighborLogicTileList[2];
            case Direction.left:
                return targetTile.NeighborLogicTileList[1];
            default:
                return null;
        }
    }
}

