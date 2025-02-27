﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum EnumCharctorStat
{
    MaxHp, CurHp, AttackPower, DefencePower, MoveSpeed, AttackReach, AttackCoolTime, AttackCurCoolTime,
    Sight
}

public class CharactorData
{
    static private int MakeCount = 1;
    public string Name;
    public bool isPlayer = true;
    public int[] Stats;
    public CharactorObj charObj;

    public CharactorData()
    {
        Stats = new int[Enum.GetValues(typeof(EnumCharctorStat)).Length];
        Name = "테스트"+MakeCount.ToString();
        for (int i = 0; i < Stats.Length; i++)
        {
            Random random = new();
            int randomPower = random.Next() % 99; //임의로 스텟 값 0~99
            Stats[i] = randomPower;
        }
        MakeCount++;
    }

    public int GetCharStat(EnumCharctorStat _charStat)
    {
        return Stats[(int)_charStat];
    }

    public void Attack(int _damage)
    {
        Stats[(int)EnumCharctorStat.CurHp] -= _damage;
        if(Stats[(int)EnumCharctorStat.CurHp] <= 0)
        {
            charObj.Dead();
        }
    }

    public void SetCharObj(CharactorObj _obj)
    {
        charObj = _obj;
    }
}
