using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace IQMediaGroup.Common.IOS.Util
{
    public static class SqlExtensions
    {
        public static SqlCommand GetCommand(this SqlConnection connection, string commandText, CommandType commandType)
        {
            var command = connection.CreateCommand();
            command.CommandTimeout = connection.ConnectionTimeout;
            command.CommandType = commandType;
            command.CommandText = commandText;
            return command;
        }

        public static void AddParameter(this SqlCommand command, string parameterName, object parameterValue)
        {
            if (!parameterName.StartsWith("@"))
                parameterName = "@" + parameterName;
            command.Parameters.AddWithValue(parameterName, parameterValue);
        }
    }
}
