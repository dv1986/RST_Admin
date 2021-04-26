
using Infrastructure.Repository;
using ModelProductImages;
using ServiceHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace ServiceProductImage
{
    public class ProductImageService : BaseService, IProductImageService
    {
        readonly IDataContext dbContext;
        public ProductImageService(IDataContext context)
        {
            dbContext = context;
        }

        #region Upload Image
        public ProductImages AddProductImages(ProductImages request)
        {
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductImages_i";
            command.AddParameter("@Description", request.Description, DbType.String);
            command.AddParameter("@IsDisplay", request.IsDisplay, DbType.Boolean);
            command.AddParameter("@DisplayOrder", request.DisplayOrder, DbType.Int32);
            command.AddParameter("@ImageName", request.ImageName, DbType.String);
            command.AddParameter("@ImagePath", request.ImagePath, DbType.String);
            command.AddParameter("@ThumbnailPath", request.ThumbnailPath, DbType.String);
            try
            {
                command.OpenConnection();
                var reader = command.OpenReader();
                while (reader.Read())
                {
                    request.RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId");
                };
            }
            finally
            {
                command.CloseConnection();
            }
            return request;
        }
        public int GetMaxProductImageId()
        {
            var result = 0;
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_get_MaxImageId";
            try
            {
                command.OpenConnection();
                var reader = command.OpenReader();
                while (reader.Read())
                {
                    result = reader.ValidateColumnExistExtractAndCastTo<int>("max_imageId");
                };
            }
            finally
            {
                command.CloseConnection();
            }
            return result;
        }
        public bool UpdateImage(int ImageId, string ModuleName, int RowId)
        {
            int result;
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.AddParameter("@RowId", RowId, DbType.Int32);
            command.AddParameter("@ProductImageId", ImageId, DbType.Int32);

            if (ModuleName.ToLower() == "category")
            {
                command.CommandText = "sp_ProductCategory_u_image";
            }
            else if (ModuleName.ToLower() == "parentcategory")
            {
                command.CommandText = "sp_ProductCategoryParent_u_image";
            }
            else if (ModuleName.ToLower() == "parentsubcategory")
            {
                command.CommandText = "sp_ProductSubCategoryParent_u_image";
            }
            else if (ModuleName.ToLower() == "subcategory")
            {
                command.CommandText = "sp_ProductSubCategory_u_image";
            }
            else if (ModuleName.ToLower() == "brand")
            {
                command.CommandText = "sp_ProductBrand_u_image";
            }
            else if (ModuleName.ToLower() == "advertisement")
            {
                command.CommandText = "sp_Advertisement_u_image";
            }

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
        #endregion

        #region ProductImage_Product
        public List<ProductImageProduct> GetProductImage_Product(int ProductId)
        {
            List<ProductImageProduct> GridRecords = new List<ProductImageProduct>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductImage_Product_Get";
            command.AddParameter("@ProductId", ProductId, DbType.Int32);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                ProductImageProduct GridRecord = new ProductImageProduct()
                {
                    RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
                    ProductId = reader.ValidateColumnExistExtractAndCastTo<int>("ProductId"),
                    ProductImageId = reader.ValidateColumnExistExtractAndCastTo<int>("ProductImageId"),
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

        public bool AddProductImage_Product(ProductImageProduct request)
        {
            int result;
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductImage_Product_i";
            command.AddParameter("@ProductId", request.ProductId, DbType.Int32);
            command.AddParameter("@ProductImageId", request.ProductImageId, DbType.Int32);
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

        public IList<ProductImageProduct> DeleteProductImage_Product_dyId(List<ProductImageProduct> tasks)
        {
            var result = new List<ProductImageProduct>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductImage_Product_d_byId";
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
                        var data = new ProductImageProduct();
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

        public bool DeleteProductImage_Product_dyProductId(ProductImageProduct request)
        {
            int result;
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductImage_Product_d_byProductId";
            command.AddParameter("@ProductId", request.ProductId, DbType.Int32);
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
        #endregion
    }
}
