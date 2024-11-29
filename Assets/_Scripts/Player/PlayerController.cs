using DG.Tweening;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;


public class PlayerController : Singleton<PlayerController>
{

    public LogicTile currentStandTile;

    private bool canMove;

    public bool CanMove => canMove;

    private bool isMoving;//标志是否移动
    public bool IsMoving => isMoving;

    private bool isInvisible;

    private bool isRecordingPath;//是否正在记录路径

    private Stack<LogicTile> _recordTileStack;//记录的瓦片路径
    
    private StepManager stepManager;
    public StepManager StepManager=> stepManager;

    private void Start()
    {
        _recordTileStack = new Stack<LogicTile>();

        //游戏开始默认不能行走 等待关卡开始的对话结束后才可以走
        canMove = false;

        FindNearestTile();

        ActivateWalkableTileVisualization();
        
        stepManager = FindObjectOfType<StepManager>();

        if (stepManager == null)
        {
            Debug.LogError("StepManager未找到");
        }
        

        StealthManager.Instance.OnStealthStateChanged += OnStealthStateChanged;
        
        EventManager.OnGameStarted += OnGameStarted;

        EventManager.OnGameFinished += OnGameFinish;

        EventManager.OnPlayerLoadData += OnPlayerLoadData;
    }

    private void Update()
    {
        if(canMove)
        {
            InputForWalking();
        }
    }

    private void InputForWalking()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            // 检查步数是否足够
            if (!stepManager.CanTakeStep())
            {
                CancelWalkableTileVisualization();
                //ui提示
                GameManager.Instance.LoadPlayerData();
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
        
        jumpSequence.Append(transform.DOScale(new Vector3(.15f, 0.25f, 1f), jumpDuration / 4).SetEase(Ease.OutQuad)); // 向上跳时压缩
        jumpSequence.Insert(0, transform.DOMoveY(startPosition.y + jumpHeight, jumpDuration / 2).SetEase(Ease.OutQuad)); // 向上跳
        jumpSequence.Append(transform.DOScale(new Vector3(.2f,.2f,1f), jumpDuration / 4).SetEase(Ease.InQuad)); // 恢复缩放
        jumpSequence.Append(transform.DOScale(new Vector3(0.25f,0.15f, 1f), jumpDuration / 4).SetEase(Ease.OutQuad)); // 落地时拉伸
        jumpSequence.Insert(jumpDuration / 2, transform.DOMoveY(endPosition.y, jumpDuration / 2).SetEase(Ease.InQuad)); // 向下落
        jumpSequence.Append(transform.DOScale(new Vector3(.2f,.2f,1f), jumpDuration / 4).SetEase(Ease.InQuad)); // 恢复正常大小
        
        jumpSequence.Insert(0, transform.DOMoveX(endPosition.x, jumpDuration).SetEase(Ease.Linear));
        
        jumpSequence.OnComplete(onComplete);
        jumpSequence.Play();
    }
    private void CompleteTileMove(LogicTile targetTile)
    {
        AudioManager.Instance.PlaySfx("Jumping");

        isMoving = false;

        currentStandTile?.GetComponent<IExitTileSpecial>()?.OnExit();

        currentStandTile = targetTile;

        ActivateWalkableTileVisualization();

        currentStandTile?.GetComponent<IEnterTileSpecial>()?.Apply();

        stepManager.UseStep();

        if(isRecordingPath)
        {
            _recordTileStack.Push(currentStandTile);
        }
        
        EventManager.OnPlayerMove?.Invoke();
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
            if (tile.LogicWalkable)
                tile.transform.GetChild(0).GetComponent<SpriteRenderer>().material.SetFloat("_Transparent", 0.5f);
        }
    }

    /// <summary>
    /// 取消当前可走的格子的高亮显示
    /// </summary>
    public void CancelWalkableTileVisualization()
    {
        foreach (var tile in currentStandTile.NeighborLogicTileList)
        {
            tile.transform.GetChild(0).GetComponent<SpriteRenderer>().material.SetFloat("_Transparent", 0f);
        }
    }
    
    private void OnDestroy()
    {
        StealthManager.Instance.OnStealthStateChanged -= OnStealthStateChanged;

        EventManager.OnGameStarted -= OnGameStarted;

        EventManager.OnGameFinished -= OnGameFinish;


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


    /// <summary>
    /// 设置是否开始记录路径
    /// </summary>
    /// <param name="isRecording"></param>
    /// <param name="startTraceBack"></param>
    public void SetRecordingPath(bool isRecording, bool startTraceBack = true)
    {
        isRecordingPath = isRecording;

        if (!isRecording && startTraceBack)
        {

            isMoving = true;

            AudioManager.Instance.PlaySfx("Traceback");

            CancelWalkableTileVisualization();

            // 开始播放跳跃动画，进入递归处理
            ExecuteJumpAnimations();
        }
    }

    /// <summary>
    /// 回溯路径单元方法
    /// </summary>
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
                AudioManager.Instance.PlaySfx("Jumping");

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

    /// <summary>
    /// 修改是否可以移动方法
    /// </summary>
    /// <param name="canMove"></param>
    public void ChangeCanMoveState(bool canMove = true)
    {
        this.canMove = canMove;
    }


    public void Dead()
    {
        //特效？ UI？
        GameManager.Instance.LoadPlayerData();

        EventManager.OnPlayerLoadData -= OnPlayerLoadData;
    }

    private void OnGameStarted()
    {
        ChangeCanMoveState(true);
    }

    private void OnGameFinish()
    {
        ChangeCanMoveState(false);
    }

    private void OnPlayerLoadData()
    {
        isRecordingPath = false;

        _recordTileStack.Clear();
    }

}
