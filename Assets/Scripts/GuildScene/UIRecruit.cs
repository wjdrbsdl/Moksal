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
    private int m_viewCount;

    public void OnClickCharSlot(CharSlot _charSlot)
    {
        //여길 들어왔다는건 유효한 슬롯을 영입 했다는 것. 

        //선택한 슬롯의 아이템을 뒤에서부터 당기는 일을 해야함
        //1. 인덱스 찾고
        int index = -1;
        for(int i = 0; i< charSlots.Length; i++)
        {
            if(_charSlot == charSlots[i])
            {
                index = i;
                break;
            }
        }
        //2. 해당 인덱스 부터 뒤에껄로 부터 당겨옴
        for(int i = index; i < charSlots.Length; i++)
        {
            if(i+1 < charSlots.Length)
            {
                charSlots[i].SetInfo(charSlots[i+1].GetCharData());
            }
        }
        //3. 맨 마지막 슬롯은 끔
        charSlots[m_viewCount - 1].gameObject.SetActive(false);
        //4. 보이는 숫자 갱신
        m_viewCount--;
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

    public void OnClickRenewCrew()
    {
        SetCharInfo();
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
        m_viewCount = tempRecruitCount; //새로 뽑아준 수만큼 뷰 킴 
        OffRestSlot(ref charSlots, tempRecruitCount);

    }
}
