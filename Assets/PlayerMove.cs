using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Transform[] targets;
    private Transform curTarget;
    private int targetIndex = 0;
    public float m_speed = 1f;

    // Update is called once per frame
    private void Start()
    {
        curTarget = targets[targetIndex];
        targetIndex ++;
    }

    void Update()
    {
        Move();
        TargetSelect();
    }

    private void Move()
    {
        
        //타겟 방향으로 속도만큼 이동
        Vector3 direct = curTarget.position - transform.position;
        
        transform.Translate(direct.normalized * Time.deltaTime * m_speed);
        return;
        float axisH = Input.GetAxisRaw("Horizontal");
        float axisV = Input.GetAxisRaw("Vertical");
        transform.Translate(new Vector3(axisH*Time.deltaTime * m_speed, 0, axisV * Time.deltaTime * m_speed));
    }

    private void TargetSelect()
    {
        if(Vector3.Distance(curTarget.position, transform.position) <= 0.2f)
        {
            if(targetIndex>= targets.Length)
            {
                targetIndex = 0;
            }
            curTarget = targets[targetIndex];
            targetIndex++;
        }
    }
}
