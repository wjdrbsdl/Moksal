using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRecruit : UIBase, ICharSlotClickCallBack
{
    public GameObject Window;
    public CharSlot SlotSample;//슬롯 만들 샘플
    public Transform grid;//만들 그리드

    public CharSlot[] charSlots; //정보 표기할 슬롯들
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

        //필요한 만큼 슬롯을 만드는 부분
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
            //지원한 캐릭터 데이터 생성 - 임의로 3개, 임의로 옵션 동일
            CharactorData charData = new CharactorData(true);
            charSlots[i].SetInfo(charData, this);
            charSlots[i].gameObject.SetActive(true);
        }
        OffRestSlot(ref charSlots, tempRecruitCount);

    }
}
