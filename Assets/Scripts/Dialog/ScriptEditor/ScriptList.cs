using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScriptList : MonoBehaviour
{
    public ScriptObject script;

    public Text scriptText;
    public GameObject selectHighlight;
    public Button selectButton;

    /*
     * ���ʿ� �ǳ�? ����
     * �ؽ�Ʈ��
     * �̺�Ʈ
     * �����ְ�.....
     * �Ƥ�������������������������
     * �밡���ͤӤ���
     * 
     * */

    public void SetText(string text)
    {
        scriptText.text = text;
    }

    public void SetHighlight(bool highlight)
    {
        selectHighlight.SetActive(highlight);
    }

    public void SetButtonCallback(UnityAction callback)
    {
        selectButton.onClick.AddListener(callback);
    }
}
