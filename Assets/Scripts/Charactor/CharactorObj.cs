using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine;


public class CharactorObj : MonoBehaviour
{
    public int m_fieldNumber;
    public bool isMonster = true;
    public BattleFieldData m_battleField; //현재 케릭터가 있는 전장
    public CharactorData m_charData;

    private float m_attackCool;
    private float m_curCool;
    private CharactorObj target;
    private Vector3 goal; //타겟없을때 목적지
    private bool isFowardGoal; //골로 향하는 중인가

    private void Update()
    {
        m_curCool += Time.deltaTime; //현재쿨 갱신
        //타겟을 찾아본다.
        //못찾았다고 치자.
        FindEnemy();
        //목적지로 움직인다
        Move();
        //공격한다
        DoAction();

    }

    private void FindEnemy()
    {
        if (target != null)
            return;

        if (isMonster)
            return;

        for (int i = 0; i < m_battleField.charactorList.Count; i++)
        {
            if (m_battleField.charactorList[i] == null)
                continue;

            if (isMonster != m_battleField.charactorList[i].isMonster)
            {
                target = m_battleField.charactorList[i];
                isFowardGoal = false;
                break;
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
            gap = m_charData.GetCharStat(EnumCharctorStat.AttackReach);
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
        transform.position += (goal - transform.position).normalized * Time.deltaTime * m_charData.GetCharStat(EnumCharctorStat.MoveSpeed);
        
    
    }

    private void DoAction()
    {
        //이동 후 
        float gap = 0.3f;
        if (target != null)
        {
            gap = m_charData.GetCharStat(EnumCharctorStat.AttackReach);
        }

        //목적지 까지 도달했으면 행동함 
        if (Vector3.Distance(transform.position, goal) <= gap)
        {
            if (target != null)
            {
                //타겟 쫓아왔으면 공격
                if(m_curCool >= m_attackCool)
                {
                    //공격하고
                    target.Attack();
                    m_curCool = 0;

                    //사망체크 나중엔 딴 함수로
                    if(target.gameObject == null)
                    {
                        target = null; //타겟 초기화 
                    }
                }
                
                return;
            }

            //그냥 목적지 까지 걸어간거면 목적지 초기화
            isFowardGoal = false;
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
        _charData.charObj = this;

        m_attackCool = _charData.GetCharStat(EnumCharctorStat.AttackCoolTime);
        m_curCool = 0;
    }

    public void Attack()
    {
        Debug.Log("타겟 공격");
        m_charData.Attack(10);
    }

    public void Dead()
    {
        BattleManager.Instance.ReportBattle(m_battleField.fieldNumber);
        Destroy(gameObject);
    }

    public void MoveNextField(BattleFieldData _nextField)
    {
        m_battleField.charactorList.Remove(this);//기존 필드에서 안녕
        transform.position = _nextField.pos; //다음 전장 시작 위치로 순간이동 하고
        m_battleField = _nextField; //캐릭터에 필드 할당
        m_battleField.charactorList.Add(this);//필드에 캐릭터 할당 
        isFowardGoal = false; //목적지 갱신
        target = null;
    }

}
