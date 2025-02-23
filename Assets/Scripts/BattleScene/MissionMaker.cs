using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MissionMaker : MonoBehaviour
{
    //배틀 매니저에 전달할 미션지를 생성 
    //맵핑 기록 
    //배틀 필드와 필드끼리 연결, 각 필드에 배칠된 몬스터 종류와 수 
    //용병 시작점 - isPlayer로 지정
    //참여 용병 기록
    
    public float spawnRadius = 5f;
    public Transform[] m_targetPoints;

    // Use this for initialization
    void Start()
    {
        Mission mission = MakeMission();
        curMission = mission;
       // Invoke(nameof(LoadBattleScene), 2f);
        
    }

    private void LoadBattleScene()
    {
        SceneManager.LoadScene(1);
    }

    public static Mission curMission;

    private Mission MakeMission()
    {
        //타겟 포인트는 - 플레이어, 몬스터 스폰지로서 맵정보에 담겨 있어야할 것들. 임의로 월드 좌표에 설정해놨음
        //파싱 데이터로 - 미션지에 몬스터 스폰지와, 몬스터 정보
        //플레이어 조작으로 - 플레이어 스폰지에, 용병을 할당해서 정보를 만들것. 
        Transform[] targetPoints = m_targetPoints;
        Mission mission = new Mission();
        
        List<BattleField> battleFieldList = new List<BattleField>();
        BattleField playerField = new BattleField(targetPoints[0], 0, true);
        battleFieldList.Add(playerField);
        
        for (int i = 1; i < targetPoints.Length; i++)
        {
            battleFieldList.Add(new BattleField(targetPoints[i], i));
        }
        mission.battleFields = battleFieldList.ToArray();
        return mission;
    }

}

