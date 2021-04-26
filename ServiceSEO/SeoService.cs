
using Infrastructure.Repository;
using ModelSEO;
using ServiceHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace ServiceSEO
{
    public class SeoService : BaseService, ISeoService
    {
        readonly IDataContext dbContext;
        public SeoService(IDataContext context)
        {
            dbContext = context;
        }

        public bool AddSeoContent(SeoContent seoContent)
        {
            int result;
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_SeoContent_i";
            command.AddParameter("@MetaTitle", seoContent.MetaTitle, DbType.String, 400);
            command.AddParameter("@MetaKeyword", seoContent.MetaKeyword, DbType.String, 400);
            command.AddParameter("@MetaDescription", seoContent.MetaDescription, DbType.String, 5000);
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
        public IList<SeoContent> UpdateSeoContent(List<SeoContent> tasks)
        {
            var result = new List<SeoContent>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_SeoContent_u";
            SqlParameter[] sqlParams = new SqlParameter[4];
            sqlParams[0] = new SqlParameter("@RowId", SqlDbType.Int, 8);
            sqlParams[1] = new SqlParameter("@MetaTitle", SqlDbType.VarChar, 400);
            sqlParams[2] = new SqlParameter("@MetaKeyword", SqlDbType.VarChar, 400);
            sqlParams[3] = new SqlParameter("@MetaDescription", SqlDbType.VarChar, 5000);

            using (var transaction = new TransactionScope())
            {
                using (command.Connection)
                {
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();

                    foreach (var item in tasks)
                    {
                        var data = new SeoContent();
                        command.Parameters.Clear();

                        sqlParams[0].Value = (object)item.RowId;
                        sqlParams[1].Value = (object)item.MetaTitle;
                        sqlParams[2].Value = (object)item.MetaKeyword;
                        sqlParams[3].Value = (object)item.MetaDescription;
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
        public IList<SeoContent> DeleteSeoContent(List<SeoContent> tasks)
        {
            var result = new List<SeoContent>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_SeoContent_d";
            SqlParameter[] sqlParams = new SqlParameter[1];
            sqlParams[0] = new SqlParameter("@RowId", SqlDbType.Int, 8);

            using (var transaction = new TransactionScope())
            {
                using (command.Connection)
                {
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();

                    foreach (var item in tasks)
                    {
                        var data = new SeoContent();
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
                            data.Message = "Error while deleting record.";
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
        public List<SeoContent> GetSeoContent(string QueryConditionPartParam)
        {
            QueryConditionPartParam = QueryConditionPartParam == null ? "" : QueryConditionPartParam;
            List<SeoContent> GridRecords = new List<SeoContent>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_SeoContent_Get";
            command.AddParameter("@QueryConditionPartParam", QueryConditionPartParam, DbType.String);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                SeoContent GridRecord = new SeoContent()
                {
                    RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
                    MetaTitle = reader.ValidateColumnExistExtractAndCastTo<string>("MetaTitle"),
                    MetaKeyword = reader.ValidateColumnExistExtractAndCastTo<string>("MetaKeyword"),
                    MetaDescription = reader.ValidateColumnExistExtractAndCastTo<string>("MetaDescription"),
                };
                GridRecords.Add(GridRecord);
            }

            command.CloseConnection();
            return GridRecords;

        }
    }
}
