using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum EnumCharctorStat
{
    MaxHp, CurHp, AttackPower, DefencePower, MoveSpeed, AttackReach, AttackCoolTime, Sight, ActionSpeed
}

[Serializable]
public class CharactorData
{
    static private int MakeCount = 1;
    public string Name;
    public bool isPlayer = true;
    public int[] Stats;
    private CharactorObj charObj;

    public CharactorData(bool _isPlayer)
    {
        Stats = new int[Enum.GetValues(typeof(EnumCharctorStat)).Length];
        Name = "테스트"+MakeCount.ToString();
        for (int i = 0; i < Stats.Length; i++)
        {
            Random random = new();
            int randomPower = random.Next() % 99+1; //임의로 스텟 값 0~99
            Stats[i] = randomPower;
        }
        Stats[(int)EnumCharctorStat.MoveSpeed] = 300;
        Stats[(int)EnumCharctorStat.AttackReach] = 1;
        Stats[(int)EnumCharctorStat.AttackCoolTime] = 200;
        Stats[(int)EnumCharctorStat.ActionSpeed] = 100;
        MakeCount++;
        isPlayer = _isPlayer;
    }

    public int GetCharStat(EnumCharctorStat _charStat)
    {
        return Stats[(int)_charStat];
    }

    public void CalStat(EnumCharctorStat _charStat, int _value)
    {
        Stats[(int)_charStat] += _value;
    }

    public void AccelerateActionSpeed(int _value)
    {
        Stats[(int)EnumCharctorStat.ActionSpeed] += _value;
        if (Stats[(int)EnumCharctorStat.ActionSpeed] <= 2)
        {
            Stats[(int)EnumCharctorStat.ActionSpeed] = 2;
        }
    }

    public void AccelerateCoolTime(int _value)
    {
        Stats[(int)EnumCharctorStat.AttackCoolTime] += _value;
        if (Stats[(int)EnumCharctorStat.AttackCoolTime] <= 2)
        {
            Stats[(int)EnumCharctorStat.AttackCoolTime] = 2;
        }
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
