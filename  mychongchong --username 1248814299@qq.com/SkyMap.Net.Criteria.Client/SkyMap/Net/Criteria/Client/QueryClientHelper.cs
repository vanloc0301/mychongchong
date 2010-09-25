namespace SkyMap.Net.Criteria.Client
{
    using SkyMap.Net.DAO;
    using SkyMap.Net.Security;
    using SkyMap.Net.Security.Principal;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;

    public static class QueryClientHelper
    {
        public static IList<string> GetAuthQuerys()
        {
            return SecurityUtil.GetCurrentPrincipalAuthResourcesByType<string>("TY_QUERY_NEW");
        }

        public static string GetWhereByConditions(DataTable dtcondition)
        {
            string str2;
            try
            {
                StringBuilder builder = new StringBuilder(0x800);
                bool flag = true;
                foreach (DataRow row in dtcondition.Rows)
                {
                    if (!flag)
                    {
                        builder.Append(CriteriaBusiness.GetRealRel(row["Relation"].ToString()));
                    }
                    else
                    {
                        flag = false;
                    }
                    builder.Append(row["TABLE_NAME"] + "." + row["Field_ID"]);
                    string realOp = CriteriaBusiness.GetRealOp(row["Operation"].ToString());
                    builder.Append(realOp);
                    builder.Append(row["Compare_Value"]);
                    if (realOp.IndexOf("%") > 0)
                    {
                        builder.Append("%");
                    }
                    builder.Append("'");
                    builder.Append(Environment.NewLine);
                }
                if (builder.Length > 0)
                {
                    builder.Insert(0, " 1=1 and ( ");
                    builder.Append(")");
                }
                else
                {
                    builder.Append(" 1=1 ");
                }
                str2 = builder.ToString();
            }
            catch (ApplicationException exception)
            {
                throw new ApplicationException("构造查询WHERE语句时发生错误:" + Environment.NewLine + exception.Message, exception);
            }
            return str2;
        }

        public static bool IsCanAccessAll
        {
            get
            {
                SmIdentity smIdentity = SecurityUtil.GetSmIdentity();
                return ((smIdentity.AdminLevel == AdminLevelType.Admin) || (smIdentity.AdminLevel == AdminLevelType.AdminData));
            }
        }

        public static IList<TyQuery> TyQuerys
        {
            get
            {
                return QueryHelper.List<TyQuery>("ALL_TyQuery");
            }
        }


    }
}

