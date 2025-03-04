using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class WanderAction : CharActionBase
{
    bool isFowardGoal = false;
    Vector3 goal;

    public WanderAction()
    {
        m_actionDelayTime = 1f;
    }

    public override void Update(CharactorObj _charObj)
    {
        if (isFowardGoal == false)
        {
            //타겟 없는 상황에 정해둔 목적지도 없으면
            Vector3 curPos = _charObj.transform.position;
            float x = Random.Range(curPos.x - 5, curPos.x + 5);
            float y = Random.Range(curPos.y - 5, curPos.y + 5);
            //임의로 목적지 설정
            goal = new Vector3(x, y, 0);
            _charObj.RestrictPos(ref goal);
            isFowardGoal = true;
        }
        bool arrive = _charObj.Move(goal, 0.3f);
        if (arrive == true)
        {
            isFowardGoal = false;
            _charObj.ResetAction();
            _charObj.SetDelayTime(m_actionDelayTime);
        }
            
    }
}
