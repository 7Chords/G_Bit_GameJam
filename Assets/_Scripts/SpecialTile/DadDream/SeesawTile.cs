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

        // 如果两侧都有对象，计算体重
        if (objectsOnThisTile != null && objectsOnOtherTile != null)
        {
            GameObject lighterObject = GetLighterObject();

            if (lighterObject != null)
            {
                TeleportObject(lighterObject);

                // 更换当前瓦片的 TileBase
                ChangeTileBase();
                anotherSeesawTile.ChangeTileBase();
            }
        }
    }

    public void OnExit()
    {
        objectsOnThisTile = null;
        objectsOnOtherTile = null;
    }

    public void ChangeTileBase()
    {
        Vector3 currentTilePosition = transform.position - new Vector3(.2f, .2f, 0);
        Vector3Int tilemapPosition = TileUpdater.Instance.tilemap.WorldToCell(currentTilePosition);
        TileUpdater.Instance.UpdateTile(tilemapPosition);
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
        LogicTile targetTile = new LogicTile();
        targetTile = targetObject == objectsOnThisTile
            ? GetDirectionalNeighbor(anotherSeesawTile)
            : GetDirectionalNeighbor(this);

        if (targetTile == null)
        {
            return;
        }

        Vector3 startPosition = targetObject.transform.position;
        Vector3 endPosition = targetTile.transform.position;

        float jumpHeight = 0.8f;
        float jumpDuration = 0.2f;

        Sequence jumpSequence = DOTween.Sequence();
        jumpSequence.Append(targetObject.transform.DOMoveY(startPosition.y + jumpHeight, jumpDuration / 2).SetEase(Ease.OutQuad)); // 向上跳
        jumpSequence.Append(targetObject.transform.DOMoveY(endPosition.y, jumpDuration / 2).SetEase(Ease.InQuad)); // 向下落
        jumpSequence.Insert(0, targetObject.transform.DOMoveX(endPosition.x, jumpDuration).SetEase(Ease.Linear)); // 水平方向移动

        jumpSequence.OnStart(() =>
        {
            BaseEnemy enemy = targetObject.GetComponent<BaseEnemy>();
            if (enemy != null)
            {
                enemy.isMoving = true;
            }

            PlayerController player = targetObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.CancelWalkableTileVisualization();
            }
        });

        jumpSequence.OnComplete(() =>
        {
            BaseEnemy enemy = targetObject.GetComponent<BaseEnemy>();
            if (enemy != null)
            {
                enemy.isMoving = false;
                enemy.currentStandTile = targetTile;
            }

            PlayerController player = targetObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.currentStandTile = targetTile;
                player.ActivateWalkableTileVisualization();
            }
        });

        jumpSequence.Play();
        AudioManager.Instance.PlaySfx("seesaw");
    }

    private LogicTile GetDirectionalNeighbor(SeesawTile targetTile)
    {
        switch (targetTile.teleportDirection)
        {
            case Direction.up:
                return targetTile.GetComponent<LogicTile>().NeighborLogicTileList[3];
            case Direction.down:
                return targetTile.GetComponent<LogicTile>().NeighborLogicTileList[0];
            case Direction.left:
                return targetTile.GetComponent<LogicTile>().NeighborLogicTileList[2];
            case Direction.right:
                return targetTile.GetComponent<LogicTile>().NeighborLogicTileList[1];
            default:
                return null;
        }
    }
}
