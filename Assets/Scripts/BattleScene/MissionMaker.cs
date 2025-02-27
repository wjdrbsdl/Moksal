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
    public bool sceneMove = false;
    public static Mission curMission;
    public int tempPlayerCount = 2;
    public int tempEnemyCount = 5;
    public int tempStep = 5;
    // Use this for initialization
    void Start()
    {
        Mission mission = MakeMission(tempStep);
        curMission = mission;
        AddPlayerChar(); //플레이어가 아군 용병을 넣는 부분
        
        if(sceneMove == true)
        {
            Invoke(nameof(LoadBattleScene), 2f);
        }
        
        
    }

    private void LoadBattleScene()
    {
        SceneManager.LoadScene(1);
    }



    private Mission MakeMission()
    {
        //파싱에서 - 기본적으로 전장지역(전장 위치, 스폰 오브젝트, 플레이어 시작여부)로 되어있어야함.
        Transform[] targetPoints = m_targetPoints; //임의로 전장 위치 

        Mission mission = new Mission(); //임의로 미션을 생성함 - 추후 db로 가져올 것. 

        //필드 정보 담을 리스트
        List<BattleFieldData> battleFieldList = new List<BattleFieldData>();

        //db 정의된 전장수만큼 필드 데이터 생성 
        //플레이어가 소환될 수 있는 시작 전장지역 - true, 소환데이터는 - 추가로 할당해야함. 
        BattleFieldData playerField = new BattleFieldData(targetPoints[0], 0, null, true);
        battleFieldList.Add(playerField);
        
        //몬스터 소환될 전장 생성
        for (int i = 1; i < targetPoints.Length; i++)
        {

            CharactorData[] spawnEnemys = new CharactorData[3];
            for(int e = 0; e < spawnEnemys.Length; e++)
            {
                bool isPlayer = false;
                CharactorData enemyData = new CharactorData(isPlayer);
                spawnEnemys[e] = enemyData;
            }
            
            battleFieldList.Add(new BattleFieldData(targetPoints[i], i, spawnEnemys, false));
        }
        mission.battleFields = battleFieldList.ToArray();
        return mission;
    }

    private Mission MakeMission(int _fieldCount)
    {
        Mission mission = new Mission(); //임의로 미션을 생성함 - 추후 db로 가져올 것. 
        //필드 정보 담을 리스트
        List<BattleFieldData> battleFieldList = new List<BattleFieldData>();

        Vector3 spawnPos = new Vector3(0, 0, 0);
        float maxRange = 15;
        float padding = 2.5f;
        bool right = true;
        //몬스터 소환될 전장 생성
        for (int i = 0; i < _fieldCount; i++)
        {
            float widht = Random.Range(5, maxRange);
            float height = Random.Range(5, maxRange);

            CharactorData[] spawnEnemys = new CharactorData[tempEnemyCount];
            for (int e = 0; e < spawnEnemys.Length; e++)
            {
                bool isPlayer = false;
                CharactorData enemyData = new CharactorData(isPlayer);
                spawnEnemys[e] = enemyData;
            }
            if (right)
            {
                spawnPos.x = (maxRange / 2) + padding;
            }
            else
            {
                spawnPos.x = -(maxRange / 2 + padding);
            }

            spawnPos.y = i * (maxRange/2 + padding);
            //다음 스폰지역 계산하기

            battleFieldList.Add(new BattleFieldData(spawnPos, widht, height, i, spawnEnemys, false));
            right = !right;
        }
        mission.battleFields = battleFieldList.ToArray();
        mission.battleFields[0].ablePlayerSpawn = true; //스폰지역으로 설정
        return mission;
    }

    private void AddPlayerChar()
    {
        //투입가능한 전장에 플레이어 넣기 -> 일단 일괄적으로 플레이어 전장이라고 표기된 부분에 넣기
        CharactorData[] spawnPlayers = new CharactorData[tempPlayerCount];
        for (int e = 0; e < spawnPlayers.Length; e++)
        {
            bool isPlayer = true;
            CharactorData playerData = new CharactorData(isPlayer);
            spawnPlayers[e] = playerData;
        }
        curMission.inputPlayers = spawnPlayers;
        curMission.GetBattleField(0).AddSpawnData(spawnPlayers);

    }
}

