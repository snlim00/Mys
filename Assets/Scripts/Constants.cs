using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * �빮�ڴ� ���̺��� ������ �� Ű�� ����ϴ� ������ �ǹ�
 */

public static class GameConstants
{
    public static bool isEditorMode = true;
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
    CreateCharacter,
    RemoveCharacter,
    Goto,
    ChangeBackground,
}

public enum KEY_SCRIPT_DATA
{
    ScriptID,
    CharacterName,
    Text,
    TextDuration,
    SkipMethod,
    SkipDelay,
    Audio0,
    Audio1,
    LinkEvent,
    Event,
    DurationTurn,
    EventDelay,
    EventDuration,
    LoopCount,
    LoopType,
    LoopDelay,
    //EventParam0,
    //EventParam1,
    //EventParam2,
    //EventParam3,
    //EventParam4,
    //EventParam5,
}
#endregion