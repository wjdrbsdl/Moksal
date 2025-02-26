using System.Numerics;
using UnityEngine;

using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System;
using CharacterCreator2D;
using System.IO;

[Serializable]
public class BattleFieldData
{
    //전투구역
    //생성된 몬스터
    //전시상황
    public int fieldNumber;
  
    //전장 위치와 너비 
    public Vector3 pos; //가운데를 기점으로. 
    public float width;
    public float height;

    public float fieldLeft;
    public float fieldRight;
    public float fieldUp;
    public float fieldDown;

    public float spawnRadius ;

    public bool isPlayer = false;
    public int m_spawnMonsterCount;
    public int m_restMonsterCount;

    public List<CharactorBase> charactorList;

    //전투가 일어나는 영역
    //배틀매니저는 각 필드를 돌아보며 전시상황을 살펴보고 
    //해당 필드를 기점으로 몬스터 스폰이나 전투 안내등을 진행 
    //

    public BattleFieldData(Transform _fieldPos, int _fieldNum, bool _isPlayer = false)
    {
        fieldNumber = _fieldNum;
        width = 5f;
        height = 3f;
        pos = _fieldPos.position;
        float fieldHalfWidth = width / 2;
        float fieldHalfHeight = height / 2;
   
        fieldLeft = pos.x - fieldHalfWidth;
        fieldRight = pos.x + fieldHalfWidth;
        fieldUp = pos.y + fieldHalfHeight;
        fieldDown = pos.y - fieldHalfHeight;
        isPlayer = _isPlayer;
        if (isPlayer == false)
        {
            m_spawnMonsterCount = 1;
            m_restMonsterCount = m_spawnMonsterCount;
        }
            
    }

    public void GenerateField()
    {
        GameObject fieldObj = MonoBehaviour.Instantiate(AssetManager.Instance.m_fieldObj, pos, Quaternion.identity);
        fieldObj.transform.localScale = new Vector3(width, height, 1);
        string path = Application.dataPath + "/New Character Data2.json";
        //정보들을 가지고 영역 생성
        //생성 갯수, 만들 샘플, 만들 위치, 구 범위로 만들어서 y 값만 0으로 조정 
        int spawnCount = m_spawnMonsterCount;
        GameObject objectSample = AssetManager.Instance.m_monsterSample.gameObject; //생성될 몬스터는 추후 asset 관리자에서 가져올것. 

        if (isPlayer)
        {
            spawnCount = 1;
            objectSample = AssetManager.Instance.m_playerSample.gameObject;
            path = Application.dataPath + "/F";
        }
        List<CharactorBase> spawnList = new();
       for (int i = 0; i < spawnCount; i++)
        {
            //캐릭터 Go 생성
            Vector3 spawnPos = Random.insideUnitSphere * spawnRadius + pos;
            spawnPos.z = 0;
            CharactorBase charactor = MonoBehaviour.Instantiate(objectSample, spawnPos, Quaternion.identity).GetComponent<CharactorBase>();
            spawnList.Add(charactor);

            //캐릭터에 정보 전달
            charactor.SetFieldNumber(fieldNumber);
            charactor.SetBattleField(this); //전장 정보 넘기고 


            //캐릭터 껍데기 
            CharacterViewer viewer = objectSample.GetComponentInChildren<CharacterViewer>();
            viewer.gameObject.transform.localScale = new Vector3(0.25f, 0.25f, 1);
            viewer.LoadFromJSON(path);
        }
        charactorList = spawnList;
    }

    public void KillMonster()
    {
        m_restMonsterCount -= 1;
    }

    public int RestMonsterCount
    {
        get
        {
            return m_restMonsterCount;
        }
    }

    public List<CharactorBase> GetCharList()
    {
        return charactorList;
    }
}
