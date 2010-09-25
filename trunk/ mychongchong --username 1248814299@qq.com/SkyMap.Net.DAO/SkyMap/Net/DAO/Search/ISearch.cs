namespace SkyMap.Net.DAO.Search
{
    using System;

    public interface ISearch
    {
        void Clear();
        void SetInnerJoins(string joinForm, string joinWhere);
        string ToStatementString();

        string From { set; }

        string GroupBy { set; }

        string OrderBy { set; }

        string Select { set; }

        string Sql { set; }

        string Where { set; }
    }
}

