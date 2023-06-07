using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruits : MonoBehaviour
{
    public GameObject fruitCollect;
    //当前水果分值
    [Tooltip("当前水果分值")]
    public int score = 1;

    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider2D;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    //玩家吃到水果
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Constants.HeroTag))
        {
            spriteRenderer.enabled = false;
            circleCollider2D.enabled = false;
            fruitCollect.SetActive(true);
            Destroy(gameObject, 0.5f);
        }
    }
}
