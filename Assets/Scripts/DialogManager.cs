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
using System.Linq;
using UnityEngine.EventSystems;

public class DialogManager : MonoBehaviour
{
    private IDisposable skipStream;

    private TextManager textMgr;
    private EventManager eventMgr;

    private void Awake()
    {
        textMgr = FindObjectOfType<TextManager>();
        eventMgr = FindObjectOfType<EventManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ScriptManager.ReadScript();

        ScriptManager.SetScriptFromID(10001);


        ScriptObject currentScript = ScriptManager.GetCurrentScript();

        ExecuteScript(currentScript);
    }

    public void ExecuteScript(ScriptObject script)
    {
        float skipDuration = script.skipDelay;
        string eventType = script.eventType;

        bool isEvent = false;

        if (eventType != null && eventType.Length != 0)
        {
            isEvent = true;
        }

        if (isEvent == true)
        {
            ExecuteEvent(script);
        }
        else
        {
            ExecuteText(script);
        }
    }

    private void ExecuteText(ScriptObject script)
    {
        Sequence textSeq = textMgr.CreateTextSequence(script);

        float skipDelay = script.skipDelay;


        if (script.skipMethod == SkipMethod.Auto)
        {
            textSeq.AppendInterval(skipDelay);
            textSeq.AppendCallback(() => NextScript());
        }

        textSeq.Play();


        //��ŵ ������ ���
        CreateSkipStream(script, textSeq);
    }

    private void ExecuteEvent(ScriptObject script)
    {
        Sequence eventSeq = eventMgr.CreateEventSequence(script);

        eventSeq.Play();

        CreateSkipStream(script, eventSeq);
    }

    private void CreateSkipStream(ScriptObject script, Sequence sequence)
    {
        skipStream = Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Space))
            .Subscribe(_ => Skip(script, sequence));
    }

    private void Skip(ScriptObject script, Sequence sequence)
    {
        if (sequence.IsActive() && script.skipMethod == SkipMethod.Skipable)
        {
            "�ؽ�Ʈ ���� ��ŵ".Log();
            sequence.Kill(true);
        }
        else if (sequence.IsActive() == false)
        {
            //��ũ��Ʈ ���� �� ���� ��ũ��Ʈ�� �ƴ� ��ȭ ���ᰡ �ǵ��� �ϱ�!

            "���� ���� �̵�".Log();
            NextScript();
        }
    }

    private void NextScript()
    {
        ScriptManager.Next();
        skipStream.Dispose();

        ExecuteScript(ScriptManager.GetCurrentScript());

        "���� ��ũ��Ʈ".�α�();
    }

    public void CloseScript(Action callback)
    {
        //scriptSequence.Kill(true);
        //skipStream.Dispose();

        //callback();
    }
}
