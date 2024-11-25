using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
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

    public List<LogicTile> logicTileList = new List<LogicTile>();

    public GameObject logicTilePrefab;

    public GameObject LogicTilesRoot;

    
    public void GetDataAndGenerateWalkableMap()
    {
        SetAllTilesWithPositionsFromTilemap();
        GenerateWalkableLogicMap();
    }

    /// <summary>
    /// 获取视觉瓦片地图的数据
    /// </summary>
    /// <returns></returns>
    private void SetAllTilesWithPositionsFromTilemap()
    {

        
        List<TileData>  currentTileDataList = new List<TileData>();

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
                    currentTileDataList.Add(tileData);
                }
            }
        }


        //当前没有瓦片地图
        if (tileDataList.Count==0)
        {
            Debug.Log("is 0");

            tileDataList = currentTileDataList;
        }
        else
        {
            Debug.Log("no 0");

            for (int i = 0; i < tileDataList.Count; i++)
            {
                //当前瓦片地图不存在某个旧的瓦片地图
                if (currentTileDataList.Find(x => x.position == tileDataList[i].position)==null)
                {
                    tileDataList.RemoveAt(i);
                    i--;
                }
            }

            foreach(var tileData in currentTileDataList)
            {
                if (tileDataList.Find(x => x.position == tileData.position) == null)
                {
                    tileDataList.Add(tileData);
                }
            }

        }


    }

    /// <summary>
    /// 根据视觉瓦片地图生成逻辑瓦片瓦片地图
    /// </summary>
    private void GenerateWalkableLogicMap()
    {

        if(logicTileList.Count==0)
        {
            foreach (var tileData in tileDataList)
            {
                GameObject logicTileGO = Instantiate(logicTilePrefab, tileData.worldPosition, Quaternion.identity);

                logicTileGO.transform.SetParent(LogicTilesRoot.transform);

                LogicTile logicTile = logicTileGO.GetComponent<LogicTile>();

                logicTile.SetCellPosition(tileData.position);

                logicTileList.Add(logicTile);
            }
            //设置每个瓦片的邻居瓦片
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
        else
        {
            foreach (var tileData in tileDataList)
            {
                //存在旧瓦片地图里没有的瓦片
                if(!logicTileList.Find(x=>x.CellPosition==tileData.position))
                {
                    GameObject logicTileGO = Instantiate(logicTilePrefab, tileData.worldPosition, Quaternion.identity);

                    logicTileGO.transform.SetParent(LogicTilesRoot.transform);

                    LogicTile logicTile = logicTileGO.GetComponent<LogicTile>();

                    logicTile.SetCellPosition(tileData.position);

                    logicTileList.Add(logicTile);

                    //自我修正偏移
                    logicTileGO.transform.position += new Vector3(0, 0.5f, 0);
                }
            }

            for (int i =0;i< logicTileList.Count; i++)
            {
                //存在新瓦片地图里没有的旧瓦片
                if(tileDataList.Find(x => x.position == logicTileList[i].CellPosition)==null)
                {
                    DestroyImmediate(logicTileList[i].gameObject);
                    logicTileList.RemoveAt(i);
                    i--;
                }
            }
        }
    }


}
