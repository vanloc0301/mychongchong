namespace SkyMap.Net.DataForms
{
    using System;

    public class clsUserInfo
    {
        public string m_strPeopleIDCode;
        public string m_strPeopleName;

        public clsUserInfo(string p_strPeopleName, string p_strPeopleIDCode)
        {
            this.m_strPeopleName = p_strPeopleName;
            this.m_strPeopleIDCode = p_strPeopleIDCode;
        }
    }
}

