using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

//ĳ���͵��� Ŭ������ ����� ���� ��� ��. 221223
public class Character : MonoBehaviour
{
    public Image image;

    public int latest = ScriptObject.UNVALID_ID; //���������� ȣ��� scriptID

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Init(ScriptObject script)
    {
        
    }
}
