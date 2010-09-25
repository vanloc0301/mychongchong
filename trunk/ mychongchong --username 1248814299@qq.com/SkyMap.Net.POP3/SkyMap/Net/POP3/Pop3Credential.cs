namespace SkyMap.Net.POP3
{
    using System;

    public class Pop3Credential
    {
        private string m_pass;
        private string[] m_sendStrings;
        private string m_server;
        private string m_user;

        public Pop3Credential()
        {
            this.m_sendStrings = new string[] { "user", "pass" };
            this.m_user = null;
            this.m_pass = null;
            this.m_server = null;
        }

        public Pop3Credential(string user, string pass, string server)
        {
            this.m_sendStrings = new string[] { "user", "pass" };
            this.m_user = user;
            this.m_pass = pass;
            this.m_server = server;
        }

        public string Pass
        {
            get
            {
                return this.m_pass;
            }
            set
            {
                this.m_pass = value;
            }
        }

        public string[] SendStrings
        {
            get
            {
                return this.m_sendStrings;
            }
        }

        public string Server
        {
            get
            {
                return this.m_server;
            }
            set
            {
                this.m_server = value;
            }
        }

        public string User
        {
            get
            {
                return this.m_user;
            }
            set
            {
                this.m_user = value;
            }
        }
    }
}

