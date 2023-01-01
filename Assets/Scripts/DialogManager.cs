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
using UnityEngine.Assertions.Must;
using UnityEditor.PackageManager;

public class DialogManager : MonoBehaviour
{
    private IDisposable skipStream = null;

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
        ("ExecuteScript: " + script.scriptID).Log();

        bool isEvent = script.isEvent;

        Sequence sequence = null;

        if (isEvent == true)
        {
            sequence = CreateEventSequence(script); //��ũ��Ʈ ���ᵵ �̺�Ʈ���� ó���� ����. 221223
        }
        else
        {
            sequence = CreateTextSequence(script);
        }

        //��ŵ ó��
        if (script.skipMethod == SkipMethod.Auto)
        {
            sequence.AppendInterval(script.skipDelay);
            //��ŵ �����̷� �ϴ� �͵� ������ �ؽ�Ʈ�� �̺�Ʈ�� ���� �ʹ� ª�� ��쿡 ���� ó�� �ʿ���!! 221223
            //(AppendINterval�� �ƴ� Insert�� �� �� ���� �� �ð��� �ִ� �͵� ����� ��)
            //��ŵ�� ���� �̰� �������� ���� 221224 (�̺�Ʈ, �ؽ�Ʈ ���� ���� �ϳ��� �������� ����ϵ��� �� ���� �˾Ƽ� ���� ���� ���� �ڷ� ���� ��.)
            
            sequence.AppendCallback(() => NextScript());
        }
        else
        {
            CreateSkipStream(script, sequence);
        }

        //���� �̺�Ʈ �������� �߰�
        AppendNextEvent(script, sequence);

        //������ ����
        sequence.Play();
    }

    private Sequence CreateTextSequence(ScriptObject script)
    {
        Sequence textSeq = textMgr.CreateTextSequence(script);

        return textSeq;
    }

    private Sequence CreateEventSequence(ScriptObject script)
    {
        Sequence eventSeq = eventMgr.CreateEventSequence(script);

        return eventSeq;
    }

    private void AppendNextEvent(ScriptObject script, Sequence sequence)
    {
        if (script.linkEvent == false) return;

        ScriptManager.Next();
        ScriptObject nextScript = ScriptManager.GetCurrentScript();

        if (nextScript.isEvent == false) return;

        Sequence nextEvent = eventMgr.CreateEventSequence(nextScript);

        sequence.Insert(0, nextEvent);

        "AppendNextEvent".Log();

        AppendNextEvent(nextScript, sequence);
    }

    private void CreateSkipStream(ScriptObject script, Sequence sequence)
    {
        skipStream = Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Space))
            .Subscribe(_ => Skip(script, sequence));
    }

    private void Skip(ScriptObject script, Sequence sequence)
    {
        if (sequence.IsPlaying() && script.skipMethod == SkipMethod.Skipable)
        {
            //sequence.Kill(true);
            sequence.Complete();
            //sequence.Pause();
        }
        else if (sequence.IsPlaying() == false)
        {
            NextScript();
        }
    }

    private void NextScript()
    {
        ScriptManager.Next();

        if(skipStream != null)
        {
            skipStream.Dispose();
        }


        ExecuteScript(ScriptManager.GetCurrentScript());


        "���� ��ũ��Ʈ".�α�();
    }
}
