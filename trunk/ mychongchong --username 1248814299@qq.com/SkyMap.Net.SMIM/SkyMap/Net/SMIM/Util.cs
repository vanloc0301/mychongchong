namespace SkyMap.Net.SMIM
{
    using SkyMap.Net.XMPP.protocol.client;
    using System;
    using System.Collections;

    public class Util
    {
        public static Hashtable ChatForms = new Hashtable();
        public static Hashtable GroupChatForms = new Hashtable();

        public static int GetRosterImageIndex(Presence pres)
        {
            if ((pres.get_Type() != 4) && (pres.get_Type() != 6))
            {
                switch (pres.get_Show())
                {
                    case -1:
                        return 1;

                    case 0:
                        return 2;

                    case 1:
                        return 4;

                    case 2:
                        return 5;

                    case 3:
                        return 3;
                }
            }
            return 0;
        }
    }
}

