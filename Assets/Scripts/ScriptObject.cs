using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
public enum SkipMethod
{
    Skipable, //��ŵ ����
    NoSkip, //��ŵ �Ұ���
    Auto, //���, �̺�Ʈ ���� ���� �ڵ����� ���� ��ũ��Ʈ�� �̵� (��ŵ �Ұ���)
    //WithNext, //�ش� ��ũ��Ʈ�� �Բ� ���� ��ũ��Ʈ ��� ����. (���� ��ũ��Ʈ�� �̺�Ʈ�� �ƴ϶�� ��ȿ ó��) //���� ó���� SKipMethod�� �ƴ� ������ �Ķ���ͷ� ó���ϴ� ���� ���� ��. 221220
}

public enum EventType
{
    None,
    ChangeCharacter,
    ChangeBackground,
}

//�ش� Ŭ������ ��� ��������� ����Լ��� ���ؼ��� ����Ǿ�� ��.
[System.Serializable]
public class ScriptObject
{
    public static readonly int UNVALID_ID = -1;
    public static readonly float DEFAULT_TEXT_DURATION = 0.1f;
    public static readonly SkipMethod DEFAULT_SKIP_METHOD = SkipMethod.Skipable;
    public static readonly EventType DEFAULT_EVENT_TYPE = EventType.None;
    public static readonly float DEFAULT_SKIP_DURATION = 0.5f;
    public static readonly float DEFAULT_EVENT_DURATION = 0.5f;
    public static readonly int EVENT_PARAM_COUNT = 6;


    public int scriptID = UNVALID_ID;
    public string characterName = null;
    public string text = null;
    public float textDuration = DEFAULT_TEXT_DURATION;
    //public SkipMethod skipMethod = DEFAULT_SKIP_METHOD; //�׳� ��Ʈ������ �ϴ°� ���Ŀ� �Լ� ȣ���� �� �� �� ���� ��. Enum���� ������ ������ ���� �����. 221217
    //public EventType eventType = DEFAULT_EVENT_TYPE;
    public SkipMethod skipMethod = DEFAULT_SKIP_METHOD;
    public float skipDelay = DEFAULT_SKIP_DURATION;
    public string eventType = null;
    public float eventDuration = DEFAULT_EVENT_DURATION;
    public string[] eventParam = new string[EVENT_PARAM_COUNT];

    public void SetVariable(string key, string value)
    {
        if(value == "" || value == null)
        {
            return;
        }

        object objValue;
        KEY_SCRIPT_DATA enumValue;
        if (Enum.TryParse(typeof(KEY_SCRIPT_DATA), key, out objValue))
        {
            enumValue = (KEY_SCRIPT_DATA)objValue;
            
            switch (enumValue)
            {
                case KEY_SCRIPT_DATA.ScriptID:
                    int.TryParse(value, out scriptID);
                    break;

                case KEY_SCRIPT_DATA.CharacterName:
                    characterName = value;
                    break;
                
                case KEY_SCRIPT_DATA.Text:
                    text = value;
                    break;

                case KEY_SCRIPT_DATA.TextDuration:
                    float.TryParse(value, out textDuration);
                    break;

                case KEY_SCRIPT_DATA.SkipMethod:
                    skipMethod = (SkipMethod)Enum.Parse(typeof(SkipMethod), value);
                    break;

                case KEY_SCRIPT_DATA.SkipDelay:
                    float.TryParse(value, out skipDelay);
                    break;

                case KEY_SCRIPT_DATA.Event:
                    eventType = value;
                    break;

                case KEY_SCRIPT_DATA.EventDuration:
                    float.TryParse(value, out eventDuration);
                    break;

                case KEY_SCRIPT_DATA.EventParam0:
                    eventParam[0] = value;
                    break;

                case KEY_SCRIPT_DATA.EventParam1:
                    eventParam[1] = value;
                    break;

                case KEY_SCRIPT_DATA.EventParam2:
                    eventParam[2] = value;
                    break;

                case KEY_SCRIPT_DATA.EventParam3:
                    eventParam[3] = value;
                    break;

                case KEY_SCRIPT_DATA.EventParam4:
                    eventParam[4] = value;
                    break;
                
                case KEY_SCRIPT_DATA.EventParam5:
                    eventParam[5] = value;
                    break;
            }

            //this.Log();
        }
    }
}