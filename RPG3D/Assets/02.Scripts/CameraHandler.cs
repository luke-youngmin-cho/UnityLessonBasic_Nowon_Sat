using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    private Transform target;
    [SerializeField] private float minDistance = 3; // ī�޶�� Ÿ�ٰ��� �ּҰŸ�
    [SerializeField] private float maxDistance = 30; // ī�޶�� Ÿ�ٰ��� �ִ�Ÿ�
    [SerializeField] private float wheelSpeed = 500; // ���콺 �� �ӵ�
    [SerializeField] private float xPointSpeed = 500; // ���콺 x �� �̵��ӵ�
    [SerializeField] private float yPointSpeed = 500; // ���콺 y �� �̵��ӵ�
    private float yMinLimit = 5;
    private float yMaxLimit = 80;
    private float x, y; // ���콺 ��ġ
    private float distance; //ī�޶�� Ÿ�ٰ��� �Ÿ�
    private Transform tr;

    private void Awake()
    {
        tr = GetComponent<Transform>();
        x = tr.eulerAngles.y;
        y = tr.eulerAngles.x;
    }

    private void Start()
    {
        target = PlayerMove.instance.transform;
        distance = Vector3.Distance(tr.position, target.position);
    }

    private void Update()
    {
        x += Input.GetAxis("Mouse X") * xPointSpeed * Time.deltaTime;
        y -= Input.GetAxis("Mouse Y") * yPointSpeed * Time.deltaTime;

        ClampAngle(ref y, yMinLimit, yMaxLimit);
        tr.rotation = Quaternion.Euler(y, x, 0);

        distance -= Input.GetAxis("Mouse ScrollWheel") * wheelSpeed * Time.deltaTime;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
    }

    private void LateUpdate()
    {
        tr.position = tr.rotation * new Vector3(0, 0, -distance) + target.position;
    }

    private void ClampAngle(ref float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;

        angle = Mathf.Clamp(angle, min, max);
    }
}

public struct st_Coord
{
    public float x, y;
}

public class c_Coord
{
    public float x, y;
}