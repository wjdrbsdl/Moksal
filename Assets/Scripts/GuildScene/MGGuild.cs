﻿using System.Collections;
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

    public void OnClickScout(CharactorData _scoutChar)
    {
        GuildData.Scout(_scoutChar);
    }
    public void OnClickFire(CharactorData _fireChar)
    {
        GuildData.Fire(_fireChar);
    }
    public List<CharactorData> GetHaveCharList()
    {
        return GuildData.GetCharList();
    }
}
