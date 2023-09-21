using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.DbBrowser
{
    public interface IProxyBase
    {
        void OpenConnection();
        void CloseConnection();
        int GetNextIntValue(string TableName, string FieldName);

        bool Execute(string Statement, IParametersBase parameters);
        bool Execute(string Statement, string parameterName, object parameterValue);
        bool Execute(string Statement);
        IDataReader GetDataReader(string Statement, IParametersBase parameters);
        IDataReader GetDataReader(string statement, string parameterName, object parameterValue);
        IDataReader GetDataReader(string Statement);

        int ConvertToInt(object value, int defaultValue = 0);
        int? ConvertToNullableInt(object value);
        long ConvertToLong(object value, long defaultValue = 0);
        long? ConvertToNullableLong(object value);
        float ConvertToFloat(object value, float defaultValue = 0f);
        float? ConvertToNullableFloat(object value);
        double ConvertToDouble(object value, double defaultValue = 0.0);
        double? ConvertToNullableDouble(object value);
        string ConvertToString(object value);
        bool ConvertToBoolean(object value);
        DateTime ConvertToDateTime(object value);
        DateTime? ConvertToNullableDateTime(object value);
        byte[] ConvertToBytes(object value);

    }
}
