using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MGGuild : SigleTon<MGGuild>
{
    private static GuildData GuildData;

    // Use this for initialization
    void Start()
    {
        if(GuildData == null)
        {
            Debug.Log("최초 길드 데이터 생성");
            GuildData guildData = new GuildData();
            GuildData = guildData;
        }
        else
        {
            Debug.Log("길드데이터 존재");
        }
    }

    public bool OnClickScout(CharactorData _scoutChar)
    {
        return GuildData.Scout(_scoutChar);
    }
    public void OnClickFire(CharactorData _fireChar)
    {
        GuildData.Fire(_fireChar);
    }

    public UIMissionApply m_missionApplyUI;
    public MissionMaker m_missionMaker;
    public void ApplyMission()
    {
        m_missionApplyUI.OpenApply(m_missionMaker.GetNewMission());
    }

    public List<CharactorData> GetHaveCharList()
    {
        return GuildData.GetCharList();
    }
}
