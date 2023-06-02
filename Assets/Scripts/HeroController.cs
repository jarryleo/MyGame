using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    enum State
    {
        Born,
        Idle,
        Running,
        Jumping,
        Falling,
        Death
    }
    //玩家当前状态
    private State playerCurrentState = State.Born;
    //玩家状态动画
    private Animator animator;
    //玩家移动速度
    public float moveSpeed = 5f;
    //玩家跳跃高度
    public float jumpForce = 15f;
    //玩家编号对应的手柄
    public int playerNumber = 1;

    //脚下是否踩到物体
    private bool isGrounded = true;

    //玩家刚体
    private Rigidbody2D rb;
    //射线生效图层
    private LayerMask layerMask;
    //记录玩家出生位置
    private Vector3 bornPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        layerMask = LayerMask.GetMask("Bg");
        bornPosition = transform.position;
        rb.isKinematic = true;
    }

    private void Update()
    {
        if (playerCurrentState == State.Born || playerCurrentState == State.Death) return;
        Check();
        Jump();
        Fall();
    }

    private void FixedUpdate()
    {
        if (playerCurrentState == State.Born || playerCurrentState == State.Death) return;
        Run();
    }

    //检查玩家脚底状态
    private void Check()
    {
        //检测玩家脚下是否有可踩踏物品
        isGrounded = RayCastCheck(Vector2.down);
    }

    //检测玩家某个方向上是否有可以踩踏物体
    private bool RayCastCheck(Vector2 vector2)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, vector2, 0.6f, layerMask);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Ground"))
            {
                return true;
            }

        }
        return false;
    }

    private void Run()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        //在地面上且没有移动为待机状态
        if (moveHorizontal == 0 && isGrounded)
        {
            SetPlayerState(State.Idle);
            return;
        }
        //地面人物移动状态切换
        if (moveHorizontal != 0 && isGrounded)
        {
            SetPlayerState(State.Running);
        }
        //根据左右运动方向修改人物朝向
        if (moveHorizontal < 0)
        {
            transform.eulerAngles = new Vector3(0, 180f, 0);
        }
        else if (moveHorizontal > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        //人物移动
        transform.position += new Vector3(moveHorizontal, 0, 0) * moveSpeed * Time.fixedDeltaTime;
    }

    private void Jump()
    {
        //按下跳跃键检测
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            DoJump();
        }
    }

    //执行跳跃动作
    public void DoJump(float force = 0)
    {
        float jf = force;
        if (force == 0 )
        {
            jf = jumpForce;
        }
        isGrounded = false;
        rb.AddForce(new Vector2(0f, jf), ForceMode2D.Impulse);
        SetPlayerState(State.Jumping);
    }

    private void Fall()
    {
        bool isFall = rb.velocity.y < -0.1; //y轴速度小于0，则是下落 (轻微变化不做下坠动画切换)
        if (isFall)
        {
            SetPlayerState(State.Falling);
        }
    }

    public void Idle()
    {
        rb.isKinematic = false;
        SetPlayerState(State.Idle);
    }


    //设置玩家状态
    private void SetPlayerState(State state)
    {
        if (state == playerCurrentState) return;
        playerCurrentState = state;
        switch (state)
        {
            case State.Born:
                animator.SetBool("isDeath", false);
                animator.SetBool("isFall", false);
                animator.SetBool("isRun", false);
                animator.SetBool("isJump", false);
                break;
            case State.Idle:
                animator.SetBool("isFall", false);
                animator.SetBool("isRun", false);
                animator.SetBool("isJump", false);
                break;
            case State.Running:
                animator.SetBool("isRun", true);
                animator.SetBool("isFall", false);
                animator.SetBool("isJump", false);
                break;
            case State.Jumping:
                animator.SetBool("isJump", true);
                animator.SetBool("isFall", false);
                break;
            case State.Falling:
                animator.SetBool("isFall", true);
                break;
            case State.Death:
                animator.SetBool("isDeath", true);
                break;
        }
    }

    public void Born()
    {
        transform.position = bornPosition;
        SetPlayerState(State.Born);
    }

    public void Flag(Vector3 flagPosition)
    {
        bornPosition = flagPosition;
    }

    public void Death()
    {
        rb.isKinematic = true;
        //角色停止运动
        rb.velocity = Vector3.zero; 
        //执行死亡动画
        SetPlayerState(State.Death);
    }
}
