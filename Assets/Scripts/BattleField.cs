using System.Numerics;
using UnityEngine;

using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System;

[Serializable]
public class BattleField
{
    //전투구역
    //생성된 몬스터
    //전시상황
    public int fieldNumber;
    public int restMonster;


    //전장 위치와 너비 
    public Vector3 pos;
    public float width;
    public float height;

    public float spawnRadius ;

    public bool isPlayer = false;
    public int m_spawnMonsterCount = 0;
    public int m_restMonsterCount = 0;

    public CharactorBase[] charactores;

    //전투가 일어나는 영역
    //배틀매니저는 각 필드를 돌아보며 전시상황을 살펴보고 
    //해당 필드를 기점으로 몬스터 스폰이나 전투 안내등을 진행 
    //

    public BattleField(Transform _fieldPos, int _fieldNum, bool _isPlayer = false)
    {
        fieldNumber = _fieldNum;
        pos = _fieldPos.position;
        isPlayer = _isPlayer;
        if (isPlayer == false)
        {
            m_spawnMonsterCount = 1;
            m_restMonsterCount = m_spawnMonsterCount;
        }
            
    }

    public void GenerateField()
    {
        //정보들을 가지고 영역 생성
        //생성 갯수, 만들 샘플, 만들 위치, 구 범위로 만들어서 y 값만 0으로 조정 
        int spawnCount = m_spawnMonsterCount;
        GameObject objectSample = AssetManager.Instance.m_monsterSample.gameObject; //생성될 몬스터는 추후 asset 관리자에서 가져올것. 

        if (isPlayer)
        {
            spawnCount = 1;
            objectSample = AssetManager.Instance.m_playerSample.gameObject;
        }
        List<CharactorBase> spawnList = new();
       for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPos = Random.insideUnitSphere * spawnRadius + pos;
            spawnPos.y = 0;
            CharactorBase charactor = MonoBehaviour.Instantiate(objectSample, spawnPos, Quaternion.identity).GetComponent<CharactorBase>();
   
            spawnList.Add(charactor);
        }
        charactores = spawnList.ToArray();
    }

    public void KillMonster()
    {
        m_restMonsterCount -= 1;
    }
}
