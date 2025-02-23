using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//길드 데이터 
public class GuildData
{
    public List<CharactorData> m_haveCharList = new List<CharactorData>();
    public int m_maxCharSpace = 5;

    public bool Scout(CharactorData _char)
    {
        if (m_haveCharList.Count >= m_maxCharSpace)
        {
            //공간부족
            return false;
        }
        m_haveCharList.Add(_char);
        return true;
    }

    public void Fire(CharactorData _char)
    {
        int index = m_haveCharList.IndexOf(_char);
        if ( index < 0)
        {
            //없는 케릭이면 리턴
            return;
        }
        m_haveCharList.RemoveAt(index);
    }

    public List<CharactorData> GetCharList()
    {
        return m_haveCharList;
    }
}
