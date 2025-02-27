using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRecruit : UIBase
{
    public GameObject Window;
    public CharSlot SlotSample;//���� ���� ����
    public Transform grid;//���� �׸���

    public CharSlot[] charSlots; //���� ǥ���� ���Ե�
    public int tempRecruitCount = 3;
    public void OnClickRecruit()
    {
        Window.SetActive(!Window.activeSelf);

        if (Window.activeSelf == false)
            return;

        //�ʿ��� ��ŭ ������ ����� �κ�
        MakeSamplePool(ref charSlots, SlotSample.gameObject, tempRecruitCount, grid);

        SetCharInfo();
    }

    private void SetCharInfo()
    {
        for (int i = 0; i < tempRecruitCount; i++)
        {
            //������ ĳ���� ������ ���� - ���Ƿ� 3��, ���Ƿ� �ɼ� ����
            CharactorData charData = new CharactorData(true);
            charSlots[i].SetInfo(charData);
            charSlots[i].gameObject.SetActive(true);
        }
        OffRestSlot(ref charSlots, tempRecruitCount);

    }
}
