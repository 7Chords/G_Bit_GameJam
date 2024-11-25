using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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

        // ������඼�ж��󣬼�������
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
            return null; // ȷ��ֻ����Ч����ʱ�űȽ�
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

        LogicTile targetTile = GetDirectionalNeighbor();
        if (targetTile == null)
        {
            return;
        }
        
        Vector3 startPosition = targetObject.transform.position;
        Vector3 endPosition = targetTile.transform.position;
        
        float jumpHeight = 0.5f;
        float jumpDuration = 0.3f;

        Sequence jumpSequence = DOTween.Sequence();
        jumpSequence.Append(targetObject.transform.DOMoveY(startPosition.y + jumpHeight, jumpDuration / 2).SetEase(Ease.OutQuad)); // ������
        jumpSequence.Append(targetObject.transform.DOMoveY(endPosition.y, jumpDuration / 2).SetEase(Ease.InQuad)); // ������
        jumpSequence.Insert(0, targetObject.transform.DOMoveX(endPosition.x, jumpDuration).SetEase(Ease.Linear)); // ˮƽ�����ƶ�

        jumpSequence.OnStart((() =>
        {
            PlayerController player = targetObject.GetComponent<PlayerController>(); 
            if (player != null)
            {
                player.CancelWalkableTileVisualization();
            }
        }));
        
        jumpSequence.OnComplete((() =>
        {
            BaseEnemy enemy = targetObject.GetComponent<BaseEnemy>();
            if (enemy != null)
            {
                enemy.currentStandTile = targetTile;
            }
            
            PlayerController player = targetObject.GetComponent<PlayerController>(); 
            if (player != null)
            {
                player.currentStandTile = targetTile;
                player.ActivateWalkableTileVisualization();
            }
        }));
        jumpSequence.Play();
    }

    private LogicTile GetDirectionalNeighbor()
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
