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
    private int m_viewCount;

    public void OnClickCharSlot(CharSlot _charSlot)
    {
        //���� ���Դٴ°� ��ȿ�� ������ ���� �ߴٴ� ��. 

        //������ ������ �������� �ڿ������� ���� ���� �ؾ���
        //1. �ε��� ã��
        int index = -1;
        for(int i = 0; i< charSlots.Length; i++)
        {
            if(_charSlot == charSlots[i])
            {
                index = i;
                break;
            }
        }
        //2. �ش� �ε��� ���� �ڿ����� ���� ��ܿ�
        for(int i = index; i < charSlots.Length; i++)
        {
            if(i+1 < charSlots.Length)
            {
                charSlots[i].SetInfo(charSlots[i+1].GetCharData());
            }
        }
        //3. �� ������ ������ ��
        charSlots[m_viewCount - 1].gameObject.SetActive(false);
        //4. ���̴� ���� ����
        m_viewCount--;
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

    public void OnClickRenewCrew()
    {
        SetCharInfo();
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
        m_viewCount = tempRecruitCount; //���� �̾��� ����ŭ �� Ŵ 
        OffRestSlot(ref charSlots, tempRecruitCount);

    }
}
