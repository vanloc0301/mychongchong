namespace SkyMap.Net.Tools.Criteria
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Data.OleDb;

    public sealed class OleDbTypeToDbType
    {
        private static Hashtable _oleDbTypeToDbTypeMapping = new Hashtable();

        static OleDbTypeToDbType()
        {
            PopulateOleDbTypeToDbTypeMapping();
        }

        private OleDbTypeToDbType()
        {
        }

        internal static DbType Convert(int oleType)
        {
            OleDbType type = (OleDbType) oleType;
            object obj2 = _oleDbTypeToDbTypeMapping[type];
            if (obj2.ToString() == "UNSUPPORTED")
            {
                throw new ApplicationException("This data type is not currently supported by ADO.NET. - " + type.ToString());
            }
            return (DbType) obj2;
        }

        private static void PopulateOleDbTypeToDbTypeMapping()
        {
            _oleDbTypeToDbTypeMapping.Add(OleDbType.BigInt, DbType.Int64);
            _oleDbTypeToDbTypeMapping.Add(OleDbType.Binary, DbType.Binary);
            _oleDbTypeToDbTypeMapping.Add(OleDbType.Boolean, DbType.Boolean);
            _oleDbTypeToDbTypeMapping.Add(OleDbType.BSTR, DbType.String);
            _oleDbTypeToDbTypeMapping.Add(OleDbType.Char, DbType.String);
            _oleDbTypeToDbTypeMapping.Add(OleDbType.Currency, DbType.Currency);
            _oleDbTypeToDbTypeMapping.Add(OleDbType.Date, DbType.Date);
            _oleDbTypeToDbTypeMapping.Add(OleDbType.DBDate, DbType.Date);
            _oleDbTypeToDbTypeMapping.Add(OleDbType.DBTime, DbType.Time);
            _oleDbTypeToDbTypeMapping.Add(OleDbType.DBTimeStamp, DbType.DateTime);
            _oleDbTypeToDbTypeMapping.Add(OleDbType.Decimal, DbType.Decimal);
            _oleDbTypeToDbTypeMapping.Add(OleDbType.Double, DbType.Double);
            _oleDbTypeToDbTypeMapping.Add(OleDbType.Empty, DbType.Object);
            _oleDbTypeToDbTypeMapping.Add(OleDbType.Error, DbType.Object);
            _oleDbTypeToDbTypeMapping.Add(OleDbType.Filetime, DbType.DateTime);
            _oleDbTypeToDbTypeMapping.Add(OleDbType.Guid, DbType.Guid);
            _oleDbTypeToDbTypeMapping.Add(OleDbType.IDispatch, "UNSUPPORTED");
            _oleDbTypeToDbTypeMapping.Add(OleDbType.Integer, DbType.Int32);
            _oleDbTypeToDbTypeMapping.Add(OleDbType.IUnknown, "UNSUPPORTED");
            _oleDbTypeToDbTypeMapping.Add(OleDbType.LongVarBinary, DbType.Binary);
            _oleDbTypeToDbTypeMapping.Add(OleDbType.LongVarChar, DbType.String);
            _oleDbTypeToDbTypeMapping.Add(OleDbType.LongVarWChar, DbType.String);
            _oleDbTypeToDbTypeMapping.Add(OleDbType.Numeric, DbType.Decimal);
            _oleDbTypeToDbTypeMapping.Add(OleDbType.PropVariant, DbType.Object);
            _oleDbTypeToDbTypeMapping.Add(OleDbType.Single, DbType.Single);
            _oleDbTypeToDbTypeMapping.Add(OleDbType.SmallInt, DbType.Int16);
            _oleDbTypeToDbTypeMapping.Add(OleDbType.TinyInt, DbType.SByte);
            _oleDbTypeToDbTypeMapping.Add(OleDbType.UnsignedBigInt, DbType.UInt64);
            _oleDbTypeToDbTypeMapping.Add(OleDbType.UnsignedInt, DbType.UInt32);
            _oleDbTypeToDbTypeMapping.Add(OleDbType.UnsignedSmallInt, DbType.UInt16);
            _oleDbTypeToDbTypeMapping.Add(OleDbType.UnsignedTinyInt, DbType.Byte);
            _oleDbTypeToDbTypeMapping.Add(OleDbType.VarBinary, DbType.Binary);
            _oleDbTypeToDbTypeMapping.Add(OleDbType.VarChar, DbType.String);
            _oleDbTypeToDbTypeMapping.Add(OleDbType.Variant, DbType.Object);
            _oleDbTypeToDbTypeMapping.Add(OleDbType.VarNumeric, DbType.VarNumeric);
            _oleDbTypeToDbTypeMapping.Add(OleDbType.VarWChar, DbType.String);
            _oleDbTypeToDbTypeMapping.Add(OleDbType.WChar, DbType.StringFixedLength);
        }
    }
}

