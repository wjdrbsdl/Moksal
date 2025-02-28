
using UnityEngine;
using UnityEngine.UI;


public enum EMissionBtnType
{
    Input, Have
}

public class IconMissionChar : MonoBehaviour
{
    public Image m_frame;
    public Image m_charIcon;
    public EMissionBtnType m_iconType = EMissionBtnType.Input;
    public UIMissionApply m_missionApply;

    public void SetIcon(CharactorData _charData)
    {

    }

    public void OnClickIcon()
    {

    }
}

