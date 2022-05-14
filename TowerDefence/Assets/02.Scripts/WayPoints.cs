using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    public static WayPoints instance;
    private Transform[] points;

    /// <summary>
    /// ù��° ����Ʈ
    /// </summary>
    public Transform GetFirstWayPoint()
    {
        return points[0];
    }

    /// <summary>
    /// ���� ����Ʈ ���������Լ�
    /// </summary>
    /// <param name="currentPointIndex"> ���� ����Ʈ �ε��� </param>
    /// <param name="nextPoint"> ��ȯ�� ���� ����Ʈ Transform </param>
    /// <returns> ��������Ʈ �������µ� ����: true ����: false </returns>
    public bool TryGetNextWayPoint(int currentPointIndex , out Transform nextPoint)
    {
        nextPoint = null;

        if (currentPointIndex < points.Length - 1)
        {
            nextPoint = points[currentPointIndex + 1];
            return true;
        }

        return false;
    }

    private void Awake()
    {
        if ( instance != null)
            Destroy(instance);
        instance = this;

        points = new Transform[transform.childCount];
        for (int i = 0; i < points.Length; i++)
            points[i] = transform.GetChild(i);
    }
}
