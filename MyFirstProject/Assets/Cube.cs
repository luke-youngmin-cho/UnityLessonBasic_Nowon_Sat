using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    // �ʵ��� �ν�����â ���� ����
    // public : �ܺ� Ŭ���� ���� ����, �ν�����â ������
    // private : �ܺ� Ŭ���� ���� �Ұ�, �ν�����â ���� ���� 
    // [HideInInsepector] public : �ܺ� Ŭ���� ���� ����, �ν�����â ���� ����
    // [SerializeField] private : �ܺ� Ŭ���� ���� �Ұ�, �ν�����â ������

    // this Ű���� 
    // ��ü �ڽ��� ��ȯ�ϴ� Ű����

    public int a = 3;
    private Transform tr;

    Vector3 move;

    private void Awake()
    {
        Debug.Log(this);
        Debug.Log(this.gameObject);
        Debug.Log(gameObject);

        tr = this.gameObject.GetComponent<Transform>();
        tr = gameObject.GetComponent<Transform>();
        tr = GetComponent<Transform>();
        tr = transform;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        tr.position = Vector3.zero;         
    }
    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Debug.Log($"h = {h}, v = {v}");
        move = new Vector3(h, 0, v);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        // position ����ÿ��� 
        // position �� �����ӽð��� ��ȭ ���� �����־�� �Ѵ�.
        // �ð� �� ��ġ ��ȭ��(�ӵ�) = ��ġ��ȭ�� / �ð�
        // ������ �ð� �� ��ġ ��ȭ��(�����Ӵ��� �ӵ�) = ��ġ��ȭ�� / ������ �ð�
        // ��ġ��ȭ�� = ������ �ð� �� ��ġ ��ȭ�� * ������ �ð�
        tr.position += move * Time.fixedDeltaTime;
        
    }
}
