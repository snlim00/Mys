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
    private IDisposable skipStream = null;

    private TextManager textMgr;
    private EventManager eventMgr;

    private List<Tween> tweenList;

    private void Awake()
    {
        textMgr = FindObjectOfType<TextManager>();
        eventMgr = FindObjectOfType<EventManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ScriptManager.ReadScript();

        DialogStart(10001);
    }

    private void DialogStart(int scriptID)
    {
        ScriptManager.SetScriptFromID(scriptID);
        ScriptObject script = ScriptManager.GetCurrentScript();

        ExecuteScript(script);
    }

    private void ExecuteScript(ScriptObject script)
    {
        tweenList = new List<Tween>();

        ("ExecuteScript: " + script.scriptID).Log();

        bool isEvent = script.isEvent;

        if (isEvent == true)
        {
            tweenList.Add(CreateEventSequence(script)); //��ũ��Ʈ ���ᵵ �̺�Ʈ���� ó���� ����. 221223
        }
        else
        {
            tweenList.Add(CreateTextSequence(script));
        }

        AppendLinkEvent(script);

        //��ŵ ó��
        if (script.skipMethod == SkipMethod.Auto)
        {
            Sequence skipSeq = DOTween.Sequence();

            //���� ū duration �̱�
            float duration = tweenList[0].Duration();
            for(int i = 1; i < tweenList.Count; ++i)
            {
                if(tweenList[i].Duration() == Mathf.Infinity) continue;

                if(duration < tweenList[i].Duration() || duration == Mathf.Infinity)
                {
                    duration = tweenList[i].Duration();
                }
            }

            float skipDelay = 0;
            if(duration == Mathf.Infinity)
            {
                skipDelay = script.skipDelay;
            }
            else
            {
                skipDelay = duration + script.skipDelay;
            }

            skipSeq.AppendInterval(skipDelay);
            skipSeq.AppendCallback(() => NextScript());

            tweenList.Add(skipSeq);
        }
        else
        {
            CreateSkipStream(script);
        }

        PlayAllTweens();
    }

    #region DoAllTweens
    private void DoAllTweens(Action<Tween> action)
    {
        foreach(Tween tween in tweenList)
        {
            action(tween);
        }
    }

    private void PlayAllTweens()
    {
        DoAllTweens(tween => tween.Play());
    }

    private void PauseAllTweens()
    {
        DoAllTweens(tween => tween.Pause());
    }

    private void CompleteAllTweens()
    {
        DoAllTweens(tween => tween.Complete());
    }
    #endregion

    #region CreateSequences
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
    #endregion

    private void AppendLinkEvent(ScriptObject script)
    {
        if (script.linkEvent == false)
        {
            "No have link event".Log();
            return;
        }

        ScriptManager.Next();
        ScriptObject nextScript = ScriptManager.GetCurrentScript();

        if (nextScript.isEvent == false)
        {
            ScriptManager.Prev();
            return;
        }

        Sequence nextEvent = eventMgr.CreateEventSequence(nextScript);

        tweenList.Add(nextEvent);

        "AppendNextEvent".Log();
    }

    private void CreateSkipStream(ScriptObject script)
    {
        skipStream = Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Space))
            .Subscribe(_ => Skip(script));
    }

    private void Skip(ScriptObject script)
    {
        "��ŵ".�α�();
        bool isPlaying = false;

        foreach(var tween in tweenList)
        {
            if(tween.IsPlaying() == true)
            {
                isPlaying = true;
                break;
            }
        }

        if(script.skipMethod == SkipMethod.Skipable && isPlaying == true) //���� ������ �ִٸ� �׻� isPlaying�� true�� ������ ����... ���� ���� Ʈ���� ���� �����ؾ� �ϳ�..?
        {
            CompleteAllTweens();
        }
        else if(isPlaying == false)
        {
            skipStream.Dispose();
            PauseAllTweens(); //Loop Event���� ���� Complete�� ������ �ʱ�(���缭�� �� ��) ������ ���⼭ ���������.
            NextScript();
        }
        else
        {
            "��ŵ ����".�α�();
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
