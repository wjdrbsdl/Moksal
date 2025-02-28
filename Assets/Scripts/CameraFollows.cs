using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollows : MonoBehaviour
{
    public static GameObject target;
    public static bool followingChar;
    private Vector3 offset;

    public static void SetCamTarget(CharactorObj _obj)
    {
        followingChar = true;
        target = _obj.gameObject;
    }

    public static void SetFieldTarget(BattleFieldData _field)
    {
        followingChar = false;
        target = _field.m_fieldObj;
    }

    private void Start()
    {
        offset = transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = target.transform.position + offset;
    }
}
