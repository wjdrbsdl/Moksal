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
    public List<CharactorObj> charPlayerList = new();

    //1. 전장 구현
    //2. 게임 시작 
    //3. 소탕이 끝난 전장의 플레이어들은 근접한 전장으로 이동
    //4. 해당 타겟지점에서 클리어 콜백을 받으면 - 다음 타겟지점 판단 - 이런저런 진행 콜백을 받을때마다 승리 패배 판단 
    //투입된 용병은 실시간으로 보여주기 

    private void Start()
    {
        ReadyStage();
    }

    public void ReadyStage()
    {
        Mission _mission = MissionMaker.curMission;
        curMission = _mission;
        BattleFieldData playerField;
        //배틀필드 생성 및 플레이어 필드 가져옴
        GenereateBattleField(_mission, out playerField);
        //플레이어 필드를 가지고 스폰된 캐릭터들 등록
        RegisterPlayer(playerField);
        //3. 카메라 플레이어 스폰된 필드 중 캐릭터 하나에 타겟
        AttachCam(playerField);
        //전투 시작
        //ContinueBattle(playerField);
    }

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
                charPlayerList.Add(charList[i]);
            }
            
        }
    }

    private void AttachCam(BattleFieldData _field)
    {
        follower.target = charPlayerList[0].gameObject;
    }

    private void ContinueBattle(BattleFieldData _curField)
    {
        //플레이 필드에 소환된 용병들에게
        //다음 배틀필드로 이동을 명령하면서 전투 시작 
        //다음 전투지는 현재 ++ 인덱스로 계산
        Debug.Log(_curField.fieldNumber + "영역에서 다음 배틀필드 이동");
        BattleFieldData nextFiled = FindNextBattleField(_curField);

        if(nextFiled == null)
        {
            Debug.Log("전장 끝");
            EndBattle();
            return;
        }
        //현재 필드에 있는 영웅들을 
        //다음 지역으로 이동 시키기 
        for (int i = 0; i < charPlayerList.Count; i++)
        {
            if (charPlayerList[i] == null)
                continue;

            charPlayerList[i].MoveNextField(nextFiled);
        }
    }

    private BattleFieldData FindNextBattleField(BattleFieldData _battleField)
    {
        //생성된 배틀필드 정보를 노드로 가족 있을것이고
        //시작 필드 넘버를 가지고 갈수 있는걸 반환 지금은 그냥 단계단계
        if (_battleField.fieldNumber + 1 >= curMission.battleFields.Length)
        {
            return null;
        }
        return curMission.battleFields[_battleField.fieldNumber + 1];
    }

    public void ReportBattle(int _fieldNum)
    {
        //몬스터 죽을때마다 보고 
        BattleFieldData reportField = curMission.GetBattleField(_fieldNum);
        reportField.KillMonster();
        Debug.Log(reportField.fieldNumber +"영역 남은 몬스터 " + reportField.RestMonsterCount);
        if (reportField.RestMonsterCount == 0)
        {
            ContinueBattle(reportField);
        }
    }

    private void EndBattle()
    {
        //전투가 끝난경우
        //결산 
        //해당 씬 파괴 
        //마을로 컴백
        Invoke(nameof(LoadGuildScene),2f);
    }

    private void LoadGuildScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
