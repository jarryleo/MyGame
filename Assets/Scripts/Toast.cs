using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toast : MonoBehaviour
{
    private Text text; // 文本框中的Text组件
    private string lastText;
    [Tooltip("文字左右内边距")]
    public float paddingHorizental = 16f;
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        // 获取文本框中的文字内容
        string content = text.text;
        if (content == lastText) return;
        lastText = content;

        // 计算文本框需要的宽度
        float width = content.Length * text.fontSize + paddingHorizental;

        // 设置文本框的宽度
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }

    //显示土司文本，并延迟duration秒后关闭
    public void Show(string msg, float duration = 3)
    {
        if (text == null)
        {
            text = GetComponent<Text>();
        }
        gameObject.SetActive(true);
        text.text = msg;
        StartCoroutine(Hide(duration));
    }

    IEnumerator Hide(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        gameObject.SetActive(false);
    }
}
