using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine_Jump : PlayerStateMachine
{
    [SerializeField] private float jumpForce;
    [SerializeField] LayerMask groundLayer;
    private GroundSensor groundSensor;
    private Rigidbody rb;
    private CapsuleCollider col;
    private float jumpDownAnimationTime;
    private bool onJumpDown;

    protected override void Awake()
    {
        base.Awake();
        groundSensor = GetComponentInChildren<GroundSensor>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        delay = 0.1f;
    }

    private void Start()
    {
        jumpDownAnimationTime = playerAnimator.GetClipTime("Jump_Down");
    }

    public override bool isExecuteOK()
    {
        if (groundSensor.isOn &&
            manager.state == PlayerState.Move &&
            playerAnimator.IsClipPlaying("Jump StateMachine") == false)
            return true;
        return false;
    }

    public override PlayerState UpdateState()
    {
        PlayerState nextState = playerState;

        switch (state)
        {
            case State.Idle:
                break;
            case State.Prepare:
                playerAnimator.SetBool("DoJump", true);
                delayTimer = delay;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                state++;
                break;
            case State.OnDelay:
                if (delayTimer < 0)
                {
                    playerAnimator.SetBool("DoJump", false);
                    state++;
                }
                delayTimer -= Time.deltaTime;
                break;
            case State.Casting:
                if (groundSensor.isOn == false)
                    state++;
                break;
            case State.OnAction:
                // 하강중에 지면과의 거리를 재서
                // JumpDown aniamtion 실행 시간에 맞춰서 animation 재생
                
                if (onJumpDown == false &&
                    Physics.Raycast(rb.position,
                                    Vector3.down,
                                    out RaycastHit hit,
                                    100.0f
                                    ))
                {
                    Debug.DrawRay(rb.position, Vector3.down * 100.0f, Color.red);
                    float gravity = PlayerMove.instance.gravity;
                    float s = hit.distance;
                    float b = 2 * rb.velocity.y / gravity;
                    float c = -2 * s / gravity;
                    float d = b * b - 4 * c;

                    if (d >= 0)
                    {
                        float r = (-b + Mathf.Sqrt(d)) / 2;
                        if (r >= 0 &&
                            r <= jumpDownAnimationTime)
                        {
                            playerAnimator.SetTrigger("DoJumpDown");
                            onJumpDown = true;
                        }                           
                    }
                }
                else if (onJumpDown == false)
                {
                    playerAnimator.SetTrigger("DoFalling");
                }
                else
                {
                    Debug.DrawRay(rb.position, Vector3.down * 100.0f, Color.green);
                }

                // downJump 가 끝나면 현재 state machine 완료
                if (onJumpDown &&
                    groundSensor.isOn &&
                    playerAnimator.IsClipPlaying("Jump_Down") == false)
                {
                    state++;
                }
                break;
            case State.Finish:
                nextState = PlayerState.Move;
                onJumpDown = false;
                break;
            default:
                break;
        }

        return nextState;
    }

    public override void FixedUpdateState()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.Prepare:
                break;
            case State.OnDelay:
                if (groundSensor.isOn)
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
                break;
            case State.Casting:
                if (groundSensor.isOn)                    
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
                break;
            case State.OnAction:
                break;
            case State.Finish:
                break;
            default:
                break;
        }
    }
}
