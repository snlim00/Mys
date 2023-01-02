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
using UnityEditor;

public class EventManager : MonoBehaviour
{
    [SerializeField] private GameObject characters;
    private GameObject characterPref;
    private Dictionary<string, Image> characterList;

    private void Awake()
    {
        characterList = new Dictionary<string, Image>();
        characterPref = Resources.Load<GameObject>("Prefabs/CharacterPref");
    }

    public Sequence CreateEventSequence(ScriptObject script)
    {
        Sequence eventSequence = DOTween.Sequence();

        CallEvent(script, eventSequence);

        return eventSequence;
    }

    public void CallEvent(ScriptObject script, Sequence sequence)
    {
        Event_CreateCharacter(sequence, script.eventDuration, script.eventParam);

        switch(script.eventType)
        {
            //case EventType.
        }
    }

    public void Event_CreateCharacter(Sequence sequence, float duration, string[] parameter)
    {
        string resource = parameter[0];
        string index = parameter[1];
        //ĳ���� ��ġ ���� ���õ� �Ķ���� �� �ڵ� �ʿ�.
        //ĳ���� ���� ���� �Ķ���� �� �ڵ� �ʿ�

        Sprite sprite = Resources.Load<Sprite>("Images/" + resource);

        Image character = Instantiate(characterPref).GetComponent<Image>();
        characterList[index] = character;

        character.sprite = sprite;
        character.transform.SetParent(characters.transform);
        character.transform.localPosition = new Vector2(0, 0);
        character.color = new Color(255, 255, 255, 0); //�׽�Ʈ �ڵ�

        sequence.Append(character.DOFade(1, duration));
    }
}