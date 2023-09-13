using Core.Helpers;
using Core.Interfaces.DbBrowser;
using Core.Proxy;
using System.Data.SQLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using LIB.Sqlite.Entities;

namespace LIB.Sqlite.Base
{
    public abstract class ProxyBase : ProxyCore
    {
        private SQLiteConnection _connection;
        protected SQLiteConnection Connection
        {
            get => _connection;
            private set => _connection = value;
        }

        public ProxyBase(string connectionString)
            : base(connectionString)
        {
        }

        public override void OpenConnection()
        {
            Connection = new SQLiteConnection($"Data Source ={ConnectionString}");
            Connection.Open();
            CreateTables();
        }

        public override void CloseConnection()
        {
            if (Connection != null) Connection.Close();
        }

        public override int GetNextIntValue(string TableName, string FieldName)
        {
            int result = 0;

            string query = $"SELECT (MAX({FieldName}) + 1) AS MAX FROM {TableName}";

            try
            {
                using (SQLiteCommand command = new SQLiteCommand(query, Connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["MAX"] != DBNull.Value)
                            {
                                result = ConvertToInt(reader["MAX"]);
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
            catch(SQLiteException sqlEx)
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
            using (SQLiteCommand command = Connection.CreateCommand())
            {
                command.CommandText = GetCreateTableIfExistsStatement();
                command.ExecuteNonQuery();
            }
        }

        #region Execcute query

        public override bool Execute(string Statement, IParametersBase parameters)
        {
            try
            {
                SQLiteCommand command = Connection.CreateCommand();
                command.CommandText = Statement;
                if(parameters.Count() > 0)
                {
                    List<string> par = parameters.GetParametersKeys();
                    foreach (string key in par)
                    {
                        command.Parameters.AddWithValue(key, parameters[key]);
                    }
                }
                int result = command.ExecuteNonQuery();
                return result > 0;
            }
            catch(SQLiteException sqlEx)
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

        public override bool Execute(string Statement, string parameterName, object parameterValue)
        {
            SQLiteParameters parameters = new SQLiteParameters();
            parameters.Add(parameterName, parameterValue);
            return Execute(Statement, parameters);
        }

        public override bool Execute(string Statement)
        {
            try
            {
                SQLiteCommand command = Connection.CreateCommand();
                command.CommandText = Statement;
                int result = command.ExecuteNonQuery();
                return result > 0;
            }
            catch (SQLiteException sqlEx)
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

        public override IDataReader GetDataReader(string Statement, IParametersBase parameters)
        {
            try
            {
                SQLiteCommand command = Connection.CreateCommand();
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
            catch (SQLiteException sqlEx)
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

        public override IDataReader GetDataReader(string Statement, string parameterName, object parameterValue)
        {
            SQLiteParameters parameters = new SQLiteParameters();
            parameters.Add(parameterName, parameterValue);
            return GetDataReader(Statement, parameters);
        }

        public override IDataReader GetDataReader(string Statement) 
        {
            try
            {
                SQLiteCommand command = Connection.CreateCommand();
                command.CommandText = Statement;
                IDataReader reader = command.ExecuteReader();
                return reader;
            }
            catch (SQLiteException sqlEx)
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

        #endregion
    }
}
