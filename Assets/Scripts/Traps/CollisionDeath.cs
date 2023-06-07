using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDeath : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool isHero = collision.gameObject.CompareTag(Constants.HeroTag);
        if (isHero)
        {
            HeroController heroController = collision.gameObject.GetComponent<HeroController>();
            heroController.Death();
        }
    }
}
