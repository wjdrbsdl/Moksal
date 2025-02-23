using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIBase : MonoBehaviour
{
    protected void MakeSamplePool<T>(ref T[] _curArray, GameObject _sampleObj, int _workCount, Transform _box)
    {
        int needSlotCount = MakeCount<T>(_curArray, _workCount);
        if (needSlotCount > 0)
        {
            MakeSlots<T>(ref _curArray, needSlotCount, _sampleObj, _box);
        }

    }
    private int MakeCount<T>(T[] _curArray, int _goalCount)
    {
        if (_curArray == null || _curArray.Length == 0)
            return _goalCount;

        int makeCount = _goalCount - _curArray.Length; //현재 만들어진 카운터
        if (makeCount < 0)
            makeCount = 0;

        return makeCount;
    }

    //수량에 맞춰서 슬랏을 만들고 해당 슬랏 정보를 가져와야할때. 
    private void MakeSlots<T>(ref T[] _curArray, int _makeCount, GameObject _slotPrefeb, Transform _parent)
    {
        List<T> newT = new();
        if (_curArray != null)
            newT = new(_curArray); //기존 만들어진 배열이 있으면 리스트에 추가해야함. 

        for (int i = 0; i < _makeCount; i++)
        {
            newT.Add(Instantiate(_slotPrefeb, _parent).GetComponent<T>());

        }

        _curArray = newT.ToArray();
    }

    protected void OffRestSlot<T>(ref T[] _curArray, int _useCount) where T : MonoBehaviour
    {
        //사용하지 않는 슬롯을 비활성화 할때
        for (int i = _useCount; i < _curArray.Length; i++)
        {
            _curArray[i].gameObject.SetActive(false);
        }
    }
}
