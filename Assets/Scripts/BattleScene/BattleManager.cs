using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BattleManager : SigleTon<BattleManager>
{
    //이번 미션 수행을 위한 정보를 가지고, 
    //스테이지를 생성하고 
    //단계를 관리 하고 
    //결과를 판단하는 곳 

    //미션 만들때 맵 데이터에 있어야할것
    public Transform[] m_targetPoints;
    public CameraFollows follower;
    public Mission curMission;

    //1. 맵을 만들어야하고 타겟지점 지정
    // - 고정된 맵 , 고정된 시작 위치, 구성 단계와, 임무목표(몬스터 토벌)
    //2. 타겟지점에 몬스터를 소환 
    //3. 시작지점에 플레이어 소환 
    //4. 게임 시작 
    //5. 플레이어들을 - 가까운 타겟지점으로 이동 
    //6. 해당 타겟지점에서 클리어 콜백을 받으면 - 다음 타겟지점 판단 - 이런저런 진행 콜백을 받을때마다 승리 패배 판단 

    public void ReadyStage(Mission _mission)
    {
        curMission = _mission;
        BattleField playerField = null;
        //2. 배틀필드 구현
        for (int i = 0; i < _mission.battleFields.Length; i++)
        {
            if (_mission.battleFields[i].isPlayer)
            {
                playerField = _mission.battleFields[i];
            }
                
            _mission.battleFields[i].GenerateField();
        }
        //3. 카메라 플레이어 스폰된 필드 중 캐릭터 하나에 타겟
        AttachCam(playerField);
        //전투 시작
        PlayBattle(playerField);
    }

    private void AttachCam(BattleField _field)
    {
        follower.target = _field.charactores[0].gameObject;
    }

    private void PlayBattle(BattleField _curField)
    {
        //플레이 필드에 소환된 용병들에게
        //다음 배틀필드로 이동을 명령하면서 전투 시작 
        //다음 전투지는 현재 ++ 인덱스로 계산
        BattleField nextFiled = NextBattleField(_curField);

        //현재 필드에 있는 영웅들을 
        //다음 지역으로 이동 시키기 
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
        //생성된 배틀필드 정보를 노드로 가족 있을것이고
        //시작 필드 넘버를 가지고 갈수 있는걸 반환 지금은 그냥 단계단계
        if(_battleField.fieldNumber+ 1 >= curMission.battleFields.Length)
        {
            return null;
        }
        return curMission.battleFields[_battleField.fieldNumber + 1];
    }

    public void ReportBattle(int _fieldNum)
    {
        //몬스터 죽을때마다 보고 
        BattleField field = curMission.GetBattleField(_fieldNum);
        field.KillMonster();

        if(field.restMonster == 0)
        {
            PlayBattle(field);
        }
    }
}
