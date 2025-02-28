using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMercenary : UIBase
{
    public GameObject Window;
    public CharSlot SlotSample;//슬롯 만들 샘플
    public Transform grid;//만들 그리드

    public CharSlot[] charSlots; //정보 표기할 슬롯들
    public int tempRecruitCount = 3;

    public void OnClickShowHaveChares()
    {
        Window.SetActive(!Window.activeSelf);

        if (Window.activeSelf == false)
            return;

        List<CharactorData> _haveMercenaryList = MGGuild.Instance.GetHaveCharList();
        //보유한 용병 데이터를 가지고 용병 정보창 생성 
        //창을 연다
        tempRecruitCount = _haveMercenaryList.Count;
        //필요한 만큼 슬롯을 만드는 부분
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

