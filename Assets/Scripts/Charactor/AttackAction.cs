using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class AttackAction : CharActionBase
{
    Vector3 goal;
    private CharactorObj target;

    public AttackAction()
    {
        m_preTime = DateTime.Now;
        m_actionDelayTime = 1f;
        m_actionCoolTime = 1f;
        m_culCoolTime = 1f;
    }

    public void SetAttackProperty(TAttackType _attackType)
    {
        m_actionDelayTime = Utility.CalHundred( _attackType.AttackDelayTime);
        m_actionCoolTime = Utility.CalHundred(_attackType.AttackCoolTime);
    }

    public void Update(CharactorObj _actionObj)
    {
        m_culCoolTime -= Time.deltaTime;
        FindEnemy(_actionObj);
        if (target != null)
        {
            goal = target.gameObject.transform.position;
            float gap = _actionObj.GetCharStat(EnumCharctorStat.AttackReach);
            bool arrive = _actionObj.Move(goal, gap);

            if(arrive == true && m_culCoolTime<=0)
            {

                if (target.IsDead())
                {
                    //이미 사망한 상태라면 공격 안함
                    target = null;
                    return;
                }

                //공격하고
                _actionObj.PlayAnim(EnumAniState.Attack, goal);
                //ActionData attackRecorde = new ActionData(m_charData, target.m_charData, GetCharStat(EnumCharctorStat.AttackPower));
                //target.Attack(attackRecorde);
                target.Attack(_actionObj.GetCharStat(EnumCharctorStat.AttackPower));
                _actionObj.ResetCool();
                _actionObj.SetDelayTime(m_actionDelayTime);
                m_culCoolTime = m_actionCoolTime; //다음 공격시간은 각자 행동에서 정의 

                //공격이 들어간순간 - gameObject 파괴 속도에 따라 타겟 널이 갈릴 수있음.
                //널인건 일단 죽였다는거, 여길왔다는건 얘가 때린거기때문에 보상 진행
                CharactorData m_charData = _actionObj.m_charData;
                if (target == null || target.IsDead())
                {
              //      Debug.Log("상대 사망");
                    m_charData.CalStat(EnumCharctorStat.MoveSpeed, 3);
                    m_charData.CalStat(EnumCharctorStat.AttackPower, 3);
                    m_charData.AccelerateActionSpeed(-2);
                    m_actionDelayTime = Utility.CalHundred(_actionObj.GetCharStat(EnumCharctorStat.ActionDelay));
                    m_charData.AccelerateCoolTime(-2);
                    m_actionCoolTime = Utility.CalHundred(_actionObj.GetCharStat(EnumCharctorStat.AttackCoolTime));
                    ResetAction(_actionObj);
                }


            }


        }
        else
        {

            ResetAction(_actionObj);
        }
    }

    private void ResetAction(CharactorObj _actionObj)
    {
        _actionObj.ResetAction();
        m_preTime = DateTime.Now;
    }

    private void FindEnemy(CharactorObj _actionObj)
    {
        if (target != null)
            return;

        if (_actionObj.isMonster)
            return;

        float minDistance = float.MaxValue;
        for (int i = 0; i < _actionObj.m_battleField.charactorList.Count; i++)
        {
            if (_actionObj.m_battleField.charactorList[i] == null)
                continue;

            if (_actionObj.isMonster != _actionObj.m_battleField.charactorList[i].isMonster)
            {
                float distance = Vector3.Distance(_actionObj.m_battleField.charactorList[i].transform.position, _actionObj.transform.position);
                if (distance <= minDistance)
                {
                    minDistance = distance;
                    target = _actionObj.m_battleField.charactorList[i];

                }

            }
        }
    }

}

