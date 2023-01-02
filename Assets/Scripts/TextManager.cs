using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;
using System;
using System.Reflection;
using System.Linq;

public class TextManager : MonoBehaviour
{
    [SerializeField] private Text textBox;

    [SerializeField] private Text characterName;

    public Sequence CreateTextSequence(ScriptObject script)
    {
        string characterName = script.characterName;
        this.characterName.text = characterName;

        string text = script.text;
        float textDuration = script.textDuration;

        float skipDuration = script.skipDelay;

        Sequence textSeq = DOTween.Sequence();
        textSeq.AppendCallback(() => textBox.text = "");

        if (textDuration == 0) //textDuration�� 0�̶�� DOText�� �������� �ʰ� �׳� �ؽ�Ʈ�� �����ϵ��� ��. (�� �׷��� �������� �� �̻������� ��) 221219
        {
            textSeq.AppendCallback(() => textBox.text = text);
        }
        else //textDuration�� �� ���ڰ� �����Ǵ� �� �ɸ��� �ð��̹Ƿ� text�� Length�� ���ؼ� ���.
        {
            textSeq.Append(textBox.DOText(text, text.Length * textDuration));
        }

        return textSeq;
    }
}
