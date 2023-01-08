using DG.Tweening;
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

        seq.AppendCallback(() => textBox.text = ""); //�ؽ�Ʈ�ڽ� ����

        if (script.textDuration == 0) //textDuration�� 0�̶�� DOText�� �������� �ʰ� �׳� �ؽ�Ʈ�� �����ϵ��� ��. (�� �׷��� �������� �� �̻������� ��) 221219
        {
            seq.AppendCallback(() => textBox.text = script.text);
        }
        else //textDuration�� �� ���ڰ� �����Ǵ� �� �ɸ��� �ð��̹Ƿ� text�� Length�� ���ؼ� ���.
        {
            seq.Append(textBox.DOText(script.text, script.text.Length * script.textDuration));
        }

        return seq;
    }
}
