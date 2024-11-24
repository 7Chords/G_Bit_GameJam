using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[System.Serializable]


public class TileData
{
    public Vector3Int position; // 瓦片的网格位置
    public string tileName;      // 瓦片的名称
    public Vector3 worldPosition; // 瓦片的真实世界坐标
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
    /// 获取视觉瓦片地图的数据
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

                // 如果这个位置有瓦片，将其添加到列表中
                if (tile != null)
                {
                    Vector3 worldPosition = walkableTilemap.CellToWorld(gridPosition); // 转换为世界坐标
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
    /// 根据视觉瓦片地图生成逻辑瓦片瓦片地图
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
