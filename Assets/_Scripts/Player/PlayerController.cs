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

    private bool isMoving;//��־�Ƿ��ƶ�
    public bool IsMoving => isMoving;

    private bool isInvisible;

    private bool isRecordingPath;//�Ƿ����ڼ�¼·��

    private Stack<LogicTile> _recordTileStack;//��¼����Ƭ·��
    
    private StepManager stepManager;
    public StepManager StepManager=> stepManager;

    private void Start()
    {
        _recordTileStack = new Stack<LogicTile>();

        //��Ϸ��ʼĬ�ϲ������� �ȴ��ؿ���ʼ�ĶԻ�������ſ�����
        canMove = false;

        FindNearestTile();

        ActivateWalkableTileVisualization();
        
        stepManager = FindObjectOfType<StepManager>();

        if (stepManager == null)
        {
            Debug.LogError("StepManagerδ�ҵ�");
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
            // ��鲽���Ƿ��㹻
            if (!stepManager.CanTakeStep())
            {
                CancelWalkableTileVisualization();
                //ui��ʾ
                GameManager.Instance.LoadPlayerData();
                Debug.Log("�����Ѻľ�");
                return;
            }

            // �����λ�ô���һ������
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
                            // ��Ծ����������Ϻ�ִ���ƶ���Ĳ���
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
        
        jumpSequence.Append(transform.DOScale(new Vector3(.15f, 0.25f, 1f), jumpDuration / 4).SetEase(Ease.OutQuad)); // ������ʱѹ��
        jumpSequence.Insert(0, transform.DOMoveY(startPosition.y + jumpHeight, jumpDuration / 2).SetEase(Ease.OutQuad)); // ������
        jumpSequence.Append(transform.DOScale(new Vector3(.2f,.2f,1f), jumpDuration / 4).SetEase(Ease.InQuad)); // �ָ�����
        jumpSequence.Append(transform.DOScale(new Vector3(0.25f,0.15f, 1f), jumpDuration / 4).SetEase(Ease.OutQuad)); // ���ʱ����
        jumpSequence.Insert(jumpDuration / 2, transform.DOMoveY(endPosition.y, jumpDuration / 2).SetEase(Ease.InQuad)); // ������
        jumpSequence.Append(transform.DOScale(new Vector3(.2f,.2f,1f), jumpDuration / 4).SetEase(Ease.InQuad)); // �ָ�������С
        
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
    /// �ҵ���ҵ�ǰ��վ���߼���Ƭ������λ�õ��������ƫ�Start����
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
    /// ������ߵĸ��Ӹ�����ʾ
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
    /// ȡ����ǰ���ߵĸ��ӵĸ�����ʾ
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
    /// ������Ӿ���Ч
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
    /// �����Ƿ�ʼ��¼·��
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

            // ��ʼ������Ծ����������ݹ鴦��
            ExecuteJumpAnimations();
        }
    }

    /// <summary>
    /// ����·����Ԫ����
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

                // ������һ����Ծ����
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
    /// �޸��Ƿ�����ƶ�����
    /// </summary>
    /// <param name="canMove"></param>
    public void ChangeCanMoveState(bool canMove = true)
    {
        this.canMove = canMove;
    }


    public void Dead()
    {
        //��Ч�� UI��
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
