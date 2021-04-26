using Infrastructure.Repository;
using ModelCategories;
using ModelCommon;
using ServiceHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;

namespace ServiceCategories
{
    public class CategoriesService : BaseService, ICategoriesService
    {
        readonly IDataContext dbContext;
        public CategoriesService(IDataContext context)
        {
            dbContext = context;
        }

        #region Category Parent
        public bool AddProductCategoryParent(ProductCategoryParent ProductCategoryParent)
        {
            int result;
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductCategoryParent_i";
            command.AddParameter("@CatParentName", ProductCategoryParent.CatParentName, DbType.String, 50);
            command.AddParameter("@Description", ProductCategoryParent.Description, DbType.String, 50);
            command.AddParameter("@SeoContentId", ProductCategoryParent.SeoContentId, DbType.Int32, 8);
            command.AddParameter("@IncludeInTopMenu", ProductCategoryParent.IncludeInTopMenu, DbType.Boolean);
            command.AddParameter("@IsNew", ProductCategoryParent.IsNew, DbType.Boolean);
            command.AddParameter("@HasDiscountApplied", ProductCategoryParent.HasDiscountApplied, DbType.Boolean);
            command.AddParameter("@IsPublished", ProductCategoryParent.IsPublished, DbType.Boolean);
            command.AddParameter("@DisplayOrder", ProductCategoryParent.DisplayOrder, DbType.Int32);
            command.AddParameter("@ProductImageId", ProductCategoryParent.ProductImageId, DbType.Int32, 8);
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
        public IList<ProductCategoryParent> UpdateProductCategoryParent(List<ProductCategoryParent> tasks)
        {
            var result = new List<ProductCategoryParent>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductCategoryParent_u";
            SqlParameter[] sqlParams = new SqlParameter[12];
            sqlParams[0] = new SqlParameter("@RowId", DbType.Int32);
            sqlParams[1] = new SqlParameter("@CatParentName", DbType.String);
            sqlParams[2] = new SqlParameter("@Description", DbType.String);
            sqlParams[3] = new SqlParameter("@SeoContentId", DbType.Int32);
            sqlParams[4] = new SqlParameter("@IncludeInTopMenu", DbType.Boolean);
            sqlParams[5] = new SqlParameter("@IsNew", DbType.Boolean);
            sqlParams[6] = new SqlParameter("@HasDiscountApplied", DbType.Boolean);
            sqlParams[7] = new SqlParameter("@IsPublished", DbType.Boolean);
            sqlParams[8] = new SqlParameter("@DisplayOrder", DbType.Int32);
            sqlParams[9] = new SqlParameter("@CreatedOnUtc", DbType.DateTime);
            sqlParams[10] = new SqlParameter("@ModifiedOnUtc", DbType.DateTime);
            sqlParams[11] = new SqlParameter("@ProductImageId", DbType.Int32);
            using (var transaction = new TransactionScope())
            {
                using (command.Connection)
                {
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();
                    foreach (var item in tasks)
                    {
                        var data = new ProductCategoryParent();
                        command.Parameters.Clear();
                        sqlParams[0].Value = (object)item.RowId;
                        sqlParams[1].Value = (object)item.CatParentName;
                        sqlParams[2].Value = (object)item.Description;
                        sqlParams[3].Value = (object)item.SeoContentId;
                        sqlParams[4].Value = (object)item.IncludeInTopMenu;
                        sqlParams[5].Value = (object)item.IsNew;
                        sqlParams[6].Value = (object)item.HasDiscountApplied;
                        sqlParams[7].Value = (object)item.IsPublished;
                        sqlParams[8].Value = (object)item.DisplayOrder;
                        sqlParams[9].Value = (object)item.CreatedOnUtc;
                        sqlParams[10].Value = (object)item.ModifiedOnUtc;
                        sqlParams[11].Value = (object)item.ProductImageId;
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
        public IList<ProductCategoryParent> DeleteProductCategoryParent(List<ProductCategoryParent> tasks)
        {
            var result = new List<ProductCategoryParent>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductCategoryParent_d";
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
                        var data = new ProductCategoryParent();
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
        public List<ProductCategoryParent> GetProductCategoryParent(string QueryConditionPartParam)
        {
            List<ProductCategoryParent> GridRecords = new List<ProductCategoryParent>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductCategoryParent_Get";
            command.AddParameter("@QueryConditionPartParam", QueryConditionPartParam, DbType.String);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                ProductCategoryParent GridRecord = new ProductCategoryParent()
                {
                    RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
                    CatParentName = reader.ValidateColumnExistExtractAndCastTo<string>("CatParentName"),
                    Description = reader.ValidateColumnExistExtractAndCastTo<string>("Description"),
                    SeoContentId = reader.ValidateColumnExistExtractAndCastTo<int>("SeoContentId"),
                    IncludeInTopMenu = reader.ValidateColumnExistExtractAndCastTo<bool>("IncludeInTopMenu"),
                    IsNew = reader.ValidateColumnExistExtractAndCastTo<bool>("IsNew"),
                    HasDiscountApplied = reader.ValidateColumnExistExtractAndCastTo<bool>("HasDiscountApplied"),
                    IsPublished = reader.ValidateColumnExistExtractAndCastTo<bool>("IsPublished"),
                    IsDeleted = reader.ValidateColumnExistExtractAndCastTo<bool>("IsDeleted"),
                    DisplayOrder = reader.ValidateColumnExistExtractAndCastTo<string>("DisplayOrder"),
                    CreatedOnUtc = reader.ValidateColumnExistExtractAndCastTo<DateTime>("CreatedOnUtc"),
                    ModifiedOnUtc = reader.ValidateColumnExistExtractAndCastTo<DateTime>("ModifiedOnUtc"),
                    ProductImageId = reader.ValidateColumnExistExtractAndCastTo<int>("ProductImageId"),
                    MetaTitle = reader.ValidateColumnExistExtractAndCastTo<string>("MetaTitle"),
                    MetaKeyword = reader.ValidateColumnExistExtractAndCastTo<string>("MetaKeyword"),
                    ImagePath = reader.ValidateColumnExistExtractAndCastTo<string>("ImagePath"),
                    ImageName = reader.ValidateColumnExistExtractAndCastTo<string>("ImageName"),
                    ThumbnailPath = reader.ValidateColumnExistExtractAndCastTo<string>("ThumbnailPath")+ "?" + new DateTime(),
                    //ImageFileStream = Helper.Base64ToImage(reader.ValidateColumnExistExtractAndCastTo<string>("ImagePath")),
                    //ImageFileStream = Helper.GetImage(reader.ValidateColumnExistExtractAndCastTo<string>("ImageName")).ToString(),

                };
                GridRecords.Add(GridRecord);
            };
            command.CloseConnection();
            return GridRecords;
        }
        #endregion

        #region Category
        public bool AddProductCategory(ProductCategory request)
        {
            int result;
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductCategory_i";
            command.AddParameter("@CatName", request.CatName, DbType.String);
            command.AddParameter("@Description", request.Description, DbType.String);
            command.AddParameter("@SeoContentId", request.SeoContentId, DbType.Int32);
            command.AddParameter("@IncludeInTopMenu", request.IncludeInTopMenu, DbType.Boolean);
            command.AddParameter("@IsNew", request.IsNew, DbType.Boolean);
            command.AddParameter("@HasDiscountApplied", request.HasDiscountApplied, DbType.Boolean);
            command.AddParameter("@IsPublished", request.IsPublished, DbType.Boolean);
            command.AddParameter("@IsDeleted", request.IsDeleted, DbType.Boolean);
            command.AddParameter("@ProductImageId", request.ProductImageId, DbType.Int32);
            command.AddParameter("@ProductCategoryParentId", request.ProductCategoryParentId, DbType.Int32);
            command.AddParameter("@DisplayOrder", request.DisplayOrder, DbType.Int32);
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
        public IList<ProductCategory> UpdateProductCategory(List<ProductCategory> tasks)
        {
            var result = new List<ProductCategory>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductCategory_u";
            SqlParameter[] sqlParams = new SqlParameter[12];
            sqlParams[0] = new SqlParameter("@RowId", DbType.Int32);
            sqlParams[1] = new SqlParameter("@CatName", DbType.String);
            sqlParams[2] = new SqlParameter("@Description", DbType.String);
            sqlParams[3] = new SqlParameter("@SeoContentId", DbType.Int32);
            sqlParams[4] = new SqlParameter("@IncludeInTopMenu", DbType.Boolean);
            sqlParams[5] = new SqlParameter("@IsNew", DbType.Boolean);
            sqlParams[6] = new SqlParameter("@HasDiscountApplied", DbType.Boolean);
            sqlParams[7] = new SqlParameter("@IsPublished", DbType.Boolean);
            sqlParams[8] = new SqlParameter("@IsDeleted", DbType.Boolean);
            sqlParams[9] = new SqlParameter("@ProductImageId", DbType.Int32);
            sqlParams[10] = new SqlParameter("@ProductCategoryParentId", DbType.Int32);
            sqlParams[11] = new SqlParameter("@DisplayOrder", DbType.Int32);
            using (var transaction = new TransactionScope())
            {
                using (command.Connection)
                {
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();
                    foreach (var item in tasks)
                    {
                        var data = new ProductCategory();
                        command.Parameters.Clear();
                        sqlParams[0].Value = (object)item.RowId;
                        sqlParams[1].Value = (object)item.CatName;
                        sqlParams[2].Value = (object)item.Description;
                        sqlParams[3].Value = (object)item.SeoContentId;
                        sqlParams[4].Value = (object)item.IncludeInTopMenu;
                        sqlParams[5].Value = (object)item.IsNew;
                        sqlParams[6].Value = (object)item.HasDiscountApplied;
                        sqlParams[7].Value = (object)item.IsPublished;
                        sqlParams[8].Value = (object)item.IsDeleted;
                        sqlParams[9].Value = (object)item.ProductImageId;
                        sqlParams[10].Value = (object)item.ProductCategoryParentId;
                        sqlParams[11].Value = (object)item.DisplayOrder;
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
        public IList<ProductCategory> DeleteProductCategory(List<ProductCategory> tasks)
        {
            var result = new List<ProductCategory>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductCategory_d";
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
                        var data = new ProductCategory();
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
        public List<ProductCategory> GetProductCategory(string QueryConditionPartParam)
        {
            List<ProductCategory> GridRecords = new List<ProductCategory>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductCategory_Get";
            command.AddParameter("@QueryConditionPartParam", QueryConditionPartParam, DbType.String);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                ProductCategory GridRecord = new ProductCategory()
                {
                    RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
                    CatName = reader.ValidateColumnExistExtractAndCastTo<string>("CatName"),
                    Description = reader.ValidateColumnExistExtractAndCastTo<string>("Description"),
                    MetaTitle = reader.ValidateColumnExistExtractAndCastTo<string>("MetaTitle"),
                    MetaKeyword = reader.ValidateColumnExistExtractAndCastTo<string>("MetaKeyword"),
                    SeoContentId = reader.ValidateColumnExistExtractAndCastTo<int>("SeoContentId"),
                    IncludeInTopMenu = reader.ValidateColumnExistExtractAndCastTo<bool>("IncludeInTopMenu"),
                    IsNew = reader.ValidateColumnExistExtractAndCastTo<bool>("IsNew"),
                    HasDiscountApplied = reader.ValidateColumnExistExtractAndCastTo<bool>("HasDiscountApplied"),
                    IsPublished = reader.ValidateColumnExistExtractAndCastTo<bool>("IsPublished"),
                    IsDeleted = reader.ValidateColumnExistExtractAndCastTo<bool>("IsDeleted"),
                    CreatedOnUtc = reader.ValidateColumnExistExtractAndCastTo<DateTime>("CreatedOnUtc"),
                    ModifiedOnUtc = reader.ValidateColumnExistExtractAndCastTo<DateTime>("ModifiedOnUtc"),
                    ProductImageId = reader.ValidateColumnExistExtractAndCastTo<int>("ProductImageId"),
                    ProductCategoryParentId = reader.ValidateColumnExistExtractAndCastTo<int>("ProductCategoryParentId"),
                    DisplayOrder = reader.ValidateColumnExistExtractAndCastTo<string>("DisplayOrder"),
                    CatParentName = reader.ValidateColumnExistExtractAndCastTo<string>("CatParentName"),
                    ImagePath = reader.ValidateColumnExistExtractAndCastTo<string>("ImagePath"),
                    ImageName = reader.ValidateColumnExistExtractAndCastTo<string>("ImageName"),
                    ThumbnailPath = reader.ValidateColumnExistExtractAndCastTo<string>("ThumbnailPath"),
                    //ImageFileStream = Helper.Base64ToImage(reader.ValidateColumnExistExtractAndCastTo<string>("ImagePath")),
                    //ImageFileStream = Helper.GetImage(reader.ValidateColumnExistExtractAndCastTo<string>("ImageName")),
                };
                GridRecords.Add(GridRecord);
            };
            command.CloseConnection();
            return GridRecords;
        }
        public List<ProductCategory> GetCategoryLookup(int CategoryParentId)
        {
            List<ProductCategory> GridRecords = new List<ProductCategory>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_CategoryLookup";
            command.AddParameter("@categoryParentId", CategoryParentId, DbType.String, 50);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                ProductCategory GridRecord = new ProductCategory()
                {
                    RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
                    CatName = reader.ValidateColumnExistExtractAndCastTo<string>("CatName"),
                };
                GridRecords.Add(GridRecord);
            }
            command.CloseConnection();
            return GridRecords;
        }
        #endregion


        #region Sub-Category-Parent
        public bool AddProductSubCategoryParent(ProductSubCategoryParent request)
        {
            int result;
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductSubCategoryParent_i";
            command.AddParameter("@ProductCategoryId", request.ProductCategoryId, DbType.Int32);
            command.AddParameter("@CatName", request.SubCategoryParentName, DbType.String);
            command.AddParameter("@Description", request.Description, DbType.String);
            command.AddParameter("@SeoContentId", request.SeoContentId, DbType.Int32);
            command.AddParameter("@IncludeInTopMenu", request.IncludeInTopMenu, DbType.Boolean);
            command.AddParameter("@IsNew", request.IsNew, DbType.Boolean);
            command.AddParameter("@HasDiscountApplied", request.HasDiscountApplied, DbType.Boolean);
            command.AddParameter("@IsPublished", request.IsPublished, DbType.Boolean);
            command.AddParameter("@DisplayOrder", request.DisplayOrder, DbType.Int32);
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
        public IList<ProductSubCategoryParent> UpdateProductSubCategoryParent(List<ProductSubCategoryParent> tasks)
        {
            var result = new List<ProductSubCategoryParent>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductSubCategoryParent_u";
            SqlParameter[] sqlParams = new SqlParameter[10];
            sqlParams[0] = new SqlParameter("@RowId", DbType.Int32);
            sqlParams[1] = new SqlParameter("@ProductCategoryId", DbType.Int32);
            sqlParams[2] = new SqlParameter("@CatName", DbType.String);
            sqlParams[3] = new SqlParameter("@Description", DbType.String);
            sqlParams[4] = new SqlParameter("@SeoContentId", DbType.Int32);
            sqlParams[5] = new SqlParameter("@IncludeInTopMenu", DbType.Boolean);
            sqlParams[6] = new SqlParameter("@IsNew", DbType.Boolean);
            sqlParams[7] = new SqlParameter("@HasDiscountApplied", DbType.Boolean);
            sqlParams[8] = new SqlParameter("@IsPublished", DbType.Boolean);
            sqlParams[9] = new SqlParameter("@DisplayOrder", DbType.Int32);
            using (var transaction = new TransactionScope())
            {
                using (command.Connection)
                {
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();
                    foreach (var item in tasks)
                    {
                        var data = new ProductSubCategoryParent();
                        command.Parameters.Clear();
                        sqlParams[0].Value = (object)item.RowId;
                        sqlParams[1].Value = (object)item.ProductCategoryId;
                        sqlParams[2].Value = (object)item.SubCategoryParentName;
                        sqlParams[3].Value = (object)item.Description;
                        sqlParams[4].Value = (object)item.SeoContentId;
                        sqlParams[5].Value = (object)item.IncludeInTopMenu;
                        sqlParams[6].Value = (object)item.IsNew;
                        sqlParams[7].Value = (object)item.HasDiscountApplied;
                        sqlParams[8].Value = (object)item.IsPublished;
                        sqlParams[9].Value = (object)item.DisplayOrder;
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
        public IList<ProductSubCategoryParent> DeleteProductSubCategoryParent(List<ProductSubCategoryParent> tasks)
        {
            var result = new List<ProductSubCategoryParent>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductSubCategoryParent_d";
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
                        var data = new ProductSubCategoryParent();
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
        public List<ProductSubCategoryParent> GetProductSubCategoryParent(string QueryConditionPartParam)
        {
            QueryConditionPartParam = QueryConditionPartParam == null ? "" : QueryConditionPartParam;
            List<ProductSubCategoryParent> GridRecords = new List<ProductSubCategoryParent>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductSubCategoryParent_Get";
            command.AddParameter("@QueryConditionPartParam", QueryConditionPartParam, DbType.String);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                ProductSubCategoryParent GridRecord = new ProductSubCategoryParent()
                {
                    RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
                    ProductCategoryId = reader.ValidateColumnExistExtractAndCastTo<int>("ProductCategoryId"),
                    SubCategoryParentName = reader.ValidateColumnExistExtractAndCastTo<string>("SubCategoryParentName"),
                    Description = reader.ValidateColumnExistExtractAndCastTo<string>("Description"),
                    SeoContentId = reader.ValidateColumnExistExtractAndCastTo<int>("SeoContentId"),
                    IncludeInTopMenu = reader.ValidateColumnExistExtractAndCastTo<bool>("IncludeInTopMenu"),
                    IsNew = reader.ValidateColumnExistExtractAndCastTo<bool>("IsNew"),
                    HasDiscountApplied = reader.ValidateColumnExistExtractAndCastTo<bool>("HasDiscountApplied"),
                    IsPublished = reader.ValidateColumnExistExtractAndCastTo<bool>("IsPublished"),
                    IsDeleted = reader.ValidateColumnExistExtractAndCastTo<bool>("IsDeleted"),
                    DisplayOrder = reader.ValidateColumnExistExtractAndCastTo<string>("DisplayOrder"),
                    CreatedOnUtc = reader.ValidateColumnExistExtractAndCastTo<DateTime>("CreatedOnUtc"),
                    ModifiedOnUtc = reader.ValidateColumnExistExtractAndCastTo<DateTime>("ModifiedOnUtc"),
                    CatName = reader.ValidateColumnExistExtractAndCastTo<string>("CatName"),
                    ProductCategoryParentId = reader.ValidateColumnExistExtractAndCastTo<int>("ProductCategoryParentId"),
                    CatParentName = reader.ValidateColumnExistExtractAndCastTo<string>("CatParentName"),
                    MetaTitle = reader.ValidateColumnExistExtractAndCastTo<string>("MetaTitle"),
                    MetaKeyword = reader.ValidateColumnExistExtractAndCastTo<string>("MetaKeyword"),
                    ImageName = reader.ValidateColumnExistExtractAndCastTo<string>("ImageName"),
                    ImagePath = reader.ValidateColumnExistExtractAndCastTo<string>("ImagePath"),
                    ThumbnailPath = reader.ValidateColumnExistExtractAndCastTo<string>("ThumbnailPath"),
                    //ImageFileStream = Helper.Base64ToImage(reader.ValidateColumnExistExtractAndCastTo<string>("ImagePath")),
                };
                GridRecords.Add(GridRecord);
            };
            command.CloseConnection();
            return GridRecords;
        }
        public List<ProductSubCategoryParent> GetSubCategoryParentLookup(int CategoryId)
        {
            List<ProductSubCategoryParent> GridRecords = new List<ProductSubCategoryParent>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_SubCategoryParentLookup";
            command.AddParameter("@categoryId", CategoryId, DbType.String, 50);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                ProductSubCategoryParent GridRecord = new ProductSubCategoryParent()
                {
                    RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
                    CatName = reader.ValidateColumnExistExtractAndCastTo<string>("CatName"),
                };
                GridRecords.Add(GridRecord);
            }
            command.CloseConnection();
            return GridRecords;
        }
        #endregion

        #region Sub-Category
        public bool AddProductSubCategory(ProductSubCategory request)
        {
            int result;
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductSubCategory_i";
            command.AddParameter("@ProductSubCategoryParentId", request.ProductSubCategoryParentId, DbType.Int32);
            command.AddParameter("@SubCategoryName", request.SubCategoryName, DbType.String);
            command.AddParameter("@Description", request.Description, DbType.String);
            command.AddParameter("@SeoContentId", request.SeoContentId, DbType.Int32);
            command.AddParameter("@IncludeInTopMenu", request.IncludeInTopMenu, DbType.Boolean);
            command.AddParameter("@IsNew", request.IsNew, DbType.Boolean);
            command.AddParameter("@HasDiscountApplied", request.HasDiscountApplied, DbType.Boolean);
            command.AddParameter("@IsPublished", request.IsPublished, DbType.Boolean);
            command.AddParameter("@DisplayOrder", request.DisplayOrder, DbType.Int32);
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
        public IList<ProductSubCategory> UpdateProductSubCategory(List<ProductSubCategory> tasks)
        {
            var result = new List<ProductSubCategory>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductSubCategory_u";
            SqlParameter[] sqlParams = new SqlParameter[10];
            sqlParams[0] = new SqlParameter("@RowId", DbType.Int32);
            sqlParams[1] = new SqlParameter("@ProductSubCategoryParentId", DbType.Int32);
            sqlParams[2] = new SqlParameter("@SubCategoryName", DbType.String);
            sqlParams[3] = new SqlParameter("@Description", DbType.String);
            sqlParams[4] = new SqlParameter("@SeoContentId", DbType.Int32);
            sqlParams[5] = new SqlParameter("@IncludeInTopMenu", DbType.Boolean);
            sqlParams[6] = new SqlParameter("@IsNew", DbType.Boolean);
            sqlParams[7] = new SqlParameter("@HasDiscountApplied", DbType.Boolean);
            sqlParams[8] = new SqlParameter("@IsPublished", DbType.Boolean);
            sqlParams[9] = new SqlParameter("@DisplayOrder", DbType.Int32);
            using (var transaction = new TransactionScope())
            {
                using (command.Connection)
                {
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();
                    foreach (var item in tasks)
                    {
                        var data = new ProductSubCategory();
                        command.Parameters.Clear();
                        sqlParams[0].Value = (object)item.RowId;
                        sqlParams[1].Value = (object)item.ProductSubCategoryParentId;
                        sqlParams[2].Value = (object)item.SubCategoryName;
                        sqlParams[3].Value = (object)item.Description;
                        sqlParams[4].Value = (object)item.SeoContentId;
                        sqlParams[5].Value = (object)item.IncludeInTopMenu;
                        sqlParams[6].Value = (object)item.IsNew;
                        sqlParams[7].Value = (object)item.HasDiscountApplied;
                        sqlParams[8].Value = (object)item.IsPublished;
                        sqlParams[9].Value = (object)item.DisplayOrder;
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
        public IList<ProductSubCategory> DeleteProductSubCategory(List<ProductSubCategory> tasks)
        {
            var result = new List<ProductSubCategory>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductSubCategory_d";
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
                        var data = new ProductSubCategory();
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
        public List<ProductSubCategory> GetProductSubCategory(string QueryConditionPartParam)
        {
            List<ProductSubCategory> GridRecords = new List<ProductSubCategory>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductSubCategory_Get";
            command.AddParameter("@QueryConditionPartParam", QueryConditionPartParam, DbType.String);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                ProductSubCategory GridRecord = new ProductSubCategory()
                {
                    RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
                    ProductSubCategoryParentId = reader.ValidateColumnExistExtractAndCastTo<int>("ProductSubCategoryParentId"),
                    SubCategoryName = reader.ValidateColumnExistExtractAndCastTo<string>("SubCategoryName"),
                    Description = reader.ValidateColumnExistExtractAndCastTo<string>("Description"),
                    SeoContentId = reader.ValidateColumnExistExtractAndCastTo<int>("SeoContentId"),
                    IncludeInTopMenu = reader.ValidateColumnExistExtractAndCastTo<bool>("IncludeInTopMenu"),
                    IsNew = reader.ValidateColumnExistExtractAndCastTo<bool>("IsNew"),
                    HasDiscountApplied = reader.ValidateColumnExistExtractAndCastTo<bool>("HasDiscountApplied"),
                    IsPublished = reader.ValidateColumnExistExtractAndCastTo<bool>("IsPublished"),
                    IsDeleted = reader.ValidateColumnExistExtractAndCastTo<bool>("IsDeleted"),
                    DisplayOrder = reader.ValidateColumnExistExtractAndCastTo<string>("DisplayOrder"),
                    CreatedOnUtc = reader.ValidateColumnExistExtractAndCastTo<DateTime>("CreatedOnUtc"),
                    ModifiedOnUtc = reader.ValidateColumnExistExtractAndCastTo<DateTime>("ModifiedOnUtc"),
                    ProductImageId = reader.ValidateColumnExistExtractAndCastTo<int>("ProductImageId"),
                    SubCategoryParent = reader.ValidateColumnExistExtractAndCastTo<string>("SubCategoryParent"),
                    CategoryId = reader.ValidateColumnExistExtractAndCastTo<int>("CategoryId"),
                    CategoryName = reader.ValidateColumnExistExtractAndCastTo<string>("CategoryName"),
                    CategoryParentName = reader.ValidateColumnExistExtractAndCastTo<string>("CategoryParentName"),
                    CategoryParentId = reader.ValidateColumnExistExtractAndCastTo<int>("CategoryParentId"),
                    MetaTitle = reader.ValidateColumnExistExtractAndCastTo<string>("MetaTitle"),
                    MetaKeyword = reader.ValidateColumnExistExtractAndCastTo<string>("MetaKeyword"),
                    ImageName = reader.ValidateColumnExistExtractAndCastTo<string>("ImageName"),
                    ImagePath = reader.ValidateColumnExistExtractAndCastTo<string>("ImagePath"),
                    ThumbnailPath = reader.ValidateColumnExistExtractAndCastTo<string>("ThumbnailPath"),
                    //ImageFileStream = Helper.Base64ToImage(reader.ValidateColumnExistExtractAndCastTo<string>("ImagePath")),
                };
                GridRecords.Add(GridRecord);
            };
            command.CloseConnection();
            return GridRecords;
        }
        public List<ProductSubCategory> GetSubCategoryLookup(int SubCategoryParentId)
        {
            List<ProductSubCategory> GridRecords = new List<ProductSubCategory>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_SubCategoryLookup";
            command.AddParameter("@subCategoryParentId", SubCategoryParentId, DbType.String, 50);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                ProductSubCategory GridRecord = new ProductSubCategory()
                {
                    RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
                    SubCategoryName = reader.ValidateColumnExistExtractAndCastTo<string>("SubCategoryName"),
                };
                GridRecords.Add(GridRecord);
            }
            command.CloseConnection();
            return GridRecords;
        }
        #endregion


        #region Product-Feature-Category
        public List<ProductsFeaturesCategory> GetProductsFeaturesCategory(string QueryConditionPartParam)
        {
            List<ProductsFeaturesCategory> GridRecords = new List<ProductsFeaturesCategory>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductsFeaturesCategory_Get";
            command.AddParameter("@QueryConditionPartParam", QueryConditionPartParam, DbType.String);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                ProductsFeaturesCategory GridRecord = new ProductsFeaturesCategory()
                {
                    RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
                    CategoryShortName = reader.ValidateColumnExistExtractAndCastTo<string>("CategoryShortName"),
                    CategoryLongName = reader.ValidateColumnExistExtractAndCastTo<string>("CategoryLongName"),
                    CategoryDescription = reader.ValidateColumnExistExtractAndCastTo<string>("CategoryDescription"),
                };
                GridRecords.Add(GridRecord);
            };
            command.CloseConnection();
            return GridRecords;
        }
        public bool AddProductsFeaturesCategory(ProductsFeaturesCategory request)
        {
            int result;
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductsFeaturesCategory_i";
            command.AddParameter("@CategoryShortName", request.CategoryShortName, DbType.String);
            command.AddParameter("@CategoryLongName", request.CategoryLongName, DbType.String);
            command.AddParameter("@CategoryDescription", request.CategoryDescription, DbType.String);
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
        public IList<ProductsFeaturesCategory> UpdateProductsFeaturesCategory(List<ProductsFeaturesCategory> tasks)
        {
            var result = new List<ProductsFeaturesCategory>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductsFeaturesCategory_u";
            SqlParameter[] sqlParams = new SqlParameter[4];
            sqlParams[0] = new SqlParameter("@RowId", DbType.Int32);
            sqlParams[1] = new SqlParameter("@CategoryShortName", DbType.String);
            sqlParams[2] = new SqlParameter("@CategoryLongName", DbType.String);
            sqlParams[3] = new SqlParameter("@CategoryDescription", DbType.String);
            using (var transaction = new TransactionScope())
            {
                using (command.Connection)
                {
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();
                    foreach (var item in tasks)
                    {
                        var data = new ProductsFeaturesCategory();
                        command.Parameters.Clear();
                        sqlParams[0].Value = (object)item.RowId;
                        sqlParams[1].Value = (object)item.CategoryShortName;
                        sqlParams[2].Value = (object)item.CategoryLongName;
                        sqlParams[3].Value = (object)item.CategoryDescription;
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
        public IList<ProductsFeaturesCategory> DeleteProductsFeaturesCategory(List<ProductsFeaturesCategory> tasks)
        {
            var result = new List<ProductsFeaturesCategory>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductsFeaturesCategory_d";
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
                        var data = new ProductsFeaturesCategory();
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

        #region Product-Feature
        public List<ProductFeatures> GetProductFeatures(string QueryConditionPartParam)
        {
            List<ProductFeatures> GridRecords = new List<ProductFeatures>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductFeatures_Get";
            command.AddParameter("@QueryConditionPartParam", QueryConditionPartParam, DbType.String);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                ProductFeatures GridRecord = new ProductFeatures()
                {
                    RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
                    FeatureName = reader.ValidateColumnExistExtractAndCastTo<string>("FeatureName"),
                    Description = reader.ValidateColumnExistExtractAndCastTo<string>("Description"),
                    ProductFeatureCategoryId = reader.ValidateColumnExistExtractAndCastTo<int>("ProductFeatureCategoryId"),
                    CategoryShortName = reader.ValidateColumnExistExtractAndCastTo<string>("CategoryShortName"),
                    CategoryLongName = reader.ValidateColumnExistExtractAndCastTo<string>("CategoryLongName"),
                };
                GridRecords.Add(GridRecord);
            };
            command.CloseConnection();
            return GridRecords;
        }
        public bool AddProductFeatures(ProductFeatures request)
        {
            int result;
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductFeatures_i";
            command.AddParameter("@FeatureName", request.FeatureName, DbType.String);
            command.AddParameter("@Description", request.Description, DbType.String);
            command.AddParameter("@ProductFeatureCategoryId", request.ProductFeatureCategoryId, DbType.Int32);
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
        public IList<ProductFeatures> UpdateProductFeatures(List<ProductFeatures> tasks)
        {
            var result = new List<ProductFeatures>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductFeatures_u";
            SqlParameter[] sqlParams = new SqlParameter[4];
            sqlParams[0] = new SqlParameter("@RowId", DbType.Int32);
            sqlParams[1] = new SqlParameter("@FeatureName", DbType.String);
            sqlParams[2] = new SqlParameter("@Description", DbType.String);
            sqlParams[3] = new SqlParameter("@ProductFeatureCategoryId", DbType.Int32);
            using (var transaction = new TransactionScope())
            {
                using (command.Connection)
                {
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();
                    foreach (var item in tasks)
                    {
                        var data = new ProductFeatures();
                        command.Parameters.Clear();
                        sqlParams[0].Value = (object)item.RowId;
                        sqlParams[1].Value = (object)item.FeatureName;
                        sqlParams[2].Value = (object)item.Description;
                        sqlParams[3].Value = (object)item.ProductFeatureCategoryId;
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
        public IList<ProductFeatures> DeleteProductFeatures(List<ProductFeatures> tasks)
        {
            var result = new List<ProductFeatures>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductFeatures_d";
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
                        var data = new ProductFeatures();
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


        #region Product-Type
        public bool AddProductType(ProductType request)
        {
            int result;
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductType_i";
            command.AddParameter("@Name", request.Name, DbType.String);
            command.AddParameter("@Description", request.Description, DbType.String);
            command.AddParameter("@SeoContentId", request.SeoContentId, DbType.Int32);
            command.AddParameter("@IncludeInTopMenu", request.IncludeInTopMenu, DbType.Boolean);
            command.AddParameter("@IsNew", request.IsNew, DbType.Boolean);
            command.AddParameter("@HasDiscountApplied", request.HasDiscountApplied, DbType.Boolean);
            command.AddParameter("@IsPublished", request.IsPublished, DbType.Boolean);
            command.AddParameter("@DisplayOrder", request.DisplayOrder, DbType.Int32);
            command.AddParameter("@ProductSizeTypeId", request.ProductSizeTypeId, DbType.Int32);
            command.AddParameter("@ProductCategoryId", request.ProductCategoryId, DbType.Int32);
            command.AddParameter("@ProductSubCategoryParentId", request.ProductSubCategoryParentId, DbType.Int32);
            command.AddParameter("@ProductSubCategoryId", request.ProductSubCategoryId, DbType.Int32);
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
        public IList<ProductType> UpdateProductType(List<ProductType> tasks)
        {
            var result = new List<ProductType>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductType_u";
            SqlParameter[] sqlParams = new SqlParameter[15];
            sqlParams[0] = new SqlParameter("@RowId", DbType.Int32);
            sqlParams[1] = new SqlParameter("@Name", DbType.String);
            sqlParams[2] = new SqlParameter("@Description", DbType.String);
            sqlParams[3] = new SqlParameter("@SeoContentId", DbType.Int32);
            sqlParams[4] = new SqlParameter("@IncludeInTopMenu", DbType.Boolean);
            sqlParams[5] = new SqlParameter("@IsNew", DbType.Boolean);
            sqlParams[6] = new SqlParameter("@HasDiscountApplied", DbType.Boolean);
            sqlParams[7] = new SqlParameter("@IsPublished", DbType.Boolean);
            sqlParams[8] = new SqlParameter("@DisplayOrder", DbType.Int32);
            sqlParams[9] = new SqlParameter("@CreatedOnUtc", DbType.DateTime);
            sqlParams[10] = new SqlParameter("@ModifiedOnUtc", DbType.DateTime);
            sqlParams[11] = new SqlParameter("@ProductSizeTypeId", DbType.Int32);
            sqlParams[12] = new SqlParameter("@ProductCategoryId", DbType.Int32);
            sqlParams[13] = new SqlParameter("@ProductSubCategoryParentId", DbType.Int32);
            sqlParams[14] = new SqlParameter("@ProductSubCategoryId", DbType.Int32);
            using (var transaction = new TransactionScope())
            {
                using (command.Connection)
                {
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();
                    foreach (var item in tasks)
                    {
                        var data = new ProductType();
                        command.Parameters.Clear();
                        sqlParams[0].Value = (object)item.RowId;
                        sqlParams[1].Value = (object)item.Name;
                        sqlParams[2].Value = (object)item.Description;
                        sqlParams[3].Value = (object)item.SeoContentId;
                        sqlParams[4].Value = (object)item.IncludeInTopMenu;
                        sqlParams[5].Value = (object)item.IsNew;
                        sqlParams[6].Value = (object)item.HasDiscountApplied;
                        sqlParams[7].Value = (object)item.IsPublished;
                        sqlParams[8].Value = (object)item.DisplayOrder;
                        sqlParams[9].Value = (object)item.CreatedOnUtc;
                        sqlParams[10].Value = (object)item.ModifiedOnUtc;
                        sqlParams[11].Value = (object)item.ProductSizeTypeId;
                        sqlParams[12].Value = (object)item.ProductCategoryId;
                        sqlParams[13].Value = (object)item.ProductSubCategoryParentId;
                        sqlParams[14].Value = (object)item.ProductSubCategoryId;
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
        public IList<ProductType> DeleteProductType(List<ProductType> tasks)
        {
            var result = new List<ProductType>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductType_d";
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
                        var data = new ProductType();
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
        public List<ProductType> GetProductType(string QueryConditionPartParam)
        {
            List<ProductType> GridRecords = new List<ProductType>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductType_Get";
            command.AddParameter("@QueryConditionPartParam", QueryConditionPartParam, DbType.String);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                ProductType GridRecord = new ProductType()
                {
                    RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
                    Name = reader.ValidateColumnExistExtractAndCastTo<string>("Name"),
                    Description = reader.ValidateColumnExistExtractAndCastTo<string>("Description"),
                    IncludeInTopMenu = reader.ValidateColumnExistExtractAndCastTo<bool>("IncludeInTopMenu"),
                    IsNew = reader.ValidateColumnExistExtractAndCastTo<bool>("IsNew"),
                    HasDiscountApplied = reader.ValidateColumnExistExtractAndCastTo<bool>("HasDiscountApplied"),
                    IsPublished = reader.ValidateColumnExistExtractAndCastTo<bool>("IsPublished"),
                    IsDeleted = reader.ValidateColumnExistExtractAndCastTo<bool>("IsDeleted"),
                    DisplayOrder = reader.ValidateColumnExistExtractAndCastTo<string>("DisplayOrder"),
                    CreatedOnUtc = reader.ValidateColumnExistExtractAndCastTo<DateTime>("CreatedOnUtc"),
                    ModifiedOnUtc = reader.ValidateColumnExistExtractAndCastTo<DateTime>("ModifiedOnUtc"),
                    ProductSizeTypeId = reader.ValidateColumnExistExtractAndCastTo<int>("ProductSizeTypeId"),
                    ProductCategoryId = reader.ValidateColumnExistExtractAndCastTo<int>("ProductCategoryId"),
                    CategoryName = reader.ValidateColumnExistExtractAndCastTo<string>("CategoryName"),
                    ProductSubCategoryParentId = reader.ValidateColumnExistExtractAndCastTo<int>("ProductSubCategoryParentId"),
                    SubCategoryParentName = reader.ValidateColumnExistExtractAndCastTo<string>("SubCategoryParentName"),
                    ProductSubCategoryId = reader.ValidateColumnExistExtractAndCastTo<int>("ProductSubCategoryId"),
                    SubCategoryName = reader.ValidateColumnExistExtractAndCastTo<string>("SubCategoryName"),
                    SeoContentId = reader.ValidateColumnExistExtractAndCastTo<int>("SeoContentId"),
                    MetaTitle = reader.ValidateColumnExistExtractAndCastTo<string>("MetaTitle"),
                    MetaKeyword = reader.ValidateColumnExistExtractAndCastTo<string>("MetaKeyword"),
                    TypeName = reader.ValidateColumnExistExtractAndCastTo<string>("TypeName"),
                };
                GridRecords.Add(GridRecord);
            };
            command.CloseConnection();
            return GridRecords;
        }
        public List<ProductType> GetProductTypeLookup(int SubCategoryParentId)
        {
            List<ProductType> GridRecords = new List<ProductType>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductTypeLookup";
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                ProductType GridRecord = new ProductType()
                {
                    RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
                    Name = reader.ValidateColumnExistExtractAndCastTo<string>("Name"),
                };
                GridRecords.Add(GridRecord);
            }
            command.CloseConnection();
            return GridRecords;
        }
        #endregion


        private Categoryhierarchy TraverseCategory(List<CategoryhierarchyRawData> lstData, int level, CategoryhierarchyRawData childs)
        {
            Categoryhierarchy menu = null;

            Console.WriteLine(childs.Name);
            menu = new Categoryhierarchy();
            menu.MenuID = childs.RowId;
            menu.Name = childs.Name;
            menu.Level = childs.Level;

            var child = lstData.Where(a => a.Level == level + 1 && a.ParentId == childs.RowId);

            foreach (var ch in child)
            {
                var mm = TraverseCategory(lstData, ch.Level, ch);
                if (menu._ChildMenus == null)
                    menu._ChildMenus = new List<Categoryhierarchy>();
                menu._ChildMenus.Add(mm);
            }
            return menu;
        }

        public Categoryhierarchy GetCategoryhierarchy()
        {
            List<CategoryhierarchyRawData> GridRecords = new List<CategoryhierarchyRawData>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_Categoryhierarchy_Get";
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                CategoryhierarchyRawData GridRecord = new CategoryhierarchyRawData()
                {
                    RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
                    Name = reader.ValidateColumnExistExtractAndCastTo<string>("Name"),
                    ParentId = reader.ValidateColumnExistExtractAndCastTo<int>("ParentId"),
                    Level = reader.ValidateColumnExistExtractAndCastTo<int>("Level")
                };
                GridRecords.Add(GridRecord);
            };
            command.CloseConnection();
            var parent = GridRecords.Where(v => v.Level == 0).FirstOrDefault();
            return TraverseCategory(GridRecords, 0, parent);
        }
    }
}
