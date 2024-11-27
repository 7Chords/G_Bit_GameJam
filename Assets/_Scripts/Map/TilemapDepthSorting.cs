using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(TilemapRenderer))]
public class TilemapDepthByZ : MonoBehaviour
{
    private TilemapRenderer tilemapRenderer;
    private Tilemap tilemap;

    void Start()
    {
        tilemapRenderer = GetComponent<TilemapRenderer>();
        tilemap = GetComponent<Tilemap>();
        AdjustTileDepth();
    }

    private void AdjustTileDepth()
    {
        BoundsInt bounds = tilemap.cellBounds;
        foreach (Vector3Int position in bounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(position)) continue;

            // 获取Tile的世界坐标
            Vector3 worldPosition = tilemap.CellToWorld(position);

            // 根据Y轴调整Z轴的深度
            float depth = - worldPosition.y * 10 - 5;

            // 更新TilemapRenderer的sortingOrder或深度
            tilemapRenderer.sortingOrder = Mathf.RoundToInt(depth);
            Debug.Log("Tile Depth: " + depth);
        }
    }
}