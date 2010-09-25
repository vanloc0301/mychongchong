namespace SkyMap.Net.Criteria.SQL
{
    using System;
    using System.Data;

    public class SQLHelper
    {
        public static void ExecuteNonQuery(IDbConnection dbcn, string commandText)
        {
            if (dbcn.State != ConnectionState.Open)
            {
                dbcn.Open();
            }
            IDbCommand command = dbcn.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = commandText;
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                dbcn.Close();
            }
        }

        private static IDataReader ExecuteReader(IDbConnection dbcn, string commandText)
        {
            IDataReader reader2;
            if (dbcn.State != ConnectionState.Open)
            {
                dbcn.Open();
            }
            IDbCommand command = dbcn.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = commandText;
            try
            {
                reader2 = command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return reader2;
        }

        private static DataTable Fill(IDataReader oDataReader)
        {
            DataTable table = new DataTable();
            DataTable schemaTable = oDataReader.GetSchemaTable();
            int num = 0;
            while (num < schemaTable.Rows.Count)
            {
                DataColumn column = table.Columns.Add((string) schemaTable.Rows[num]["ColumnName"]);
                if (schemaTable.Rows[num]["DataType"] is Type)
                {
                    column.DataType = schemaTable.Rows[num]["DataType"] as Type;
                }
                else
                {
                    column.DataType = Type.GetType((string) schemaTable.Rows[num]["DataType"]);
                }
                num++;
            }
            object[] objArray = new object[num];
            while (oDataReader.Read())
            {
                DataRow row = table.NewRow();
                for (int i = 0; i < num; i++)
                {
                    objArray[i] = oDataReader.GetValue(i);
                }
                row.ItemArray = objArray;
                table.Rows.Add(row);
            }
            return table;
        }

        public static void FillDataset(DataSet ds, IDbConnection dbcn, string commandText, string TableName)
        {
            DataTable table = null;
            IDataReader oDataReader = ExecuteReader(dbcn, commandText);
            try
            {
                table = Fill(oDataReader);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                oDataReader.Close();
            }
            if (ds == null)
            {
                ds = new DataSet();
            }
            table.TableName = TableName;
            ds.Tables.Add(table);
        }

        public static void FillDataset(DataSet ds, IDbConnection dbcn, string[] commandText, string[] TableName)
        {
            for (int i = 0; i < commandText.Length; i++)
            {
                FillDataset(ds, dbcn, commandText[i], TableName[i]);
            }
        }
    }
}

