using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


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
    //脚下是否踩到物体
    private bool isGrounded = true;
    //是否站在另一个玩家上面
    private bool isOnHero = true;

    //玩家刚体
    private Rigidbody2D rb;
    //射线生效图层
    private LayerMask layerMask;
    //记录玩家出生位置
    private Vector3 bornPosition;

    //玩家移动速度
    public float moveSpeed = 5f;
    //玩家跳跃高度
    public float jumpForce = 15f;
    //移动
    private float moveHorizontal = 0f;


    //玩家编号
    public int playerNumber = 1;
    [Tooltip("玩家可跳跃物体tag")]
    public List<string> canJumpTags = new();
    //用户输入控制
    private PlayerInput playerInput;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        layerMask = LayerMask.GetMask("Bg");
        bornPosition = transform.position;
        rb.isKinematic = true;
        //注册交互事件
        playerInput = GetComponent<PlayerInput>();
        playerInput.onActionTriggered += OnJump;
        playerInput.onActionTriggered += OnMove;
        //分配手柄
        InputManager.BindPlayerInput(playerNumber, playerInput);

        canJumpTags.Add("Ground");
        canJumpTags.Add("Hero");
    }

    void Update()
    {
        if (BanInput()) return;
        Check();
        Fall();
    }

    void FixedUpdate()
    {
        if (BanInput()) return;
        Run();
    }

    //是否可以控制
    private bool BanInput()
    {
        bool bornOrDeath = playerCurrentState == State.Born || playerCurrentState == State.Death;
        return bornOrDeath;
    }

    //检查玩家脚底状态
    private void Check()
    {
        //检测玩家脚下是否有可踩踏物品
        isGrounded = RayCastCheck(Vector2.down) || isOnHero;
    }

    private void Run()
    {
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
        transform.position += new Vector3(moveHorizontal, 0, 0) * moveSpeed * Time.deltaTime;
    }

    //检测玩家某个方向上是否有可以踩踏物体
    private bool RayCastCheck(Vector2 vector2)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, vector2, 0.6f, layerMask);
        if (hit.collider != null)
        {
            if (canJumpTags.Contains(hit.collider.tag))
            {
                return true;
            }

        }
        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool isHero = collision.collider.CompareTag("Hero");
        if (isHero && transform.position.y > collision.transform.up.y)
        {
            isOnHero = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        bool isHero = collision.collider.CompareTag("Hero");
        if (isHero)
        {
            isOnHero = false;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (BanInput()) return;
        if (context.action.name != "Move") return;

        Vector2 move = context.action.ReadValue<Vector2>();
        moveHorizontal = (move.x);
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (BanInput()) return;
        if (context.action.name != "Jump") return;

        if (isGrounded)
        {
            DoJump();
        }
    }

    //执行跳跃动作
    public void DoJump(float force = 0)
    {
        float jf = force;
        if (force == 0)
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
        moveHorizontal = 0f;
        transform.position = bornPosition;
        SetPlayerState(State.Born);
    }

    public void Flag(Vector3 flagPosition)
    {
        bornPosition = flagPosition;
    }

    public void Death()
    {
        moveHorizontal = 0f;
        rb.isKinematic = true;
        //角色停止运动
        rb.velocity = Vector3.zero;
        //执行死亡动画
        SetPlayerState(State.Death);
    }
}
