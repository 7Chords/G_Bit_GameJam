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

            // ��ȡTile����������
            Vector3 worldPosition = tilemap.CellToWorld(position);

            // ����Y�����Z������
            float depth = - worldPosition.y * 10 - 5;

            // ����TilemapRenderer��sortingOrder�����
            tilemapRenderer.sortingOrder = Mathf.RoundToInt(depth);
            Debug.Log("Tile Depth: " + depth);
        }
    }
}