namespace SkyMap.Net.POP3
{
    using System;
    using System.Net.Sockets;
    using System.Text;

    public class Pop3StateObject
    {
        public byte[] buffer = new byte[0x100];
        public const int BufferSize = 0x100;
        public StringBuilder sb = new StringBuilder();
        public Socket workSocket = null;
    }
}

