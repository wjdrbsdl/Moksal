using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BattleManager : SigleTon<BattleManager>
{
    //�̹� �̼� ������ ���� ������ ������, 
    //���������� �����ϰ� 
    //�ܰ踦 ���� �ϰ� 
    //����� �Ǵ��ϴ� �� 

    //�̼� ���鶧 �� �����Ϳ� �־���Ұ�
    public Transform[] m_targetPoints;
    public CameraFollows follower;
    public Mission curMission;
    private List<CharactorObj> m_charPlayerList = new();
    public UIBattleCharIcon m_playerIconUI;
    //1. ���� ����
    //2. ���� ���� 
    //3. ������ ���� ������ �÷��̾���� ������ �������� �̵�
    //4. �ش� Ÿ���������� Ŭ���� �ݹ��� ������ - ���� Ÿ������ �Ǵ� - �̷����� ���� �ݹ��� ���������� �¸� �й� �Ǵ� 
    //���Ե� �뺴�� �ǽð����� �����ֱ� 

    private void Start()
    {
        m_actionRecorder = new ActionRecorder();
        ReadyStage();
    }

    public void ReadyStage()
    {
        Mission _mission = MissionMaker.curMission;
        curMission = _mission;
        BattleFieldData playerField;
        //��Ʋ�ʵ� ���� �� �÷��̾� �ʵ� ������
        GenereateBattleField(_mission, out playerField);
        //�÷��̾� �ʵ带 ������ ������ ĳ���͵� ���
        RegisterPlayer(playerField);
        //
        MakeBattleCharIcon();
        //3. ī�޶� �÷��̾� ������ �ʵ� �� ĳ���� �ϳ��� Ÿ��
        AttachCam(playerField);
        //���� ����
        //ContinueBattle(playerField);
    }

    #region ���� �غ�
    private void GenereateBattleField(Mission _mission, out BattleFieldData _playerField)
    {
        _playerField = null;
        for (int i = 0; i < _mission.battleFields.Length; i++)
        {
            if (_mission.battleFields[i].ablePlayerSpawn)
            {
                _playerField = _mission.battleFields[i];
            }

            _mission.battleFields[i].GenerateField();
        }
    }
    
    private void RegisterPlayer(BattleFieldData _playerField)
    {
        List<CharactorObj> charList = _playerField.GetCharList();
        for (int i = 0; i < charList.Count; i++)
        {
            if (charList[i].isMonster == false)
            {
                m_charPlayerList.Add(charList[i]);
            }
            
        }
    }

    private void MakeBattleCharIcon()
    {
        m_playerIconUI.SetCharIcons(m_charPlayerList);
    }

    private void AttachCam(BattleFieldData _field)
    {
       CameraFollows.SetCamTarget( m_charPlayerList[0]);
    }
    #endregion

    #region ���� ����
    private void ContinueBattle(BattleFieldData _curField)
    {
        //�÷��� �ʵ忡 ��ȯ�� �뺴�鿡��
        //���� ��Ʋ�ʵ�� �̵��� ����ϸ鼭 ���� ���� 
        //���� �������� ���� ++ �ε����� ���
        Debug.Log(_curField.fieldNumber + "�������� ���� ��Ʋ�ʵ� �̵�");
        BattleFieldData nextFiled = FindNextBattleField(_curField);

        if(nextFiled == null)
        {
            Debug.Log("���� ��");
            EndBattle();
            return;
        }
        //���� �ʵ忡 �ִ� �������� 
        //���� �������� �̵� ��Ű�� 
        for (int i = 0; i < m_charPlayerList.Count; i++)
        {
            if (m_charPlayerList[i] == null)
                continue;

            m_charPlayerList[i].MoveNextField(nextFiled);
        }
    }

    private BattleFieldData FindNextBattleField(BattleFieldData _battleField)
    {
        //������ ��Ʋ�ʵ� ������ ���� ���� �������̰�
        //���� �ʵ� �ѹ��� ������ ���� �ִ°� ��ȯ ������ �׳� �ܰ�ܰ�
        if (_battleField.fieldNumber + 1 >= curMission.battleFields.Length)
        {
            return null;
        }
        return curMission.battleFields[_battleField.fieldNumber + 1];
    }


    #endregion

    #region ���� ��Ȳ
    public void ReportBattle(int _fieldNum)
    {
        //���� ���������� ���� 
        BattleFieldData reportField = curMission.GetBattleField(_fieldNum);
        reportField.KillMonster();
        Debug.Log(reportField.fieldNumber + "���� ���� ���� " + reportField.RestMonsterCount);
        if (reportField.RestMonsterCount == 0)
        {
            ContinueBattle(reportField);
        }
    }
    #endregion 

    private void EndBattle()
    {
        //������ �������
        //��� 
        //�ش� �� �ı� 
        //������ �Ĺ�
        m_actionRecorder.Print();
        Invoke(nameof(LoadGuildScene),2f);
    }

    private void LoadGuildScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    private ActionRecorder m_actionRecorder;
    public void RecordData(ActionData _data)
    {
        m_actionRecorder.AddData(_data);
    }
}
