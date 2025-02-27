using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharIcon : MonoBehaviour
{
    private CharactorObj m_charObj;

    public void OnClickIcon()
    {
        CameraFollows.SetCamTarget(m_charObj);
    }
}
