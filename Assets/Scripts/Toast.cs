using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toast : MonoBehaviour
{
    private Text text; // �ı����е�Text���
    private string lastText;
    [Tooltip("���������ڱ߾�")]
    public float paddingHorizental = 16f;
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        // ��ȡ�ı����е���������
        string content = text.text;
        if (content == lastText) return;
        lastText = content;

        // �����ı�����Ҫ�Ŀ��
        float width = content.Length * text.fontSize + paddingHorizental;

        // �����ı���Ŀ��
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }

    //��ʾ��˾�ı������ӳ�duration���ر�
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
