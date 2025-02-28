using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMissionApply : MonoBehaviour
{
    public GameObject Window;
    public IconMissionChar SlotSample;//슬롯 만들 샘플
    public Transform m_inputGrid;//만들 그리드
    public Transform m_haveGrid;//만들 그리드

    public IconMissionChar[] m_inputCharIcons; //정보 표기할 슬롯들
    public IconMissionChar[] m_haveCharIcons; //정보 표기할 슬롯들

    public void OpenApply(Mission _mission)
    {
        List<CharactorData> haveCharList = MGGuild.Instance.GetHaveCharList();
    }


    public void SelectChar(EMissionBtnType _btnType, CharactorData _charData)
    {

    }
}
