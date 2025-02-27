using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRecruit : UIBase, ICharSlotClickCallBack
{
    public GameObject Window;
    public CharSlot SlotSample;//���� ���� ����
    public Transform grid;//���� �׸���

    public CharSlot[] charSlots; //���� ǥ���� ���Ե�
    public int tempRecruitCount = 3;

    public void OnClickCharSlot(CharSlot _charSlot)
    {
        int index = -1;
        for(int i = 0; i< charSlots.Length; i++)
        {
            if(_charSlot == charSlots[i])
            {
                index = i;
                break;
            }
        }
        for(int i = index; i < charSlots.Length; i++)
        {
            charSlots[i] = null;
            if(i+1 < charSlots.Length)
            {
                charSlots[i] = charSlots[i + 1];
            }
        }
        RenewCharSlot();
    }


    public void OnClickRecruit()
    {
        Window.SetActive(!Window.activeSelf);

        if (Window.activeSelf == false)
            return;

        //�ʿ��� ��ŭ ������ ����� �κ�
        MakeSamplePool(ref charSlots, SlotSample.gameObject, tempRecruitCount, grid);

        SetCharInfo();
    }

    public void RenewCharSlot()
    {
        int infoCount = 0;
        for (int i = 0; i < charSlots.Length; i++)
        {
            if (charSlots[i] == null)
            {
                break;
            }
            charSlots[i].Renew();
            charSlots[i].gameObject.SetActive(true);
            infoCount++;
        }
        OffRestSlot(ref charSlots, infoCount);
    }

    private void SetCharInfo()
    {
        for (int i = 0; i < tempRecruitCount; i++)
        {
            //������ ĳ���� ������ ���� - ���Ƿ� 3��, ���Ƿ� �ɼ� ����
            CharactorData charData = new CharactorData(true);
            charSlots[i].SetInfo(charData, this);
            charSlots[i].gameObject.SetActive(true);
        }
        OffRestSlot(ref charSlots, tempRecruitCount);

    }
}
