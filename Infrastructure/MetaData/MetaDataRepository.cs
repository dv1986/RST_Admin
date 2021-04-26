using Infrastructure.Grid;
using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Infrastructure.MetaData
{
    public class MetaDataRepository : RepositoryBase<ColumnMetaData>, IMetaDataRepository
    {
        public MetaDataRepository(IDataContext context) : base(context)
        {
        }

        protected override void Map(IDataRecord record, Dictionary<string, int> columnsOrdinal, ColumnMetaData item)
        {

            item.TableName = record.GetString(columnsOrdinal["TableName"]);
            item.ColumnName = record.GetString(columnsOrdinal["ColumnName"]).Trim();
            item.Name = record.GetString(columnsOrdinal["COSName"]);
            item.Title = record.GetString(columnsOrdinal["COSTitle"]);
            item.DataType = record.GetString(columnsOrdinal["DataType"]);
            item.ColumnStyle = record.GetString(columnsOrdinal["ColumnStyle"]);
            item.Aggregate = record.GetString(columnsOrdinal["Aggregate"]);
            item.ColumnFormat = record.GetString(columnsOrdinal["ColumnFormat"]);
            item.AllowZero = record.GetString(columnsOrdinal["AllowZero"]);
        }

        protected override Dictionary<string, int> GetColumnOrdinal(IDataRecord record)
        {
            return new Dictionary<string, int>
            {
                {"TableName", record.GetOrdinal("TableName")},
                {"ColumnName", record.GetOrdinal("ColumnName")},
                {"COSName", record.GetOrdinal("COSName")},
                {"COSTitle", record.GetOrdinal("COSTitle")},
                {"DataType", record.GetOrdinal("DataType")},
                {"ColumnStyle", record.GetOrdinal("ColumnStyle")},
                {"Aggregate", record.GetOrdinal("Aggregate")},
                {"ColumnFormat", record.GetOrdinal("ColumnFormat")},
                {"AllowZero", record.GetOrdinal("AllowZero")}
            };
        }

        public List<ColumnMetaData> GetColumnsMetaData()
        {
            var result = new List<ColumnMetaData>();
            var command = Context.CreateCommand();
            command.CommandText = "SELECT TableName, ColumnName, COSName, COSTitle, DataType,ColumnStyle,Aggregate,ColumnFormat,AllowZero  FROM dbo.COSMetaData";
            command.CommandType = CommandType.Text;
            if (command.Connection.State != ConnectionState.Open)
                command.Connection.Open();
            IDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
            var columnOrdinal = GetColumnOrdinal(reader);
            while (reader.Read())
            {
                var item = new ColumnMetaData();
                Map(reader, columnOrdinal, item);
                result.Add(item);
            }
            reader.Close();
            return result;
        }

        public void UpdateColumnMetaData(List<ColumnMetaData> data)
        {
            var unitofWork = Context.CreateUnitOfWork();
            try
            {
                foreach (var metaData in data)
                {
                    SetMetaData(metaData);
                }
                unitofWork.Complete();
            }
            finally
            {
                unitofWork.Dispose();
            }

        }

        private void SetMetaData(ColumnMetaData data)
        {
            var command = Context.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = @"IF EXISTS(SELECT Id FROM dbo.COSMetaData WHERE TableName=@TableName AND ColumnName = @ColumnName)
                UPDATE dbo.COSMetaData SET COSName = @Name, COSTitle=@Title, ColumnStyle=@ColumnStyle,Aggregate=@Aggregate,ColumnFormat=@ColumnFormat,AllowZero=@AllowZero WHERE TableName=@TableName AND ColumnName = @ColumnName
                ELSE
                INSERT INTO dbo.COSMetaData
                (
                    TableName,
                    ColumnName,
                    DataType,
                    COSName,
                    COSTitle,
                    PossibleValuesQuery,
                    PossibleValuesQueryType,
                    PossibleValues,
                    CreationDateTime,
                    LastModificationDateTime,
                    ModificationCount,
                    LastModifiedBy,
                    IsDynamicColumn,
                    ColumnStyle,
                    Aggregate,
                    ColumnFormat,
                    AllowZero
                )
                VALUES
                (   @TableName,        -- TableName - varchar(150)
                    @ColumnName,       -- ColumnName - nvarchar(100)
                    N'',       -- DataType - nvarchar(100)
                    @Name,       -- COSName - nvarchar(max)
                    @Title,       -- COSTitle - nvarchar(max)
                    N'',       -- PossibleValuesQuery - nvarchar(150)
                    N'',       -- PossibleValuesQueryType - nvarchar(30)
                    N'',       -- PossibleValues - nvarchar(max)
                    GETDATE(), -- CreationDateTime - datetime
                    GETDATE(), -- LastModificationDateTime - datetime
                    0,         -- ModificationCount - int
                    N'',       -- LastModifiedBy - nvarchar(100)
                    1,       -- IsDynamicColumn - bit
                    @ColumnStyle,
                    @Aggregate,
                    @ColumnFormat,
                    @AllowZero
                    )";
            command.AddParameter("@Name", (data.Name == data.Key) ? "" : data.Name, DbType.String);
            command.AddParameter("@Title", data.Title, DbType.String);
            command.AddParameter("@TableName", data.TableName, DbType.String);
            command.AddParameter("@ColumnName", data.ColumnName, DbType.String);
            command.AddParameter("@ColumnStyle", data.ColumnStyle == null ? "" : data.ColumnStyle, DbType.String);
            command.AddParameter("@Aggregate", data.Aggregate == null ? "" : data.Aggregate, DbType.String);
            command.AddParameter("@ColumnFormat", data.ColumnFormat == null ? "" : data.ColumnFormat, DbType.String);
            command.AddParameter("@AllowZero", data.AllowZero == null ? "N" : data.AllowZero, DbType.String);
            if (command.Connection.State != ConnectionState.Open)
            {
                command.Connection.Open();
            }
            command.ExecuteNonQuery();
            command.CloseConnection();
        }

        public string GetGridState(string userName, string gridKey, IDataContext conext)
        {
            var command = conext.CreateCommand();
            command.CommandText = "SELECT columns_state FROM cosbcs_grid_states WHERE username = @username AND grid_key = @grid_key";
            command.CommandType = CommandType.Text;
            command.AddParameter("@username", userName, DbType.String);
            command.AddParameter("@grid_key", gridKey, DbType.String);
            if (command.Connection.State != ConnectionState.Open)
                command.Connection.Open();

            var result = command.ExecuteScalar();

            command.CloseConnection();

            if (result != null && !Convert.IsDBNull(result))
            {
                return result.ToString();
            }
            return String.Empty;
        }
        public bool ResetGridState(string userName, string gridKey, IDataContext conext)
        {
            string key = gridKey.Substring(gridKey.IndexOf("_"));
            var command = conext.CreateCommand();
            command.CommandText = "Delete FROM cosbcs_grid_states WHERE SUBSTRING(grid_key,CHARINDEX('_',grid_key),LEN(grid_key)) = @grid_key";
            command.CommandType = CommandType.Text;
            command.AddParameter("@username", userName, DbType.String);
            command.AddParameter("@grid_key", key, DbType.String);
            if (command.Connection.State != ConnectionState.Open)
                command.Connection.Open();

            int res = command.ExecuteNonQuery();

            command.CloseConnection();

            if (res > 0)
                return true;
            return false;
        }
        public void SetGridState(string userName, string gridKey, string columnsState, IDataContext conext)
        {

            var command = conext.CreateCommand();
            command.CommandText = @"IF EXISTS (SELECT columns_state FROM dbo.cosbcs_grid_states WHERE username = @username AND grid_key = @grid_key)
                                    BEGIN

                                    UPDATE dbo.cosbcs_grid_states
                                    SET columns_state = @columns_state
                                    WHERE username = @username AND grid_key = @grid_key
                                    END
                                    ELSE
                                    BEGIn

                                    INSERT	dbo.cosbcs_grid_states
                                    (
                                        grid_key,
                                        username,
                                        columns_state
                                    )
                                    VALUES
                                    (   @grid_key, -- grid_key - nvarchar(50)
                                       @username, -- user_name - nvarchar(50)
                                        @columns_state  -- column_state - nvarchar(max)
                                        )
	                                    END";
            command.CommandType = CommandType.Text;
            command.AddParameter("@username", userName, DbType.String);
            command.AddParameter("@grid_key", gridKey, DbType.String);
            command.AddParameter("@columns_state", columnsState, DbType.String);
            if (command.Connection.State != ConnectionState.Open)
                command.Connection.Open();

            var result = command.ExecuteNonQuery();

            command.CloseConnection();

        }
    }
}
