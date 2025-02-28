
using UnityEngine;
using UnityEngine.UI;


public enum EMissionBtnType
{
    Input, Wait
}

public class IconMissionChar : MonoBehaviour
{
    public Image m_frame;
    public Image m_charIcon;
    public EMissionBtnType m_btnType = EMissionBtnType.Input;
    public UIMissionApply m_missionApply;
    public CharactorData m_charData;

    public void SetIcon(CharactorData _charData, EMissionBtnType _btnType)
    {
        m_charData = _charData;
       m_btnType = _btnType;
    }
    public void SetIcon(EMissionBtnType _btnType)
    {
        m_btnType = _btnType;
    }
    public void InputChar(CharactorData _charData)
    {
        m_charData = _charData;
    }


    public void OnClickIcon()
    {
        m_missionApply.SelectChar(m_btnType, m_charData);
    }
}

