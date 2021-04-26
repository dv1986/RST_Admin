
using Infrastructure.Repository;
using ModelSpecifications;
using ServiceHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace ServiceSpecification
{
    public class SpecificationService : BaseService, ISpecificationService
    {
        readonly IDataContext dbContext;
        public SpecificationService(IDataContext context)
        {
            dbContext = context;
        }

		#region Colors
		public List<Colors> GetColors(string QueryConditionPartParam)
		{
			List<Colors> GridRecords = new List<Colors>();
			var command = dbContext.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "sp_Colors_Get";
			command.OpenConnection();
			var reader = command.OpenReader();
			while (reader.Read())
			{
				Colors GridRecord = new Colors()
				{
					RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
					ColorName = reader.ValidateColumnExistExtractAndCastTo<string>("ColorName"),
					ColorCodeRGB = reader.ValidateColumnExistExtractAndCastTo<string>("ColorCodeRGB"),
					ColorCodeHex = reader.ValidateColumnExistExtractAndCastTo<string>("ColorCodeHex"),
				};
				GridRecords.Add(GridRecord);
			};
			command.CloseConnection();
			return GridRecords;
		}
		public bool AddColors(Colors request)
		{
			int result;
			var command = dbContext.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "sp_Colors_i";
			command.AddParameter("@ColorName", request.ColorName, DbType.String);
			command.AddParameter("@ColorCodeRGB", request.ColorCodeRGB, DbType.String);
			command.AddParameter("@ColorCodeHex", request.ColorCodeHex, DbType.String);
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
		public IList<Colors> UpdateColors(List<Colors> tasks)
		{
			var result = new List<Colors>();
			var command = dbContext.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "sp_Colors_u";
			SqlParameter[] sqlParams = new SqlParameter[4];
			sqlParams[0] = new SqlParameter("@RowId", DbType.Int32);
			sqlParams[1] = new SqlParameter("@ColorName", DbType.String);
			sqlParams[2] = new SqlParameter("@ColorCodeRGB", DbType.String);
			sqlParams[3] = new SqlParameter("@ColorCodeHex", DbType.String);
			using (var transaction = new TransactionScope())
			{
				using (command.Connection)
				{
					if (command.Connection.State == ConnectionState.Closed)
						command.Connection.Open();
					foreach (var item in tasks)
					{
						var data = new Colors();
						command.Parameters.Clear();
						sqlParams[0].Value = (object)item.RowId;
						sqlParams[1].Value = (object)item.ColorName;
						sqlParams[2].Value = (object)item.ColorCodeRGB;
						sqlParams[3].Value = (object)item.ColorCodeHex;
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
		public IList<Colors> DeleteColors(List<Colors> tasks)
		{
			var result = new List<Colors>();
			var command = dbContext.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "sp_Colors_d";
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
						var data = new Colors();
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
		#endregion


		#region MeasureDimension
		public List<MeasureDimension> GetMeasureDimension(string QueryConditionPartParam)
		{
			List<MeasureDimension> GridRecords = new List<MeasureDimension>();
			var command = dbContext.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "sp_MeasureDimension_Get";
			command.OpenConnection();
			var reader = command.OpenReader();
			while (reader.Read())
			{
				MeasureDimension GridRecord = new MeasureDimension()
				{
					RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
					Name = reader.ValidateColumnExistExtractAndCastTo<string>("Name"),
					SystemKeyword = reader.ValidateColumnExistExtractAndCastTo<string>("SystemKeyword"),
					Ratio = reader.ValidateColumnExistExtractAndCastTo<string>("Ratio"),
					DisplayOrder = reader.ValidateColumnExistExtractAndCastTo<int>("DisplayOrder"),
				};
				GridRecords.Add(GridRecord);
			};
			command.CloseConnection();
			return GridRecords;
		}
		public bool AddMeasureDimension(MeasureDimension request)
		{
			int result;
			var command = dbContext.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "sp_MeasureDimension_i";
			command.AddParameter("@Name", request.Name, DbType.String);
			command.AddParameter("@SystemKeyword", request.SystemKeyword, DbType.String);
			command.AddParameter("@Ratio", Convert.ToDecimal(request.Ratio), DbType.Decimal);
			command.AddParameter("@DisplayOrder", Convert.ToInt32(request.DisplayOrder), DbType.Int32);
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
		public IList<MeasureDimension> UpdateMeasureDimension(List<MeasureDimension> tasks)
		{
			var result = new List<MeasureDimension>();
			var command = dbContext.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "sp_MeasureDimension_u";
			SqlParameter[] sqlParams = new SqlParameter[5];
			sqlParams[0] = new SqlParameter("@RowId", DbType.Int32);
			sqlParams[1] = new SqlParameter("@Name", DbType.String);
			sqlParams[2] = new SqlParameter("@SystemKeyword", DbType.String);
			sqlParams[3] = new SqlParameter("@Ratio", DbType.Decimal);
			sqlParams[4] = new SqlParameter("@DisplayOrder", DbType.Int32);
			using (var transaction = new TransactionScope())
			{
				using (command.Connection)
				{
					if (command.Connection.State == ConnectionState.Closed)
						command.Connection.Open();
					foreach (var item in tasks)
					{
						var data = new MeasureDimension();
						command.Parameters.Clear();
						sqlParams[0].Value = (object)item.RowId;
						sqlParams[1].Value = (object)item.Name;
						sqlParams[2].Value = (object)item.SystemKeyword;
						sqlParams[3].Value = (object)item.Ratio;
						sqlParams[4].Value = (object)item.DisplayOrder;
						command.Parameters.Add(sqlParams[0]);
						command.Parameters.Add(sqlParams[1]);
						command.Parameters.Add(sqlParams[2]);
						command.Parameters.Add(sqlParams[3]);
						command.Parameters.Add(sqlParams[4]);
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
		public IList<MeasureDimension> DeleteMeasureDimension(List<MeasureDimension> tasks)
		{
			var result = new List<MeasureDimension>();
			var command = dbContext.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "sp_MeasureDimension_d";
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
						var data = new MeasureDimension();
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
		#endregion


		#region ProductFabric
		public List<ProductFabric> GetProductFabric(string QueryConditionPartParam)
		{
			List<ProductFabric> GridRecords = new List<ProductFabric>();
			var command = dbContext.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "sp_ProductFabric_Get";
			command.OpenConnection();
			var reader = command.OpenReader();
			while (reader.Read())
			{
				ProductFabric GridRecord = new ProductFabric()
				{
					RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
					FabricName = reader.ValidateColumnExistExtractAndCastTo<string>("FabricName"),
					Description = reader.ValidateColumnExistExtractAndCastTo<string>("Description"),
				};
				GridRecords.Add(GridRecord);
			};
			command.CloseConnection();
			return GridRecords;
		}
		public bool AddProductFabric(ProductFabric request)
		{
			int result;
			var command = dbContext.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "sp_ProductFabric_i";
			command.AddParameter("@FabricName", request.FabricName, DbType.String);
			command.AddParameter("@Description", request.Description, DbType.String);
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
		public IList<ProductFabric> UpdateProductFabric(List<ProductFabric> tasks)
		{
			var result = new List<ProductFabric>();
			var command = dbContext.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "sp_ProductFabric_u";
			SqlParameter[] sqlParams = new SqlParameter[3];
			sqlParams[0] = new SqlParameter("@RowId", DbType.Int32);
			sqlParams[1] = new SqlParameter("@FabricName", DbType.String);
			sqlParams[2] = new SqlParameter("@Description", DbType.String);
			using (var transaction = new TransactionScope())
			{
				using (command.Connection)
				{
					if (command.Connection.State == ConnectionState.Closed)
						command.Connection.Open();
					foreach (var item in tasks)
					{
						var data = new ProductFabric();
						command.Parameters.Clear();
						sqlParams[0].Value = (object)item.RowId;
						sqlParams[1].Value = (object)item.FabricName;
						sqlParams[2].Value = (object)item.Description;
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
		public IList<ProductFabric> DeleteProductFabric(List<ProductFabric> tasks)
		{
			var result = new List<ProductFabric>();
			var command = dbContext.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "sp_ProductFabric_d";
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
						var data = new ProductFabric();
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
		#endregion


		#region ProductTag
		public List<ProductTag> GetProductTag(string QueryConditionPartParam)
		{
			List<ProductTag> GridRecords = new List<ProductTag>();
			var command = dbContext.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "sp_ProductTag_Get";
			command.OpenConnection();
			var reader = command.OpenReader();
			while (reader.Read())
			{
				ProductTag GridRecord = new ProductTag()
				{
					RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
					Name = reader.ValidateColumnExistExtractAndCastTo<string>("Name"),
				};
				GridRecords.Add(GridRecord);
			};
			command.CloseConnection();
			return GridRecords;
		}
		public bool AddProductTag(ProductTag request)
		{
			int result;
			var command = dbContext.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "sp_ProductTag_i";
			command.AddParameter("@Name", request.Name, DbType.String);
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
		public IList<ProductTag> UpdateProductTag(List<ProductTag> tasks)
		{
			var result = new List<ProductTag>();
			var command = dbContext.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "sp_ProductTag_u";
			SqlParameter[] sqlParams = new SqlParameter[2];
			sqlParams[0] = new SqlParameter("@RowId", DbType.Int32);
			sqlParams[1] = new SqlParameter("@Name", DbType.String);
			using (var transaction = new TransactionScope())
			{
				using (command.Connection)
				{
					if (command.Connection.State == ConnectionState.Closed)
						command.Connection.Open();
					foreach (var item in tasks)
					{
						var data = new ProductTag();
						command.Parameters.Clear();
						sqlParams[0].Value = (object)item.RowId;
						sqlParams[1].Value = (object)item.Name;
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
		public IList<ProductTag> DeleteProductTag(List<ProductTag> tasks)
		{
			var result = new List<ProductTag>();
			var command = dbContext.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "sp_ProductTag_d";
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
						var data = new ProductTag();
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
		#endregion

		/// <summary>
		/// Size Type
		/// </summary>
		#region Size Type
		public List<ProductSizeType> GetProductSizeType(string QueryConditionPartParam)
		{
			List<ProductSizeType> GridRecords = new List<ProductSizeType>();
			var command = dbContext.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "sp_ProductSizeType_Get";
			command.OpenConnection();
			var reader = command.OpenReader();
			while (reader.Read())
			{
				ProductSizeType GridRecord = new ProductSizeType()
				{
					RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
					TypeName = reader.ValidateColumnExistExtractAndCastTo<string>("TypeName"),
					TypeCode = reader.ValidateColumnExistExtractAndCastTo<string>("TypeCode"),
					MeasureDimensionId = reader.ValidateColumnExistExtractAndCastTo<int>("MeasureDimensionId"),
					CreatedOnUtc = reader.ValidateColumnExistExtractAndCastTo<DateTime>("CreatedOnUtc"),
					Description = reader.ValidateColumnExistExtractAndCastTo<string>("Description"),
					MeasurementDimension = reader.ValidateColumnExistExtractAndCastTo<string>("MeasurementDimension"),
				};
				GridRecords.Add(GridRecord);
			};
			command.CloseConnection();
			return GridRecords;
		}
		public bool AddProductSizeType(ProductSizeType request)
		{
			int result;
			var command = dbContext.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "sp_ProductSizeType_i";
			command.AddParameter("@TypeName", request.TypeName, DbType.String);
			command.AddParameter("@TypeCode", request.TypeCode, DbType.String);
			command.AddParameter("@MeasureDimensionId", request.MeasureDimensionId, DbType.Int32);
			command.AddParameter("@Description", request.Description, DbType.String);
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
		public IList<ProductSizeType> UpdateProductSizeType(List<ProductSizeType> tasks)
		{
			var result = new List<ProductSizeType>();
			var command = dbContext.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "sp_ProductSizeType_u";
			SqlParameter[] sqlParams = new SqlParameter[5];
			sqlParams[0] = new SqlParameter("@RowId", DbType.Int32);
			sqlParams[1] = new SqlParameter("@TypeName", DbType.String);
			sqlParams[2] = new SqlParameter("@TypeCode", DbType.String);
			sqlParams[3] = new SqlParameter("@MeasureDimensionId", DbType.Int32);
			sqlParams[4] = new SqlParameter("@Description", DbType.String);
			using (var transaction = new TransactionScope())
			{
				using (command.Connection)
				{
					if (command.Connection.State == ConnectionState.Closed)
						command.Connection.Open();
					foreach (var item in tasks)
					{
						var data = new ProductSizeType();
						command.Parameters.Clear();
						sqlParams[0].Value = (object)item.RowId;
						sqlParams[1].Value = (object)item.TypeName;
						sqlParams[2].Value = (object)item.TypeCode;
						sqlParams[3].Value = (object)item.MeasureDimensionId;
						sqlParams[4].Value = (object)item.Description;
						command.Parameters.Add(sqlParams[0]);
						command.Parameters.Add(sqlParams[1]);
						command.Parameters.Add(sqlParams[2]);
						command.Parameters.Add(sqlParams[3]);
						command.Parameters.Add(sqlParams[4]);
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
		public IList<ProductSizeType> DeleteProductSizeType(List<ProductSizeType> tasks)
		{
			var result = new List<ProductSizeType>();
			var command = dbContext.CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "sp_ProductSizeType_d";
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
						var data = new ProductSizeType();
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
		#endregion
	}
}
