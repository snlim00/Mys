using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UniRx;
using System;
using System.Reflection;

public class DialogManager : MonoBehaviour
{
    [SerializeField]
    private Text textBox;

    [SerializeField]
    private Text characterName;

    private IDisposable skipStream;

    // Start is called before the first frame update
    void Start()
    {
        ScriptManager.scripts = CSVReader.ReadScript("Data/ScriptTable.CSV");

        ScriptManager.GetCurrentScript();


        ExecuteScript();
    }

    public void ExecuteScript()
    {
        ScriptObject currentScript = ScriptManager.GetCurrentScript();

        float skipDuration = currentScript.skipDuration;
        string eventType = currentScript.eventType;

        bool isEvent = false;

        if (eventType != null && eventType.Length != 0)
        {
            isEvent = true;
        }

        if (isEvent == true)
        {
            //var eventParam = currentScript.eventParam;

            //MethodInfo eventMethod = this.GetType().GetMethod(eventType);
            //eventMethod.Invoke(this, eventParam);
        }
        else
        {
            ExecuteDialog(currentScript);
        }
    }

    private void ExecuteDialog(ScriptObject script)
    {
        string characterName = script.characterName;
        this.characterName.text = characterName;

        string text = script.text;
        float textDuration = script.textDuration;

        float skipDuration = script.skipDuration;

        var textSequence = DOTween.Sequence().Pause();
        textSequence.AppendCallback(() => textBox.text = "");

        if (textDuration == 0) //textDuration�� 0�̶�� DOText�� �������� �ʰ� �׳� �ؽ�Ʈ�� �����ϵ��� ��. (�� �׷��� �������� �� �̻������� ��) 221219
        {
            textSequence.AppendCallback(() => textBox.text = text);
        }
        else //textDuration�� �� ���ڰ� �����Ǵ� �� �ɸ��� �ð��̹Ƿ� text�� Length�� ���ؼ� ���.
        {
            textSequence.Append(textBox.DOText(text, text.Length * textDuration).SetEase(Ease.Linear)); //SetEase�� ������ ������ �⺻������ �ٸ� ����� Ease�� �����. 221219
        }

        textSequence.AppendCallback(() => "�ؽ�Ʈ ������ ����".�α�());
        var skipSequence = DOTween.Sequence().Pause();
        skipSequence.AppendInterval(skipDuration);
        skipSequence.AppendCallback(() => NextScript()); //skipDuration �ڿ� ��ŵ ������ ���·� ���� ��. (����� skipDuration �ڿ� �ڵ����� ��ŵ��.)


        if (script.skipMethod == SkipMethod.Auto)
        {
            //textSequence.AppendCallback(() => NextScript());
        }
        else
        {
            textSequence.Append(skipSequence);
        }

        textSequence.Play();


        //��ŵ ������ ���
        skipStream = Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Space))
            .Subscribe(_ => Skip(script, textSequence));
    }

    private void Skip(ScriptObject script, Sequence textSequence)
    {
        if (textSequence.IsActive() && script.skipMethod == SkipMethod.Skipable)
        {
            "�ؽ�Ʈ ���� ��ŵ".Log();
            textSequence.Kill(true);
        }
        else if (textSequence.IsActive() == false)
        {
            "���� ���� �̵�".Log();
            NextScript();
        }
    }

    private void NextScript()
    {
        ScriptManager.Next();
        skipStream.Dispose();

        ExecuteScript();

        "���� ��ũ��Ʈ".�α�();
    }

    public void CloseScript(Action callback)
    {
        //scriptSequence.Kill(true);
        //skipStream.Dispose();

        //callback();
    }
}
