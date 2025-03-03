using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class CharActionBase
{
    protected float m_actionDelayTime = 1f; //해당 공격이후 지체시간
    protected float m_actionCoolTime = 1f; //공격 주기
    protected float m_culCoolTime = 1f;  //현재 지난 주기
    protected DateTime m_preTime; //이전 행동 설정했던 시간

    public void CalDeltaTime()
    {
        //해당 행동이 시작되면, 이전 쿨타임을 계산
        TimeSpan span = DateTime.Now - m_preTime;
        m_culCoolTime -= (float)span.TotalMilliseconds;
    }

    public abstract void Update(CharactorObj _charObj);
}
