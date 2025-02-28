using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ActionRecorder
{


}

public struct AttackRecorde
{
    public string attackerName;
    public string targetName;
    public int inputDamage;
    public int adaptDamage;

    public AttackRecorde(string _attacker, string _target, int _damage)
    {
        attackerName = _attacker;
        targetName = _target;
        inputDamage = adaptDamage = _damage;
    }
}

