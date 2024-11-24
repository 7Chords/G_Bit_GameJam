using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[System.Serializable]


public class TileData
{
    public Vector3Int position; // ��Ƭ������λ��
    public string tileName;      // ��Ƭ������
    public Vector3 worldPosition; // ��Ƭ����ʵ��������
}

public class MapGenerator : Singleton<MapGenerator>
{
    public Tilemap walkableTilemap;

    public List<TileData> tileDataList;

    public List<LogicTile> logicTileList;

    public GameObject logicTilePrefab;

    public GameObject LogicTilesRoot;

    
    public void GetDataAndGenerateWalkableMap()
    {
        List<TileData> allTiles = GetAllTilesWithPositionsFromTilemap();
        GenerateWalkableLogicMap();
    }

    /// <summary>
    /// ��ȡ�Ӿ���Ƭ��ͼ������
    /// </summary>
    /// <returns></returns>
    private List<TileData> GetAllTilesWithPositionsFromTilemap()
    {
        tileDataList = new List<TileData>();
        BoundsInt bounds = walkableTilemap.cellBounds;

        for (int x = bounds.x; x <= bounds.xMax; x++)
        {
            for (int y = bounds.y; y <= bounds.yMax; y++)
            {
                Vector3Int gridPosition = new Vector3Int(x, y, 0);

                TileBase tile = walkableTilemap.GetTile(gridPosition);

                // ������λ������Ƭ��������ӵ��б���
                if (tile != null)
                {
                    Vector3 worldPosition = walkableTilemap.CellToWorld(gridPosition); // ת��Ϊ��������
                    TileData tileData = new TileData
                    {
                        position = gridPosition,
                        tileName = tile.name,
                        worldPosition = worldPosition
                    };
                    tileDataList.Add(tileData);
                }
            }
        }

        return tileDataList;
    }

    /// <summary>
    /// �����Ӿ���Ƭ��ͼ�����߼���Ƭ��Ƭ��ͼ
    /// </summary>
    private void GenerateWalkableLogicMap()
    {
        logicTileList = new List<LogicTile>();

        foreach (var tileData in tileDataList)
        {
            GameObject logicTileGO = Instantiate(logicTilePrefab, tileData.worldPosition, Quaternion.identity);

            logicTileGO.transform.SetParent(LogicTilesRoot.transform);

            LogicTile logicTile = logicTileGO.GetComponent<LogicTile>();

            logicTile.SetCellPosition(tileData.position);

            logicTileList.Add(logicTile);
        }

        foreach (var logicTile in logicTileList)
        {
            List<LogicTile> neighborLogicTileList = logicTileList.FindAll
                (x => ((x.CellPosition.x == logicTile.CellPosition.x
            && Mathf.Abs(x.CellPosition.y - logicTile.CellPosition.y) == 1)
            || (x.CellPosition.y == logicTile.CellPosition.y
            && Mathf.Abs(x.CellPosition.x - logicTile.CellPosition.x) == 1)));

            logicTile.SetNeighborLogicTileList(neighborLogicTileList);
        }
        LogicTilesRoot.transform.position += new Vector3(0, 0.5f, 0);
    }


}
