using System.Collections;
using System.Net.NetworkInformation;
using UnityEditor.Animations;
using UnityEngine;


public class CharactorObj : MonoBehaviour
{
    public int m_fieldNumber;
    public bool isMonster = true;
    public BattleFieldData m_battleField; //현재 케릭터가 있는 전장
    public CharactorData m_charData;

    private Animator m_animator;
    private float m_attackSpeed; //공격 속도 - 공격 프레임이 끝나는 주기? -> 하는 행동에 따라 값을 받아올것. 
    private float m_curWaitTime = 0; //동작 대기 시간 
    private float m_attackCool;
    private float m_curCool;
    private CharactorObj target;
    private Vector3 goal; //타겟없을때 목적지
    private bool isFowardGoal; //골로 향하는 중인가
    private EnumAniState m_aniState;
    enum EnumAniState
    {
        Idle, Move, Attack
    }

    private void Update()
    {
        m_curCool += Time.deltaTime; //현재쿨 갱신
        if(AbleActionState() == false)
        {
            return;
        }
        //타겟을 찾아본다.
        //못찾았다고 치자.
        FindEnemy();
        //목적지로 움직인다
        Move();
        //공격한다
        DoAction();
        //애니메이션
    

    }

    private bool AbleActionState()
    {
        //행동할 수 있는 상태인가
        if (m_curWaitTime <= 0)
        {
            return true;
        }

        m_curWaitTime -= Time.deltaTime;
        return false;
    }

    private void FindEnemy()
    {
        if (target != null)
            return;

        if (isMonster)
            return;

        float minDistance = float.MaxValue;
        for (int i = 0; i < m_battleField.charactorList.Count; i++)
        {
            if (m_battleField.charactorList[i] == null)
                continue;

            if (isMonster != m_battleField.charactorList[i].isMonster)
            {
                float distance = Vector3.Distance(m_battleField.charactorList[i].transform.position, transform.position);
                if(distance <= minDistance)
                {
                    minDistance = distance;
                    target = m_battleField.charactorList[i];
             
                }
                
            }
            if(target != null)
            {
                isFowardGoal = false;
            }

        }
    }

    private void Move()
    {
        //타겟있으면 현재 타겟있는 위치를 목표 위치로
        //어느 선까지 이동할건지
        float gap = 0.3f;
        if (target != null)
        {
            goal = target.gameObject.transform.position;
            gap = GetCharStat(EnumCharctorStat.AttackReach);
        }
        else if(isFowardGoal == false)
        {
            //타겟 없는 상황에 정해둔 목적지도 없으면
            Vector3 curPos = transform.position;
            float x = Random.Range(curPos.x - 5, curPos.x + 5);
            float y = Random.Range(curPos.y - 5, curPos.y + 5);
            //임의로 목적지 설정
            goal = new Vector3(x, y, 0);
            RestrictPos(ref goal);
            isFowardGoal = true;
        }

        //목적지 까지 도달했으면 이동 안함 
        if(Vector3.Distance(goal, transform.position) <= gap)
        {
            return;
        }

        m_aniState = EnumAniState.Move;
        PlayAnim();
        transform.position += (goal - transform.position).normalized * Time.deltaTime * GetCharStat(EnumCharctorStat.MoveSpeed);
        
    
    }

