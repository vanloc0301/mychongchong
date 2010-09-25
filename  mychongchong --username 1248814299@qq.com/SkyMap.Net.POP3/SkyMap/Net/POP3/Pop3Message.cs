namespace SkyMap.Net.POP3
{
    using System;
    using System.Collections;
    using System.Net.Sockets;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;

    public class Pop3Message
    {
        private string m_body;
        private Socket m_client;
        private string m_contentType;
        private const int m_contentTypeState = 3;
        private const int m_endOfHeader = -98;
        private string m_from;
        private const int m_fromState = 0;
        private long m_inboxPosition = 0L;
        private bool m_isMultipart = false;
        private string[] m_lineTypeString = new string[] { "From", "To", "Subject", "Content-Type" };
        private ManualResetEvent m_manualEvent = new ManualResetEvent(false);
        private Pop3MessageComponents m_messageComponents;
        private long m_messageSize = 0L;
        private string m_multipartBoundary;
        private const int m_notKnownState = -99;
        private Pop3StateObject m_pop3State = null;
        private string m_subject;
        private const int m_subjectState = 2;
        private string m_to;
        private const int m_toState = 1;

        public Pop3Message(long position, long size, Socket client)
        {
            this.m_inboxPosition = position;
            this.m_messageSize = size;
            this.m_client = client;
            this.m_pop3State = new Pop3StateObject();
            this.m_pop3State.workSocket = this.m_client;
            this.m_pop3State.sb = new StringBuilder();
            this.LoadEmail();
            IEnumerator multipartEnumerator = this.MultipartEnumerator;
            while (multipartEnumerator.MoveNext())
            {
                Pop3Component current = (Pop3Component) multipartEnumerator.Current;
                if (current.IsBody)
                {
                    this.m_body = current.Data;
                    break;
                }
            }
        }

        private int GetHeaderLineType(string line)
        {
            for (int i = 0; i < this.m_lineTypeString.Length; i++)
            {
                string str = this.m_lineTypeString[i];
                if (Regex.Match(line, "^" + str + ":.*$").Success)
                {
                    return i;
                }
                if (line.Length == 0)
                {
                    return -98;
                }
            }
            return -99;
        }

        private void LoadEmail()
        {
            this.Send("retr " + this.m_inboxPosition);
            this.StartReceive();
            this.ParseEmail(this.m_pop3State.sb.ToString().Split(new char[] { '\r' }));
            this.m_pop3State = null;
        }

        private void ParseEmail(string[] lines)
        {
            long startOfBody = this.ParseHeader(lines);
            long length = lines.Length;
            this.m_messageComponents = new Pop3MessageComponents(lines, startOfBody, this.m_multipartBoundary, this.m_contentType);
        }

        private long ParseHeader(string[] lines)
        {
            int length = lines.Length;
            long num2 = 0L;
            for (int i = 0; i < length; i++)
            {
                string line = lines[i].Replace("\n", "");
                switch (this.GetHeaderLineType(line))
                {
                    case 0:
                        this.m_from = Pop3Parse.From(line);
                        goto Label_0123;

                    case 1:
                        this.m_to = Pop3Parse.To(line);
                        goto Label_0123;

                    case 2:
                        this.m_subject = Pop3Parse.Subject(line);
                        goto Label_0123;

                    case 3:
                        this.m_contentType = Pop3Parse.ContentType(line);
                        this.m_isMultipart = Pop3Parse.IsMultipart(this.m_contentType);
                        if (this.m_isMultipart)
                        {
                            if (!this.m_contentType.Substring(this.m_contentType.Length - 1, 1).Equals(";"))
                            {
                                break;
                            }
                            i++;
                            this.m_multipartBoundary = Pop3Parse.MultipartBoundary(lines[i].Replace("\n", ""));
                        }
                        goto Label_0123;

                    case -98:
                        num2 = i + 1;
                        goto Label_0123;

                    default:
                        goto Label_0123;
                }
                this.m_multipartBoundary = Pop3Parse.MultipartBoundary(this.m_contentType);
            Label_0123:
                if (num2 > 0L)
                {
                    return num2;
                }
            }
            return num2;
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                Pop3StateObject asyncState = (Pop3StateObject) ar.AsyncState;
                int count = asyncState.workSocket.EndReceive(ar);
                if (count > 0)
                {
                    asyncState.sb.Append(Encoding.ASCII.GetString(asyncState.buffer, 0, count));
                    this.StartReceiveAgain(asyncState.sb.ToString());
                }
            }
            catch (Exception exception)
            {
                this.m_manualEvent.Set();
                throw new Pop3ReceiveException("RecieveCallback error" + exception.ToString());
            }
        }

        private void Send(string data)
        {
            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes(data + "\r\n");
                this.m_client.Send(bytes);
            }
            catch (Exception exception)
            {
                throw new Pop3SendException(exception.ToString());
            }
        }

        private void StartReceive()
        {
            this.m_client.BeginReceive(this.m_pop3State.buffer, 0, 0x100, SocketFlags.None, new AsyncCallback(this.ReceiveCallback), this.m_pop3State);
            this.m_manualEvent.WaitOne();
        }

        private void StartReceiveAgain(string data)
        {
            if (!data.EndsWith("\r\n.\r\n"))
            {
                this.m_client.BeginReceive(this.m_pop3State.buffer, 0, 0x100, SocketFlags.None, new AsyncCallback(this.ReceiveCallback), this.m_pop3State);
            }
            else
            {
                this.m_manualEvent.Set();
            }
        }

        public override string ToString()
        {
            IEnumerator multipartEnumerator = this.MultipartEnumerator;
            string str = "From    : " + this.m_from + "\r\nTo      : " + this.m_to + "\r\nSubject : " + this.m_subject + "\r\n";
            while (multipartEnumerator.MoveNext())
            {
                str = str + ((Pop3Component) multipartEnumerator.Current).ToString() + "\r\n";
            }
            return str;
        }

        public string Body
        {
            get
            {
                return this.m_body;
            }
        }

        public string From
        {
            get
            {
                return this.m_from;
            }
        }

        public long InboxPosition
        {
            get
            {
                return this.m_inboxPosition;
            }
        }

        public bool IsMultipart
        {
            get
            {
                return this.m_isMultipart;
            }
        }

        public IEnumerator MultipartEnumerator
        {
            get
            {
                return this.m_messageComponents.ComponentEnumerator;
            }
        }

        public string Subject
        {
            get
            {
                return this.m_subject;
            }
        }

        public string To
        {
            get
            {
                return this.m_to;
            }
        }
    }
}

