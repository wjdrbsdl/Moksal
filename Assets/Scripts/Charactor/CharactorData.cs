using System;



public enum EnumCharctorStat
{
    MaxHp, CurHp, AttackPower, DefencePower, MoveSpeed, AttackReach, AttackCoolTime, Sight, ActionDelay
}

public enum EnumCharTemperament
{
    광기, 겁쟁이, 주먹숭배, 마조히즘, 독선자
}


[Serializable]
public class CharactorData
{
    static private int MakeCount = 1;
    public string Name;
    public bool isPlayer = true;
    public int[] Stats;
    private CharactorObj charObj;
    public EnumCharTemperament[] temperament;

    public ICharAction[] haveActions; //보유한 행동
    public static TAttackType[] gAttackData = { new TAttackType(10, 100, 100, 2 ), new TAttackType(1, 300, 300, 10) };
    public TAttackType curAttack;

    public CharactorData(bool _isPlayer)
    {
        Stats = new int[Enum.GetValues(typeof(EnumCharctorStat)).Length];
        Name = "테스트"+MakeCount.ToString();
        for (int i = 0; i < Stats.Length; i++)
        {
            Random random = new();
            int randomPower = random.Next() % 99+1; //임의로 스텟 값 0~99
            Stats[i] = randomPower;
        }
        Stats[(int)EnumCharctorStat.MoveSpeed] = 300;
        Stats[(int)EnumCharctorStat.AttackReach] = 1;
        Stats[(int)EnumCharctorStat.AttackCoolTime] = 200;
        Stats[(int)EnumCharctorStat.ActionDelay] = 100;
        MakeCount++;
        isPlayer = _isPlayer;

        //행동 설정
        haveActions = new ICharAction[]{ new AttackAction(), new WanderAction() }; //보유행동 입력
        //공격인 경유 공격 유형대로 속성 값 설정
        curAttack = gAttackData[MakeCount % 2]; //해당 캐릭 공격유형 설정 - 공격액션없으면 안할짓
        (haveActions[0] as AttackAction).SetAttackProperty(curAttack); //쿨, 딜레이
        SetCharStat(EnumCharctorStat.AttackReach, curAttack.AttackReach); //사거리
        SetCharStat(EnumCharctorStat.AttackPower, curAttack.AttackPower); //파워

        //기질
        DiceTemperament();
    }

    private void DiceTemperament()
    {
        int randomCount = 5; //배열을 섞을 횟수
        Random random = new Random();
        int temperCount = Enum.GetValues(typeof(EnumCharTemperament)).Length;
        int[] indexArray = new int[temperCount];
        for (int i = 0; i < indexArray.Length; i++)
        {
            indexArray[i] = i;
        }

        //섞기
        for (int i = 0; i < randomCount; i++)
        {
            int startIndex = i % temperCount;
            int dice = random.Next() % temperCount;
            int tempValue = indexArray[startIndex];
            indexArray[startIndex] = indexArray[dice];
            indexArray[dice] = tempValue;
        }
        //2개 할당
        temperament = new EnumCharTemperament[] { (EnumCharTemperament)indexArray[0], (EnumCharTemperament)indexArray[1] };
    }

    public int GetCharStat(EnumCharctorStat _charStat)
    {
        return Stats[(int)_charStat];
    }

    public void SetCharStat(EnumCharctorStat _charStat, int _value)
    {
        Stats[(int)_charStat] = _value;
    }

    public void CalStat(EnumCharctorStat _charStat, int _value)
    {
        Stats[(int)_charStat] += _value;
    }

    public void AccelerateActionSpeed(int _value)
    {
        Stats[(int)EnumCharctorStat.ActionDelay] += _value;
        if (Stats[(int)EnumCharctorStat.ActionDelay] <= 2)
        {
            Stats[(int)EnumCharctorStat.ActionDelay] = 2;
        }
    }

    public void AccelerateCoolTime(int _value)
    {
        Stats[(int)EnumCharctorStat.AttackCoolTime] += _value;
        if (Stats[(int)EnumCharctorStat.AttackCoolTime] <= 2)
        {
            Stats[(int)EnumCharctorStat.AttackCoolTime] = 2;
        }
    }

    public void Attack(int _damage)
    {
        Stats[(int)EnumCharctorStat.CurHp] -= _damage;
        if(Stats[(int)EnumCharctorStat.CurHp] <= 0)
        {
            charObj.Dead();
        }
    }

    public void SetCharObj(CharactorObj _obj)
    {
        charObj = _obj;
    }
}
