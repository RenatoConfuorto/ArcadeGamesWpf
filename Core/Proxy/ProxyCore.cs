using Core.Interfaces.DbBrowser;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Proxy
{
    public abstract class ProxyCore : IProxyBase
    {
        private string _connectionString;
        protected string ConnectionString
        {
            get => _connectionString;
            private set => _connectionString = value;
        }

        public ProxyCore(string connectionString)
        {
            try
            {
                ConnectionString = connectionString;
                OpenConnection();
            }
            catch (DbException dbEx)
            {

            }
            catch (Exception ex)
            {

            }
        }

        public abstract void OpenConnection();

        public abstract void CloseConnection();

        public abstract int GetNextIntValue(string TableName, string FieldName);

        #region Execute query
        public abstract string GetCreateTableIfExistsStatement();

        public abstract bool Execute(string Statement, IParametersBase parameters);

        public abstract bool Execute(string Statement);

        public abstract IDataReader GetDataReader(string Statement, IParametersBase parameters);

        public abstract IDataReader GetDataReader(string Statement);
        #endregion

        #region GetData
        public int ConvertToInt(object value, int defaultValue = 0)
        {
            if (value != null)
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
            if (value != null)
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
            if (value != null)
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
    }
}
