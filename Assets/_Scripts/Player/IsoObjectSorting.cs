using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerSortingOrder : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        UpdateSortingOrder();

        EventManager.OnPlayerMove += UpdateSortingOrder;
    }

    void OnDestroy()
    {
        EventManager.OnPlayerMove -= UpdateSortingOrder;
    }

    void UpdateSortingOrder()
    {
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 10);
    }
}