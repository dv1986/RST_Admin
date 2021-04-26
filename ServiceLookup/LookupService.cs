using Infrastructure.Repository;
using ModelLookup;
using ServiceHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Transactions;

namespace ServiceLookup
{
    public class LookupService: BaseService, ILookupService
    {
        readonly IDataContext dbContext;
        public LookupService(IDataContext context)
        {
            dbContext = context;
        }
        #region Country
        public bool AddCountry(Country country)
        {
            int result;
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_Country_i";
            command.AddParameter("@country", country.CountryName, DbType.String, 50);
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
        public IList<Country> UpdateCountry(List<Country> tasks)
        {
            var result = new List<Country>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_Country_u";
            SqlParameter[] sqlParams = new SqlParameter[2];
            sqlParams[0] = new SqlParameter("@row_id", SqlDbType.Int, 8);
            sqlParams[1] = new SqlParameter("@country", SqlDbType.VarChar, 50);

            using (var transaction = new TransactionScope())
            {
                using (command.Connection)
                {
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();

                    foreach (var item in tasks)
                    {
                        var data = new Country();
                        command.Parameters.Clear();

                        sqlParams[0].Value = (object)item.RowId;
                        sqlParams[1].Value = (object)item.CountryName;
                        command.Parameters.Add(sqlParams[0]);
                        command.Parameters.Add(sqlParams[1]);
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
        public IList<Country> DeleteCountry(List<Country> tasks)
        {
            var result = new List<Country>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_Country_d";
            SqlParameter[] sqlParams = new SqlParameter[1];
            sqlParams[0] = new SqlParameter("@row_id", SqlDbType.Int, 8);

            using (var transaction = new TransactionScope())
            {
                using (command.Connection)
                {
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();

                    foreach (var item in tasks)
                    {
                        var data = new Country();
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
        public List<Country> GetCountry(string SearchStr)
        {
            List<Country> GridRecords = new List<Country>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_Country_GetAll";
            command.AddParameter("@searchStr", SearchStr, DbType.String, 50);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                Country GridRecord = new Country();
                if (!reader.IsDBNull(reader.GetOrdinal("row_id")))
                    GridRecord.RowId = reader.GetInt32(reader.GetOrdinal("row_id"));
                if (!reader.IsDBNull(reader.GetOrdinal("country_name")))
                    GridRecord.CountryName = reader.GetString(reader.GetOrdinal("country_name"));
                GridRecords.Add(GridRecord);
            }
            command.CloseConnection();
            return GridRecords;
        }

        public List<Country> GetCountryLookup(string SearchStr)
        {
            List<Country> GridRecords = new List<Country>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_Country_lookup";
            command.AddParameter("@searchStr", SearchStr, DbType.String, 50);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                Country GridRecord = new Country();
                if (!reader.IsDBNull(reader.GetOrdinal("row_id")))
                    GridRecord.RowId = reader.GetInt32(reader.GetOrdinal("row_id"));
                if (!reader.IsDBNull(reader.GetOrdinal("country_name")))
                    GridRecord.CountryName = reader.GetString(reader.GetOrdinal("country_name"));
                GridRecords.Add(GridRecord);
            }
            command.CloseConnection();
            return GridRecords;
        }
        #endregion

        #region State
        public bool AddState(State state)
        {
            int result;
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_State_i";
            command.AddParameter("@country_id", state.CountryId, DbType.Int32, 8);
            command.AddParameter("@state", state.StateName, DbType.String, 50);
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
        public IList<State> UpdateState(List<State> tasks)
        {
            var result = new List<State>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_State_u";
            SqlParameter[] sqlParams = new SqlParameter[2];
            sqlParams[0] = new SqlParameter("@row_id", SqlDbType.Int, 8);
            sqlParams[1] = new SqlParameter("@state", SqlDbType.VarChar, 50);

            using (var transaction = new TransactionScope())
            {
                using (command.Connection)
                {
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();

                    foreach (var item in tasks)
                    {
                        var data = new State();
                        command.Parameters.Clear();

                        sqlParams[0].Value = (object)item.RowId;
                        sqlParams[1].Value = (object)item.StateName;
                        command.Parameters.Add(sqlParams[0]);
                        command.Parameters.Add(sqlParams[1]);
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
        public IList<State> DeleteState(List<State> tasks)
        {
            var result = new List<State>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_State_d";
            SqlParameter[] sqlParams = new SqlParameter[1];
            sqlParams[0] = new SqlParameter("@row_id", SqlDbType.Int, 8);

            using (var transaction = new TransactionScope())
            {
                using (command.Connection)
                {
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();

                    foreach (var item in tasks)
                    {
                        var data = new State();
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
        public List<State> GetState(string SearchStr)
        {
            List<State> GridRecords = new List<State>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_State_GetAll";
            command.AddParameter("@searchStr", SearchStr, DbType.String, 50);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                State GridRecord = new State();
                if (!reader.IsDBNull(reader.GetOrdinal("row_id")))
                    GridRecord.RowId = reader.GetInt32(reader.GetOrdinal("row_id"));
                if (!reader.IsDBNull(reader.GetOrdinal("state_name")))
                    GridRecord.StateName = reader.GetString(reader.GetOrdinal("state_name"));
                if (!reader.IsDBNull(reader.GetOrdinal("country_name")))
                    GridRecord.CountryName = reader.GetString(reader.GetOrdinal("country_name"));
                GridRecords.Add(GridRecord);
            }
            command.CloseConnection();
            return GridRecords;
        }
        public List<State> GetStateLookup(int CountryId)
        {
            List<State> GridRecords = new List<State>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_State_Lookup";
            command.AddParameter("@countryId", CountryId, DbType.String, 50);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                State GridRecord = new State();
                if (!reader.IsDBNull(reader.GetOrdinal("row_id")))
                    GridRecord.RowId = reader.GetInt32(reader.GetOrdinal("row_id"));
                if (!reader.IsDBNull(reader.GetOrdinal("state_name")))
                    GridRecord.StateName = reader.GetString(reader.GetOrdinal("state_name"));
                if (!reader.IsDBNull(reader.GetOrdinal("country_name")))
                    GridRecord.CountryName = reader.GetString(reader.GetOrdinal("country_name"));
                GridRecords.Add(GridRecord);
            }
            command.CloseConnection();
            return GridRecords;
        }

        #endregion

        #region City
        public bool AddCity(City city)
        {
            int result;
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_City_i";
            command.AddParameter("@state_id", city.StateId, DbType.Int32, 8);
            command.AddParameter("@city", city.CityName, DbType.String, 50);
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
        public IList<City> UpdateCity(List<City> tasks)
        {
            var result = new List<City>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_City_u";
            SqlParameter[] sqlParams = new SqlParameter[2];
            sqlParams[0] = new SqlParameter("@row_id", SqlDbType.Int, 8);
            sqlParams[1] = new SqlParameter("@city", SqlDbType.VarChar, 50);

            using (var transaction = new TransactionScope())
            {
                using (command.Connection)
                {
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();

                    foreach (var item in tasks)
                    {
                        var data = new City();
                        command.Parameters.Clear();

                        sqlParams[0].Value = (object)item.RowId;
                        sqlParams[1].Value = (object)item.CityName;
                        command.Parameters.Add(sqlParams[0]);
                        command.Parameters.Add(sqlParams[1]);
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
        public IList<City> DeleteCity(List<City> tasks)
        {
            var result = new List<City>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_City_d";
            SqlParameter[] sqlParams = new SqlParameter[1];
            sqlParams[0] = new SqlParameter("@row_id", SqlDbType.Int, 8);

            using (var transaction = new TransactionScope())
            {
                using (command.Connection)
                {
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();

                    foreach (var item in tasks)
                    {
                        var data = new City();
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
        public List<City> GetCityLookup(int StateId)
        {
            List<City> GridRecords = new List<City>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_City_Lookup";
            command.AddParameter("@stateId", StateId, DbType.String, 50);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                City GridRecord = new City();
                if (!reader.IsDBNull(reader.GetOrdinal("row_id")))
                    GridRecord.RowId = reader.GetInt32(reader.GetOrdinal("row_id"));
                if (!reader.IsDBNull(reader.GetOrdinal("city_name")))
                    GridRecord.CityName = reader.GetString(reader.GetOrdinal("city_name"));
                GridRecords.Add(GridRecord);
            }
            command.CloseConnection();
            return GridRecords;
        }
        public List<City> GetCity(int CountryId, int StateId)
        {
            List<City> GridRecords = new List<City>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_City_GetAll";
            SqlParameter[] sqlParams = new SqlParameter[2];
            sqlParams[0] = new SqlParameter("@countryId", SqlDbType.Int, 8);
            sqlParams[1] = new SqlParameter("@stateId", SqlDbType.Int, 8);
            sqlParams[0].Value = CountryId;
            sqlParams[1].Value = StateId;
            command.Parameters.Add(sqlParams[0]);
            command.Parameters.Add(sqlParams[1]);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                City GridRecord = new City();
                if (!reader.IsDBNull(reader.GetOrdinal("row_id")))
                    GridRecord.RowId = reader.GetInt32(reader.GetOrdinal("row_id"));
                if (!reader.IsDBNull(reader.GetOrdinal("city_name")))
                    GridRecord.CityName = reader.GetString(reader.GetOrdinal("city_name"));
                if (!reader.IsDBNull(reader.GetOrdinal("state_name")))
                    GridRecord.StateName = reader.GetString(reader.GetOrdinal("state_name"));
                if (!reader.IsDBNull(reader.GetOrdinal("country_name")))
                    GridRecord.CountryName = reader.GetString(reader.GetOrdinal("country_name"));
                GridRecords.Add(GridRecord);
            }
            command.CloseConnection();
            return GridRecords;
        }
        #endregion

        #region Menu
        public bool AddMenu(Menu menu)
        {
            int result;
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_menu_i";
            command.AddParameter("@menu_name", menu.MenuName, DbType.String, 50);
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

        public IList<Menu> UpdateMenu(List<Menu> tasks)
        {
            var result = new List<Menu>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_menu_u";
            SqlParameter[] sqlParams = new SqlParameter[3];
            sqlParams[0] = new SqlParameter("@row_id", SqlDbType.Int, 8);
            sqlParams[1] = new SqlParameter("@menu_name", SqlDbType.VarChar, 50);
            sqlParams[2] = new SqlParameter("@icon_name", SqlDbType.VarChar, 50);

            using (var transaction = new TransactionScope())
            {
                using (command.Connection)
                {
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();

                    foreach (var item in tasks)
                    {
                        var data = new Menu();
                        command.Parameters.Clear();

                        sqlParams[0].Value = (object)item.RowId;
                        sqlParams[1].Value = (object)item.MenuName;
                        sqlParams[2].Value = (object)item.IconName;
                        command.Parameters.Add(sqlParams[0]);
                        command.Parameters.Add(sqlParams[1]);
                        command.Parameters.Add(sqlParams[2]);
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

        public IList<Menu> DeleteMenu(List<Menu> tasks)
        {
            var result = new List<Menu>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_Menu_d";
            SqlParameter[] sqlParams = new SqlParameter[1];
            sqlParams[0] = new SqlParameter("@row_id", SqlDbType.Int, 8);

            using (var transaction = new TransactionScope())
            {
                using (command.Connection)
                {
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();

                    foreach (var item in tasks)
                    {
                        var data = new Menu();
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

        public List<Menu> GetMenu(string SearchStr)
        {
            List<Menu> GridRecords = new List<Menu>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_menu_Get";
            command.AddParameter("@searchStr", SearchStr, DbType.String, 50);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                Menu GridRecord = new Menu();
                if (!reader.IsDBNull(reader.GetOrdinal("row_id")))
                    GridRecord.RowId = reader.GetInt32(reader.GetOrdinal("row_id"));
                if (!reader.IsDBNull(reader.GetOrdinal("menu_name")))
                    GridRecord.MenuName = reader.GetString(reader.GetOrdinal("menu_name"));
                if (!reader.IsDBNull(reader.GetOrdinal("icon_name")))
                    GridRecord.IconName = reader.GetString(reader.GetOrdinal("icon_name"));
                GridRecords.Add(GridRecord);
            }
            command.CloseConnection();
            return GridRecords;
        }
        #endregion

        #region Sub-Menu
        public bool AddSubMenu(SubMenu subMenu)
        {
            int result;
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_sub_menu_i";
            command.AddParameter("@menu_id", subMenu.MenuId, DbType.Int32, 8);
            command.AddParameter("@sub_menu_name", subMenu.SubMenuName, DbType.String, 50);
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

        public IList<SubMenu> UpdateSubMenu(List<SubMenu> tasks)
        {
            var result = new List<SubMenu>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_sub_menu_u";
            SqlParameter[] sqlParams = new SqlParameter[2];
            sqlParams[0] = new SqlParameter("@row_id", SqlDbType.Int, 8);
            sqlParams[1] = new SqlParameter("@sub_menu_name", SqlDbType.VarChar, 50);

            using (var transaction = new TransactionScope())
            {
                using (command.Connection)
                {
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();

                    foreach (var item in tasks)
                    {
                        var data = new SubMenu();
                        command.Parameters.Clear();

                        sqlParams[0].Value = (object)item.RowId;
                        sqlParams[1].Value = (object)item.SubMenuName;
                        command.Parameters.Add(sqlParams[0]);
                        command.Parameters.Add(sqlParams[1]);
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

        public IList<SubMenu> DeleteSubMenu(List<SubMenu> tasks)
        {
            var result = new List<SubMenu>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_Sub_Menu_d";
            SqlParameter[] sqlParams = new SqlParameter[1];
            sqlParams[0] = new SqlParameter("@row_id", SqlDbType.Int, 8);

            using (var transaction = new TransactionScope())
            {
                using (command.Connection)
                {
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();

                    foreach (var item in tasks)
                    {
                        var data = new SubMenu();
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

        public List<SubMenu> GetSubMenu(string SearchStr)
        {
            List<SubMenu> GridRecords = new List<SubMenu>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_sub_menu_Get";
            command.AddParameter("@searchStr", SearchStr, DbType.String, 50);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                SubMenu GridRecord = new SubMenu();
                if (!reader.IsDBNull(reader.GetOrdinal("row_id")))
                    GridRecord.RowId = reader.GetInt32(reader.GetOrdinal("row_id"));
                if (!reader.IsDBNull(reader.GetOrdinal("menu_id")))
                    GridRecord.MenuId = reader.GetInt32(reader.GetOrdinal("menu_id"));
                if (!reader.IsDBNull(reader.GetOrdinal("menu_name")))
                    GridRecord.MenuName = reader.GetString(reader.GetOrdinal("menu_name"));
                if (!reader.IsDBNull(reader.GetOrdinal("sub_menu_name")))
                    GridRecord.SubMenuName = reader.GetString(reader.GetOrdinal("sub_menu_name"));
                GridRecords.Add(GridRecord);
            }
            command.CloseConnection();
            return GridRecords;
        }
        #endregion

        #region Subscription
        public List<Subscription> GetSubscriptionList()
        {
            List<Subscription> GridRecords = new List<Subscription>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_Subscription_Get";
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                Subscription GridRecord = new Subscription()
                {
                    RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
                    SubscriptionName = reader.ValidateColumnExistExtractAndCastTo<string>("SubscriptionName"),
                };
                GridRecords.Add(GridRecord);
            };
            command.CloseConnection();
            return GridRecords;
        }
        #endregion
    }
}
