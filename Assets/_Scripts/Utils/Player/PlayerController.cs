using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerController : Singleton<PlayerController>
{
    public Tilemap tilemap; // Tilemap
    public GameObject moveButtonPrefab; // 移动按钮
    public Canvas canvas;
    public float moveSpeed = 5f;
    private Vector3Int currentCell;
    private bool isMoving = false;
    
    private StepManager stepManager;//步数管理器

    private void Start()
    {
        Vector3 worldPosition = transform.position;
        currentCell = tilemap.WorldToCell(worldPosition);
        transform.position = tilemap.GetCellCenterWorld(currentCell);

        stepManager = FindObjectOfType<StepManager>();
        if (stepManager == null) Debug.Log("stepManager未找到");
        
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
        
        //步数用尽
        if(!stepManager.CanTakeStep()) return;
        
        // 生成可移动格子上的按钮
        Vector3Int[] directions = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };
        foreach (Vector3Int direction in directions)
        {
            Vector3Int targetCell = currentCell + direction;
            Vector3 targetWorldPosition = tilemap.GetCellCenterWorld(targetCell);

            // 检查是否是合法移动位置
            if (tilemap.HasTile(targetCell))
            {
                CreateMoveButton(targetWorldPosition, direction);
            }
        }
    }

    private void CreateMoveButton(Vector3 position, Vector3Int direction)
    {
        // 在目标格子位置生成按钮
        GameObject button = Instantiate(moveButtonPrefab, canvas.transform);
        button.transform.position = Camera.main.WorldToScreenPoint(position);
        
        Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(() => StartCoroutine(MoveInDirection(direction)));
    }

    private System.Collections.IEnumerator MoveInDirection(Vector3Int direction)
    {
        isMoving = true;

        // 计算目标格子位置
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

