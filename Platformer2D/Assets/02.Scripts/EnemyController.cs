using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("���� ����")]
    public EnemyState state;
    public IdleState idleState;
    public MoveState moveState;
    public AttackState attackState;
    public HurtState hurtState;
    public DieState dieState;

    [Header("AI")]
    public AIState aiState;
    public bool aiAutoFollow;
    public float aiDetectRange;
    public bool aiAttackEnable;
    public float aiBehaviorTimeMin;
    public float aiBehaviorTimeMax;
    public float aiBehaviorTimer;
    public LayerMask aiTargetLayer;


    [Header("����")]
    public float moveSpeed = 1f;
    private Vector2 move;
    int _direction; // + 1 : right, - 1 : left
    public int direction
    {
        set
        {
            if (value < 0)
            {
                _direction = -1;
                transform.eulerAngles = Vector3.zero;
            }
            else if (value > 0)
            {
                _direction = 1;
                transform.eulerAngles = new Vector3(0, 180f, 0); 
            }
        }
        get { return _direction; }
    }


    [Header("�ִϸ��̼�")]
    Animator animator;
    float animationTimer;
    float attackTime;
    float hurtTime;
    float dieTime;

    [Header("Kinematics")]
    Rigidbody2D rb;
    CapsuleCollider2D col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        animator = GetComponentInChildren<Animator>();
        attackTime = GetAnimationTime("Attack");
        hurtTime = GetAnimationTime("Hurt");
        dieTime = GetAnimationTime("Die");
    }

    private void Start()
    {
        aiState = AIState.DecideRandomBehavior;
    }

    public void Knockback(Vector2 dir, float force, float time)
    {
        rb.velocity = Vector2.zero;
        StartCoroutine(E_Knockback(dir, force, time));
    }

    IEnumerator E_Knockback(Vector2 dir, float force, float time)
    {
        float timer = time;
        while (timer > 0)
        {
            rb.AddForce(dir * force, ForceMode2D.Force);
            timer -= Time.deltaTime;
            yield return null; // ������ ���
        }
    }


    private void Update()
    {
        UpdateAIState();

        if (move.x < 0) direction = -1;
        else if (move.x > 0) direction = 1;

        if (Mathf.Abs(move.x) > 0)
        {
            if (state == EnemyState.Idle)
                ChangeEnemyState(EnemyState.Move);
        }
        else
        {
            if (state == EnemyState.Move)
                ChangeEnemyState(EnemyState.Idle);
        }

        UpdateEnemyState();
    }

    private void UpdateAIState()
    {
        if (aiAutoFollow)
        {
            if (Physics2D.OverlapCircle(rb.position, aiDetectRange, aiTargetLayer))
                aiState = AIState.FollowTarget;
        }

        switch (aiState)
        {
            case AIState.Idle:
                break;
            case AIState.DecideRandomBehavior:
                move.x = 0;
                aiBehaviorTimer = Random.Range(aiBehaviorTimeMin, aiBehaviorTimeMax);
                aiState = (AIState)Random.Range(2, 5);
                break;
            case AIState.TakeARest:
                if (aiBehaviorTimer < 0)
                    aiState = AIState.DecideRandomBehavior;
                else
                    aiBehaviorTimer -= Time.deltaTime;
                break;
            case AIState.MoveLeft:
                if (aiBehaviorTimer < 0)
                    aiState = AIState.DecideRandomBehavior;
                else
                {
                    move.x = -1;
                    aiBehaviorTimer -= Time.deltaTime;
                }
                break;
            case AIState.MoveRight:
                if (aiBehaviorTimer < 0)
                    aiState = AIState.DecideRandomBehavior;
                else
                {
                    move.x = 1;
                    aiBehaviorTimer -= Time.deltaTime;
                }
                break;
            case AIState.FollowTarget:
                
                Collider2D target = Physics2D.OverlapCircle(rb.position, aiDetectRange, aiTargetLayer);

                // Ÿ���� ������ ������� �ٽ� �ൿ �ٲ�
                if (target == null)
                {
                    aiState = AIState.DecideRandomBehavior;
                }   
                // Ÿ���� �����ȿ� ������
                else
                {
                    // Ÿ�� ���󰡱�
                    if (target.transform.position.x > rb.position.x + col.size.x)
                        move.x = 1;
                    else if (target.transform.position.x < rb.position.x - col.size.x)
                        move.x = -1;
                }
                break;
            case AIState.AttackTarget:
                break;
            default:
                break;
        }
    }

    private void UpdateEnemyState()
    {
        switch (state)
        {
            case EnemyState.Idle:
                UpdateIdleState();
                break;
            case EnemyState.Move:
                UpdateMoveState();
                break;
            case EnemyState.Attack:
                UpdateAttackState();
                break;
            case EnemyState.Hurt:
                UpdateHurtState();
                break;
            case EnemyState.Die:
                UpdateDieState();
                break;
            default:
                break;
        }
    }

    private void UpdateIdleState()
    {
        switch (idleState)
        {
            case IdleState.Idle:
                break;
            case IdleState.Prepare:
                animator.Play("Idle");
                idleState++;
                break;
            case IdleState.Casting:
                idleState++;
                break;
            case IdleState.OnAction:
                // �ƹ��͵� ����
                break;
            case IdleState.Finish:
                break;
            default:
                break;
        }
    }

    private void UpdateMoveState()
    {
        switch (moveState)
        {
            case MoveState.Idle:
                break;
            case MoveState.Prepare:
                animator.Play("Move");
                moveState++;
                break;
            case MoveState.Casting:
                moveState++;
                break;
            case MoveState.OnAction:
                // �ƹ��͵� ����
                break;
            case MoveState.Finish:
                break;
            default:
                break;
        }
    }

    private void UpdateAttackState()
    {

    }

    private void UpdateHurtState()
    {
        switch (hurtState)
        {
            case HurtState.Idle:
                break;
            case HurtState.Prepare:
                animator.Play("Hurt");
                animationTimer = hurtTime;
                hurtState++;
                break;
            case HurtState.Casting:
                hurtState++;
                break;
            case HurtState.OnAction:
                if (animationTimer < 0)
                    hurtState++;
                else
                    animationTimer -= Time.deltaTime;
                break;
            case HurtState.Finish:
                ChangeEnemyState(EnemyState.Idle);
                break;
            default:
                break;
        }
    }

    private void UpdateDieState()
    {
        switch (dieState)
        {
            case DieState.Idle:
                break;
            case DieState.Prepare:
                animator.Play("Die");
                animationTimer = dieTime;
                dieState++;
                break;
            case DieState.Casting:
                dieState++;
                break;
            case DieState.OnAction:
                if (animationTimer < 0)
                    dieState++;
                else
                    animationTimer -= Time.deltaTime;
                break;
            case DieState.Finish:
                Destroy(this.gameObject);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// ���ʹ� ���¸� �ٲٴ� �Լ�
    /// ���� �������� �ʱ�ȭ�ϰ�, ���ο� ���¸� ���� �������¸� �غ���·� ������.
    /// </summary>
    /// <param name="newState"> �ٲٰ����ϴ� ���ο� ����</param>
    public void ChangeEnemyState(EnemyState newState)
    {
        if (state == newState) return;

        // �������� �ʱ�ȭ
        switch (state)
        {
            case EnemyState.Idle:
                idleState = IdleState.Idle;
                break;
            case EnemyState.Move:
                moveState = MoveState.Idle;
                break;
            case EnemyState.Attack:
                attackState = AttackState.Idle;
                break;
            case EnemyState.Hurt:
                hurtState = HurtState.Idle;
                break;
            case EnemyState.Die:
                dieState = DieState.Idle;
                break;
            default:
                break;
        }

        // ���� ����
        state = newState;

        // ���ŵ� ������ �������� �ӽ� �غ�
        switch (state)
        {
            case EnemyState.Idle:
                idleState = IdleState.Prepare;
                break;
            case EnemyState.Move:
                moveState = MoveState.Prepare;
                break;
            case EnemyState.Attack:
                attackState = AttackState.Prepare;
                break;
            case EnemyState.Hurt:
                hurtState = HurtState.Prepare;
                break;
            case EnemyState.Die:
                dieState = DieState.Prepare;
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        rb.position += new Vector2(move.x * moveSpeed, move.y) * Time.fixedDeltaTime;
    }


    private float GetAnimationTime(string name)
    {
        float time = 0f;
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
        for (int i = 0; i < ac.animationClips.Length; i++)
        {
            if (ac.animationClips[i].name == name)
                time = ac.animationClips[i].length;
        }

        return time;
    }

    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, aiDetectRange);
    }

    public enum AIState
    {
        Idle,
        DecideRandomBehavior,
        TakeARest,
        MoveLeft,
        MoveRight,
        FollowTarget,
        AttackTarget,
    }

    public enum IdleState
    {
        Idle,
        Prepare,
        Casting,
        OnAction,
        Finish
    }

    public enum MoveState
    {
        Idle,
        Prepare,
        Casting,
        OnAction,
        Finish
    }

    public enum AttackState
    {
        Idle,
        Prepare,
        Casting,
        OnAction,
        Finish
    }

    public enum HurtState
    {
        Idle,
        Prepare,
        Casting,
        OnAction,
        Finish
    }

    public enum DieState
    {
        Idle,
        Prepare,
        Casting,
        OnAction,
        Finish
    }
}

public enum EnemyState
{
    Idle,
    Move,
    Attack,
    Hurt,
    Die,
}

