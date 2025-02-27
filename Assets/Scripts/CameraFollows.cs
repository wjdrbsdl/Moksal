using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollows : MonoBehaviour
{
    public static GameObject target;
    private Vector3 offset;

    public static void SetCamTarget(CharactorObj _obj)
    {
        target = _obj.gameObject;
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
