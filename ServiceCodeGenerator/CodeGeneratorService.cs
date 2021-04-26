using Infrastructure.Repository;
using Microsoft.CSharp;
using ModelCodeGenerator;
using ServiceHelper;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ServiceCodeGenerator
{
    public class CodeGeneratorService : BaseService, ICodeGeneratorService
    {
        readonly IDataContext dbContext;
        public CodeGeneratorService(IDataContext context)
        {
            dbContext = context;
        }

        public bool SpExists(string SPName)
        {
            bool exists = false;
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;

            command.CommandText = "sp_codegen_ProcedureExists";
            command.AddParameter("@procedureName", SPName, DbType.String);

            command.OpenConnection();
            var reader = command.OpenReader();
            if (reader.Read())
            {

                if (!reader.IsDBNull(reader.GetOrdinal("cnt")))
                    if (reader.GetInt32(reader.GetOrdinal("cnt")) > 0)
                        exists = true;
            }

            reader.Close();
            return exists;
        }
        public List<ProcedureParameter> GetSpParameters(string SPName)
        {
            List<ProcedureParameter> parameters = new List<ProcedureParameter>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;

            command.CommandText = "sp_codegen_GetProcedureParams";
            command.AddParameter("@procedureName", SPName, DbType.String);

            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                ProcedureParameter parameter = new ProcedureParameter();
                if (!reader.IsDBNull(reader.GetOrdinal("name")))
                    parameter.Name = reader.GetString(reader.GetOrdinal("name"));
                if (!reader.IsDBNull(reader.GetOrdinal("datatype")))
                    parameter.DataType = reader.GetString(reader.GetOrdinal("datatype"));
                if (!reader.IsDBNull(reader.GetOrdinal("default_value")))
                    parameter.Value = reader.GetValue(reader.GetOrdinal("default_value"));
                parameters.Add(parameter);
            }

            reader.Close();
            return parameters;
        }
        public List<OutputColumn> GetOutPutColumns(string SPName, List<ProcedureParameter> procedureParameters)
        {
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = SPName;

            foreach (ProcedureParameter parameter in procedureParameters)
            {
                if (parameter.Value == null)
                    continue;

                DbType dbType = GetDbType(parameter.DataType);
                Type type = GetType(dbType);
                if (type == typeof(bool))
                {
                    if (parameter.Value.ToString() == "1")
                        parameter.Value = true;
                    else if (parameter.Value.ToString() == "0")
                        parameter.Value = false;
                }
                if (type == typeof(Guid))
                {
                    parameter.Value = Guid.Parse(parameter.Value.ToString());
                }
                if (type == typeof(string))
                {
                    parameter.Value = parameter.Value.ToString().Trim();
                }
                command.AddParameter(parameter.Name, Convert.ChangeType(parameter.Value, GetType(dbType)), dbType);
            }
            command.OpenConnection();
            var reader = command.OpenReader();
            List<OutputColumn> Columns = new List<OutputColumn>();
            if (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    OutputColumn column = new OutputColumn();
                    column.HeaderName = reader.GetName(i);
                    column.ColumnDBType = reader.GetDataTypeName(i);
                    column.ColumnType = reader.GetFieldType(i);

                    using (var provider = new CSharpCodeProvider())
                    {
                        var typeRef = new CodeTypeReference(column.ColumnType);
                        column.Type = provider.GetTypeOutput(typeRef);
                    }
                    if (column.ColumnType == typeof(DateTime))
                        column.Type = column.ColumnType.Name;
                    if (column.ColumnType == typeof(Guid))
                        column.Type = column.ColumnType.Name;
                    Columns.Add(column);
                }
            }
            reader.Close();
            return Columns;
        }
        public DbType GetDbType(string type)
        {
            SqlDbType SQLDBType = (SqlDbType)Enum.Parse(typeof(SqlDbType), type, true);
            switch (SQLDBType)
            {
                case SqlDbType.VarChar: return DbType.String;
                case SqlDbType.VarBinary: return DbType.Binary;
                case SqlDbType.TinyInt: return DbType.Byte;
                case SqlDbType.Bit: return DbType.Boolean;
                case SqlDbType.Money: return DbType.Currency;
                case SqlDbType.DateTime: return DbType.DateTime;
                case SqlDbType.Date: return DbType.Date;
                case SqlDbType.Decimal: return DbType.Decimal;
                case SqlDbType.Float: return DbType.Double;
                case SqlDbType.UniqueIdentifier: return DbType.Guid;
                case SqlDbType.SmallInt: return DbType.Int16;
                case SqlDbType.Int: return DbType.Int32;
                case SqlDbType.BigInt: return DbType.Int64;
                case SqlDbType.Variant: return DbType.Object;
                case SqlDbType.Real: return DbType.Single;
                case SqlDbType.NVarChar: return DbType.String;
                case SqlDbType.Char: return DbType.AnsiStringFixedLength;
                case SqlDbType.NChar: return DbType.StringFixedLength;
                default:
                    return DbType.String;
            }
        }
        public Type GetType(DbType type)
        {
            switch (type)
            {

                case DbType.AnsiString: return Type.GetType("System.String");
                case DbType.Binary: return Type.GetType("System.String");
                case DbType.Byte: return typeof(Byte);
                case DbType.Boolean: return Type.GetType("System.Boolean");
                case DbType.Currency: return Type.GetType("System.Decimal");
                case DbType.DateTime: return Type.GetType("System.DateTime");
                case DbType.Decimal: return Type.GetType("System.Decimal");
                case DbType.Double: return Type.GetType("System.Double");
                case DbType.Guid: return typeof(Guid);
                case DbType.Int16: return Type.GetType("System.Int16");
                case DbType.Int32: return Type.GetType("System.Int32");
                case DbType.Int64: return Type.GetType("System.Int64");
                case DbType.Object: return Type.GetType("System.Object");
                case DbType.Single: return Type.GetType("System.String");
                case DbType.String: return Type.GetType("System.String");
                case DbType.AnsiStringFixedLength: return Type.GetType("System.String");
                case DbType.StringFixedLength: return Type.GetType("System.String");
                default:
                    return Type.GetType("System.String");
            }

        }

        public GeneratedCode GenerateCode(string ProcedureName, List<ProcedureParameter> ProcedureParameters)
        {
            GeneratedCode code = new GeneratedCode();
            List<OutputColumn> OutputColumns = GetOutPutColumns(ProcedureName, ProcedureParameters);
            foreach (OutputColumn column in OutputColumns)
            {
                List<string> splitInfo = column.HeaderName.Split('$').ToList();
                if (splitInfo.Count > 1)
                    column.Field = splitInfo[1];
            }
            code.ColDefs = GenerateGridColDefs(OutputColumns);
            code.ModelClass = GenerateModelClass(OutputColumns, ProcedureName);
            code.RepoFunction = GenerateRepositoryFunction(OutputColumns, ProcedureParameters, ProcedureName);
            return code;
        }

        private string GenerateModelClass(List<OutputColumn> columns, string SPName)
        {
            string tempstr = SPName.Remove(0, 3);
            string YourModelClassName = tempstr.Remove(tempstr.LastIndexOf('_'), tempstr.Length- tempstr.LastIndexOf('_'));
            StringBuilder ModelClass = new StringBuilder("public class "+ YourModelClassName + "");
            ModelClass.Append("\n");
            ModelClass.Append("{");
            foreach (OutputColumn column in columns)
            {
                ModelClass.Append("\t");
                ModelClass.Append("public ");
                ModelClass.Append(column.Type);
                ModelClass.Append(" ");
                ModelClass.Append(column.HeaderName);
                ModelClass.Append(" ");
                ModelClass.Append("{ get; set; }");
                ModelClass.Append("\n");
            }
            ModelClass.Append("}");
            return ModelClass.ToString();
        }

       


        private string GenerateRepositoryFunction(List<OutputColumn> columns, List<ProcedureParameter> procedureParameters, string procedureName)
        {
            string tempstr = procedureName.Remove(0, 3);
            string YourModelClassName = tempstr.Remove(tempstr.LastIndexOf('_'), 4);
            StringBuilder RepositoryFunction = new StringBuilder("public List<" + YourModelClassName + "> Get"+ YourModelClassName + " (");
            foreach (ProcedureParameter parameter in procedureParameters)
            {
                DbType dbType = GetDbType(parameter.DataType);
                Type type = GetType(dbType);
                string parameterDataType = type.Name;
                using (var provider = new CSharpCodeProvider())
                {
                    var typeRef = new CodeTypeReference(type);
                    parameterDataType = provider.GetTypeOutput(typeRef);
                }
                RepositoryFunction.Append(parameterDataType);
                RepositoryFunction.Append(" ");
                RepositoryFunction.Append(parameter.Name.TrimStart('@'));
                RepositoryFunction.Append(", ");
            }
            RepositoryFunction = new StringBuilder(RepositoryFunction.ToString().Trim().TrimEnd(','));
            RepositoryFunction.Append(")");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("{");
            RepositoryFunction.Append("\t");
            RepositoryFunction.Append("List<" + YourModelClassName + "> GridRecords = new List<" + YourModelClassName + ">();");
            RepositoryFunction.Append("\n\t");
            RepositoryFunction.Append("var command = dbContext.CreateCommand();");
            RepositoryFunction.Append("\n\t");
            RepositoryFunction.Append("command.CommandType = CommandType.StoredProcedure;");
            RepositoryFunction.Append("\n\t");
            RepositoryFunction.Append("command.CommandText = \"");
            RepositoryFunction.Append(procedureName);
            RepositoryFunction.Append("\";");
            RepositoryFunction.Append("\n\t");
            foreach (ProcedureParameter parameter in procedureParameters)
            {
                if (parameter.Value == null)
                    continue;
                DbType dbType = GetDbType(parameter.DataType);
                Type type = GetType(dbType);
                RepositoryFunction.Append("command.AddParameter(\"" + parameter.Name + "\", " + parameter.Name.TrimStart('@') + ", DbType." + dbType.ToString() + ");");
                RepositoryFunction.Append("\n\t");
            }
            RepositoryFunction.Append("command.OpenConnection();");
            RepositoryFunction.Append("\n\t");
            RepositoryFunction.Append("var reader = command.OpenReader();");
            RepositoryFunction.Append("\n\t");
            RepositoryFunction.Append("while (reader.Read())");
            RepositoryFunction.Append("\n\t");
            RepositoryFunction.Append("{");
            RepositoryFunction.Append("\n\t\t");
            RepositoryFunction.Append("" + YourModelClassName + " GridRecord = new " + YourModelClassName + "()");
            RepositoryFunction.Append("\n\t");
            RepositoryFunction.Append("{");
            foreach (OutputColumn column in columns)
            {
                //RepositoryFunction.Append("\n\t\t");
                //RepositoryFunction.Append("if (!reader.IsDBNull(reader.GetOrdinal(\"" + column.HeaderName + "\")))");
                //RepositoryFunction.Append("\n\t\t\t");
                //RepositoryFunction.Append("GridRecord." + column.Field + " = reader." + GetMethodName(column) + "(reader.GetOrdinal(\"" + column.HeaderName + "\"));");

                RepositoryFunction.Append("\n\t\t\t");
                RepositoryFunction.Append(column.HeaderName + " = reader.ValidateColumnExistExtractAndCastTo<" + column.Type + ">(\"" + column.HeaderName + "\"),");
            }
            RepositoryFunction.Append("\n\t");
            RepositoryFunction.Append("};");
            RepositoryFunction.Append("\n\t");
            RepositoryFunction.Append("GridRecords.Add(GridRecord);");
            RepositoryFunction.Append("\n\t");
            RepositoryFunction.Append("};");
            RepositoryFunction.Append("\n\t");
            RepositoryFunction.Append("command.CloseConnection();");
            RepositoryFunction.Append("\n\t");
            RepositoryFunction.Append("return GridRecords;");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("}");
            return RepositoryFunction.ToString();
        }

        private string GetMethodName(OutputColumn column)
        {
            if (column.ColumnType.Name == "Byte[]")
                return "GetBytes";
            if (column.ColumnType.Name == "Byte")
                return "GetByte";
            if (column.ColumnType.Name == "Char")
                return "GetChar";
            if (column.ColumnType.Name == "Char[]")
                return "GetChars";
            if (column.ColumnType.Name == "Object")
                return "GetValue";
            return "Get" + column.ColumnType.Name;
        }
        private string GenerateGridColDefs(List<OutputColumn> columns)
        {
            StringBuilder colDefs = new StringBuilder("columnDefs=[");
            colDefs.Append("\n");
            foreach (OutputColumn column in columns)
            {
                colDefs.Append("\t");
                colDefs.Append("{ headerName: '");
                colDefs.Append(column.HeaderName);
                colDefs.Append("', field: '");
                colDefs.Append(column.HeaderName);
                colDefs.Append("' },");
                colDefs.Append("\n");
            }
            colDefs.Append("];");
            return colDefs.ToString();
        }

        public string GenerateDataInsertFunction(string SPName)
        {
            string tempstr = SPName.Remove(0, 3);
            string YourModelClassName = tempstr.Remove(tempstr.LastIndexOf('_'), 2);
            var parameters = GetSpParameters(SPName);
            StringBuilder RepositoryFunction = new StringBuilder("public bool Add" + YourModelClassName + "(" + YourModelClassName + " request)");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("{");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("int result;");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("var command = dbContext.CreateCommand();");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("command.CommandType = CommandType.StoredProcedure;");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("command.CommandText = \"" + SPName + "\";");
            foreach (var item in parameters)
            {
                RepositoryFunction.Append("\n");
                RepositoryFunction.Append("command.AddParameter(\"" + item.Name + "\", request." + item.Name.TrimStart('@') + ", DbType." + GetDbType(item.DataType) + ");");
            }
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("try");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("{");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("command.OpenConnection();");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("result = command.ExecuteNonQuery();");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("}");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("finally");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("{");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("command.CloseConnection();");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("}");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("return (result > 0);");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("}");
            return RepositoryFunction.ToString();
        }

        public string GenerateDataUpdateFunction(string SPName)
        {
            string tempstr = SPName.Remove(0, 3);
            string YourModelClassName = tempstr.Remove(tempstr.LastIndexOf('_'), 2);

            var parameters = GetSpParameters(SPName);

            StringBuilder RepositoryFunction = new StringBuilder("public IList<" + YourModelClassName + "> Update" + YourModelClassName + "(List<" + YourModelClassName + "> tasks)");
            RepositoryFunction.Append("\n\t");
            RepositoryFunction.Append("{");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("var result = new List<" + YourModelClassName + ">();");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("var command = dbContext.CreateCommand();");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("command.CommandType = CommandType.StoredProcedure;");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("command.CommandText = \"" + SPName + "\";");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("SqlParameter[] sqlParams = new SqlParameter[" + parameters.Count + "];");
            int count = 0;
            foreach (var item in parameters)
            {
                RepositoryFunction.Append("\n");
                RepositoryFunction.Append("sqlParams[" + count + "] = new SqlParameter(\"" + item.Name + "\", DbType." + GetDbType(item.DataType) + ");");
                count++;
            }

            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("using (var transaction = new TransactionScope())");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("{");
            RepositoryFunction.Append("\n\t");
            RepositoryFunction.Append("using (command.Connection)");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("{");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("if (command.Connection.State == ConnectionState.Closed)");
            RepositoryFunction.Append("\n\t");
            RepositoryFunction.Append("command.Connection.Open();");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("foreach (var item in tasks)");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("{");
            RepositoryFunction.Append("\n\t");
            RepositoryFunction.Append("var data = new " + YourModelClassName + "();");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("command.Parameters.Clear();");


            count = 0;
            foreach (var item in parameters)
            {
                RepositoryFunction.Append("\n");
                RepositoryFunction.Append("sqlParams[" + count + "].Value = (object)item." + item.Name.TrimStart('@') + ";");
                count++;
            }

            count = 0;
            foreach (var item in parameters)
            {
                RepositoryFunction.Append("\n");
                RepositoryFunction.Append("command.Parameters.Add(sqlParams[" + count + "]);");
                count++;
            }

            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("try");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("{");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("command.ExecuteNonQuery();");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("data.RowId = item.RowId;");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("data.Message = \"\";");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("}");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append(" catch (Exception ex)");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("{");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("data.RowId = item.RowId;");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("data.Message = \"Error while updating record.\";");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("}");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("finally");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("{");
            RepositoryFunction.Append("\n\t");
            RepositoryFunction.Append(" result.Add(data);");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("}");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("}");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("}");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("transaction.Complete();");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("}");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("return result;");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("}");
            return RepositoryFunction.ToString();
        }

        public string GenerateDataDeleteFunction(string SPName)
        {
            string tempstr = SPName.Remove(0, 3);
            string YourModelClassName = tempstr.Remove(tempstr.LastIndexOf('_'), 2);

            var parameters = GetSpParameters(SPName);

            StringBuilder RepositoryFunction = new StringBuilder("public IList<" + YourModelClassName + "> Delete" + YourModelClassName + "(List<" + YourModelClassName + "> tasks)");
            RepositoryFunction.Append("\n\t");
            RepositoryFunction.Append("{");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("var result = new List<" + YourModelClassName + ">();");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("var command = dbContext.CreateCommand();");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("command.CommandType = CommandType.StoredProcedure;");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("command.CommandText = \"" + SPName + "\";");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("SqlParameter[] sqlParams = new SqlParameter[1];");
            int count = 0;
            foreach (var item in parameters)
            {
                RepositoryFunction.Append("\n");
                RepositoryFunction.Append("sqlParams[" + count + "] = new SqlParameter(\"" + item.Name + "\", DbType." + GetDbType(item.DataType) + ");");
                count++;
                break;
            }

            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("using (var transaction = new TransactionScope())");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("{");
            RepositoryFunction.Append("\n\t");
            RepositoryFunction.Append("using (command.Connection)");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("{");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("if (command.Connection.State == ConnectionState.Closed)");
            RepositoryFunction.Append("\n\t");
            RepositoryFunction.Append("command.Connection.Open();");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("foreach (var item in tasks)");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("{");
            RepositoryFunction.Append("\n\t");
            RepositoryFunction.Append("var data = new " + YourModelClassName + "();");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("command.Parameters.Clear();");


            count = 0;
            foreach (var item in parameters)
            {
                RepositoryFunction.Append("\n");
                RepositoryFunction.Append("sqlParams[" + count + "].Value = (object)item." + item.Name.TrimStart('@') + ";");
                count++;
                break;
            }

            count = 0;
            foreach (var item in parameters)
            {
                RepositoryFunction.Append("\n");
                RepositoryFunction.Append("command.Parameters.Add(sqlParams[" + count + "]);");
                count++;
                break;
            }

            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("try");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("{");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("command.ExecuteNonQuery();");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("data.RowId = item.RowId;");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("data.Message = \"\";");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("}");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append(" catch (Exception ex)");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("{");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("data.RowId = item.RowId;");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("data.Message = \"Error while updating record.\";");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("}");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("finally");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("{");
            RepositoryFunction.Append("\n\t");
            RepositoryFunction.Append(" result.Add(data);");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("}");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("}");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("}");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("transaction.Complete();");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("}");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("return result;");
            RepositoryFunction.Append("\n");
            RepositoryFunction.Append("}");
            return RepositoryFunction.ToString();
        }


    }
}
