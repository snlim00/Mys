using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    [SerializeField] private Text textBox;
    [SerializeField] private Text characterName;

    public Sequence CreateTextSequence(ScriptObject script)
    {
        Sequence seq = DOTween.Sequence();

        string text = script.text.Replace("<br>", Environment.NewLine);

        seq.AppendCallback(() => characterName.text = script.characterName);
        seq.AppendCallback(() => textBox.text = ""); //�ؽ�Ʈ�ڽ� ����

        if (script.textDuration == 0) //textDuration�� 0�̶�� DOText�� �������� �ʰ� �׳� �ؽ�Ʈ�� �����ϵ��� ��. (�� �׷��� �������� �� �̻������� ��) 221219
        {
            seq.AppendCallback(() => textBox.text = text);
        }
        else //textDuration�� �� ���ڰ� �����Ǵ� �� �ɸ��� �ð��̹Ƿ� text�� Length�� ���ؼ� ���.
        {
            seq.Append(textBox.DOText(text, text.Length * script.textDuration));
        }

        return seq;
    }
}
