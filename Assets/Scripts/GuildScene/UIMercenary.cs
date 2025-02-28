using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMercenary : UIBase
{
    public GameObject Window;
    public CharSlot SlotSample;//���� ���� ����
    public Transform grid;//���� �׸���

    public CharSlot[] charSlots; //���� ǥ���� ���Ե�
    public int tempRecruitCount = 3;

    public void OnClickShowHaveChares()
    {
        Window.SetActive(!Window.activeSelf);

        if (Window.activeSelf == false)
            return;

        List<CharactorData> _haveMercenaryList = MGGuild.Instance.GetHaveCharList();
        //������ �뺴 �����͸� ������ �뺴 ����â ���� 
        //â�� ����
        tempRecruitCount = _haveMercenaryList.Count;
        //�ʿ��� ��ŭ ������ ����� �κ�
        MakeSamplePool(ref charSlots, SlotSample.gameObject, tempRecruitCount, grid);

        SetCharInfo(_haveMercenaryList);
    }

    private void SetCharInfo(List<CharactorData> _haveMercenaryList)
    {
        for (int i = 0; i < _haveMercenaryList.Count; i++)
        {
            charSlots[i].SetInfo(_haveMercenaryList[i]);
            charSlots[i].gameObject.SetActive(true);
        }
        OffRestSlot(ref charSlots, tempRecruitCount);

    }
}

