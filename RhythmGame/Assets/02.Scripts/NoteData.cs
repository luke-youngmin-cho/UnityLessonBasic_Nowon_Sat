using UnityEngine;

/// <summary>
/// Ư�� �ð��� Ű�� ���� ��Ʈ �����͸� �����ϱ����� Ŭ����
/// </summary>

// Tip
// C# ���� �� ���İ� ���� ������ �ִµ�, 
// �������� ���״�� ���� �а� ���� ����, 
// �������� ���� : �Ϲ����� ������ Ÿ�Ե� ( int, float, double, enum, structure ��... )
// ���������� �ּҸ� �����ؼ� �ش� �ּ��� ���� �а� ���� ����
// ���������� ���� : Ŭ����, �������̽�, ��������Ʈ 
// �������ĵ��� �⺻������ Serialize �� �ȵ� (�ּҰ��� �״�� Serialize �ϴ°��� �ǹ̰� ���⶧����)
// ������ Serialzable �Ӽ��� �ָ�
// ���� ������ Serialize �õ��Ҷ� �ش� �ּ��� �������� �ִ� ������ �о��.

[System.Serializable] // �ش� Ŭ���� Ÿ���� ������Ʈ�� Serialze �����ϵ��� ���ִ� �Ӽ�
public struct NoteData 
{   
    public float time; // ���������� �ð�
    public KeyCode keyCode; // Ű���� �Է�
    public float speed;
}

public enum NoteType
{
    None,
    Down,
    Press
}
