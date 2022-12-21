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
    [SerializeField] private GameObject characterPref;
    private List<Image> characterList;

    private void Awake()
    {
        characterList = new List<Image>();
    }

    public Sequence CreateEventSequence(ScriptObject script)
    {
        "�̺�Ʈ ����".�α�();

        Sequence eventSequence = DOTween.Sequence().Pause();

        Event_CreateCharacter(eventSequence, 10, null);

        return eventSequence;
    }

    public void Event_CreateCharacter(Sequence sequence, float duration, string[] parameter)
    {
        //�۵� Ȯ����. Ȯ���Ͽ� �پ��ϰ� ����� ��� ����� ��. 221222

        //string resource = parameter[0];
        //int index = int.Parse(parameter[1]);
        int index = 0;

        Image character = Instantiate(characterPref).GetComponent<Image>();
        //characterList[index] = character;
        character.transform.SetParent(characters.transform);
        character.transform.localPosition = new Vector2(0, 0);

        sequence.Append(character.DOFade(0, 10));
    }
}
