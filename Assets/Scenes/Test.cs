using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UniRx;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI.Extensions;


public class Human
{
    public string name;

    public int age;
    public int height;
    public int weight;

    public int iq;

    //override : �����
    public virtual string Introduce()
    {
        return "���� " + name + "�Դϴ�.";
    }
}

public class Student : Human
{
    public string school;

    public override string Introduce()
    {
        string prevIntroduce = base.Introduce();

        return prevIntroduce + school + "�� �ٴϰ� �ֽ��ϴ�.";
    }
}

public class Test : MonoBehaviour
{
    // main �Լ�
    private void Start()
    {
        Student uiman = new();
        uiman.name = "���Ǹ�";
        uiman.school = "�Ѽ����̹����Ȱ���б�";
    }
}
