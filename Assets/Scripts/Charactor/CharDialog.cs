using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharDialog : MonoBehaviour
{
    public Text dialogText;
    public CharactorObj targetCharObj;
    private float remainTime;

    public void ShowDialog(CharactorObj _char, string _text)
    {
        gameObject.SetActive(true);
        targetCharObj = _char;
        dialogText.text = _text;
        remainTime = 2f; //유지시간
    }

    private void Update()
    {
        Timer();
        FollowTarget();
    }

    private void Timer()
    {
        remainTime -= Time.deltaTime;
        if (remainTime <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void FollowTarget()
    {
        //캐릭 오브젝트 월드 좌표를 카메라 캔버스 스크린 좌표로 전환해서 해당 텍스트 제어
        transform.position = Camera.main.WorldToScreenPoint(targetCharObj.transform.position);
    }
}
