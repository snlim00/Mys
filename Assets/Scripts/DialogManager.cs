using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEditor;
using UniRx;

public class TweenObject
{
    public Tween tween;

    public ScriptObject script;

    public int durationTurn = 0; //�ش� Ʈ���� �縮������� ���� �� ��. (���� �� �� �ʱ�ȭ ���� �ǵ帮�� ����)
    public int remainingTurn = 0; //�ش� Ʈ���� ���������� ���� ��.
    public bool isSkipped = false;

    public bool isInfinityLoop
    {
        get { return tween.Loops() == -1; }
    }

    public void Skip(bool completeInfinityLoop = false)
    {
        if (script.isEvent == false)
        {
            tween.Complete();
            DialogManager.instance.RemoveTween(this);
            return;
        }

        if(remainingTurn > 0 && isSkipped == false) //���� ���� �����ϸ�, ���� ��ŵ���� ���� �̺�Ʈ��� �ϸ� ���ҽ�Ű�� ��ŵ���� ����.
        {
            remainingTurn -= 1;
            isSkipped = true;
        }
        else if(remainingTurn <= 0)
        {
            if (isInfinityLoop == false)
            {
                tween.Complete();
                DialogManager.instance.RemoveTween(this);
            }
            else
            {
                if(completeInfinityLoop == true)
                {
                    tween.Goto(tween.Duration(false));
                    tween.Pause();
                    DialogManager.instance.RemoveTween(this);
                }
            }
        }
    }

    public TweenObject(Tween tween, ScriptObject script)
    {
        this.tween = tween;
        this.script = script;
    }
}

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance = null;

    private IDisposable skipStream = null;

    private TextManager textMgr;
    private EventManager eventMgr;

    public List<TweenObject> tweenList = new();

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
        }
        instance = this;

        textMgr = FindObjectOfType<TextManager>();
        eventMgr = FindObjectOfType<EventManager>();
    }

    private void Start()
    {
        ScriptManager.ReadScript();

        DialogStart(10001);
    }

    public void RemoveTween(TweenObject tweenObj)
    {
        tweenList.Remove(tweenObj);
    }

    private void DialogStart(int scriptID)
    {
        ScriptObject script = ScriptManager.GetScriptFromID(scriptID);

        ScriptManager.SetCurrentScript(script);

        ExecuteScript(script);
    }

    private void DoAllTweens(Action<TweenObject> action)
    {
        for(int i = 0; i < tweenList.Count; ++i)
        {
            action(tweenList[i]);
        }
    }

    private TweenObject CreateTextSequence(ScriptObject script)
    {
        Tween tween = textMgr.CreateTextSequence(script);
        TweenObject tweenObj = new(tween, script);

        return tweenObj;
    }

    private TweenObject CreateEventSequence(ScriptObject script)
    {
        Tween tween = eventMgr.CreateEventSequence(script);
        TweenObject tweenObj = new(tween, script);

        if (script.eventData.durationTurn > 0)
        {
            tweenObj.durationTurn = script.eventData.durationTurn;
            tweenObj.remainingTurn = script.eventData.durationTurn;
        }

        return tweenObj;
    }

    private void ExecuteScript(ScriptObject script)
    {
        if (script.isEvent == true)
        {
            tweenList.Add(CreateEventSequence(script));
        }
        else
        {
            tweenList.Add(CreateTextSequence(script));
        }

        AppendLinkEvent(script);

        DoAllTweens(tweenObj =>
        {
            if (tweenObj.tween.IsPlaying() == false)
            {
                tweenObj.tween.Play();
            }

            tweenObj.isSkipped = false;
        });

        SetSkip(script);
    }

    private void AppendLinkEvent(ScriptObject script)
    {
        if (script.linkEvent == false)
        {
            return;
        }

        ScriptObject nextScript = ScriptManager.GetNextScript();

        if(nextScript.isEvent == false)
        {
            return;
        }

        TweenObject nextEvent = CreateEventSequence(nextScript);

        tweenList.Add(nextEvent);

        ScriptManager.SetCurrentScript(nextScript);
    }

    private bool ExistPlayingTween()
    {
        bool isPlaying = false;

        foreach (var tweenObj in tweenList)
        {
            Tween tween = tweenObj.tween;

            if (tween.IsPlaying() == true)
            {
                if (tweenObj.isInfinityLoop == false) //���� ������ �÷��� �� ���θ� ������� ����.
                {
                    isPlaying = true;
                    break;
                }
            }
        }

        return isPlaying;
    }

    private void Next()
    {
        DoAllTweens(tweenObj =>
        {
            tweenObj.Skip(true);
        });

        skipStream.Dispose();

        ExecuteNextScript();
    }

    private void Skip(ScriptObject script)
    {
        //�÷��� ���� Ʈ���� �ִ��� Ȯ��.
        bool isPlaying = ExistPlayingTween();
        ("Skip : " + isPlaying).Log();

        if (script.skipMethod == SkipMethod.Skipable && isPlaying == true)
        {
            DoAllTweens(tweenObj =>
            {
                tweenObj.Skip();
            });
        }
        else if (isPlaying == false)
        {
            Next();
        }
    }

    private void SetSkip(ScriptObject script)
    {
        if (script.skipMethod == SkipMethod.Auto)
        {
            Sequence skipSeq = DOTween.Sequence();

            //���� ū duration �̱�
            float duration = tweenList[0].tween.Duration();
            for (int i = 1; i < tweenList.Count; ++i)
            {
                if (tweenList[i].tween.Duration() == Mathf.Infinity)
                    continue;

                if (duration < tweenList[i].tween.Duration() || duration == Mathf.Infinity)
                {
                    duration = tweenList[i].tween.Duration();
                }
            }

            float skipInterval = script.skipDelay;
            if (duration != Mathf.Infinity)
            {
                skipInterval += duration;
            }

            skipSeq.AppendInterval(skipInterval);
            skipSeq.AppendCallback(Next);
        }
        else
        {
            skipStream = Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.Space))
                .Subscribe(_ => Skip(script));
        }
    }

    private void ExecuteNextScript()
    {
        ScriptManager.Next();

        if(skipStream != null)
        {
            skipStream.Dispose();
        }

        ExecuteScript(ScriptManager.currentScript);
    }
}
