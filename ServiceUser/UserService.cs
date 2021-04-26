
using Infrastructure.Repository;
using ModelAdvertisement;
using ModelLookup;
//using Entity.Framework.Core.Models;
//using Infrastructure.Repository;
using ModelUser;
//using RepositoryUsers;
//using RST.Shared;
using ServiceHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace ServiceUsers
{
    public class UserService : BaseService, IUserService
    {
        readonly IDataContext dbContext;
        public UserService(IDataContext context)
        {
            dbContext = context;
        }


        public bool AddUser(Users user)
        {
            int result;
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_user_i";
            command.AddParameter("@first_name", user.Firstname, DbType.String, 50);
            command.AddParameter("@last_name", user.Lastname, DbType.String, 50);
            command.AddParameter("@email", user.Email, DbType.String, 50);
            command.AddParameter("@date_of_birth", user.DateOfBirth, DbType.Date, 16);
            command.AddParameter("@gender", user.Gender, DbType.String, 5);
            command.AddParameter("@mobile_number", user.Mobile, DbType.String, 10);
            command.AddParameter("@phone_number", user.Phone, DbType.String, 10);
            command.AddParameter("@garage_name", user.GarageName, DbType.String, 50);
            command.AddParameter("@address_line_1", user.AddressLine1, DbType.String, 100);
            command.AddParameter("@address_line_2", user.AddressLine2, DbType.String, 100);
            command.AddParameter("@country_id", user.CountryId, DbType.Int32, 8);
            command.AddParameter("@state_id", user.StateId, DbType.Int32, 8);
            command.AddParameter("@city_id", user.CityId, DbType.Int32, 8);
            command.AddParameter("@zip_code", user.ZipCode, DbType.String, 7);
            command.AddParameter("@Password", Helper.Encrypt(user.Password), DbType.String, 50);
            command.AddParameter("@user_type_id", user.UserTypeId, DbType.Int32, 8);
            command.AddParameter("@subscription_id", user.SubscriptionId, DbType.Int32, 8);
            command.AddParameter("@advertise_image_id", user.AdvertiseImageId, DbType.Int32, 8);
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

        public bool UpdatePassword(Users user)
        {
            int result;
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_userPassword_u";
            command.AddParameter("@mobile_number", user.Mobile, DbType.String, 10);
            command.AddParameter("@Password", Helper.Encrypt(user.Password), DbType.String, 50);
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

        public IList<Users> UpdateUser(List<Users> tasks)
        {
            var result = new List<Users>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_User_u";
            SqlParameter[] sqlParams = new SqlParameter[15];
            sqlParams[0] = new SqlParameter("@row_id", SqlDbType.Int, 8);
            sqlParams[1] = new SqlParameter("@first_name", SqlDbType.VarChar, 50);
            sqlParams[2] = new SqlParameter("@last_name", SqlDbType.VarChar, 50);
            sqlParams[3] = new SqlParameter("@email ", SqlDbType.VarChar, 50);
            sqlParams[4] = new SqlParameter("@date_of_birth ", SqlDbType.Date, 10);
            sqlParams[5] = new SqlParameter("@mobile_number ", SqlDbType.VarChar, 10);
            sqlParams[6] = new SqlParameter("@phone_number ", SqlDbType.VarChar, 10);
            sqlParams[7] = new SqlParameter("@garage_name ", SqlDbType.VarChar, 50);
            sqlParams[8] = new SqlParameter("@address_line_1 ", SqlDbType.VarChar, 100);
            sqlParams[9] = new SqlParameter("@address_line_2 ", SqlDbType.VarChar, 100);
            sqlParams[10] = new SqlParameter("@zip_code ", SqlDbType.VarChar, 7);
            sqlParams[11] = new SqlParameter("@gender ", SqlDbType.VarChar, 5);
            sqlParams[12] = new SqlParameter("@user_type_id ", SqlDbType.Int, 8);
            sqlParams[13] = new SqlParameter("@subscription_id ", SqlDbType.Int, 8);
            sqlParams[14] = new SqlParameter("@Password ", SqlDbType.VarChar, 50);

            using (var transaction = new TransactionScope())
            {
                using (command.Connection)
                {
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();

                    foreach (var item in tasks)
                    {
                        var data = new Users();
                        command.Parameters.Clear();

                        sqlParams[0].Value = (object)item.RowId;
                        sqlParams[1].Value = (object)item.Firstname;
                        sqlParams[2].Value = (object)item.Lastname;
                        sqlParams[3].Value = (object)item.Email;
                        sqlParams[4].Value = (object)item.DateOfBirth;
                        sqlParams[5].Value = (object)item.Mobile;
                        sqlParams[6].Value = (object)item.Phone;
                        sqlParams[7].Value = (object)item.GarageName;
                        sqlParams[8].Value = (object)item.AddressLine1;
                        sqlParams[9].Value = (object)item.AddressLine2;
                        sqlParams[10].Value = (object)item.ZipCode;
                        sqlParams[11].Value = (object)item.Gender;
                        sqlParams[12].Value = (object)item.UserTypeId;
                        sqlParams[13].Value = (object)item.SubscriptionId;
                        sqlParams[14].Value = (object)Helper.Encrypt(item.Password);
                        command.Parameters.Add(sqlParams[0]);
                        command.Parameters.Add(sqlParams[1]);
                        command.Parameters.Add(sqlParams[2]);
                        command.Parameters.Add(sqlParams[3]);
                        command.Parameters.Add(sqlParams[4]);
                        command.Parameters.Add(sqlParams[5]);
                        command.Parameters.Add(sqlParams[6]);
                        command.Parameters.Add(sqlParams[7]);
                        command.Parameters.Add(sqlParams[8]);
                        command.Parameters.Add(sqlParams[9]);
                        command.Parameters.Add(sqlParams[10]);
                        command.Parameters.Add(sqlParams[11]);
                        command.Parameters.Add(sqlParams[12]);
                        command.Parameters.Add(sqlParams[13]);
                        command.Parameters.Add(sqlParams[14]);
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

        public IList<Users> DeleteUser(List<Users> tasks)
        {
            var result = new List<Users>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_User_d";
            SqlParameter[] sqlParams = new SqlParameter[1];
            sqlParams[0] = new SqlParameter("@row_id", SqlDbType.VarChar, 50);
            using (var transaction = new TransactionScope())
            {
                using (command.Connection)
                {
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();

                    foreach (var item in tasks)
                    {
                        var data = new Users();
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

        public List<Users> GetAll(string UserName)
        {
            UserName = UserName == null ? "" : UserName;
            List<Users> GridRecords = new List<Users>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_Get_AllUsers";
            command.AddParameter("@userName", UserName, DbType.String, 16);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                Users GridRecord = new Users()
                {
                    RowId = reader.ValidateColumnExistExtractAndCastTo<int>("row_id"),
                    Firstname = reader.ValidateColumnExistExtractAndCastTo<string>("first_name"),
                    Lastname = reader.ValidateColumnExistExtractAndCastTo<string>("last_name"),
                    Email = reader.ValidateColumnExistExtractAndCastTo<string>("email"),
                    Gender = reader.ValidateColumnExistExtractAndCastTo<string>("gender"),
                    Mobile = reader.ValidateColumnExistExtractAndCastTo<string>("mobile_number"),
                    Phone = reader.ValidateColumnExistExtractAndCastTo<string>("phone_number"),
                    GarageName = reader.ValidateColumnExistExtractAndCastTo<string>("garage_name"),
                    AddressLine1 = reader.ValidateColumnExistExtractAndCastTo<string>("address_line_1"),
                    AddressLine2 = reader.ValidateColumnExistExtractAndCastTo<string>("address_line_2"),
                    CountryId = reader.ValidateColumnExistExtractAndCastTo<int>("country_id"),
                    CountryName = reader.ValidateColumnExistExtractAndCastTo<string>("country_name"),
                    StateId = reader.ValidateColumnExistExtractAndCastTo<int>("state_id"),
                    StateName = reader.ValidateColumnExistExtractAndCastTo<string>("state_name"),
                    CityId = reader.ValidateColumnExistExtractAndCastTo<int>("city_id"),
                    CityName = reader.ValidateColumnExistExtractAndCastTo<string>("city_name"),
                    ZipCode = reader.ValidateColumnExistExtractAndCastTo<string>("zip_code"),
                    DateOfBirth = reader.ValidateColumnExistExtractAndCastTo<DateTime>("date_of_birth"),
                    UserTypeId = reader.ValidateColumnExistExtractAndCastTo<int>("user_type_id"),
                    UserTypeName = reader.ValidateColumnExistExtractAndCastTo<string>("UserTypeName"),
                    SubscriptionId = reader.ValidateColumnExistExtractAndCastTo<int>("subscription_id"),
                    SubscriptionName = reader.ValidateColumnExistExtractAndCastTo<string>("SubscriptionName"),
                    AdvertiseImageId = reader.ValidateColumnExistExtractAndCastTo<int>("advertise_image_id"),
                    ImagePath = reader.ValidateColumnExistExtractAndCastTo<string>("ImagePath"),
                    ImageName = reader.ValidateColumnExistExtractAndCastTo<string>("ImageName"),
                    ThumbnailPath = reader.ValidateColumnExistExtractAndCastTo<string>("ThumbnailPath"),
                    Password = Helper.Decrypt(reader.ValidateColumnExistExtractAndCastTo<string>("Password")),
                };
                GridRecords.Add(GridRecord);
            }
            command.CloseConnection();
            return GridRecords;
        }


        public Users GetUsersDetail(int Id)
        {
            List<Users> GridRecords = new List<Users>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_UserDetailbyId_Get";
            command.AddParameter("@userId", Id, DbType.Int32, 8);
            command.OpenConnection();
            var reader = command.OpenReader();
            Users Record = new Users();
            while (reader.Read())
            {
                Record.Firstname = reader.ValidateColumnExistExtractAndCastTo<string>("first_name");
                Record.Lastname = reader.ValidateColumnExistExtractAndCastTo<string>("last_name");
                Record.AddressLine1 = reader.ValidateColumnExistExtractAndCastTo<string>("address_line_1");
                Record.AddressLine2 = reader.ValidateColumnExistExtractAndCastTo<string>("address_line_2");
                Record.CityName = reader.ValidateColumnExistExtractAndCastTo<string>("city_name");
                Record.StateName = reader.ValidateColumnExistExtractAndCastTo<string>("state_name");
                Record.CountryName = reader.ValidateColumnExistExtractAndCastTo<string>("country_name");
                Record.ZipCode = reader.ValidateColumnExistExtractAndCastTo<string>("zip_code");
                Record.Mobile = reader.ValidateColumnExistExtractAndCastTo<string>("mobile_number");
                Record.Email = reader.ValidateColumnExistExtractAndCastTo<string>("email");

            }
            command.CloseConnection();
            return Record;
        }

        public Users GetUsersDetail(string MobileNumber)
        {
            List<Users> GridRecords = new List<Users>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_GetUserDetailByMobileNumber";
            command.AddParameter("@mobile", MobileNumber, DbType.String, 12);
            command.OpenConnection();
            var reader = command.OpenReader();
            Users Record = new Users();
            while (reader.Read())
            {
                Record.RowId = reader.ValidateColumnExistExtractAndCastTo<int>("row_id");
                Record.Firstname = reader.ValidateColumnExistExtractAndCastTo<string>("first_name");
                Record.Lastname = reader.ValidateColumnExistExtractAndCastTo<string>("last_name");
                Record.AddressLine1 = reader.ValidateColumnExistExtractAndCastTo<string>("address_line_1");
                Record.AddressLine2 = reader.ValidateColumnExistExtractAndCastTo<string>("address_line_2");
                Record.CityName = reader.ValidateColumnExistExtractAndCastTo<string>("city_name");
                Record.StateName = reader.ValidateColumnExistExtractAndCastTo<string>("state_name");
                Record.CountryName = reader.ValidateColumnExistExtractAndCastTo<string>("country_name");
                Record.ZipCode = reader.ValidateColumnExistExtractAndCastTo<string>("zip_code");
                Record.Mobile = reader.ValidateColumnExistExtractAndCastTo<string>("mobile_number");
                Record.Email = reader.ValidateColumnExistExtractAndCastTo<string>("email");

            }
            command.CloseConnection();
            return Record;
        }

        public string IsValidUserAndPasswordCombination(string userRef, string Password)
        {
            string result = "";
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_IsValidUser";
            command.AddParameter("@userRef", userRef, DbType.String, 50);
            command.AddParameter("@Password", Password, DbType.String, 50);
            try
            {
                command.OpenConnection();
                var reader = command.OpenReader();
                while (reader.Read())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("isValid")))
                        result = reader.GetString(reader.GetOrdinal("isValid"));
                }
            }
            finally
            {
                command.CloseConnection();
            }
            return result;
        }

        public Users GetUserInfo(string userRef, string Password)
        {
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_GetUserById";
            command.AddParameter("@userRef", userRef, DbType.String, 50);
            command.AddParameter("@Password", Password, DbType.String, 50);
            command.OpenConnection();
            var reader = command.OpenReader();
            Users GridRecord = null;
            while (reader.Read())
            {
                GridRecord = new Users()
                {
                    RowId = reader.ValidateColumnExistExtractAndCastTo<int>("row_id"),
                    Firstname = reader.ValidateColumnExistExtractAndCastTo<string>("first_name"),
                    Lastname = reader.ValidateColumnExistExtractAndCastTo<string>("last_name"),
                    Email = reader.ValidateColumnExistExtractAndCastTo<string>("email"),
                    AddressLine1 = reader.ValidateColumnExistExtractAndCastTo<string>("address_line_1"),
                    AddressLine2 = reader.ValidateColumnExistExtractAndCastTo<string>("address_line_2"),
                    CityName = reader.ValidateColumnExistExtractAndCastTo<string>("city_name"),
                    StateName = reader.ValidateColumnExistExtractAndCastTo<string>("state_name"),
                    CountryName = reader.ValidateColumnExistExtractAndCastTo<string>("country_name"),
                    ZipCode = reader.ValidateColumnExistExtractAndCastTo<string>("zip_code"),
                    Mobile = reader.ValidateColumnExistExtractAndCastTo<string>("mobile_number"),
                };
            }
            command.CloseConnection();
            return GridRecord;
        }


        public string ValidateEmailandMobile(string Email, string Mobile)
        {
            string result = "";
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ValidateEmailandMobile";
            command.AddParameter("@email", Email, DbType.String, 50);
            command.AddParameter("@mobilenumber", Mobile, DbType.String, 12);
            try
            {
                command.OpenConnection();
                var reader = command.OpenReader();
                while (reader.Read())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("error")))
                        result = reader.GetString(reader.GetOrdinal("error"));
                }
            }
            finally
            {
                command.CloseConnection();
            }
            return result;
        }


        #region User Permission
        public bool AddUserPermission(User_SubMenuDTO user)
        {
            if (!DeleteUserPermission(user.UserId))
                return false;
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_user_permission_i";
            SqlParameter[] sqlParams = new SqlParameter[2];
            sqlParams[0] = new SqlParameter("@user_id", SqlDbType.Int, 8);
            sqlParams[1] = new SqlParameter("@sub_menu_id", SqlDbType.Int, 8);

            using (var transaction = new TransactionScope())
            {
                using (command.Connection)
                {
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();

                    foreach (var subMenuId in user.SubMenuIds)
                    {
                        command.Parameters.Clear();
                        sqlParams[0].Value = user.UserId;
                        sqlParams[1].Value = subMenuId;
                        command.Parameters.Add(sqlParams[0]);
                        command.Parameters.Add(sqlParams[1]);
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {

                        }
                        finally
                        {

                        }
                    }
                }
                transaction.Complete();
            }
            return true;
        }

        private bool DeleteUserPermission(int UserId)
        {
            int result;
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_user_permission_d";
            command.AddParameter("@user_id", UserId, DbType.String, 16);
            try
            {
                command.OpenConnection();
                result = command.ExecuteNonQuery();
                result = 1;
            }
            finally
            {
                command.CloseConnection();
            }
            return (result > 0);
        }

        public List<UserPermissionDTO> GetUserPermissionforUser(int UserId)
        {
            List<UserPermission> AllGridRecords = GetUserPermission(0);
            List<UserPermission> UserGridRecords = GetUserPermission(UserId);

            var treeNodes = Helper.BuildTreeAndReturnRootNodes(AllGridRecords, UserGridRecords);

            return treeNodes;
        }

        private List<UserPermission> GetUserPermission(int UserId)
        {
            List<UserPermission> GridRecords = new List<UserPermission>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_user_permission_Get";
            command.AddParameter("@userId", UserId, DbType.String, 16);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                UserPermission GridRecord = new UserPermission();
                if (!reader.IsDBNull(reader.GetOrdinal("row_id")))
                    GridRecord.RowId = reader.GetInt32(reader.GetOrdinal("row_id"));
                if (!reader.IsDBNull(reader.GetOrdinal("menuId")))
                    GridRecord.MenuId = reader.GetInt32(reader.GetOrdinal("menuId"));
                if (!reader.IsDBNull(reader.GetOrdinal("menu_name")))
                    GridRecord.MenuName = reader.GetString(reader.GetOrdinal("menu_name"));
                if (!reader.IsDBNull(reader.GetOrdinal("subMenuId")))
                    GridRecord.SubMenuId = reader.GetInt32(reader.GetOrdinal("subMenuId"));
                if (!reader.IsDBNull(reader.GetOrdinal("sub_menu_name")))
                    GridRecord.SubMenuName = reader.GetString(reader.GetOrdinal("sub_menu_name"));
                if (!reader.IsDBNull(reader.GetOrdinal("icon_name")))
                    GridRecord.IconName = reader.GetString(reader.GetOrdinal("icon_name"));
                GridRecords.Add(GridRecord);
            }

            command.CloseConnection();
            return GridRecords;
        }
        #endregion

        public List<UserType> GetUserType()
        {
            List<UserType> GridRecords = new List<UserType>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_UserType_Get";
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                UserType GridRecord = new UserType()
                {
                    RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
                    UserTypeName = reader.ValidateColumnExistExtractAndCastTo<string>("UserTypeName"),
                };
                GridRecords.Add(GridRecord);
            }

            command.CloseConnection();
            return GridRecords;
        }

        public List<Advertisement> GetAdvertisement()
        {
            List<Advertisement> GridRecords = new List<Advertisement>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_Advertisement_Get";
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                Advertisement GridRecord = new Advertisement()
                {
                    Id = reader.ValidateColumnExistExtractAndCastTo<int>("row_id"),
                    ImageUrl = reader.ValidateColumnExistExtractAndCastTo<string>("ThumbnailPath"),
                    IsShowOnHomePage = true,
                    Url = "http://motoiz.in/"
                };
                GridRecords.Add(GridRecord);
            }

            command.CloseConnection();
            return GridRecords;
        }
    }
}
