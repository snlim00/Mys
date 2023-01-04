using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * �빮�ڴ� ���̺��� ������ �� Ű�� ����ϴ� ������ �ǹ�
 */

public static class Constants //�ش� ������Ʈ������ ������ ����. 221217
{
    public static int essentialKeyCount = 2;

    public static int idKey = 0;
    public static int nameKey = 1;
}

#region Script
public enum KEY_SCRIPT_DATA
{
    ScriptID,
    CharacterName,
    LinkText,
    Text,
    TextDuration,
    SkipMethod,
    SkipDelay,
    LinkEvent,
    Event,
    EventDuration,
    LoopCount,
    LoopDelay,
    //EventParam0,
    //EventParam1,
    //EventParam2,
    //EventParam3,
    //EventParam4,
    //EventParam5,
}
#endregion