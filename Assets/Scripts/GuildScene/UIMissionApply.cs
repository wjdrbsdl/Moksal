using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class UIMissionApply : UIBase
{
    public GameObject Window;
    public IconMissionChar SlotSample;//���� ���� ����
    public Transform m_inputGrid;//���� �׸���
    public Transform m_haveGrid;//���� �׸���

    public IconMissionChar[] m_inputCharIcons; //���� ǥ���� ���Ե�
    public IconMissionChar[] m_waitCharIcons; //���� ǥ���� ���Ե�

    private List<CharactorData> m_inputList;
    private List<CharactorData> m_waitList;
    private int ableInputCount; //���԰��ɼ�
    public void OpenApply(Mission _mission)
    {
        //��û â �¿���
        Window.SetActive(!Window.activeSelf);

        if (Window.activeSelf == false)
            return;

        //��û ���� ǥ�� �ʱ�ȭ
        int inputCount = _mission.inputPlayerCount;
        MakeSamplePool(ref m_inputCharIcons, SlotSample.gameObject, inputCount, m_inputGrid);
        List<CharactorData> haveCharList = MGGuild.Instance.GetHaveCharList();
    
        MakeSamplePool(ref m_waitCharIcons, SlotSample.gameObject, haveCharList.Count, m_haveGrid);
        InputOff(); //���� �뺴�� �� ������ 
        SetHaveChars(haveCharList);

        //��û ���� ������ �ʱ�ȭ
        m_inputList = new();
        m_waitList = new();
        for (int i = 0; i < haveCharList.Count; i++)
        {
            // ������ ���� 
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

        //���� ������ ������ �ٽ� ����
        RenewSlots();
    }

    public void GoMission()
    {
        if(m_inputList.Count>=1)
        MGGuild.Instance.GoMission(m_inputList);
    }

    private void TakeOffChar(CharactorData _charData)
    {
        //���Կ��� �����Ÿ�
        //���� ����Ʈ���� ����
        //��� ����Ʈ�� �߰� 
        m_inputList.Remove(_charData);
        m_waitList.Add(_charData);
    }

    private void InputChar(CharactorData _charData)
    {  
        //��⿡�� �����Ÿ�
        //���� ����Ʈ ���� Ȯ���ؼ� ���԰����ϸ� 
        //��� ����Ʈ���� ����
        //���� ����Ʈ�� �߰� 
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
