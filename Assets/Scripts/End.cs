using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    [Tooltip("ָ����һ��������")]
    public string nextScene;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isHero = collision.gameObject.CompareTag(Constants.HeroTag);
        if (isHero)
        {
            if (nextScene != null)
            {
                SceneManager.LoadScene(nextScene);
            }
        }
    }
}
