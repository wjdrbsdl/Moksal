using System.Collections;
using UnityEngine;


public class MGGuild : SigleTon<MGGuild>
{
    public static GuildData GuildData;

    // Use this for initialization
    void Start()
    {
        GuildData guildData = new GuildData();
        GuildData = guildData;
    }

    public void OnClickScout(CharactorData _scoutChar)
    {
        GuildData.Scout(_scoutChar);
    }
    public void OnClickFire(CharactorData _fireChar)
    {
        GuildData.Fire(_fireChar);
    }
}
