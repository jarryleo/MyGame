using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    [Tooltip("指定下一场景名称")]
    public string nextScene;

    private int heroCount = 0; //进入终点的英雄数量
    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isHero = collision.gameObject.CompareTag(Constants.HeroTag);
        if (isHero)
        {
            heroCount++;
            if (nextScene != null && heroCount >= 2)
            {
                SceneManager.LoadScene(nextScene);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        bool isHero = collision.gameObject.CompareTag(Constants.HeroTag);
        if (isHero)
        {
            heroCount--;
        }
    }
}
