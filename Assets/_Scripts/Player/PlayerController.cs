using DG.Tweening;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;


public class PlayerController : MonoBehaviour
{

    public LogicTile currentStandTile;

    private bool isMoving;//��־�Ƿ��ƶ�
    private bool isInvisible;
    
    private StepManager stepManager;
    private StealthManager stealthManager;

    private void Start()
    {
        FindNearestTile();

        ActivateWalkableTileVisualization();
        
        stepManager = FindObjectOfType<StepManager>();
        if (stepManager == null)
        {
            Debug.LogError("StepManagerδ�ҵ�");
        }
        
        stealthManager = FindObjectOfType<StealthManager>();
        if (stealthManager == null)
        {
            Debug.LogError("StealthManager δ�ҵ���");
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
            // ��鲽���Ƿ��㹻
            if (!stepManager.CanTakeStep())
            {
                CancelWalkableTileVisualization();
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
                    if (currentStandTile.NeighborLogicTileList.Contains(hitLogicTile))
                    {

                        CancelWalkableTileVisualization();
                        isMoving = true;

                        // ��Ծ����
                        Vector3 startPosition = transform.position;
                        Vector3 endPosition = hitLogicTile.transform.position;

                        float jumpHeight = 0.5f;
                        float jumpDuration = 0.3f;

                        Sequence jumpSequence = DOTween.Sequence();
                        jumpSequence.Append(transform.DOMoveY(startPosition.y + jumpHeight, jumpDuration / 2)
                            .SetEase(Ease.OutQuad)); // ������
                        jumpSequence.Append(transform.DOMoveY(endPosition.y, jumpDuration / 2)
                            .SetEase(Ease.InQuad)); // ������
                        jumpSequence.Insert(0, transform.DOMoveX(endPosition.x, jumpDuration)
                            .SetEase(Ease.Linear)); // ˮƽ�����ƶ�
                        jumpSequence.Play();

                        jumpSequence.OnComplete(() =>
                        {
                            isMoving = false;

                            currentStandTile?.GetComponent<IExitTileSpecial>()?.OnExit();
                            
                            currentStandTile = hitLogicTile;
                            
                            currentStandTile?.GetComponent<IEnterTileSpecial>()?.Apply();

                            ActivateWalkableTileVisualization();

                            stepManager.UseStep();
                            
                            EventManager.OnPlayerMove?.Invoke();
                        });
                    }
                }
            }
        }
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
            tile.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    /// <summary>
    /// ȡ����ǰ���ߵĸ��ӵĸ�����ʾ
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
}
