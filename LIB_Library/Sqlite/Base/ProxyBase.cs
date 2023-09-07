using Core.Helpers;
using Core.Interfaces.DbBrowser;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Sqlite.Base
{
    public abstract class ProxyBase : IProxyBase
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
            catch (SqliteException sqlEx)
            {

            }
            catch (Exception ex)
            {

            }
        }

        public void OpenConnection()
        {
            Connection = new SqliteConnection(ConnectionString);
            Connection.Open();
            CreateTables();
        }

        public void CloseConnection()
        {
            if (Connection != null) Connection.Close();
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
            catch(SqliteException sqlEx)
            {
                MessageDialogHelper.ShowInfoMessage(sqlEx.Message);
            }
            catch (Exception ex)
            {
                MessageDialogHelper.ShowInfoMessage(ex.Message);
                //LogWriter.WriteLog($"{e.Message} \r\n {query}");
            }
            return result;
        }

        private void CreateTables()
        {
            using (SqliteCommand command = Connection.CreateCommand())
            {
                command.CommandText = GetCreateTableIfExistsStatement();
                command.ExecuteNonQuery();
            }
        }

        #region Execcute query
        public abstract string GetCreateTableIfExistsStatement();

        public bool Execute(string Statement, IParameterBase parameters)
        {
            try
            {
                SqliteCommand command = Connection.CreateCommand();
                command.CommandText = Statement;
                if(parameters.Count() > 0)
                {
                    List<string> par = parameters.GetParametersKeys();
                    foreach (string key in par)
                    {
                        command.Parameters.AddWithValue(key, parameters[key]);
                    }
                }
                command.ExecuteNonQuery();
                return true;
            }
            catch(SqliteException sqlEx)
            {
                MessageDialogHelper.ShowInfoMessage(sqlEx.Message);
                return false;
            }
            catch(Exception ex)
            {
                MessageDialogHelper.ShowInfoMessage(ex.Message);
                return false;
            }
        }

        public bool Execute(string Statement)
        {
            try
            {
                SqliteCommand command = Connection.CreateCommand();
                command.CommandText = Statement;
                command.ExecuteNonQuery();
                return true;
            }
            catch (SqliteException sqlEx)
            {
                MessageDialogHelper.ShowInfoMessage(sqlEx.Message);
                return false;
            }
            catch (Exception ex)
            {
                MessageDialogHelper.ShowInfoMessage(ex.Message);
                return false;
            }
        }

        public IDataReader GetDataReader(string Statement, IParameterBase parameters)
        {
            try
            {
                SqliteCommand command = Connection.CreateCommand();
                command.CommandText = Statement;
                if (parameters.Count() > 0)
                {
                    List<string> par = parameters.GetParametersKeys();
                    foreach (string key in par)
                    {
                        command.Parameters.AddWithValue(key, parameters[key]);
                    }
                }
                IDataReader reader = command.ExecuteReader();
                return reader;
            }
            catch (SqliteException sqlEx)
            {
                MessageDialogHelper.ShowInfoMessage(sqlEx.Message);
                return null;
            }
            catch (Exception ex)
            {
                MessageDialogHelper.ShowInfoMessage(ex.Message);
                return null;
            }
        }

        public IDataReader GetDataReader(string Statement) 
        {
            try
            {
                SqliteCommand command = Connection.CreateCommand();
                command.CommandText = Statement;
                IDataReader reader = command.ExecuteReader();
                return reader;
            }
            catch (SqliteException sqlEx)
            {
                MessageDialogHelper.ShowInfoMessage(sqlEx.Message);
                return null;
            }
            catch (Exception ex)
            {
                MessageDialogHelper.ShowInfoMessage(ex.Message);
                return null;
            }
        }

        #region GetData
        public int ConvertToInt(object value, int defaultValue = 0)
        {
            if(value != null)
            {
                return Convert.ToInt32(value);
            }
            else
            {
                return defaultValue;
            }
        }

        public int? ConvertToNullableInt(object value)
        {
            if(value != null)
            {
                return Convert.ToInt32(value);
            }
            else
            {
                return null;
            }
        }

        public long ConvertToLong(object value, long defaultValue = 0)
        {
            if(value != null)
            {
                return Convert.ToInt64(value);
            }
            else
            {
                return defaultValue;
            }
        }

        public long? ConvertToNullableLong(object value)
        {
            if (value != null)
            {
                return Convert.ToInt64(value);
            }
            else
            {
                return null;
            }
        }

        public float ConvertToFloat(object value, float defaultValue = 0)
        {
            if (value != null)
            {
                return Convert.ToSingle(value);
            }
            else
            {
                return defaultValue;
            }
        }

        public float? ConvertToNullableFloat(object value)
        {
            if (value != null)
            {
                return Convert.ToSingle(value);
            }
            else
            {
                return null;
            }
        }

        public double ConvertToDouble(object value, double defaultValue = 0)
        {
            if (value != null)
            {
                return Convert.ToDouble(value);
            }
            else
            {
                return defaultValue;
            }
        }

        public double? ConvertToNullableDouble(object value)
        {
            if (value != null)
            {
                return Convert.ToDouble(value);
            }
            else
            {
                return null;
            }
        }

        public string ConvertToString(object value)
        {
            if (value != null)
            {
                return Convert.ToString(value);
            }
            else
            {
                return null;
            }
        }

        public bool ConvertToBoolean(object value)
        {
            return Convert.ToBoolean(value);
        }

        public DateTime ConvertToDateTime(object value)
        {
            if (value != null)
            {
                return Convert.ToDateTime(value);
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        public DateTime? ConvertToNullableDateTime(object value)
        {
            if (value != null)
            {
                return Convert.ToDateTime(value);
            }
            else
            {
                return null;
            }
        }

        public byte[] ConvertToBytes(object value)
        {
            if (value != null)
            {
                return value as byte[];
            }
            else
            {
                return null;
            }
        } 
        #endregion

        #endregion
    }
}
