using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isHero = collision.gameObject.CompareTag(Constants.HeroTag);
        if (isHero)
        {
            HeroController heroController = collision.gameObject.GetComponent<HeroController>();
            heroController.Flag(transform.position);
            animator.SetBool("Checked", true);
        }
    }
}