    private void DoAction()
    {
        //이동 후 
        float gap = 0.3f;
        if (target != null)
        {
            gap = GetCharStat(EnumCharctorStat.AttackReach);
        }

        //목적지 까지 도달했으면 행동함 
        if (Vector3.Distance(transform.position, goal) <= gap)
        {
            if (target != null)
            {
                //타겟 쫓아왔으면 공격
                if(m_curCool >= m_attackCool)
                {
                    if (target.IsDead())
                    {
                        //이미 사망한 상태라면 공격 안함
                        target = null;
                        return;
                    }

                        //공격하고
                    m_aniState = EnumAniState.Attack;
                    PlayAnim();
                    target.Attack(GetCharStat(EnumCharctorStat.AttackPower));
                    m_curCool = 0;
                    m_curWaitTime = m_attackSpeed;

                    //공격이 들어간순간 - gameObject 파괴 속도에 따라 타겟 널이 갈릴 수있음.
                    //널인건 일단 죽였다는거, 여길왔다는건 얘가 때린거기때문에 보상 진행
                    if(target!=null || target.IsDead())
                    {
                        Debug.Log("상대 사망");
                        m_charData.CalStat(EnumCharctorStat.MoveSpeed, 1);
                        m_charData.CalStat(EnumCharctorStat.AttackPower, 3);
                        m_charData.AccelerateActionSpeed(-2);
                        m_attackSpeed = Utility.CalHundred(GetCharStat(EnumCharctorStat.ActionSpeed));
                        m_charData.AccelerateCoolTime(-2);
                        m_attackCool = Utility.CalHundred(GetCharStat(EnumCharctorStat.AttackCoolTime));
                     
                    }

                }
                
                return;
            }

            //그냥 목적지 까지 걸어간거면 목적지 초기화
            isFowardGoal = false;
            m_aniState = EnumAniState.Idle;
        }
    }

    private void PlayAnim()
    {
        int look = 1;
        if((goal.x - transform.position.x) < 0)
        {
            //목적지가 오른쪽에있으면
            look = -1;
        }
        transform.localScale = new Vector3(look, 1, 1);

        switch (m_aniState)
        {
            case EnumAniState.Move:
                m_animator.SetBool("Move", true);
                break;

            case EnumAniState.Attack:
                m_animator.SetBool("Move", false);
                m_animator.SetTrigger("AttackTrigger");
                break;
        }

        
    }

    private void RestrictPos(ref Vector3 _goal)
    {
        _goal.x = Mathf.Min(m_battleField.fieldRight, _goal.x);
        _goal.x = Mathf.Max(m_battleField.fieldLeft, _goal.x);
        _goal.y = Mathf.Min(m_battleField.fieldUp, _goal.y);
        _goal.y = Mathf.Max(m_battleField.fieldDown, _goal.y);
    }

    public void SetBattleField(BattleFieldData _fieldData)
    {
        m_battleField = _fieldData;
    }

    public void SetCharactorData(CharactorData _charData)
    {
        m_charData = _charData;
        _charData.SetCharObj(this);

        m_aniState = EnumAniState.Idle;
        m_animator = GetComponentInChildren<Animator>();

        m_attackCool = Utility.CalHundred(GetCharStat(EnumCharctorStat.AttackCoolTime));
        m_curCool = 0;
        m_attackSpeed = Utility.CalHundred(GetCharStat(EnumCharctorStat.ActionSpeed));
    }

 
    public int GetCharStat(EnumCharctorStat _charStat)
    {
        return m_charData.GetCharStat(_charStat);
    }

    public void Attack(int _power)
    {
        Debug.Log("타겟 공격 "+_power.ToString());
        m_charData.Attack(_power);
    }

    public void Dead()
    {
        BattleManager.Instance.ReportBattle(m_battleField.fieldNumber);
        Invoke(nameof(ObjDestroy), 0.1f);
    }

    private void ObjDestroy()
    {
        Destroy(gameObject);
    }

    public bool IsDead()
    {
        return GetCharStat(EnumCharctorStat.CurHp) <= 0;
    }

    public void MoveNextField(BattleFieldData _nextField)
    {
        m_battleField.charactorList.Remove(this);//기존 필드에서 안녕
        //다음 필드 중심에서 랜덤
        Vector3 movePos = Random.insideUnitSphere * 3 + _nextField.pos;
        movePos.z = 0;
        //위치에 이동
        transform.position = movePos; //다음 전장 시작 위치로 순간이동 하고
        m_battleField = _nextField; //캐릭터에 필드 할당
        m_battleField.charactorList.Add(this);//필드에 캐릭터 할당 
        isFowardGoal = false; //목적지 갱신
        target = null;
    }

}
