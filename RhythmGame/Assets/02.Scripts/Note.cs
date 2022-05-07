using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public KeyCode keyCode;
    public float speed = 1f;
    Transform tr;

    //====================================================
    //****************** Public Methods ******************
    //====================================================

    public void Hit(HitType hitType)
    {
        switch (hitType)
        {
            case HitType.Bad:
                break;
            case HitType.Miss:
                break;
            case HitType.Good:
                // todo -> ���� ����
                break;
            case HitType.Great:
                // todo -> ���� ����
                break;
            case HitType.Cool:
                // todo -> ���� ����
                break;
            default:
                break;
        }
        // todo -> ��ƮŸ�� �˾� UI On
    }

    //====================================================
    //****************** Private Methods *****************
    //====================================================

    private void Awake()
    {
        tr = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        tr.Translate(Vector2.down * speed * Time.fixedDeltaTime);
    }
    
}

public enum HitType
{
    Bad,
    Miss,
    Good,
    Great,
    Cool
}