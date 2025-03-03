using UnityEngine;


public enum EnumAniState
{
    Idle, Move, Attack
}
public class CharactorObj : MonoBehaviour
{
    #region 고유 데이터
    public bool isMonster = true;
    public CharactorData m_charData;
    #endregion

    #region 플레이어 행동 데이터
    public BattleFieldData m_battleField; //현재 케릭터가 있는 전장
    private Animator m_animator;
    public float m_actionSpeed; //공격 속도 - 공격 프레임이 끝나는 주기? -> 하는 행동에 따라 값을 받아올것. 
    public float m_curWaitTime = 0; //동작 대기 시간 
    
    public float m_actionCool;
    public float m_curCool;

 
    #endregion

    private CharActionBase curAction;
    private void Update()
    {
        m_curCool += Time.deltaTime; //현재쿨 갱신
        if(AbleActionState() == false)
        {
            return;
        }
        if(curAction == null)
        {
            int randomAction = Random.Range(0, 2);
            ShowDialog();
            curAction = m_charData.haveActions[randomAction];
            curAction.CalDeltaTime();
        }
        curAction.Update(this);
   
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

    public bool Move(Vector3 _goal, float _paddingDistance)
    {
        //타겟있으면 현재 타겟있는 위치를 목표 위치로
        //어느 선까지 이동할건지
        float gap = _paddingDistance;
     
        //목적지 까지 도달했으면 이동 안함 
        if(Vector3.Distance(_goal, transform.position) <= gap)
        {
            return true;
        }

        PlayAnim(EnumAniState.Move, _goal);
        transform.position += (_goal - transform.position).normalized * Time.deltaTime * Utility.CalHundred(GetCharStat(EnumCharctorStat.MoveSpeed));
        return false;
    }

    public void PlayAnim(EnumAniState _aniEnum, Vector3 _goal)
    {
        int look = 1;
        if((_goal.x - transform.position.x) < 0)
        {
            //목적지가 오른쪽에있으면
            look = -1;
        }
        

        switch (_aniEnum)
        {
            case EnumAniState.Move:
                m_animator.SetBool("Move", true);
                break;

            case EnumAniState.Attack:
                m_animator.SetBool("Move", false);
                m_animator.SetTrigger("AttackTrigger");
                break;
            case EnumAniState.Idle:
                m_animator.SetBool("Move", false);
                return;
        }

        transform.localScale = new Vector3(look, 1, 1);


    }

    public void ResetAction()
    {
        //각 파트에서 액션을 초기화할때 호출 
        curAction = null;
        PlayAnim(EnumAniState.Idle, transform.position);
    }

    public void ResetCool()
    {
        m_curCool = 0;
    }

    public void SetDelayTime(float _delayTime)
    {
        //특정 행동후 대기시간 설정
        m_curWaitTime = _delayTime;
    }


    CharDialog dialog;
    #region 공용
    public void ShowDialog()
    {
        if(dialog == null)
        dialog = CharDialogManager.Instance.GetDialogObj();

        dialog.ShowDialog(this," 수행");
    }

    public void RestrictPos(ref Vector3 _goal)
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

        m_animator = GetComponentInChildren<Animator>();

        m_actionCool = Utility.CalHundred(GetCharStat(EnumCharctorStat.AttackCoolTime));
        ResetCool();
        m_actionSpeed = Utility.CalHundred(GetCharStat(EnumCharctorStat.ActionDelay));
    }

    public int GetCharStat(EnumCharctorStat _charStat)
    {
        return m_charData.GetCharStat(_charStat);
    }

    public void Attack(int _power)
    {
     //   Debug.Log("타겟 공격 "+_power.ToString());
        m_charData.Attack(_power);
    }

    public void Attack(ActionData _attackRecord)
    {
        //피해자쪽에서 방어율 계산해서 최종 적용할 데미지 산출
        BattleManager.Instance.RecordData(_attackRecord);
        m_charData.Attack(_attackRecord.adaptDamage);
    }

    public void Dead()
    {
        BattleManager.Instance.ReportBattle(m_battleField.fieldNumber);
        Destroy(dialog.gameObject);
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
    }
    #endregion

}
