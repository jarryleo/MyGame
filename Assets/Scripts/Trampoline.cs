using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{

    [Tooltip("�Ĵ�����")]
    public float jumpForce = 20f;

    //�Ĵ�����
    private Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Constants.HeroTag))
        {
            animator.SetTrigger("isJump");
            //���ֵ���һ���ԣ����������ٶ�Ӱ��
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            collision.gameObject.GetComponent<HeroController>().DoJump(jumpForce);
        }
    }

}
