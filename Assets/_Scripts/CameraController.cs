using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform player;
    public float followThreshold = 5.0f; // 玩家和相机中心允许的最大偏移距离
    public float followSpeed = 5.0f;

    private Vector3 offset; // 相机和玩家的初始偏移量
    private bool isMoving;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();

        offset = new Vector2(transform.position.x - player.position.x, transform.position.y - player.position.y);
    }


    void LateUpdate()
    {
        Vector3 targetPosition = player.position + offset;
        
        Vector2 current2D = new Vector2(transform.position.x, transform.position.y);
        Vector2 target2D = new Vector2(targetPosition.x, targetPosition.y);
        
        if (Vector2.Distance(current2D, target2D) > followThreshold)
        {
            StartCoroutine(SmoothMove(new Vector3(targetPosition.x, targetPosition.y, transform.position.z)));
        }
    }
    
    
    IEnumerator SmoothMove(Vector3 targetPosition)
    {
        isMoving = true;

        // 平滑动画：逐渐移动相机到目标位置
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, 0.5f * Time.deltaTime);
            yield return null;
        }

        // 强制对齐最终位置，避免因距离太小导致不精确
        transform.position = targetPosition;

        isMoving = false;
    }
}
