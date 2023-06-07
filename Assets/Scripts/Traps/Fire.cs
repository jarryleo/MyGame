using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isHero = collision.gameObject.CompareTag(Constants.HeroTag);
        if (isHero)
        {
            HeroController heroController = collision.gameObject.GetComponent<HeroController>();
            heroController.Death();
        }
    }
}
