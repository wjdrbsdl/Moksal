﻿using System;

[Serializable]
public class Mission
{
    public BattleFieldData[] battleFields; //위치, 너비, 소환할 몬스터, 소환할 수 
    public CharactorData[] inputPlayers; //투입될 용병들
    public int finalGoal; //이것도 마지막 배틀필드로?
    public int inputPlayerCount; //참여 용병들

    public Mission()
    {
        Random random = new();
        inputPlayerCount = random.Next() % 3 + 1;
    }

    public BattleFieldData GetBattleField(int _index)
    {
      return battleFields[_index];
    }
}