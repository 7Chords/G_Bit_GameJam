using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerController : Singleton<PlayerController>
{
    public Tilemap tilemap; // Tilemap
    public GameObject moveButtonPrefab; // �ƶ���ť
    public Canvas canvas;
    public float moveSpeed = 5f;
    private Vector3Int currentCell;
    private bool isMoving = false;
    
    private StepManager stepManager;//����������

    private void Start()
    {
        Vector3 worldPosition = transform.position;
        currentCell = tilemap.WorldToCell(worldPosition);
        transform.position = tilemap.GetCellCenterWorld(currentCell);

        stepManager = FindObjectOfType<StepManager>();
        if (stepManager == null) Debug.Log("stepManagerδ�ҵ�");
        
        GenerateMoveButtons();
    }

    private void Update()
    {
        if (isMoving) return;
    }

    private void GenerateMoveButtons()
    {
        foreach (Transform child in canvas.transform)
        {
            Destroy(child.gameObject);
        }
        
        //�����þ�
        if(!stepManager.CanTakeStep()) return;
        
        // ���ɿ��ƶ������ϵİ�ť
        Vector3Int[] directions = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };
        foreach (Vector3Int direction in directions)
        {
            Vector3Int targetCell = currentCell + direction;
            Vector3 targetWorldPosition = tilemap.GetCellCenterWorld(targetCell);

            // ����Ƿ��ǺϷ��ƶ�λ��
            if (tilemap.HasTile(targetCell))
            {
                CreateMoveButton(targetWorldPosition, direction);
            }
        }
    }

    private void CreateMoveButton(Vector3 position, Vector3Int direction)
    {
        // ��Ŀ�����λ�����ɰ�ť
        GameObject button = Instantiate(moveButtonPrefab, canvas.transform);
        button.transform.position = Camera.main.WorldToScreenPoint(position);
        
        Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(() => StartCoroutine(MoveInDirection(direction)));
    }

    private System.Collections.IEnumerator MoveInDirection(Vector3Int direction)
    {
        isMoving = true;

        // ����Ŀ�����λ��
        Vector3Int targetCell = currentCell + direction;
        Vector3 targetWorldPosition = tilemap.GetCellCenterWorld(targetCell);

        while (Vector3.Distance(transform.position, targetWorldPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWorldPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        
        transform.position = targetWorldPosition;
        currentCell = targetCell;
        isMoving = false;
        
        stepManager.UseStep();
        
        GenerateMoveButtons();
    }
}

