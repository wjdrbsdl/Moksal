using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharIcon : MonoBehaviour
{
    private CharactorObj m_charObj;

    public void SetInfo(CharactorObj _charObj)
    {
        m_charObj = _charObj;
    }

    public void OnClickIcon()
    {
        CameraFollows.SetCamTarget(m_charObj);
    }
}
