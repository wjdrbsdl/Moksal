using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public struct TAttackType
{
    public int AttackPower;
    public int AttackCoolTime;
    public int AttackDelayTime;
    public int AttackReach;

    public TAttackType(int _power, int _coolTime, int _delay, int _reach)
    {
        AttackPower = _power;
        AttackCoolTime = _coolTime;
        AttackDelayTime = _delay;
        AttackReach = _reach;
    }
}
