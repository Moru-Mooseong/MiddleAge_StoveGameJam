using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Game_Patten : SingleTone<Game_Patten>
{

    public enum UnitData { Level, HP, Movespeed }
    [SerializeField] private TextAsset assets;
    [SerializeField] List<string> PattenData;

    protected override void Awake()
    {
        base.Awake();
        PattenData = GetDataString();
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
    public int GetPattenCount()
    {
        return PattenData.Count;
    }
    public List<int> GetData(int level)
    {
        if (level == 0)
        { level++; }
        string List_Value = PattenData[level];
        string[] chop_string = List_Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        ///Debug.Log($"������ �� : {List_Value}// ���� ����(��) : {chop_string.Length}");
        List<int> returnValue = new List<int>();
        for (int i = 1; i < chop_string.Length; i++)
        {
            int a = int.Parse("0");
            if (!int.TryParse(chop_string[i], out a))
            { a = 0; }
            returnValue.Add(a);
            ///Debug.Log($"�⺻��Ʈ�� �� {chop_string[i]} // ����ȯ ����� {a} // ��ȯ����Ʈ ���� {returnValue.Count}");
//            int.Parse(List_Value, System.Globalization.NumberStyles.Integer);
        }
        return returnValue;
    }

    public void FindValue(int level, Unit.State jop, out float _value)
    {
        List<int> dummyData = GetData(level);
        int outData = dummyData[(int)jop];
        _value = outData;
    }
}
