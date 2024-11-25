using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    up,
    down,
    left,
    right
}

public class SeesawTile : MonoBehaviour, IEnterTileSpecial, IExitTileSpecial
{
    public SeesawTile anotherSeesawTile;
    public Direction teleportDirection;
    
    private GameObject objectsOnThisTile;
    private GameObject objectsOnOtherTile;
    
    public void Apply()
    {
        if (anotherSeesawTile == null)
        {
            return;
        }

        UpdateObjectsOnTiles();

        // 如果两侧都有对象，计算体重
        if (objectsOnThisTile && objectsOnOtherTile)
        {
            GameObject lighterObject = GetLighterObject();
            if (lighterObject != null)
            {
                TeleportObject(lighterObject);
            }
        }
    }

    public void OnExit()
    {
        objectsOnThisTile = null;
        objectsOnOtherTile = null;
    }
    
    private void UpdateObjectsOnTiles()
    {
        objectsOnThisTile = GetObjectsOnTile(transform.position);
        objectsOnOtherTile = anotherSeesawTile.GetObjectsOnTile(anotherSeesawTile.transform.position);
    }
    
    private GameObject GetObjectsOnTile(Vector3 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.1f);

        foreach (var collider in colliders)
        {
            if (collider.GetComponent<Weight>() != null)
            {
                return collider.gameObject;
            }
        }

        return null;
    }

    
    private GameObject GetLighterObject()
    {
        if (objectsOnThisTile == null || objectsOnOtherTile == null)
        {
            return null; // 确保只有有效对象时才比较
        }

        float weightOnThisTile = objectsOnThisTile.GetComponent<Weight>().weight;
        float weightOnOtherTile = objectsOnOtherTile.GetComponent<Weight>().weight;

        return weightOnThisTile < weightOnOtherTile ? objectsOnThisTile : objectsOnOtherTile;
    }


    private void TeleportObject(GameObject targetObject)
    {
        LogicTile anotherTileLogic = anotherSeesawTile.GetComponent<LogicTile>();
        if (anotherTileLogic == null)
        {
            return;
        }
        
        LogicTile targetTile = GetDirectionalNeighbor(anotherTileLogic);
        if (targetTile == null)
        {
            return;
        }

        // 设置传送位置
        targetObject.transform.position = targetTile.transform.position;
        
        BaseEnemy enemy = targetObject.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            enemy.currentStandTile = targetTile;
        }

        PlayerController player = targetObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.currentStandTile = targetTile;
        }
    }

    private LogicTile GetDirectionalNeighbor(LogicTile tile)
    {
        switch (teleportDirection)
        {
            case Direction.up:
                return anotherSeesawTile.GetComponent<LogicTile>().NeighborLogicTileList[3];
            case Direction.down:
                return anotherSeesawTile.GetComponent<LogicTile>().NeighborLogicTileList[0];
            case Direction.left:
                return anotherSeesawTile.GetComponent<LogicTile>().NeighborLogicTileList[2];
            case Direction.right:
                return anotherSeesawTile.GetComponent<LogicTile>().NeighborLogicTileList[1];
            default:
                return null;
        }
    }
}
