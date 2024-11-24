using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform player;
    public float followThreshold = 5.0f; // ��Һ����������������ƫ�ƾ���
    public float followSpeed = 5.0f;

    private Vector3 offset; // �������ҵĳ�ʼƫ����
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

        // ƽ�����������ƶ������Ŀ��λ��
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, 0.5f * Time.deltaTime);
            yield return null;
        }

        // ǿ�ƶ�������λ�ã����������̫С���²���ȷ
        transform.position = targetPosition;

        isMoving = false;
    }
}
