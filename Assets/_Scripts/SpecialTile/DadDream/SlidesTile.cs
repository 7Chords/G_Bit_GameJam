using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum Flag
{
    None,  // 中间部分
    Begin, // 头
    End,   // 尾
}

public class SlidesTile : MonoBehaviour, IEnterTileSpecial
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
                if (collider.GetComponent<PlayerController>() || collider.GetComponent<BaseEnemy>())
                {
                    currentObject = collider.gameObject;
                    break;
                }
            }

            if (currentObject != null)
            {
                SlideToTail();
            }
        }
    }

    private LogicTile FindNextTile(LogicTile tile)
    {
        if (tile == null) return null;

        SlidesTile slidesTile = tile.GetComponent<LogicTile>().NeighborLogicTileList[0].GetComponent<SlidesTile>();
        if (slidesTile != null && slidesTile.type != Flag.Begin) 
        {
            return slidesTile.GetComponent<LogicTile>();
        }

        return null;
    }

    private void SlideToTail()
    {
        LogicTile currentTile = GetComponent<LogicTile>();
        if (currentTile == null)
        {
            return;
        }

        LogicTile nextTile = currentTile;
        List<Vector3> path = new List<Vector3>();
        HashSet<LogicTile> visitedTiles = new HashSet<LogicTile>(); // 用于检测循环路径

        while (nextTile != null)
        {
            if (visitedTiles.Contains(nextTile))
            {
                Debug.LogError("手动调整逻辑瓦片邻居顺序");
                break;
            }

            visitedTiles.Add(nextTile);
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
        if (targetObject == null || path == null || path.Count < 2)
        {
            return;
        }

        Sequence slideSequence = DOTween.Sequence();
        float slideDuration = 0.1f;

        for (int i = 1; i < path.Count; i++)
        {
            Vector3 endPoint = path[i];
            slideSequence.Append(targetObject.transform.DOMove(endPoint, slideDuration).SetEase(Ease.Linear));
        }

        slideSequence.OnStart(() =>
        {
            if (targetObject == null)
            {
                slideSequence.Kill();
                Debug.LogWarning("Target object destroyed during slide start.");
                return;
            }

            if (targetObject.TryGetComponent(out BaseEnemy enemy))
            {
                enemy.isMoving = true;
            }

            if (targetObject.TryGetComponent(out PlayerController player))
            {
                player.CancelWalkableTileVisualization();
            }
        });

        slideSequence.OnComplete(() =>
        {
            if (targetObject == null)
            {
                Debug.LogWarning("Target object destroyed during slide.");
                return;
            }

            if (targetObject.TryGetComponent(out BaseEnemy enemy))
            {
                enemy.isMoving = false;
                enemy.currentStandTile = targetTile;
            }

            if (targetObject.TryGetComponent(out PlayerController player))
            {
                PlayerController.Instance.currentStandTile = targetTile;
                PlayerController.Instance.ActivateWalkableTileVisualization();
            }
        });

        slideSequence.Play();
        AudioManager.Instance.PlaySfx("Slides");
    }

}
