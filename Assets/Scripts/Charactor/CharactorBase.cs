﻿using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine;


public class CharactorBase : MonoBehaviour
{
    public int m_fieldNumber;
    public bool isMonster = true;
    public BattleFieldData m_battleField; //현재 케릭터가 있는 전장
    public CharactorData m_charData;

    private Transform target;
    private Vector3 goal; //타겟없을때 목적지
    private bool isFowardGoal; //골로 향하는 중인가

    private void Update()
    {
        //타겟을 찾아본다.
        //못찾았다고 치자.
        FindEnemy();
        //목적지로 움직인다
        Move();
        //공격한다

    }

    private void FindEnemy()
    {
        if (target != null)
            return;

        if (isMonster)
            return;

        for (int i = 0; i < m_battleField.charactorList.Count; i++)
        {
             if(isMonster != m_battleField.charactorList[i].isMonster)
            {
                target = m_battleField.charactorList[i].gameObject.transform;
                isFowardGoal = false;
                break;
            }

        }
    }

    private void Move()
    {
        //타겟있으면 현재 타겟있는 위치를 목표 위치로
        if(target != null)
        {
            goal = target.position;
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

        //목적지를 향해 이동 
        transform.position += (goal - transform.position).normalized * Time.deltaTime * m_charData.GetCharStat(EnumCharctorStat.MoveSpeed);
        
        if(Vector3.Distance(transform.position, goal) <= 0.3f)
        {
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

    public void SetFieldNumber(int _num)
    {
        m_fieldNumber = _num;
    }

    public void Attack()
    {
        m_charData.Attack(10);
    }
    public void Dead()
    {
        BattleManager.Instance.ReportBattle(m_fieldNumber);
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

    private void OnTriggerEnter(Collider other)
    {
        if (isMonster == true)
            return;

        other.GetComponent<CharactorBase>().Attack();
    }
}
