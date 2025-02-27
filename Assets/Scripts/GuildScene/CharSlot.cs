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

    public void Renew()
    {
        string statInfo = "";
        statInfo = curCharData.Name + "\n";
        for (int i = 0; i < curCharData.Stats.Length - 1; i++)
        {
            statInfo += (EnumCharctorStat)(i) + ":" + curCharData.Stats[i].ToString() + "\n";
        }
        statInfo += (EnumCharctorStat)(curCharData.Stats.Length - 1) + ":" + curCharData.Stats[curCharData.Stats.Length - 1].ToString();
        InfoText.text = statInfo;
    }

    public void OnClickBtn()
    {
        if(function == EnumCharSlotFunction.Scout)
        {
            MGGuild.Instance.OnClickScout(curCharData);
            iClickCallBack.OnClickCharSlot(this);
        }
        else if (function == EnumCharSlotFunction.Fire)
        {
            MGGuild.Instance.OnClickFire(curCharData);
        }
    }
}
