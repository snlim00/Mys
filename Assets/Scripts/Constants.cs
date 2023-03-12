using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * �빮�ڴ� ���̺��� ������ �� Ű�� ����ϴ� ������ �ǹ�
 */

public static class GameConstants
{
    public static bool isEditorMode = false;
}

public static class Constants //�ش� ������Ʈ������ ������ ����. 221217
{
    public static int essentialKeyCount = 2;

    public static int idKey = 0;
    public static int nameKey = 1;
}

#region Script
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
    SetBackground,
    CreateCharacter,
    RemoveCharacter,
    RemoveAllCharacter,
    AddLovePoint,
    Goto,
    Branch,
}

public enum ScriptDataKey
{
    ScriptID,
    ScriptType,
    CharacterName,
    Text,
    TextDuration,
    SkipMethod,
    SkipDelay,
    LinkEvent,
    Audio0,
    Audio1,
    EventType,
    DurationTurn,
    EventDelay,
    EventDuration,
    LoopCount,
    LoopType,
    LoopDelay,
    EventParam0,
    EventParam1,
    EventParam2,
    EventParam3,
    EventParam4,
    EventParam5,
    EventParam6,
}
#endregion

public static class CharacterName
{
    public const string Jihyae = "Jihyae";
    public const string Yunha = "Yunha";
    public const string Seeun = "Seeun";
}