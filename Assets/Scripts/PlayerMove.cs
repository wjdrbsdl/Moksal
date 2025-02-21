using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    public Transform curTarget;
    public float m_speed = 3f;

    public void SetTarget(Vector3 _pos)
    {
        Transform newTarget = new GameObject().transform;
        newTarget.position = _pos;
        curTarget = newTarget;
    } 

    void Update()
    {
        Move();
    }

    private void Move()
    {

        if (curTarget == null)
            return;

        //타겟 방향으로 속도만큼 이동
        Vector3 direct = curTarget.position - transform.position;
      //  direct.y = 0;
       // Debug.Log(direct.normalized);
        transform.Translate(direct.normalized * Time.deltaTime * m_speed);
        return;

        
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }
}
