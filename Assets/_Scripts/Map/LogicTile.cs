using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicTile : MonoBehaviour
{
    public Vector3Int _cellPosition;
    public Vector3Int CellPosition => _cellPosition;

    public List<LogicTile> _neighborLogicTileList;
    public List<LogicTile> NeighborLogicTileList => _neighborLogicTileList;

    private bool _logicWalkable;
    public bool LogicWalkable => _logicWalkable;
    public void SetCellPosition(Vector3Int cellPosition)
    {
        _cellPosition = cellPosition;
    }

    public void SetNeighborLogicTileList(List<LogicTile> neighborLogicTileList)
    {
        _neighborLogicTileList = neighborLogicTileList;
    }

    public void SetLogicWalkable(bool walkable)
    {
        _logicWalkable = walkable;
    }

    public virtual void Awake()
    {
        _logicWalkable = true;
    }
}
