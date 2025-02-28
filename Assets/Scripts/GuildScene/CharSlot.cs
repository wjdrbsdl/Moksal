using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnumCharSlotFunction
{
    Scout, Fire
}

public class CharSlot : MonoBehaviour
{
    public Text InfoText;
    public EnumCharSlotFunction function = EnumCharSlotFunction.Scout;
    private CharactorData curCharData;
    private ICharSlotClickCallBack iClickCallBack;

    public void SetInfo(CharactorData _charData)
    {
        curCharData = _charData;
        string statInfo = "";
        statInfo = _charData.Name + "\n";
        for (int i = 0; i < _charData.Stats.Length-1; i++)
        {
            statInfo += (EnumCharctorStat)(i) + ":" + _charData.Stats[i].ToString()+"\n";
        }
        statInfo += (EnumCharctorStat)(_charData.Stats.Length-1) + ":" + _charData.Stats[_charData.Stats.Length - 1].ToString();
        InfoText.text = statInfo;
    }

    public void SetInfo(CharactorData _charData, ICharSlotClickCallBack _clickInterface)
    {
        iClickCallBack = _clickInterface;
        curCharData = _charData;
        string statInfo = "";
        statInfo = _charData.Name + "\n";
        for (int i = 0; i < _charData.Stats.Length - 1; i++)
        {
            statInfo += (EnumCharctorStat)(i) + ":" + _charData.Stats[i].ToString() + "\n";
        }
        statInfo += (EnumCharctorStat)(_charData.Stats.Length - 1) + ":" + _charData.Stats[_charData.Stats.Length - 1].ToString();
        InfoText.text = statInfo;
    }

    public CharactorData GetCharData()
    {
       return curCharData;
    }

    public void OnClickBtn()
    {
        if(function == EnumCharSlotFunction.Scout)
        {
            bool isHire = MGGuild.Instance.OnClickScout(curCharData);
            if (isHire)
                iClickCallBack.OnClickCharSlot(this);
            else
                Debug.Log("길드 공간 부족");
        }
        else if (function == EnumCharSlotFunction.Fire)
        {
            MGGuild.Instance.OnClickFire(curCharData);
        }
    }
}
