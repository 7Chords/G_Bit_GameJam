using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TileUpdater: Singleton<TileUpdater>
{
    public Tilemap tilemap;
    public TileBase updatedTile; // Ìæ»»µÄ Tile
    
    public void UpdateTile(Vector3Int position)
    {
        TileBase originalTile = tilemap.GetTile(position);

        if (originalTile != null)
        {
            tilemap.SetTile(position, updatedTile);
            tilemap.RefreshTile(position);
        }
        else
        {
            Debug.LogWarning($"No tile found at position {position}");
        }
    }

    public void UpdateAllTiles()
    {
        BoundsInt bounds = tilemap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                TileBase originalTile = tilemap.GetTile(position);

                if (originalTile != null)
                {
                    tilemap.SetTile(position, updatedTile);
                }
            }
        }

        tilemap.RefreshAllTiles(); 
    }
}
