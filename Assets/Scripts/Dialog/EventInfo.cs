using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.UI;

public class EventParamInfo
{
    public string explain;

    public VariableType varType;
    public string paramName;
    public ScriptDataKey targetKey;
    public string[] options;
    public InputField.ContentType contentType = InputField.ContentType.Standard;

    public EventParamInfo(VariableType varType, string paramName, ScriptDataKey targetKey, string[] options = null, string explain = null, InputField.ContentType contentType = InputField.ContentType.Standard)
    {
        this.varType = varType;
        this.paramName = paramName;
        this.targetKey = targetKey;
        this.options = options;

        this.explain = explain;

        this.contentType = contentType;
    }
}

public class EventInfo
{
    public static Dictionary<EventType, EventInfo> infos = new();

    public string explain = null;
    public bool canUseEventDuration = false;

    public List<EventParamInfo> paramInfo = new();

    public static EventInfo GetEventInfo(EventType eventType)
    {
        if(infos.ContainsKey(eventType))
        {
            return infos[eventType];
        }
        else
        {
            return infos[EventType.None];
        }
    }

    public static void Init()
    {
        string[] characterNames = { CharacterName.Jihyae, CharacterName.Yunha, CharacterName.Seeun };

        //None
        {
            EventInfo info = new();
            info.canUseEventDuration = false;
            infos.Add(EventType.None, info);
        }

        //SetBackground
        {
            EventInfo info = new();

            info.canUseEventDuration = true;
            infos.Add(EventType.SetBackground, info);

            info.paramInfo.Add(new(VariableType.InputField, "Resource", ScriptDataKey.EventParam0, null, "배경의 리소스 경로를 입력해주세요. (\"Assets/Resources/Image/Background/\" 이후의 경로만 입력)"));
        }

        //CreateCharacter
        {
            EventInfo info = new();

            info.canUseEventDuration = true;
            infos.Add(EventType.CreateCharacter, info);

            info.paramInfo.Add(new(VariableType.InputField, "Resource", ScriptDataKey.EventParam0, null, "오브젝트의 리소스 경로를 입력해주세요. (\"Assets/Resources/Image/Character/\" 이후의 경로만 입력)"));
            string[] options = { "0", "1", "2", "3", "4" };
            info.paramInfo.Add(new(VariableType.Dropdown, "Position", ScriptDataKey.EventParam1, options, "오브젝트의 위치를 정해주세요."));
        }

        //RemoveCharacter
        {
            EventInfo info = new();
            info.canUseEventDuration = false;
            infos.Add(EventType.RemoveCharacter, info);

            info.paramInfo.Add(new(VariableType.InputField, "Object", ScriptDataKey.EventParam0, null, "오브젝트를 선택해주세요."));
        }

        //RemoveAllCharacter
        {

        }

        //Branch
        {
            EventInfo info = new();
            info.canUseEventDuration = false;
            infos.Add(EventType.Branch, info);

            info.paramInfo.Add(new(VariableType.Dropdown, "Character", ScriptDataKey.EventParam0, characterNames, "캐릭터를 선택해주세요."));
            info.paramInfo.Add(new(VariableType.InputField, "Point Amount 1", ScriptDataKey.EventParam0, null, "호감도 요구 조건을 적어주세요.", InputField.ContentType.IntegerNumber));
            info.paramInfo.Add(new(VariableType.InputField, "Point Amount 2", ScriptDataKey.EventParam2, null, "호감도 요구 조건을 적어주세요.", InputField.ContentType.IntegerNumber));
            info.paramInfo.Add(new(VariableType.InputField, "Point Amount 3", ScriptDataKey.EventParam4, null, "호감도 요구 조건을 적어주세요.", InputField.ContentType.IntegerNumber));
        }

        //AddLovePoint
        {
            EventInfo info = new();

            info.canUseEventDuration = false;
            infos.Add(EventType.AddLovePoint, info);

            info.paramInfo.Add(new(VariableType.Dropdown, "Character", ScriptDataKey.EventParam0, characterNames, "캐릭터를 선택해주세요."));
            info.paramInfo.Add(new(VariableType.InputField, "Point Amount", ScriptDataKey.EventParam1, null, "호감도의 양을 적어주세요.", InputField.ContentType.IntegerNumber));
        }

        //Goto
        {     
            EventInfo info = new();
            info.canUseEventDuration = false;
            infos.Add(EventType.Goto, info);

            info.paramInfo.Add(new(VariableType.InputField, "Script", ScriptDataKey.EventParam0, null, "이동할 스크립트를 선택해주세요."));
        }
    }
}