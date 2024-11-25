using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
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

    public List<LogicTile> logicTileList = new List<LogicTile>();

    public GameObject logicTilePrefab;

    public GameObject LogicTilesRoot;

    
    public void GetDataAndGenerateWalkableMap()
    {
        SetAllTilesWithPositionsFromTilemap();
        GenerateWalkableLogicMap();
    }

    /// <summary>
    /// ��ȡ�Ӿ���Ƭ��ͼ������
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
                    currentTileDataList.Add(tileData);
                }
            }
        }


        //��ǰû����Ƭ��ͼ
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
                //��ǰ��Ƭ��ͼ������ĳ���ɵ���Ƭ��ͼ
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
    /// �����Ӿ���Ƭ��ͼ�����߼���Ƭ��Ƭ��ͼ
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
            //����ÿ����Ƭ���ھ���Ƭ
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
                //���ھ���Ƭ��ͼ��û�е���Ƭ
                if(!logicTileList.Find(x=>x.CellPosition==tileData.position))
                {
                    GameObject logicTileGO = Instantiate(logicTilePrefab, tileData.worldPosition, Quaternion.identity);

                    logicTileGO.transform.SetParent(LogicTilesRoot.transform);

                    LogicTile logicTile = logicTileGO.GetComponent<LogicTile>();

                    logicTile.SetCellPosition(tileData.position);

                    logicTileList.Add(logicTile);

                    //��������ƫ��
                    logicTileGO.transform.position += new Vector3(0, 0.5f, 0);
                }
            }

            for (int i =0;i< logicTileList.Count; i++)
            {
                //��������Ƭ��ͼ��û�еľ���Ƭ
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
