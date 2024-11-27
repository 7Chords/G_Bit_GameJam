using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum Flag{
    None, // 中间部分
    Begin, // 头
    End, // 尾
}

public class SlidesTile : MonoBehaviour,IEnterTileSpecial
{
    public Flag type;
    private GameObject currentObject;

    private LogicTile targetTile;
    
    public void Apply()
    {
        if (type == Flag.Begin)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f);

            foreach (var collider in colliders)
            {
                if(collider.GetComponent<PlayerController>() || collider.GetComponent<BaseEnemy>())
                    currentObject = collider.gameObject;
            }

            if (currentObject != null) SlideToTail();
        }
    }
    
    private LogicTile FindNextTile(LogicTile tile)
    {
        foreach (var neighbor in tile.NeighborLogicTileList)
        {
            SlidesTile slidesTile = neighbor.GetComponent<SlidesTile>();
            if (slidesTile != null && slidesTile.type != Flag.Begin)
            {
                return neighbor;
            }
        }

        return null;
    }

    private void SlideToTail()
    {
        LogicTile currentTile = GetComponent<LogicTile>();
        LogicTile nextTile = currentTile;

        List<Vector3> path = new List<Vector3>();
        
        while (nextTile != null)
        {
            path.Add(nextTile.transform.position);

            SlidesTile slideTile = nextTile.GetComponent<SlidesTile>();
            if (slideTile != null && slideTile.type == Flag.End)
            {
                targetTile = slideTile.GetComponent<LogicTile>();
                break;
            }

            nextTile = FindNextTile(nextTile);
        }
        
        if (currentObject != null && path.Count > 1)
        {
            MoveAlongPath(currentObject, path);
        }
    }
    
    private void MoveAlongPath(GameObject targetObject, List<Vector3> path)
    {
        Sequence slideSequence = DOTween.Sequence();
        float slideDuration = 0.1f;
        
        for (int i = 1; i < path.Count; i++)
        {
            Vector3 endPoint = path[i];
            slideSequence.Append(targetObject.transform.DOMove(endPoint, slideDuration).SetEase(Ease.Linear));
        }

        slideSequence.OnStart((() =>
        {
            BaseEnemy enemy = targetObject.GetComponent<BaseEnemy>();
            if (enemy != null)
            {
                enemy.isMoving = true;
            }

            PlayerController.Instance.CancelWalkableTileVisualization();
        }));
        
        // 动画完成后更新逻辑位置
        slideSequence.OnComplete(() =>
        {
            BaseEnemy enemy = targetObject.GetComponent<BaseEnemy>();
            if (enemy != null)
            {
                enemy.isMoving = false;
                enemy.currentStandTile = targetTile;
            }

            PlayerController.Instance.currentStandTile = targetTile;
            PlayerController.Instance.ActivateWalkableTileVisualization();
        });

        slideSequence.Play();
    }
    
}
