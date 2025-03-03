using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SigleTon<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
               // Debug.Log("인스턴스가 null" + typeof(T).Name);
                instance = FindObjectOfType<T>();
            }
            if(instance == null)
            {
                instance = Instantiate(new GameObject()).AddComponent<T>();
            }
      
            return instance;
        }
    }

    protected void Awake()
    {
        //Debug.Log("싱글톤 생성시작" + typeof(T).Name);
        if (instance == null)
        {
           // Debug.Log("싱글톤 새로운거 ");
            instance = gameObject.GetComponent<T>();
        }
    }

}