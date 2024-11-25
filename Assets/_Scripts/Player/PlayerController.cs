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
    public bool IsMoving => isMoving;
    private bool isInvisible;
    private bool isRecordingPath;//是否正在记录路径
    private Stack<LogicTile> _recordTileStack;
    
    private StepManager stepManager;
    private StealthManager stealthManager;

    private void Start()
    {
        _recordTileStack = new Stack<LogicTile>();

        FindNearestTile();

        ActivateWalkableTileVisualization();
        
        stepManager = FindObjectOfType<StepManager>();

        if (stepManager == null)
        {
            Debug.LogError("StepManager未找到");
        }
        
        stealthManager = FindObjectOfType<StealthManager>();
        if (stealthManager == null)
        {
            Debug.LogError("StealthManager未找到");
        }
        
        if (stealthManager != null)
        {
            stealthManager.OnStealthStateChanged += OnStealthStateChanged;
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
                    if (currentStandTile.NeighborLogicTileList.Contains(hitLogicTile) && hitLogicTile.LogicWalkable)
                    {

                        CancelWalkableTileVisualization();

                        isMoving = true;
                        
                        PerformJumpAnimation(hitLogicTile,(() =>
                        {
                            // 跳跃动画播放完毕后执行移动后的操作
                            CompleteTileMove(hitLogicTile);
                        }));
                    }
                }
            }
        }
    }
    
    private void PerformJumpAnimation(LogicTile targetTile, TweenCallback onComplete)
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = targetTile.transform.position;

        float jumpHeight = 0.5f;
        float jumpDuration = 0.3f;

        Sequence jumpSequence = DOTween.Sequence();
        jumpSequence.Append(transform.DOMoveY(startPosition.y + jumpHeight, jumpDuration / 2).SetEase(Ease.OutQuad)); // 向上跳
        jumpSequence.Append(transform.DOMoveY(endPosition.y, jumpDuration / 2).SetEase(Ease.InQuad)); // 向下落
        jumpSequence.Insert(0, transform.DOMoveX(endPosition.x, jumpDuration).SetEase(Ease.Linear)); // 水平方向移动
        jumpSequence.OnComplete(onComplete);
        jumpSequence.Play();
    }
    
    private void CompleteTileMove(LogicTile targetTile)
    {
        isMoving = false;

        currentStandTile?.GetComponent<IExitTileSpecial>()?.OnExit();

        currentStandTile = targetTile;

        ActivateWalkableTileVisualization();

        EventManager.OnPlayerMove?.Invoke();

        currentStandTile?.GetComponent<IEnterTileSpecial>()?.Apply();


        stepManager.UseStep();

        if(isRecordingPath)
        {
            _recordTileStack.Push(currentStandTile);
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
    public void ActivateWalkableTileVisualization()
    {

        foreach (var tile in currentStandTile.NeighborLogicTileList)
        {
            if(tile.LogicWalkable)
            tile.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    /// <summary>
    /// 取消当前可走的格子的高亮显示
    /// </summary>
    public void CancelWalkableTileVisualization()
    {
        foreach (var tile in currentStandTile.NeighborLogicTileList)
        {
            tile.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        }
    }
    
    private void OnDestroy()
    {
        if (stealthManager != null)
        {
            stealthManager.OnStealthStateChanged -= OnStealthStateChanged;
        }
    }

    /// <summary>
    /// 隐身的视觉特效
    /// </summary>
    private void OnStealthStateChanged(bool isInvisible)
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            Color color = renderer.color;
            color.a = isInvisible ? 0.5f : 1f;
            renderer.color = color;
        }
    }


    public void SetRecordingPath(bool isRecording, bool startTraceBack = true)
    {
        isRecordingPath = isRecording;

        if (!isRecording && startTraceBack)
        {

            isMoving = true;

            CancelWalkableTileVisualization();

            // 开始播放跳跃动画，进入递归处理
            ExecuteJumpAnimations();
        }
    }

    private void ExecuteJumpAnimations()
    {
        if (_recordTileStack.Count > 0)
        {
            LogicTile tile = _recordTileStack.Pop();

            PerformJumpAnimation(tile, () =>
            {
                currentStandTile?.GetComponent<IExitTileSpecial>()?.OnExit();
                currentStandTile = tile;
                currentStandTile?.GetComponent<IEnterTileSpecial>()?.Apply();

                // 继续下一个跳跃动画
                ExecuteJumpAnimations();
            });
        }
        else
        {
            ActivateWalkableTileVisualization();

            isMoving = false;

        }
    }
}
