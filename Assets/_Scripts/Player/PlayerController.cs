using DG.Tweening;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;


public class PlayerController : MonoBehaviour
{

    public LogicTile currentStandTile;

    private bool isMoving;//标志是否移动

    private StepManager stepManager;

    private void Start()
    {
        FindNearestTile();

        ActivateWalkableTileVisualization();
        
        stepManager = FindObjectOfType<StepManager>();
        if (stepManager == null)
        {
            Debug.LogError("StepManager未找到");
        }
    }

    private void Update()
    {
        InputForWalking();
    }

    private void InputForWalking()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            // 检查步数是否足够
            if (!stepManager.CanTakeStep())
            {
                CancelWalkableTileVisualization();
                Debug.Log("步数已耗尽");
                return;
            }

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
                        isMoving = true;

                        // 跳跃动画
                        Vector3 startPosition = transform.position;
                        Vector3 endPosition = hitLogicTile.transform.position;

                        float jumpHeight = 0.5f;
                        float jumpDuration = 0.3f;

                        Sequence jumpSequence = DOTween.Sequence();
                        jumpSequence.Append(transform.DOMoveY(startPosition.y + jumpHeight, jumpDuration / 2)
                            .SetEase(Ease.OutQuad)); // 向上跳
                        jumpSequence.Append(transform.DOMoveY(endPosition.y, jumpDuration / 2)
                            .SetEase(Ease.InQuad)); // 向下落
                        jumpSequence.Insert(0, transform.DOMoveX(endPosition.x, jumpDuration)
                            .SetEase(Ease.Linear)); // 水平方向移动
                        jumpSequence.Play();

                        jumpSequence.OnComplete(() =>
                        {
                            isMoving = false;

                            currentStandTile = hitLogicTile;
                            ActivateWalkableTileVisualization();

                            stepManager.UseStep();
                        });
                    }
                }
            }
        }
    }


    /// <summary>
    /// 找到玩家当前所站的逻辑瓦片，并令位置到那里，消除偏差，Start调用
    /// </summary>
    private void FindNearestTile()
    {
        float nearestDis = 9999;

        foreach (var logicTile in MapGenerator.Instance.logicTileList)
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
