using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Core.Sqlite
{
    public abstract class ProxyBase
    {
        private string _connectionString;
        private SqliteConnection _connection;
        protected string ConnectionString
        {
            get => _connectionString;
            private set => _connectionString = value;
        }
        protected SqliteConnection Connection
        {
            get => _connection;
            private set => _connection = value;
        }

        public ProxyBase(string connectionString)
        {
            try
            {
                ConnectionString = connectionString;
                OpenConnection();
            }
            catch(SqliteException sqlEx)
            {

            }
            catch(Exception ex)
            {

            }
        }

        public void OpenConnection()
        {
            Connection = new SqliteConnection(ConnectionString);
            CreateTables();
            Connection.Open();
        }

        public void CloseConnection()
        {
            if(Connection != null) Connection.Close();
        }

        public int GetNextIntValue(string TableName, string FieldName)
        {
            int result = 0;

            string query = $"SELECT (MAX({FieldName}) + 1) AS MAX FROM {TableName}";

            try
            {
                using (SqliteCommand command = new SqliteCommand(query, Connection))
                {
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["MAX"] != DBNull.Value)
                            {
                                result = Convert.ToInt32(reader["MAX"]);
                            }
                            else
                            {
                                result = 1;
                            }
                        }
                    }
                }
                if (result == 0)
                {
                    //LogWriter.WriteLog($"Impossibile trovare un valore per il campo({FieldName}) nella tabella({TableName})");
                }
            }
            catch (Exception e)
            {
                //LogWriter.WriteLog($"{e.Message} \r\n {query}");
            }
            return result;
        }

        public abstract void CreateTables();
    }
}
