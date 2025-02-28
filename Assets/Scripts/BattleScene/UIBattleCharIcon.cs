using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBattleCharIcon : UIBase
{
    public GameObject Window;
    public BattleCharIcon SlotSample;//���� ���� ����
    public Transform grid;//������ �׸���

    public BattleCharIcon[] m_charSlots; //���� ǥ���� ���Ե�

    private List<CharactorObj> m_charList;

    public void SetCharIcons(List<CharactorObj> _playerList)
    {
        m_charList = _playerList;
        MakeSamplePool(ref m_charSlots, SlotSample.gameObject, _playerList.Count, grid);

        SetIconInfo();
    }

    private void SetIconInfo()
    {
        for (int i = 0; i < m_charList.Count; i++)
        {
            m_charSlots[i].gameObject.SetActive(true);
            m_charSlots[i].SetInfo(m_charList[i]);
        }
        OffRestSlot(ref m_charSlots, m_charList.Count);
    }
    
}
