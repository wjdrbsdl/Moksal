using System.Collections;
using UnityEngine;


public class CharDialogManager : SigleTon<CharDialogManager>
{
    public CharDialog dialogSample;
    public Transform dialogBox;

    public CharDialog GetDialogObj()
    {
        return Instantiate(dialogSample, dialogBox);
    }
}
