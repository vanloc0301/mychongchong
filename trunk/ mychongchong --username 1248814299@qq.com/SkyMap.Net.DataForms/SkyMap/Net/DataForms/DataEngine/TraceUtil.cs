namespace SkyMap.Net.DataForms.DataEngine
{
    using Nini.Config;
    using SkyMap.Net.DataForms;
    using System;

    public class TraceUtil
    {
        private static string _NotNeedTraceFields = string.Empty;
        private static string fld_deleted = "DELETED_DATA";
        private static string fld_field = "FIELD_NAME";
        private static string fld_key = "KEY_VALUE";
        private static string fld_memo = "MEMO";
        private static string fld_new = "NEW_VALUE";
        private static string fld_old = "OLD_VALUE";
        private static string fld_staffid = "STAFF_ID";
        private static string fld_staffname = "STAFF_NAME";
        private static string fld_table = "TABLE_NAME";
        private static string fld_timestamp = "OP_DATA";
        private static string fld_type = "TRACE_TYPE";
        public static bool IsNeedSignedDataSet = false;
        private static string recDelTableName;
        public static bool ShowTraceHistory = false;
        private static TraceLevel traceLevel = TraceLevel.None;
        private static string traceTableName;
        private static string traceType = string.Empty;

        static TraceUtil()
        {
            IConfig traceConfig = SupportClass.GetTraceConfig();
            if ((traceConfig != null) && (traceConfig.GetKeys().Length != 0))
            {
                IsNeedSignedDataSet = bool.Parse(traceConfig.GetString("IsNeedSignedDataSet"));
                traceType = traceConfig.GetString("TraceType", string.Empty);
                string[] strArray = traceConfig.GetString("TraceLevel").Split(new char[] { '|' });
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (i == 0)
                    {
                        traceLevel = (TraceLevel) Enum.Parse(typeof(TraceLevel), strArray[i], false);
                    }
                    else
                    {
                        traceLevel |= (TraceLevel) Enum.Parse(typeof(TraceLevel), strArray[i], false);
                    }
                }
                if ((traceLevel & TraceLevel.None) != TraceLevel.None)
                {
                    ShowTraceHistory = bool.Parse(traceConfig.GetString("ShowTraceHistory"));
                    traceTableName = traceConfig.GetString("TraceTableName");
                    recDelTableName = traceConfig.GetString("RecDelTableName");
                    _NotNeedTraceFields = traceConfig.GetString("NotNeedTraceFields");
                    fld_deleted = traceConfig.GetString("FLD_DELETED");
                    fld_field = traceConfig.GetString("FLD_FIELD");
                    fld_key = traceConfig.GetString("FLD_KEY");
                    fld_memo = traceConfig.GetString("FLD_MEMO");
                    fld_new = traceConfig.GetString("FLD_NEW");
                    fld_old = traceConfig.GetString("FLD_OLD");
                    fld_staffid = traceConfig.GetString("FLD_STAFFID");
                    fld_staffname = traceConfig.GetString("FLD_STAFFNAME");
                    fld_table = traceConfig.GetString("FLD_TABLE");
                    fld_timestamp = traceConfig.GetString("FLD_TIMESTAMP");
                    fld_type = traceConfig.GetString("FLD_TYPE");
                }
            }
        }

        public static bool CanTraceLevel(TraceLevel tl)
        {
            return ((traceLevel & tl) == tl);
        }

        public static string FLD_DELETED
        {
            get
            {
                return fld_deleted;
            }
        }

        public static string FLD_FIELD
        {
            get
            {
                return fld_field;
            }
        }

        public static string FLD_KEY
        {
            get
            {
                return fld_key;
            }
        }

        public static string FLD_MEMO
        {
            get
            {
                return fld_memo;
            }
        }

        public static string FLD_NEW
        {
            get
            {
                return fld_new;
            }
        }

        public static string FLD_OLD
        {
            get
            {
                return fld_old;
            }
        }

        public static string FLD_STAFFFID
        {
            get
            {
                return fld_staffid;
            }
        }

        public static string FLD_STAFFNAME
        {
            get
            {
                return fld_staffname;
            }
        }

        public static string FLD_TABLE
        {
            get
            {
                return fld_table;
            }
        }

        public static string FLD_TIMESTAMP
        {
            get
            {
                return fld_timestamp;
            }
        }

        public static string FLD_TYPE
        {
            get
            {
                return fld_type;
            }
        }

        public static string NotNeedTraceFields
        {
            get
            {
                return _NotNeedTraceFields;
            }
        }

        public static string RecDelTableName
        {
            get
            {
                return recDelTableName;
            }
        }

        public static string TraceTableName
        {
            get
            {
                return traceTableName;
            }
        }

        public static string TraceType
        {
            get
            {
                return traceType;
            }
        }
    }
}

