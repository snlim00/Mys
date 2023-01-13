using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EditorScriptManager
{
    public static List<ScriptObject> scripts = new List<ScriptObject>();

    public static int currentIndex { get; private set; } = 0;
    public static ScriptObject currentScript
    {
        get
        {
            return scripts[currentIndex];
        }
    }

    /// <summary>
    /// Data/ScriptTable.CSV ������ �о��.<br/>
    /// ��ü ���� ������ �� ���� ȣ��.
    /// </summary>
    public static void SetScript(int targetScriptGroupID)
    {
        scripts = new();

        //���� �׷��� ��ũ��Ʈ�� ��� ����Ʈ�� �߰�.
        foreach (var script in ScriptManager.scripts)
        {
            int groupID = ScriptManager.GetGroupID(script.scriptID);

            if (targetScriptGroupID == groupID)
            {
                scripts.Add(script);
            }
        }

        scripts.Sort((a, b) =>
        {
            return a.scriptID.CompareTo(b.scriptID);
        });
    }

    public static void SetCurrentScript(ScriptObject script)
    {
        currentIndex = scripts.IndexOf(script);
    }

    public static void SetCurrentScript(int scriptID)
    {
        ScriptObject script = GetScriptFromID(scriptID);

        SetCurrentScript(script);
    }

    public static ScriptObject Next()
    {
        ScriptObject nextScript = GetNextScript();

        SetCurrentScript(nextScript);

        return nextScript;
    }

    public static ScriptObject GetNextScript()
    {
        int currID = currentScript.scriptID;
        int nextID = currentScript.scriptID + 1;

        if (IsSameScriptGroup(currID, nextID) == false)
        {
            ("�ٸ� �׷��� ��ũ��Ʈ�� �����߽��ϴ�. " + currID + ", " + nextID + " / " + GetGroupID(currID) + ", " + GetGroupID(nextID)).LogWarning();
            return null;
        }

        ScriptObject nextScript = GetScriptFromID(nextID);

        return nextScript;
    }

    public static ScriptObject GetPrevScript()
    {
        int currID = currentScript.scriptID;
        int prevID = currID - 1;

        if (IsSameScriptGroup(currID, prevID) == false)
        {
            ("�ٸ� �׷��� ��ũ��Ʈ�� �����߽��ϴ�. " + currID + ", " + prevID + " / " + GetGroupID(currID) + ", " + GetGroupID(prevID)).LogWarning();
            return null;
        }

        ScriptObject prevScript = GetScriptFromID(prevID);

        return prevScript;
    }

    public static ScriptObject GetPrevScriptFromID(int scriptID)
    {
        int currID = scriptID;
        int prevID = currID - 1;

        if (IsSameScriptGroup(currID, prevID) == false)
        {
            ("�ٸ� �׷��� ��ũ��Ʈ�� �����߽��ϴ�. " + currID + ", " + prevID + " / " + GetGroupID(currID) + ", " + GetGroupID(prevID)).LogWarning();
            return null;
        }

        ScriptObject prevScript = GetScriptFromID(prevID);

        return prevScript;
    }

    public static ScriptObject GetScriptFromID(int id)
    {
        foreach (var script in scripts)
        {
            if (script.scriptID == id)
            {
                return script;
            }
        }

        ("Script ID�� ã�� �� �����ϴ� : " + id).LogWarning();
        return null;
    }

    public static int GetGroupID(int id)
    {
        int prefixID = (int)Mathf.Floor(id / 10000f);

        return prefixID;
    }

    public static int GetFirstScriptIDFromGroupID(int id)
    {
        return (id * 10000) + 1;
    }

    public static bool IsSameScriptGroup(int id1, int id2)
    {
        return (GetGroupID(id1) == GetGroupID(id2));
    }
}

