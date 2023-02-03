using DG.Tweening.Plugins;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum VariableType
{
    InputField,
    Dropdown,
}

public class Variable : MonoBehaviour
{
    [SerializeField] private Text varName;
    [SerializeField] private InputField inputField;
    [SerializeField] private Dropdown dropdown;

    private Node targetNode;
    private ScriptDataKey targetKey;

    public VariableType type = VariableType.InputField;

    public void Init(VariableType type)
    {
        if(type == VariableType.Dropdown)
        {
            inputField.gameObject.SetActive(false);
            dropdown.gameObject.SetActive(true);

            this.type = VariableType.Dropdown;
        }

        transform.SetParent(ScriptInspector.instance.transform);

        transform.localScale = Vector3.one;
    }

    public void SetDropdownOption(string[] options)
    {
        if(type != VariableType.Dropdown)
        {
            "Varaible Type�� Dropdown�� �ƴմϴ�".�α�();
            return;
        }

        dropdown.ClearOptions();

        List<Dropdown.OptionData> optionList = new();

        foreach(string option in options)
        {
            optionList.Add(new(option));
        }

        dropdown.AddOptions(optionList);
    }

    public void SetContentType(InputField.ContentType contentType)
    {
        inputField.contentType = contentType;
    }

    public void SetTarget(Node targetNode, ScriptDataKey targetKey)
    {
        this.targetNode = targetNode;
        this.targetKey = targetKey;
    }

    public void SetName(string name)
    {
        varName.text = name;
    }

    public void SetValue(string value)
    {
        if (type == VariableType.InputField)
        {
            inputField.text = value;
        }
        else if (type == VariableType.Dropdown)
        {
            var options = dropdown.options;

            for(int i = 0; i < options.Count; ++i)
            {
                if (options[i].text == value)
                {
                    dropdown.value = i;

                    break;
                }
            }
        }
    }

    public string GetValue()
    {
        if(type == VariableType.InputField)
        {
            return inputField.text;
        }
        else if(type == VariableType.Dropdown)
        {
            var options = dropdown.options;

            int value = dropdown.value;

            return options[value].text;
        }

        return null;
    }

    public void ApplyValue()
    {
        targetNode.script.SetVariable(targetKey, GetValue());
    }
}