using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float posTolerance = 0.1f;
    private Transform tr;
    private int currentPointIndex = 0;
    private Transform nextWayPoint;
    private float originPosY;


    private void Awake()
    {
        tr = GetComponent<Transform>();
        originPosY = tr.position.y;
    }

    private void Start()
    {
        nextWayPoint = WayPoints.instance.GetFirstWayPoint();
    }

    private void FixedUpdate()
    {
        Vector3 targetPos = new Vector3(nextWayPoint.position.x,
                                        originPosY,
                                        nextWayPoint.position.z);

        Vector3 dir = (targetPos - tr.position).normalized;

        // Ÿ����ġ�� �����ߴ��� üũ
        if (Vector3.Distance(tr.position, targetPos) < posTolerance)
        {
            // ���� Ÿ�� ����Ʈ �޾ƿ�
            if (WayPoints.instance.TryGetNextWayPoint(currentPointIndex, out nextWayPoint))
            {
                currentPointIndex++;
                tr.LookAt(nextWayPoint);
            }
            // ���� ����
            else
            {
                OnReachedToEnd();
            }
        }

        tr.Translate(dir * moveSpeed * Time.fixedDeltaTime, Space.World);
    }

    private void OnReachedToEnd()
    {
        gameObject.SetActive(false);
    }
}
