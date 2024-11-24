using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicTile : MonoBehaviour
{
    private Vector3Int _cellPosition;
    public Vector3Int CellPosition => _cellPosition;

    private List<LogicTile> _neighborLogicTileList;
    public List<LogicTile> NeighborLogicTileList => _neighborLogicTileList;

    private bool _walkable;
    public bool Walkable => _walkable;
    public void SetCellPosition(Vector3Int cellPosition)
    {
        _cellPosition = cellPosition;
    }

    public void SetNeighborLogicTileList(List<LogicTile> neighborLogicTileList)
    {
        _neighborLogicTileList = neighborLogicTileList;
    }


}
