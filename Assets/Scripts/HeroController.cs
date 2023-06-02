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
    //��ҵ�ǰ״̬
    private State playerCurrentState = State.Born;
    //���״̬����
    private Animator animator;
    //����ƶ��ٶ�
    public float moveSpeed = 5f;
    //�����Ծ�߶�
    public float jumpForce = 15f;
    //��ұ�Ŷ�Ӧ���ֱ�
    public int playerNumber = 1;

    //�����Ƿ�ȵ�����
    private bool isGrounded = true;

    //��Ҹ���
    private Rigidbody2D rb;
    //������Чͼ��
    private LayerMask layerMask;
    //��¼��ҳ���λ��
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

    //�����ҽŵ�״̬
    private void Check()
    {
        //�����ҽ����Ƿ��пɲ�̤��Ʒ
        isGrounded = RayCastCheck(Vector2.down);
    }

    //������ĳ���������Ƿ��п��Բ�̤����
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
        //�ڵ�������û���ƶ�Ϊ����״̬
        if (moveHorizontal == 0 && isGrounded)
        {
            SetPlayerState(State.Idle);
            return;
        }
        //���������ƶ�״̬�л�
        if (moveHorizontal != 0 && isGrounded)
        {
            SetPlayerState(State.Running);
        }
        //���������˶������޸����ﳯ��
        if (moveHorizontal < 0)
        {
            transform.eulerAngles = new Vector3(0, 180f, 0);
        }
        else if (moveHorizontal > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        //�����ƶ�
        transform.position += new Vector3(moveHorizontal, 0, 0) * moveSpeed * Time.fixedDeltaTime;
    }

    private void Jump()
    {
        //������Ծ�����
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            DoJump();
        }
    }

    //ִ����Ծ����
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
        bool isFall = rb.velocity.y < -0.1; //y���ٶ�С��0���������� (��΢�仯������׹�����л�)
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


    //�������״̬
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
        //��ɫֹͣ�˶�
        rb.velocity = Vector3.zero; 
        //ִ����������
        SetPlayerState(State.Death);
    }
}
