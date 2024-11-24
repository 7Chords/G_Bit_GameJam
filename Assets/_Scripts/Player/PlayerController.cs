using DG.Tweening;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;


[System.Serializable]
public class TileData
{
    public Vector3Int position; // ��Ƭ������λ��
    public string tileName;      // ��Ƭ������
    public Vector3 worldPosition; // ��Ƭ����ʵ��������
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
            // �����λ�ô���һ������
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
    /// �ҵ���ҵ�ǰ��վ���߼���Ƭ������λ�õ��������ƫ�Start����
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
    /// ������ߵĸ��Ӹ�����ʾ
    /// </summary>
    private void ActivateWalkableTileVisualization()
    {
        
        foreach (var tile in currentStandTile.NeighborLogicTileList)
        {
            tile.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    /// <summary>
    /// ȡ����ǰ���ߵĸ��ӵĸ�����ʾ
    /// </summary>
    private void CancelWalkableTileVisualization()
    {
        foreach (var tile in currentStandTile.NeighborLogicTileList)
        {
            tile.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        }
    }


}
