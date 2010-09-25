namespace SkyMap.Net.POP3
{
    using System;
    using System.Collections;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Text.RegularExpressions;

    public class Pop3Client
    {
        private Pop3Credential m_credential;
        private long m_directPosition = -1L;
        private long m_inboxPosition = 0L;
        private Pop3Message m_pop3Message = null;
        private const int m_pop3port = 110;
        private Socket m_socket = null;
        private const int MAX_BUFFER_READ_SIZE = 0x100;

        public Pop3Client(string user, string pass, string server)
        {
            this.m_credential = new Pop3Credential(user, pass, server);
        }

        public void CloseConnection()
        {
            this.Send("quit");
            this.m_socket = null;
            this.m_pop3Message = null;
        }

        public bool DeleteEmail()
        {
            bool flag = false;
            this.Send("dele " + this.m_inboxPosition);
            if (Regex.Match(this.GetPop3String(), @"^.*\+OK.*$").Success)
            {
                flag = true;
            }
            return flag;
        }

        private Socket GetClientSocket()
        {
            Socket socket = null;
            try
            {
                IPHostEntry entry = null;
                entry = Dns.Resolve(this.m_credential.Server);
                foreach (IPAddress address in entry.AddressList)
                {
                    IPEndPoint remoteEP = new IPEndPoint(address, 110);
                    Socket socket2 = new Socket(remoteEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    socket2.Connect(remoteEP);
                    if (socket2.Connected)
                    {
                        socket = socket2;
                        goto Label_008D;
                    }
                }
            }
            catch (Exception exception)
            {
                throw new Pop3ConnectException(exception.ToString());
            }
        Label_008D:
            if (socket == null)
            {
                throw new Pop3ConnectException("Error : connecting to " + this.m_credential.Server);
            }
            return socket;
        }

        public string GetMailUidl(int pos)
        {
            string str = null;
            this.Send("uidl " + pos.ToString());
            string str2 = this.GetPop3String();
            if (str2.Substring(0, 3) == "+OK")
            {
                str = str2.Split(new char[] { '\r' })[0].Split(new char[] { ' ' })[2];
            }
            return str;
        }

        private string GetPop3String()
        {
            if (this.m_socket == null)
            {
                throw new Pop3MessageException("Connection to POP3 server is closed");
            }
            byte[] buffer = new byte[0x100];
            string str = null;
            try
            {
                int count = this.m_socket.Receive(buffer, buffer.Length, SocketFlags.None);
                str = Encoding.ASCII.GetString(buffer, 0, count);
            }
            catch (Exception exception)
            {
                throw new Pop3ReceiveException(exception.ToString());
            }
            return str;
        }

        private void LoginToInbox()
        {
            this.Send("user " + this.m_credential.User);
            if (!this.GetPop3String().Substring(0, 3).Equals("+OK"))
            {
                throw new Pop3LoginException("login not excepted");
            }
            this.Send("pass " + this.m_credential.Pass);
            if (!this.GetPop3String().Substring(0, 3).Equals("+OK"))
            {
                throw new Pop3LoginException("login/password not accepted");
            }
        }

        public bool NextEmail()
        {
            long num;
            if (this.m_directPosition == -1L)
            {
                if (this.m_inboxPosition == 0L)
                {
                    num = 1L;
                }
                else
                {
                    num = this.m_inboxPosition + 1L;
                }
            }
            else
            {
                num = this.m_directPosition + 1L;
                this.m_directPosition = -1L;
            }
            this.Send("list " + num.ToString());
            string str = this.GetPop3String();
            if (str.Substring(0, 4).Equals("-ERR"))
            {
                return false;
            }
            this.m_inboxPosition = num;
            long size = long.Parse(str.Split(new char[] { '\r' })[0].Split(new char[] { ' ' })[2]);
            this.m_pop3Message = new Pop3Message(this.m_inboxPosition, size, this.m_socket);
            return true;
        }

        public bool NextEmail(long directPosition)
        {
            if (directPosition < 0L)
            {
                throw new Pop3MessageException("Position less than zero");
            }
            this.m_directPosition = directPosition;
            return this.NextEmail();
        }

        public void OpenInbox()
        {
            this.m_socket = this.GetClientSocket();
            if (!this.GetPop3String().Substring(0, 3).Equals("+OK"))
            {
                throw new Exception("Invalid initial POP3 response");
            }
            this.LoginToInbox();
        }

        private void Send(string data)
        {
            if (this.m_socket == null)
            {
                throw new Pop3MessageException("Pop3 connection is closed");
            }
            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes(data + "\r\n");
                this.m_socket.Send(bytes);
            }
            catch (Exception exception)
            {
                throw new Pop3SendException(exception.ToString());
            }
        }

        public string Body
        {
            get
            {
                return this.m_pop3Message.Body;
            }
        }

        public string From
        {
            get
            {
                return this.m_pop3Message.From;
            }
        }

        public bool IsMultipart
        {
            get
            {
                return this.m_pop3Message.IsMultipart;
            }
        }

        public long MessageCount
        {
            get
            {
                long num = 0L;
                if (this.m_socket == null)
                {
                    throw new Pop3MessageException("Pop3 server not connected");
                }
                this.Send("stat");
                string input = this.GetPop3String();
                if (Regex.Match(input, "^.*\\+OK[ |\t]+([0-9]+)[ |\t]+.*$").Success)
                {
                    num = long.Parse(Regex.Replace(input.Replace("\r\n", ""), "^.*\\+OK[ |\t]+([0-9]+)[ |\t]+.*$", "$1"));
                }
                return num;
            }
        }

        public IEnumerator MultipartEnumerator
        {
            get
            {
                return this.m_pop3Message.MultipartEnumerator;
            }
        }

        public string Subject
        {
            get
            {
                return this.m_pop3Message.Subject;
            }
        }

        public string To
        {
            get
            {
                return this.m_pop3Message.To;
            }
        }

        public Pop3Credential UserDetails
        {
            get
            {
                return this.m_credential;
            }
            set
            {
                this.m_credential = value;
            }
        }
    }
}

