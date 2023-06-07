using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{

    [Tooltip("蹦床弹力")]
    public float jumpForce = 20f;

    //蹦床动画
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
            //保持弹力一致性，不受下落速度影响
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            collision.gameObject.GetComponent<HeroController>().DoJump(jumpForce);
        }
    }

}
