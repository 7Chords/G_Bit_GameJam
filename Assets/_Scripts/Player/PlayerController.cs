using DG.Tweening;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;


[System.Serializable]
public class TileData
{
    public Vector3Int position; // 瓦片的网格位置
    public string tileName;      // 瓦片的名称
    public Vector3 worldPosition; // 瓦片的真实世界坐标
}

public class PlayerController : MonoBehaviour
{
    public Tilemap walkableTilemap;

    public List<TileData> tileDataList;

    public List<LogicTile> logicTileList;

    public GameObject logicTilePrefab;

    public GameObject LogicTilesRoot;

    public LogicTile currentStandTile;

    private void Start()
    {
        List<TileData> allTiles = GetAllTilesWithPositionsFromTilemap();
        GenerateLogicMap();
        FindNearestTile();
        ActivateWalkableTileVisualization();
    }

    private void Update()
    {
        InputForWalking();
    }

    private void InputForWalking()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 从鼠标位置创建一条射线
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                LogicTile hitLogicTile = hit.collider.transform.parent.GetComponent<LogicTile>();

                if (hitLogicTile != null)
                {
                    if (currentStandTile.NeighborLogicTileList.Contains(hitLogicTile))
                    {
                        CancelWalkableTileVisualization();
                        transform.position = hitLogicTile.transform.position;
                        currentStandTile = hitLogicTile;
                        ActivateWalkableTileVisualization();
                    }
                }
            }
        }
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
    private void GenerateLogicMap()
    {
        logicTileList = new List<LogicTile>();

        foreach (var tileData in tileDataList)
        {
            GameObject logicTileGO = Instantiate(logicTilePrefab,tileData.worldPosition,Quaternion.identity);

            logicTileGO.transform.SetParent(LogicTilesRoot.transform);

            LogicTile logicTile = logicTileGO.GetComponent<LogicTile>();

            logicTile.SetCellPosition(tileData.position);

            logicTileList.Add(logicTile);
        }

        foreach(var logicTile in logicTileList)
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

    /// <summary>
    /// 找到玩家当前所站的逻辑瓦片，并令位置到那里，消除偏差，Start调用
    /// </summary>
    private void FindNearestTile()
    {
        float nearestDis = 9999;

        foreach(var logicTile in logicTileList)
        {
            float currentDis = Vector3.Distance(logicTile.transform.position, transform.position);
            if (currentDis < nearestDis)
            {
                nearestDis = currentDis;
                currentStandTile = logicTile;
            }
        }
        transform.position = currentStandTile.transform.position;
    }

    /// <summary>
    /// 激活可走的格子高亮显示
    /// </summary>
    private void ActivateWalkableTileVisualization()
    {
        
        foreach (var tile in currentStandTile.NeighborLogicTileList)
        {
            tile.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    /// <summary>
    /// 取消当前可走的格子的高亮显示
    /// </summary>
    private void CancelWalkableTileVisualization()
    {
        foreach (var tile in currentStandTile.NeighborLogicTileList)
        {
            tile.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        }
    }


}
