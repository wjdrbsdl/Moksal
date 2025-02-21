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

    //1. ���� �������ϰ� Ÿ������ ����
    // - ������ �� , ������ ���� ��ġ, ���� �ܰ��, �ӹ���ǥ(���� ���)
    //2. Ÿ�������� ���͸� ��ȯ 
    //3. ���������� �÷��̾� ��ȯ 
    //4. ���� ���� 
    //5. �÷��̾���� - ����� Ÿ���������� �̵� 
    //6. �ش� Ÿ���������� Ŭ���� �ݹ��� ������ - ���� Ÿ������ �Ǵ� - �̷����� ���� �ݹ��� ���������� �¸� �й� �Ǵ� 

    public void ReadyStage(Mission _mission)
    {
        curMission = _mission;
        BattleField playerField = null;
        //2. ��Ʋ�ʵ� ����
        for (int i = 0; i < _mission.battleFields.Length; i++)
        {
            if (_mission.battleFields[i].isPlayer)
            {
                playerField = _mission.battleFields[i];
            }
                
            _mission.battleFields[i].GenerateField();
        }
        //3. ī�޶� �÷��̾� ������ �ʵ� �� ĳ���� �ϳ��� Ÿ��
        AttachCam(playerField);
        //���� ����
        PlayBattle(playerField);
    }

    private void AttachCam(BattleField _field)
    {
        follower.target = _field.charactores[0].gameObject;
    }

    private void PlayBattle(BattleField _curField)
    {
        //�÷��� �ʵ忡 ��ȯ�� �뺴�鿡��
        //���� ��Ʋ�ʵ�� �̵��� ����ϸ鼭 ���� ���� 
        //���� �������� ���� ++ �ε����� ���
        BattleField nextFiled = NextBattleField(_curField);

        //���� �ʵ忡 �ִ� �������� 
        //���� �������� �̵� ��Ű�� 
        for (int i = 0; i < _curField.charactores.Length; i++)
        {
            if (nextFiled.charactores[i] == null)
                continue;

            CharPlayer player = _curField.charactores[i] as CharPlayer;
            if(player != null)
            {
                player.SetTarget(nextFiled.pos);
            }

        }
    }

    private BattleField NextBattleField(BattleField _battleField)
    {
        //������ ��Ʋ�ʵ� ������ ���� ���� �������̰�
        //���� �ʵ� �ѹ��� ������ ���� �ִ°� ��ȯ ������ �׳� �ܰ�ܰ�
        if(_battleField.fieldNumber+ 1 >= curMission.battleFields.Length)
        {
            return null;
        }
        return curMission.battleFields[_battleField.fieldNumber + 1];
    }

    public void ReportBattle(int _fieldNum)
    {
        //���� ���������� ���� 
        BattleField field = curMission.GetBattleField(_fieldNum);
        field.KillMonster();

        if(field.restMonster == 0)
        {
            PlayBattle(field);
        }
    }
}
