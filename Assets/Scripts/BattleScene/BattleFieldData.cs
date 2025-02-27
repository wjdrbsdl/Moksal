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

    public bool ablePlayerSpawn = false;
    public int m_restMonsterCount;

    public CharactorData[] m_spawnCharData; //스폰시킬 케릭 데이터들
    public List<CharactorObj> charactorList; //현재 필드에 있는 케릭터 Obj

    //전투가 일어나는 영역
    //배틀매니저는 각 필드를 돌아보며 전시상황을 살펴보고 
    //해당 필드를 기점으로 몬스터 스폰이나 전투 안내등을 진행 
    //

    public BattleFieldData(Transform _fieldPos, int _fieldNum, CharactorData[] _spawnObjects, bool _ablePlayerSpawn = false)
    {
        fieldNumber = _fieldNum;
        //공간 설정
        width = 5f;
        height = 3f;
        pos = _fieldPos.position;
        CalRestrict();

        //플레이어 스폰지역인가 -> 필요없음. 넣는 캐릭터 정보로 판단
        ablePlayerSpawn = _ablePlayerSpawn;

        m_spawnCharData = _spawnObjects;
        if(m_spawnCharData == null)
        {
            //스폰 데이터 들어온게 없으면 크기 0 짜리로라도 만들어놓기. 
            m_spawnCharData = new CharactorData[] { }; 
        }
    }

    public BattleFieldData(Vector3 _pos, float _width, float _height , int _fieldNum, CharactorData[] _spawnObjects, bool _ablePlayerSpawn = false)
    {
        fieldNumber = _fieldNum;
        //공간 설정
        width = _width;
        height = _height;
        pos = _pos;
        CalRestrict();
        //플레이어 스폰지역인가 -> 필요없음. 넣는 캐릭터 정보로 판단
        ablePlayerSpawn = _ablePlayerSpawn;

        m_spawnCharData = _spawnObjects;
        if (m_spawnCharData == null)
        {
            //스폰 데이터 들어온게 없으면 크기 0 짜리로라도 만들어놓기. 
            m_spawnCharData = new CharactorData[] { };
        }
    }

    public void GenerateField()
    {
        GameObject fieldObj = MonoBehaviour.Instantiate(AssetManager.Instance.m_fieldObj, pos, Quaternion.identity);
        fieldObj.transform.localScale = new Vector3(width, height, 1);
        
        //정보들을 가지고 영역 생성
        //생성 갯수, 만들 샘플, 만들 위치, 구 범위로 만들어서 y 값만 0으로 조정 
        int spawnCount = m_spawnCharData.Length;
        GameObject objectSample = AssetManager.Instance.m_monsterSample.gameObject; //생성될 몬스터는 추후 asset 관리자에서 가져올것. 
    
        charactorList = new();
        for (int i = 0; i < spawnCount; i++)
        {
            //스폰하려는 캐릭터 데이터
            CharactorData charData = m_spawnCharData[i];
            bool isPlayer = charData.isPlayer;
            string path = Application.dataPath + "/New Character Data2.json";
            if(charData.isPlayer)
                path = Application.dataPath + "/F";

            //스폰할 위치 계산
            Vector3 spawnPos = Random.insideUnitSphere * spawnRadius + pos;
            spawnPos.z = 0;

            //캐릭 오브젝트 생성
            CharactorObj charactorObj = MonoBehaviour.Instantiate(objectSample, spawnPos, Quaternion.identity).GetComponent<CharactorObj>();
            charactorList.Add(charactorObj);
            charactorObj.isMonster = !isPlayer; 

            //캐릭 오브젝트에 값 할당
            charactorObj.SetBattleField(this); //전장 정보
            charactorObj.SetCharactorData(charData); //오브젝트에 캐릭데이터 할당

            //캐릭터 데이터에 obj 연결
            charData.SetCharObj(charactorObj);

            //캐릭터 껍데기 
            CharacterViewer viewer = objectSample.GetComponentInChildren<CharacterViewer>();
            viewer.gameObject.transform.localScale = new Vector3(0.25f, 0.25f, 1);
            viewer.LoadFromJSON(path);

            if (!isPlayer)
            {
                RestMonsterCount++;
            }
                
        }
      
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
        set
        {
            m_restMonsterCount = value;
        }
    }

    public List<CharactorObj> GetCharList()
    {
        return charactorList;
    }

    public void AddSpawnData(CharactorData[] _addCharData)
    {
        //스폰시킬 데이터 정보를 넣으면 
        CharactorData[] renewSpawnData = new CharactorData[m_spawnCharData.Length + _addCharData.Length];
        for (int i = 0; i < m_spawnCharData.Length; i++)
        {
            renewSpawnData[i] = m_spawnCharData[i];
        }
        for (int i = 0; i < _addCharData.Length; i++)
        {
            renewSpawnData[i + m_spawnCharData.Length] = _addCharData[i];
        }
        m_spawnCharData = renewSpawnData;
    }

    public void CalRestrict()
    {
    
        float fieldHalfWidth = width / 2;
        float fieldHalfHeight = height / 2;
        fieldLeft = pos.x - fieldHalfWidth;
        fieldRight = pos.x + fieldHalfWidth;
        fieldUp = pos.y + fieldHalfHeight;
        fieldDown = pos.y - fieldHalfHeight;
    }
}
