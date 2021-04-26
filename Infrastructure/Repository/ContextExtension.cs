using System;
using System.Data;

namespace Infrastructure.Repository
{
    public static class ContextExtension
    {
        public static void AddParameter(this IDbCommand command, string parameterName, object value,
            DbType parameterType)
        {
            command.AddParameter(parameterName, value, parameterType, -1, ParameterDirection.Input);
        }

        public static void AddParameter(this IDbCommand command, string parameterName, object value,
            DbType parameterType, int size)
        {
            command.AddParameter(parameterName, value, parameterType, size, ParameterDirection.Input);
        }

        public static void AddParameter(this IDbCommand command, string parameterName, object value,
            DbType parameterType, int size, ParameterDirection direction)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.DbType = parameterType;
            parameter.Value = value;
            if (size > 0)
            {
                parameter.Size = size;
            }
            parameter.Direction = direction;

            command.Parameters.Add(parameter);
        }

        public static void CloseConnection(this IDbCommand command)
        {
            if (command.Transaction == null && command.Connection.State != ConnectionState.Closed)
            {
                command.Connection.Close();
            }
        }

        public static void OpenConnection(this IDbCommand command)
        {
            if (command.Connection.State == ConnectionState.Closed)
                command.Connection.Open();
        }

        public static IDataReader OpenReader(this IDbCommand command)
        {
            if (command.Transaction == null)
            {
                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            else
            {
                return command.ExecuteReader(CommandBehavior.Default);
            }
        }
        public static bool ColumnExists(this IDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}