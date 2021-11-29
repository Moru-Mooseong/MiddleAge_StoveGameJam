using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CSV_Reader : SingleTone<CSV_Reader>
{

    public enum UnitData { Level, HP, Movespeed}
    [SerializeField] private TextAsset assets;
    [SerializeField] List<string> levelData;

    protected override void Awake()
    {
        base.Awake();
        levelData = GetDataString();
    }

    /// <summary>
    /// �����͸� ������� �߶� ����Ʈ�� ��ȯ
    /// </summary>
    private List<string> GetDataString()
    {
        string[] data = assets.text.Split(new char[] { '\n' }, StringSplitOptions.None);
        List<string> returnData = new List<string>();
        for (int i = 0; i < data.Length - 1; i++)
        {
            returnData.Add(data[i]);
        }
        return returnData;
    }

    public List<float> GetData(int level)
    {
        if (level == 0)
        { level++; }
        string List_Value = levelData[level];
        string[] chop_string = List_Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        ///Debug.Log($"������ �� : {List_Value}// ���� ����(��) : {chop_string.Length}");
        List<float> returnValue = new List<float>();
        for (int i = 0; i < chop_string.Length-1; i++)
        {
            float a = float.Parse("0");
            if (!float.TryParse(chop_string[i], out a))
            { a = 0; }
            returnValue.Add(a);
            ///Debug.Log($"�⺻��Ʈ�� �� {chop_string[i]} // ����ȯ ����� {a} // ��ȯ����Ʈ ���� {returnValue.Count}");
//            int.Parse(List_Value, System.Globalization.NumberStyles.Integer);
        }
        return returnValue;
    }

    public void FindValue(int level, UnitData data, out float _value)
    {
        List<float> dummyData = GetData(level);
        float outData = dummyData[(int)data];
        _value = outData;
    }

}
