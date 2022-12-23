using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO : ��ũ��Ʈ�� getter, setter�� ���� ó�� ���ֱ�. (�迭 ���� �ʰ�, ���� scriptID, scriptID ���ڸ� ���� ��) 221223
public static class ScriptManager
{
    private static int currentIndex = 0;
    private static ScriptObject currentScript = null;
    public static List<ScriptObject> scripts = new List<ScriptObject>();

    public static void ReadScript()
    {
        ScriptManager.scripts = CSVReader.ReadScript("Data/ScriptTable.CSV");
    }

    public static void SetCurrentIndex(int idx)
    {
        currentIndex = idx;

        ScriptObject script = scripts[currentIndex];
    }

    public static int GetCurrentIndex()
    {
        return currentIndex;
    }

    public static ScriptObject GetCurrentScript()
    {
        ScriptObject script = scripts[currentIndex];

        return script;
    }

    public static ScriptObject GetNextScript()
    {
        return scripts[currentIndex + 1];
    }

    public static int Next()
    {
        currentIndex += 1; //��ũ��Ʈ �� ID�� �ٲ�� �Ѿ�� �ʵ��� �� ��.

        SetCurrentIndex(currentIndex);

        return currentIndex;
    }


    public static int GetCurrentScriptIndex()
    {
        return currentIndex;
    }

    public static int GetCurrentScriptID()
    {
        return currentScript.scriptID;
    }

    public static void SetScriptFromID(int scriptID)
    {
        bool isFind = false;

        foreach(ScriptObject script in scripts)
        {
            if(script.scriptID == scriptID)
            {
                int index = scripts.IndexOf(script);

                SetCurrentIndex(index);

                isFind = true;
            }
        }

        if(isFind == false)
        {
            ("Script ID�� ã�� �� �����ϴ� : " + scriptID).Log();
        }
    }
}
