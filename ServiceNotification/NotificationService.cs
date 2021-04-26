using Infrastructure.Repository;
using ModelNotification;
using ServiceHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Transactions;

namespace ServiceNotification
{
    public class NotificationService : BaseService, INotificationService
    {
        readonly IDataContext dbContext;
        public NotificationService(IDataContext context)
        {
            dbContext = context;
        }

        public bool AddNotification(Notification request)
        {
            int result;
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_Notification_i";
            command.AddParameter("@ProductId", request.ProductId, DbType.Int32);
            command.AddParameter("@TextPrompt", request.TextPrompt, DbType.String);
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

        public IList<Notification> UpdateNotification(List<Notification> tasks)
        {
            var result = new List<Notification>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_Notification_u";
            SqlParameter[] sqlParams = new SqlParameter[4];
            sqlParams[0] = new SqlParameter("@RowId", DbType.Int32);
            sqlParams[1] = new SqlParameter("@ProductId", DbType.Int32);
            sqlParams[2] = new SqlParameter("@TextPrompt", DbType.String);
            sqlParams[3] = new SqlParameter("@IsActive", DbType.Int32);
            using (var transaction = new TransactionScope())
            {
                using (command.Connection)
                {
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();
                    foreach (var item in tasks)
                    {
                        var data = new Notification();
                        command.Parameters.Clear();
                        sqlParams[0].Value = (object)item.RowId;
                        sqlParams[1].Value = (object)item.ProductId;
                        sqlParams[2].Value = (object)item.TextPrompt;
                        sqlParams[3].Value = (object)item.IsActive;
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

        public IList<Notification> DeleteNotification(List<Notification> tasks)
        {
            var result = new List<Notification>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_Notification_d";
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
                        var data = new Notification();
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

        public List<Notification> GetNotification(string QueryConditionPartParam)
        {
            List<Notification> GridRecords = new List<Notification>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_Notification_Get";
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                Notification GridRecord = new Notification()
                {
                    RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
                    ProductId = reader.ValidateColumnExistExtractAndCastTo<int>("ProductId"),
                    TextPrompt = reader.ValidateColumnExistExtractAndCastTo<string>("TextPrompt"),
                    IsActive = reader.ValidateColumnExistExtractAndCastTo<bool>("IsActive"),
                    ProductTitle = reader.ValidateColumnExistExtractAndCastTo<string>("ProductTitle"),
                    SKUCode = reader.ValidateColumnExistExtractAndCastTo<string>("SKUCode"),
                    ShortDescription = reader.ValidateColumnExistExtractAndCastTo<string>("ShortDescription"),
                    ModelNumber = reader.ValidateColumnExistExtractAndCastTo<string>("ModelNumber"),
                    ModelYear = reader.ValidateColumnExistExtractAndCastTo<int>("ModelYear"),
                };
                GridRecords.Add(GridRecord);
            };
            command.CloseConnection();
            return GridRecords;
        }

        public List<Notification> GetApprovedNotification()
        {
            List<Notification> GridRecords = new List<Notification>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ApprovedNotification_Get";
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                Notification GridRecord = new Notification()
                {
                    ProductId = reader.ValidateColumnExistExtractAndCastTo<int>("ProductId"),
                    TextPrompt = reader.ValidateColumnExistExtractAndCastTo<string>("TextPrompt"),
                    ProductTitle = reader.ValidateColumnExistExtractAndCastTo<string>("ProductTitle"),
                    IsActive = reader.ValidateColumnExistExtractAndCastTo<bool>("IsActive"),
                    SKUCode = reader.ValidateColumnExistExtractAndCastTo<string>("SKUCode"),
                    ShortDescription = reader.ValidateColumnExistExtractAndCastTo<string>("ShortDescription"),
                    ModelNumber = reader.ValidateColumnExistExtractAndCastTo<string>("ModelNumber"),
                    ModelYear = reader.ValidateColumnExistExtractAndCastTo<int>("ModelYear"),
                };
                GridRecords.Add(GridRecord);
            };
            command.CloseConnection();
            return GridRecords;
        }
    }
}
