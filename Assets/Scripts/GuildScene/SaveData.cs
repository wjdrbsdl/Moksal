using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class SaveData
{
    string path = Application.dataPath + "/와우.txt";

    public void Save()
    {
        List<CharactorData> list = MGGuild.Instance.GetHaveCharList();
        string haveCharJson = JsonUtility.ToJson(MGGuild.Instance.GetGuildData(), true);
        File.WriteAllText(path, haveCharJson);
        Debug.Log(haveCharJson);
    }
}
