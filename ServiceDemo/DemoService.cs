using Infrastructure.Repository;
using ModelDemo;
using ServiceHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ServiceDemo
{
    public class DemoService : BaseService, IDemoService
    {
        readonly IDataContext dbContext;
        public DemoService(IDataContext context)
        {
            dbContext = context;
        }

        public List<DemoDTO> GetAll(string searchString)
        {
            searchString = searchString == null ? "" : searchString;
            List<DemoDTO> GridRecords = new List<DemoDTO>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_Get_All";
            command.AddParameter("@searchString", searchString, DbType.String, 16);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                DemoDTO GridRecord = new DemoDTO();
                if (!reader.IsDBNull(reader.GetOrdinal("row_id")))
                    GridRecord.RowId = reader.GetInt32(reader.GetOrdinal("row_id"));
                if (!reader.IsDBNull(reader.GetOrdinal("name")))
                    GridRecord.Name = reader.GetString(reader.GetOrdinal("name"));
                if (!reader.IsDBNull(reader.GetOrdinal("email_field")))
                    GridRecord.Email = reader.GetString(reader.GetOrdinal("email_field"));
                if (!reader.IsDBNull(reader.GetOrdinal("date_field")))
                    GridRecord.DateField = reader.GetDateTime(reader.GetOrdinal("date_field"));
                if (!reader.IsDBNull(reader.GetOrdinal("decimal_field")))
                    GridRecord.Decimal2Digit = reader.GetDecimal(reader.GetOrdinal("decimal_field"));
                if (!reader.IsDBNull(reader.GetOrdinal("decimal4_field")))
                    GridRecord.Decimal4Digit = reader.GetDecimal(reader.GetOrdinal("decimal4_field"));
                if (!reader.IsDBNull(reader.GetOrdinal("number_field")))
                    GridRecord.NumberField = reader.GetInt32(reader.GetOrdinal("number_field"));
                if (!reader.IsDBNull(reader.GetOrdinal("phone")))
                    GridRecord.Phone = reader.GetString(reader.GetOrdinal("phone"));
                if (!reader.IsDBNull(reader.GetOrdinal("currency_field")))
                    GridRecord.CurrencyField = reader.GetDecimal(reader.GetOrdinal("currency_field"));
                GridRecord.Percentage = GridRecord.Decimal2Digit / 100;
                GridRecord.PercentageDecimal = GridRecord.Decimal2Digit / 100;
                GridRecords.Add(GridRecord);
            }
            command.CloseConnection();
            return GridRecords;
        }
    }
}
