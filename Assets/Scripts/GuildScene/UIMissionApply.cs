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
    public IconMissionChar[] m_waitCharIcons; //정보 표기할 슬롯들

    private List<CharactorData> m_inputList;
    private List<CharactorData> m_waitList;
    private int ableInputCount; //투입가능수
    public void OpenApply(Mission _mission)
    {
        //신청 창 온오프
        Window.SetActive(!Window.activeSelf);

        if (Window.activeSelf == false)
            return;

        //신청 정보 표기 초기화
        int inputCount = _mission.inputPlayerCount;
        MakeSamplePool(ref m_inputCharIcons, SlotSample.gameObject, inputCount, m_inputGrid);
        List<CharactorData> haveCharList = MGGuild.Instance.GetHaveCharList();
    
        MakeSamplePool(ref m_waitCharIcons, SlotSample.gameObject, haveCharList.Count, m_haveGrid);
        InputOff(); //투입 용병은 다 꺼놓기 
        SetHaveChars(haveCharList);

        //신청 위한 데이터 초기화
        m_inputList = new();
        m_waitList = new();
        for (int i = 0; i < haveCharList.Count; i++)
        {
            // 데이터 복사 
            m_waitList.Add(haveCharList[i]);
        }
        ableInputCount = _mission.inputPlayerCount;
    }

    private void InputOff()
    {
        for (int i = 0; i < m_inputCharIcons.Length; i++)
        {
            m_inputCharIcons[i].gameObject.SetActive(false);
            m_inputCharIcons[i].SetIcon(EMissionBtnType.Input);
        }
    }

    private void SetHaveChars(List<CharactorData> _haveCharList)
    {
        for (int i = 0; i < _haveCharList.Count; i++)
        {
            m_waitCharIcons[i].gameObject.SetActive(true);
            m_waitCharIcons[i].SetIcon(_haveCharList[i], EMissionBtnType.Wait);
        }
        OffRestSlot(ref m_waitCharIcons, _haveCharList.Count);
    }

    public void SelectChar(EMissionBtnType _btnType, CharactorData _charData)
    {
        if(_btnType == EMissionBtnType.Input)
        {
         
            TakeOffChar(_charData);
        }
        else if(_btnType == EMissionBtnType.Wait)
        {
         
            InputChar(_charData);
            
        }

        //뭐든 했으면 슬롯은 다시 갱신
        RenewSlots();
    }

    public void GoMission()
    {
        if(m_inputList.Count>=1)
        MGGuild.Instance.GoMission(m_inputList);
    }

    private void TakeOffChar(CharactorData _charData)
    {
        //투입에서 누른거면
        //투입 리스트에서 빼고
        //대기 리스트에 추가 
        m_inputList.Remove(_charData);
        m_waitList.Add(_charData);
    }

    private void InputChar(CharactorData _charData)
    {  
        //대기에서 누른거면
        //투입 리스트 수량 확인해서 투입가능하면 
        //대기 리스트에서 빼고
        //투기 리스트에 추가 
        if (m_inputList.Count == ableInputCount)
            return;

        m_inputList.Add(_charData);
        m_waitList.Remove(_charData);
    }

    private void RenewSlots()
    {
        for (int i = 0; i < m_inputList.Count; i++)
        {
            m_inputCharIcons[i].gameObject.SetActive(true);
            m_inputCharIcons[i].InputChar(m_inputList[i]);
        }
        OffRestSlot(ref m_inputCharIcons, m_inputList.Count);

        for (int i = 0; i < m_waitList.Count; i++)
        {
            m_waitCharIcons[i].gameObject.SetActive(true);
            m_waitCharIcons[i].InputChar(m_waitList[i]);
        }
        OffRestSlot(ref m_waitCharIcons, m_waitList.Count);
    }

}
