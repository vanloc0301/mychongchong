namespace SkyMap.Net.Tools.Organize
{
    using System;
    using System.Collections.Generic;

    public class ParticipantTypeList
    {
        public static List<ParticipantType> Get()
        {
            List<ParticipantType> list = new List<ParticipantType>(4);
            list.Add(new ParticipantType("DEPT", "部门"));
            list.Add(new ParticipantType("ROLE", "角色"));
            list.Add(new ParticipantType("STAFF", "人员"));
            list.Add(new ParticipantType("ALL", "所有人员"));
            return list;
        }
    }
}

