using System;
using System.Collections.Generic;
using UnityEngine;


public class ActionRecorder
{
    public List<ActionData> actionDataList;

    public ActionRecorder()
    {
        actionDataList = new();
    }

    public void AddData(ActionData _data)
    {
        actionDataList.Add(_data);
    }

    public void Print()
    {
        for (int i = 0; i < actionDataList.Count; i++)
        {
            Debug.Log(actionDataList[i].attackerName + "이 " + actionDataList[i].targetName + "을 공격");
        }
    }

}


public struct ActionData
{
    public string actionCode;
    public string attackerName;
    public string targetName;
    public int inputDamage;
    public int adaptDamage;

    public ActionData(string _attacker, string _target, int _damage)
    {
        actionCode = "attack";
        attackerName = _attacker;
        targetName = _target;
        inputDamage = adaptDamage = _damage;
    }

    public ActionData(CharactorData _attackerData, CharactorData _targetData, int _damage)
    {
        actionCode = "attack";
        attackerName = _attackerData.Name;
        targetName = _targetData.Name;
        inputDamage = adaptDamage = _damage;
    }
}

