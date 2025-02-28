using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class UIMissionApply : UIBase
{
    public GameObject Window;
    public IconMissionChar SlotSample;//슬롯 만들 샘플
    public Transform m_inputGrid;//만들 그리드
    public Transform m_haveGrid;//만들 그리드

    public IconMissionChar[] m_inputCharIcons; //정보 표기할 슬롯들
    public IconMissionChar[] m_haveCharIcons; //정보 표기할 슬롯들

    public void OpenApply(Mission _mission)
    {
        int inputCount = _mission.inputPlayerCount;
        MakeSamplePool(ref m_inputCharIcons, SlotSample.gameObject, inputCount, m_inputGrid);

        List<CharactorData> haveCharList = MGGuild.Instance.GetHaveCharList();
        MakeSamplePool(ref m_haveCharIcons, SlotSample.gameObject, haveCharList.Count, m_haveGrid);

        InputOff(); //투입 용병은 다 꺼놓기 
        SetHaveChars(haveCharList);
    }

    private void InputOff()
    {
        for (int i = 0; i < m_inputCharIcons.Length; i++)
        {
            m_inputCharIcons[i].gameObject.SetActive(false);
            m_haveCharIcons[i].SetIcon(EMissionBtnType.Input);
        }
    }

    private void SetHaveChars(List<CharactorData> _haveCharList)
    {
        for (int i = 0; i < _haveCharList.Count; i++)
        {
            m_haveCharIcons[i].gameObject.SetActive(true);
            m_haveCharIcons[i].SetIcon(_haveCharList[i], EMissionBtnType.Have);
        }
        OffRestSlot(ref m_haveCharIcons, _haveCharList.Count);
    }

    public void SelectChar(EMissionBtnType _btnType, CharactorData _charData)
    {

    }
}
