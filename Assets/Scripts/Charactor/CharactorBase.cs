using System.Collections;
using UnityEngine;


public class CharactorBase : MonoBehaviour
{
    public int m_fieldNumber;
    public bool isMonster = true;

    private void OnDestroy()
    {
        if(isMonster == true)
        {
            BattleManager.Instance.ReportBattle(m_fieldNumber);
        }
    }

    public void SetFieldNumber(int _num)
    {
        m_fieldNumber = _num;
    }
}
