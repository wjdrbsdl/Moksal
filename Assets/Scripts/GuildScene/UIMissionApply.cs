using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMissionApply : MonoBehaviour
{
    public GameObject Window;
    public IconMissionChar SlotSample;//���� ���� ����
    public Transform m_inputGrid;//���� �׸���
    public Transform m_haveGrid;//���� �׸���

    public IconMissionChar[] m_inputCharIcons; //���� ǥ���� ���Ե�
    public IconMissionChar[] m_haveCharIcons; //���� ǥ���� ���Ե�

    public void OpenApply(Mission _mission)
    {
        List<CharactorData> haveCharList = MGGuild.Instance.GetHaveCharList();
    }


    public void SelectChar(EMissionBtnType _btnType, CharactorData _charData)
    {

    }
}
