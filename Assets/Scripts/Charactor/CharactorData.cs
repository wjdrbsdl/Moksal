using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum EnumCharctorStat
{
    MaxHp, CurHp, AttackPower, DefencePower, MoveSpeed, AttackReach, AttackCoolTime, AttackCurCoolTime
}

public class CharactorData
{
    static private int MakeCount = 1;
    public string Name;
    public int[] Stats;
    
    public CharactorData()
    {
        Stats = new int[Enum.GetValues(typeof(EnumCharctorStat)).Length];
        Name = "테스트"+MakeCount.ToString();
        MakeCount++;
    }
}
