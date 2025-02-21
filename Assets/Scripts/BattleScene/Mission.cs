using System;

[Serializable]
public class Mission
{
    public BattleField[] battleFields; //위치, 너비, 소환할 몬스터, 소환할 수 
    public int finalGoal; //이것도 마지막 배틀필드로?
    public int players; //참여 용병들


    public BattleField GetBattleField(int _index)
    {
      return battleFields[_index];
    }
}