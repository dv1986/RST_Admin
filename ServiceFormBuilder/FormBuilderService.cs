using Infrastructure.Repository;
using ModelFormBuilder;
using ServiceHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace ServiceFormBuilder
{
    public class FormBuilderService : BaseService, IFormBuilderService
    {
        readonly IDataContext dbContext;
        public FormBuilderService(IDataContext context)
        {
            dbContext = context;
        }

        public bool AddFormBuilder(FormBuilder request)
        {
            int result;
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_FormBuilder_i";
            command.AddParameter("@FormName", request.FormName, DbType.String);
            command.AddParameter("@FormJson", request.FormJson, DbType.String);
            try
            {
                command.OpenConnection();
                result = command.ExecuteNonQuery();
            }
            finally
            {
                command.CloseConnection();
            }
            return (result > 0);
        }

        public bool UpdateFormBuilderbyId(FormBuilder request)
        {
            int result;
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_FormBuilder_u_byId";
            command.AddParameter("@RowId", request.RowId, DbType.String);
            command.AddParameter("@FormName", request.FormName, DbType.String);
            command.AddParameter("@FormJson", request.FormJson, DbType.String);
            try
            {
                command.OpenConnection();
                result = command.ExecuteNonQuery();
            }
            finally
            {
                command.CloseConnection();
            }
            return (result > 0);
        }

        public IList<FormBuilder> UpdateFormBuilder(List<FormBuilder> tasks)
        {
            var result = new List<FormBuilder>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_FormBuilder_u";
            SqlParameter[] sqlParams = new SqlParameter[4];
            sqlParams[0] = new SqlParameter("@RowId", DbType.Int32);
            sqlParams[1] = new SqlParameter("@FormName", DbType.String);
            sqlParams[2] = new SqlParameter("@FormJson", DbType.String);
            sqlParams[3] = new SqlParameter("@IsDeleted", DbType.Boolean);
            using (var transaction = new TransactionScope())
            {
                using (command.Connection)
                {
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();
                    foreach (var item in tasks)
                    {
                        var data = new FormBuilder();
                        command.Parameters.Clear();
                        sqlParams[0].Value = (object)item.RowId;
                        sqlParams[1].Value = (object)item.FormName;
                        sqlParams[2].Value = (object)item.FormJson;
                        sqlParams[3].Value = (object)item.IsDeleted;
                        command.Parameters.Add(sqlParams[0]);
                        command.Parameters.Add(sqlParams[1]);
                        command.Parameters.Add(sqlParams[2]);
                        command.Parameters.Add(sqlParams[3]);
                        try
                        {
                            command.ExecuteNonQuery();
                            data.RowId = item.RowId;
                            data.Message = "";
                        }
                        catch (Exception ex)
                        {
                            data.RowId = item.RowId;
                            data.Message = "Error while updating record.";
                        }
                        finally
                        {
                            result.Add(data);
                        }
                    }
                }
                transaction.Complete();
            }
            return result;
        }

        public IList<FormBuilder> DeleteFormBuilder(List<FormBuilder> tasks)
        {
            var result = new List<FormBuilder>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_FormBuilder_d";
            SqlParameter[] sqlParams = new SqlParameter[1];
            sqlParams[0] = new SqlParameter("@RowId", DbType.Int32);
            using (var transaction = new TransactionScope())
            {
                using (command.Connection)
                {
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();
                    foreach (var item in tasks)
                    {
                        var data = new FormBuilder();
                        command.Parameters.Clear();
                        sqlParams[0].Value = (object)item.RowId;
                        command.Parameters.Add(sqlParams[0]);
                        try
                        {
                            command.ExecuteNonQuery();
                            data.RowId = item.RowId;
                            data.Message = "";
                        }
                        catch (Exception ex)
                        {
                            data.RowId = item.RowId;
                            data.Message = "Error while updating record.";
                        }
                        finally
                        {
                            result.Add(data);
                        }
                    }
                }
                transaction.Complete();
            }
            return result;
        }

        public List<FormBuilder> GetFormBuilder(int Id)
        {
            string QueryConditionPartParam = "";
            if (Id != 0)
            {
                QueryConditionPartParam = " AND a.RowId=" + Id + "";
            }
            List<FormBuilder> GridRecords = new List<FormBuilder>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_FormBuilder_Get";
            command.AddParameter("@QueryConditionPartParam", QueryConditionPartParam, DbType.String);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                FormBuilder GridRecord = new FormBuilder()
                {
                    RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
                    FormName = reader.ValidateColumnExistExtractAndCastTo<string>("FormName"),
                    FormJson = reader.ValidateColumnExistExtractAndCastTo<string>("FormJson"),
                    IsDeleted = reader.ValidateColumnExistExtractAndCastTo<bool>("IsDeleted"),
                };
                GridRecords.Add(GridRecord);
            };
            command.CloseConnection();
            return GridRecords;
        }

    }
}
