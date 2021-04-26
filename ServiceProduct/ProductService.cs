using Infrastructure.Repository;
using ModelProduct;
using ModelProduct.Enum;
using ServiceHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;

namespace ServiceProduct
{
    public class ProductService : BaseService, IProductService
    {
        readonly IDataContext dbContext;
        public ProductService(IDataContext context)
        {
            dbContext = context;
        }


        #region Brand
        public bool AddBrands(Brands request)
        {
            int result;
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_Brands_i";
            command.AddParameter("@ShortName", request.ShortName, DbType.String);
            command.AddParameter("@FullName", request.FullName, DbType.String);
            command.AddParameter("@Description", request.Description, DbType.String);
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
        public IList<Brands> UpdateBrands(List<Brands> tasks)
        {
            var result = new List<Brands>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_Brands_u";
            SqlParameter[] sqlParams = new SqlParameter[4];
            sqlParams[0] = new SqlParameter("@RowId", DbType.Int32);
            sqlParams[1] = new SqlParameter("@ShortName", DbType.String);
            sqlParams[2] = new SqlParameter("@FullName", DbType.String);
            sqlParams[3] = new SqlParameter("@Description", DbType.String);
            //sqlParams[4] = new SqlParameter("@ProductImageId", DbType.Int32);
            using (var transaction = new TransactionScope())
            {
                using (command.Connection)
                {
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();
                    foreach (var item in tasks)
                    {
                        var data = new Brands();
                        command.Parameters.Clear();
                        sqlParams[0].Value = (object)item.RowId;
                        sqlParams[1].Value = (object)item.ShortName;
                        sqlParams[2].Value = (object)item.FullName;
                        sqlParams[3].Value = (object)item.Description;
                        //sqlParams[4].Value = (object)item.ProductImageId;
                        command.Parameters.Add(sqlParams[0]);
                        command.Parameters.Add(sqlParams[1]);
                        command.Parameters.Add(sqlParams[2]);
                        command.Parameters.Add(sqlParams[3]);
                        //command.Parameters.Add(sqlParams[4]);
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
        public IList<Brands> DeleteBrands(List<Brands> tasks)
        {
            var result = new List<Brands>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_Brands_d";
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
                        var data = new Brands();
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
        public List<Brands> GetBrands(string QueryConditionPartParam)
        {
            List<Brands> GridRecords = new List<Brands>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_Brands_Get";
            command.AddParameter("@QueryConditionPartParam", QueryConditionPartParam, DbType.String);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                Brands GridRecord = new Brands()
                {
                    RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
                    ShortName = reader.ValidateColumnExistExtractAndCastTo<string>("ShortName"),
                    FullName = reader.ValidateColumnExistExtractAndCastTo<string>("FullName"),
                    Description = reader.ValidateColumnExistExtractAndCastTo<string>("Description"),
                    CreatedOn = reader.ValidateColumnExistExtractAndCastTo<DateTime>("CreatedOn"),
                    ImagePath = reader.ValidateColumnExistExtractAndCastTo<string>("ImagePath"),
                    ImageName = reader.ValidateColumnExistExtractAndCastTo<string>("ImageName"),
                    ThumbnailPath = reader.ValidateColumnExistExtractAndCastTo<string>("ThumbnailPath"),
                    //ImageFileStream = Helper.Base64ToImage(reader.ValidateColumnExistExtractAndCastTo<string>("ImagePath")),
                };
                GridRecords.Add(GridRecord);
            };
            command.CloseConnection();
            return GridRecords;
        }
        #endregion


        #region Product Attribute Parent
        public List<ProductAttributeParent> GetProductAttributeParent(string QueryConditionPartParam)
        {
            List<ProductAttributeParent> GridRecords = new List<ProductAttributeParent>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductAttributeParent_Get";
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                ProductAttributeParent GridRecord = new ProductAttributeParent()
                {
                    RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
                    Name = reader.ValidateColumnExistExtractAndCastTo<string>("Name"),
                    Description = reader.ValidateColumnExistExtractAndCastTo<string>("Description"),
                };
                GridRecords.Add(GridRecord);
            };
            command.CloseConnection();
            return GridRecords;
        }

        public List<ProductAttributeParent> GetProductAttributeParentbyProdcutType(int ProductTypeId)
        {
            List<ProductAttributeParent> GridRecords = new List<ProductAttributeParent>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductAttributeParentByProductType_Get";
            command.AddParameter("@ProductTypeId", ProductTypeId, DbType.Int32);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                ProductAttributeParent GridRecord = new ProductAttributeParent()
                {
                    RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
                    Name = reader.ValidateColumnExistExtractAndCastTo<string>("Name"),
                    Description = reader.ValidateColumnExistExtractAndCastTo<string>("Description"),
                };
                GridRecords.Add(GridRecord);
            };
            command.CloseConnection();
            return GridRecords;
        }
        public bool AddProductAttributeParent(ProductAttributeParent request)
        {
            int result;
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductAttributeParent_i";
            command.AddParameter("@Name", request.Name, DbType.String);
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
        public IList<ProductAttributeParent> UpdateProductAttributeParent(List<ProductAttributeParent> tasks)
        {
            var result = new List<ProductAttributeParent>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductAttributeParent_u";
            SqlParameter[] sqlParams = new SqlParameter[3];
            sqlParams[0] = new SqlParameter("@RowId", DbType.Int32);
            sqlParams[1] = new SqlParameter("@Name", DbType.String);
            sqlParams[2] = new SqlParameter("@Description", DbType.String);
            using (var transaction = new TransactionScope())
            {
                using (command.Connection)
                {
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();
                    foreach (var item in tasks)
                    {
                        var data = new ProductAttributeParent();
                        command.Parameters.Clear();
                        sqlParams[0].Value = (object)item.RowId;
                        sqlParams[1].Value = (object)item.Name;
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
        public IList<ProductAttributeParent> DeleteProductAttributeParent(List<ProductAttributeParent> tasks)
        {
            var result = new List<ProductAttributeParent>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductAttributeParent_d";
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
                        var data = new ProductAttributeParent();
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


        #region Product Attribute
        public List<ProductAttribute> GetProductAttribute(string QueryConditionPartParam)
        {
            if (QueryConditionPartParam != null && QueryConditionPartParam != "")
                QueryConditionPartParam = " AND a.ProductAttributeParentId=" + int.Parse(QueryConditionPartParam) + "";
            List<ProductAttribute> GridRecords = new List<ProductAttribute>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductAttribute_Get";
            command.AddParameter("@QueryConditionPartParam", QueryConditionPartParam, DbType.String);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                ProductAttribute GridRecord = new ProductAttribute()
                {
                    RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
                    ProductAttributeParentId = reader.ValidateColumnExistExtractAndCastTo<int>("ProductAttributeParentId"),
                    AttributeName = reader.ValidateColumnExistExtractAndCastTo<string>("AttributeName"),
                    ProductAttributeParentName = reader.ValidateColumnExistExtractAndCastTo<string>("ProductAttributeParentName"),
                };
                GridRecords.Add(GridRecord);
            };
            command.CloseConnection();
            return GridRecords;
        }
        public bool AddProductAttribute(ProductAttribute request)
        {
            int result;
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductAttribute_i";
            command.AddParameter("@ProductAttributeParentId", request.ProductAttributeParentId, DbType.Int32);
            command.AddParameter("@AttributeName", request.AttributeName, DbType.String);
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
        public IList<ProductAttribute> UpdateProductAttribute(List<ProductAttribute> tasks)
        {
            var result = new List<ProductAttribute>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductAttribute_u";
            SqlParameter[] sqlParams = new SqlParameter[3];
            sqlParams[0] = new SqlParameter("@RowId", DbType.Int32);
            sqlParams[1] = new SqlParameter("@ProductAttributeParentId", DbType.Int32);
            sqlParams[2] = new SqlParameter("@AttributeName", DbType.String);
            using (var transaction = new TransactionScope())
            {
                using (command.Connection)
                {
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();
                    foreach (var item in tasks)
                    {
                        var data = new ProductAttribute();
                        command.Parameters.Clear();
                        sqlParams[0].Value = (object)item.RowId;
                        sqlParams[1].Value = (object)item.ProductAttributeParentId;
                        sqlParams[2].Value = (object)item.AttributeName;
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
        public IList<ProductAttribute> DeleteProductAttribute(List<ProductAttribute> tasks)
        {
            var result = new List<ProductAttribute>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductAttribute_d";
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
                        var data = new ProductAttribute();
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

        #region Product
        public int AddProduct(Product product)
        {
            int ProductId = 0;
            using (var transaction = new TransactionScope())
            {
                var command = dbContext.CreateCommand();
                try
                {
                    int PriceId = 0;
                    ProductPrice price = new ProductPrice();
                    price.ProductMRP = product.ProductMRP;
                    price.Comments = product.Comments;
                    price.InclusiveSalesTax = product.InclusiveSalesTax;
                    price.RetailPrice = product.RetailPrice;
                    price.SellingPrice = product.SellingPrice;
                    price.SpecialPrice = product.SpecialPrice;
                    price.SpecialPriceEndDate = product.SpecialPriceEndDate;
                    price.SpecialPriceStartDate = product.SpecialPriceStartDate;
                    price.ProductMRP = product.ProductMRP;
                    PriceId = AddProductPrice(price);
                    // Get Price Id from here


                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "sp_Product_i";
                    command.AddParameter("@ProductTitle", product.ProductTitle, DbType.String);
                    command.AddParameter("@ShortDescription", product.ShortDescription, DbType.String);
                    command.AddParameter("@FullDescription", product.FullDescription, DbType.String);
                    command.AddParameter("@SKUCode", product.SKUCode, DbType.String);
                    command.AddParameter("@ModelYear", product.ModelYear, DbType.Int32);
                    command.AddParameter("@ModelNumber", product.ModelNumber, DbType.String);
                    command.AddParameter("@ProductFabricId", product.ProductFabricId, DbType.Int32);
                    command.AddParameter("@SeoContentId", product.SeoContentId, DbType.Int32);
                    command.AddParameter("@ProductTypeId", product.ProductTypeId, DbType.Int32);
                    command.AddParameter("@SubCategoryParentId", product.SubCategoryParentId, DbType.Int32);
                    command.AddParameter("@SubCategoryId", product.SubCategoryId, DbType.Int32);
                    command.AddParameter("@CategoryId", product.CategoryId, DbType.Int32);
                    command.AddParameter("@SellerId", product.SellerId, DbType.Int32);
                    command.AddParameter("@AdminComment", product.AdminComment, DbType.String);
                    command.AddParameter("@ShowOnHomePage", product.ShowOnHomePage, DbType.Boolean);
                    command.AddParameter("@AllowCustomerReviews", product.AllowCustomerReviews, DbType.Boolean);
                    command.AddParameter("@ApprovedRatingSum", product.ApprovedRatingSum, DbType.Int32);
                    command.AddParameter("@NotApprovedRatingSum", product.NotApprovedRatingSum, DbType.Int32);
                    command.AddParameter("@ApprovedTotalReviews", product.ApprovedTotalReviews, DbType.Int32);
                    command.AddParameter("@NotApprovedTotalReviews", product.NotApprovedTotalReviews, DbType.Int32);
                    command.AddParameter("@StockQuantity", product.StockQuantity, DbType.Int32);
                    command.AddParameter("@DisplayStockAvailability", product.DisplayStockAvailability, DbType.Boolean);
                    command.AddParameter("@DisplayStockQuantity", product.DisplayStockQuantity, DbType.Boolean);
                    command.AddParameter("@MinStockQuantity", product.MinStockQuantity, DbType.Int32);
                    command.AddParameter("@LowStockActivityId", product.LowStockActivityId, DbType.Int32);
                    command.AddParameter("@NotifyAdminForQuantityBelow", product.NotifyAdminForQuantityBelow, DbType.Int32);
                    command.AddParameter("@OrderMinimumQuantity", product.OrderMinimumQuantity, DbType.Int32);
                    command.AddParameter("@OrderMaximumQuantity", product.OrderMaximumQuantity, DbType.Int32);
                    command.AddParameter("@AllowedQuantities", product.AllowedQuantities, DbType.String);
                    command.AddParameter("@IsOutOfStock", product.IsOutOfStock, DbType.Boolean);
                    command.AddParameter("@DisableBuyButton", product.DisableBuyButton, DbType.Boolean);
                    command.AddParameter("@DisableWishlistButton", product.DisableWishlistButton, DbType.Boolean);
                    command.AddParameter("@HasDiscountsApplied", product.HasDiscountsApplied, DbType.Boolean);
                    command.AddParameter("@DisplayOrder", product.DisplayOrder, DbType.Int32);
                    command.AddParameter("@Published", product.Published, DbType.Boolean);
                    command.AddParameter("@BrandId", product.BrandId, DbType.Int32);
                    command.AddParameter("@ProductTagId", product.ProductTagId, DbType.Int32);
                    command.AddParameter("@ProductPriceId", PriceId, DbType.Int32);
                    command.AddParameter("@DealProductCategoryId", product.DealProductCategoryId, DbType.Int32);
                    command.AddParameter("@CommisionRateId", product.CommisionRateId, DbType.Int32);
                    command.AddParameter("@QuantityBeforeUpdate", product.QuantityBeforeUpdate, DbType.Int32);
                    command.AddParameter("@IsRecommended", product.IsRecommended, DbType.Boolean);
                    command.AddParameter("@IsFeatured", product.IsFeatured, DbType.Boolean);
                    command.AddParameter("@TotalPercentOff", product.TotalPercentOff, DbType.Decimal);
                    command.AddParameter("@CreatedBy", product.CreatedBy, DbType.Int32);

                    command.OpenConnection();
                    var reader = command.OpenReader();
                    while (reader.Read())
                    {
                        ProductId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId");
                    }

                    if (product.AttributeIds != null)
                    {
                        ProductAttribute_Product request = new ProductAttribute_Product();
                        request.ProductId = ProductId;
                        foreach (var item in product.AttributeIds)
                        {
                            request.ProductAttributeId = item;
                            AddProductAttribute_Product(request);
                        }
                    }

                    if (product.ColorIds != null)
                    {
                        ProductColor_Mapping colorRequest = new ProductColor_Mapping();
                        colorRequest.ProductId = ProductId;
                        foreach (var item in product.ColorIds)
                        {
                            colorRequest.ColorId = item.Value;
                            AddProductColor_Mapping(colorRequest);
                        }
                    }
                }
                finally
                {
                    command.CloseConnection();
                }

                transaction.Complete();
            }
            return ProductId;
        }
        public IList<Product> UpdateProduct(List<Product> tasks)
        {
            var result = new List<Product>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_Product_u";
            SqlParameter[] sqlParams = new SqlParameter[41];
            sqlParams[0] = new SqlParameter("@RowId", DbType.Int32);
            sqlParams[1] = new SqlParameter("@ProductTitle", DbType.String);
            sqlParams[2] = new SqlParameter("@ShortDescription", DbType.String);
            sqlParams[3] = new SqlParameter("@FullDescription", DbType.String);
            sqlParams[4] = new SqlParameter("@SKUCode", DbType.String);
            sqlParams[5] = new SqlParameter("@ProductFabricId", DbType.Int32);
            sqlParams[6] = new SqlParameter("@SeoContentId", DbType.Int32);
            sqlParams[7] = new SqlParameter("@ProductTypeId", DbType.Int32);
            sqlParams[8] = new SqlParameter("@SellerId", DbType.Int32);
            sqlParams[9] = new SqlParameter("@AdminComment", DbType.String);
            sqlParams[10] = new SqlParameter("@ShowOnHomePage", DbType.Boolean);
            sqlParams[11] = new SqlParameter("@AllowCustomerReviews", DbType.Boolean);
            sqlParams[12] = new SqlParameter("@StockQuantity", DbType.Int32);
            sqlParams[13] = new SqlParameter("@DisplayStockAvailability", DbType.Boolean);
            sqlParams[14] = new SqlParameter("@DisplayStockQuantity", DbType.Boolean);
            sqlParams[15] = new SqlParameter("@MinStockQuantity", DbType.Int32);
            sqlParams[16] = new SqlParameter("@LowStockActivityId", DbType.Int32);
            sqlParams[17] = new SqlParameter("@NotifyAdminForQuantityBelow", DbType.Int32);
            sqlParams[18] = new SqlParameter("@OrderMinimumQuantity", DbType.Int32);
            sqlParams[19] = new SqlParameter("@OrderMaximumQuantity", DbType.Int32);
            sqlParams[20] = new SqlParameter("@AllowedQuantities", DbType.String);
            sqlParams[21] = new SqlParameter("@IsOutOfStock", DbType.Boolean);
            sqlParams[22] = new SqlParameter("@DisableBuyButton", DbType.Boolean);
            sqlParams[23] = new SqlParameter("@DisableWishlistButton", DbType.Boolean);
            sqlParams[24] = new SqlParameter("@HasDiscountsApplied", DbType.Boolean);
            sqlParams[25] = new SqlParameter("@DisplayOrder", DbType.Int32);
            sqlParams[26] = new SqlParameter("@Published", DbType.Boolean);
            sqlParams[27] = new SqlParameter("@IsDeleted", DbType.Boolean);
            sqlParams[28] = new SqlParameter("@UpdatedOnUtc", DbType.DateTime);
            sqlParams[29] = new SqlParameter("@BrandId", DbType.Int32);
            sqlParams[30] = new SqlParameter("@ProductTagId", DbType.Int32);
            sqlParams[31] = new SqlParameter("@ProductPriceId", DbType.Int32);
            sqlParams[32] = new SqlParameter("@DealProductCategoryId", DbType.Int32);
            sqlParams[33] = new SqlParameter("@CommisionRateId", DbType.Int32);
            sqlParams[34] = new SqlParameter("@QuantityBeforeUpdate", DbType.Int32);
            sqlParams[35] = new SqlParameter("@QuantityUpdatedOnUtc", DbType.DateTime);
            sqlParams[36] = new SqlParameter("@TotalPercentOff", DbType.Decimal);
            sqlParams[37] = new SqlParameter("@IsRecommended", DbType.Boolean);
            sqlParams[38] = new SqlParameter("@IsFeatured", DbType.Boolean);
            sqlParams[39] = new SqlParameter("@ModelYear", DbType.Int32);
            sqlParams[40] = new SqlParameter("@ModelNumber", DbType.String);
            using (var transaction = new TransactionScope())
            {
                command.OpenConnection();
                foreach (var item in tasks)
                {
                    var data = new Product();
                    command.Parameters.Clear();
                    sqlParams[0].Value = (object)item.RowId;
                    sqlParams[1].Value = (object)item.ProductTitle;
                    sqlParams[2].Value = (object)item.ShortDescription;
                    sqlParams[3].Value = (object)item.FullDescription;
                    sqlParams[4].Value = (object)item.SKUCode;
                    sqlParams[5].Value = (object)item.ProductFabricId;
                    sqlParams[6].Value = (object)item.SeoContentId;
                    sqlParams[7].Value = (object)item.ProductTypeId;
                    sqlParams[8].Value = (object)item.SellerId;
                    sqlParams[9].Value = (object)item.AdminComment;
                    sqlParams[10].Value = (object)item.ShowOnHomePage;
                    sqlParams[11].Value = (object)item.AllowCustomerReviews;
                    sqlParams[12].Value = (object)item.StockQuantity;
                    sqlParams[13].Value = (object)item.DisplayStockAvailability;
                    sqlParams[14].Value = (object)item.DisplayStockQuantity;
                    sqlParams[15].Value = (object)item.MinStockQuantity;
                    sqlParams[16].Value = (object)item.LowStockActivityId;
                    sqlParams[17].Value = (object)item.NotifyAdminForQuantityBelow;
                    sqlParams[18].Value = (object)item.OrderMinimumQuantity;
                    sqlParams[19].Value = (object)item.OrderMaximumQuantity;
                    sqlParams[20].Value = (object)item.AllowedQuantities;
                    sqlParams[21].Value = (object)item.IsOutOfStock;
                    sqlParams[22].Value = (object)item.DisableBuyButton;
                    sqlParams[23].Value = (object)item.DisableWishlistButton;
                    sqlParams[24].Value = (object)item.HasDiscountsApplied;
                    sqlParams[25].Value = (object)item.DisplayOrder;
                    sqlParams[26].Value = (object)item.Published;
                    sqlParams[27].Value = (object)item.IsDeleted;
                    sqlParams[28].Value = (object)item.UpdatedOnUtc;
                    sqlParams[29].Value = (object)item.BrandId;
                    sqlParams[30].Value = (object)item.ProductTagId;
                    sqlParams[31].Value = (object)item.ProductPriceId;
                    sqlParams[32].Value = (object)item.DealProductCategoryId;
                    sqlParams[33].Value = (object)item.CommisionRateId;
                    sqlParams[34].Value = (object)item.QuantityBeforeUpdate;
                    sqlParams[35].Value = (object)item.QuantityUpdatedOnUtc;
                    sqlParams[36].Value = (object)item.TotalPercentOff;
                    sqlParams[37].Value = (object)item.IsRecommended;
                    sqlParams[38].Value = (object)item.IsFeatured;
                    sqlParams[39].Value = (object)item.ModelYear;
                    sqlParams[40].Value = (object)item.ModelNumber;
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
                    command.Parameters.Add(sqlParams[15]);
                    command.Parameters.Add(sqlParams[16]);
                    command.Parameters.Add(sqlParams[17]);
                    command.Parameters.Add(sqlParams[18]);
                    command.Parameters.Add(sqlParams[19]);
                    command.Parameters.Add(sqlParams[20]);
                    command.Parameters.Add(sqlParams[21]);
                    command.Parameters.Add(sqlParams[22]);
                    command.Parameters.Add(sqlParams[23]);
                    command.Parameters.Add(sqlParams[24]);
                    command.Parameters.Add(sqlParams[25]);
                    command.Parameters.Add(sqlParams[26]);
                    command.Parameters.Add(sqlParams[27]);
                    command.Parameters.Add(sqlParams[28]);
                    command.Parameters.Add(sqlParams[29]);
                    command.Parameters.Add(sqlParams[30]);
                    command.Parameters.Add(sqlParams[31]);
                    command.Parameters.Add(sqlParams[32]);
                    command.Parameters.Add(sqlParams[33]);
                    command.Parameters.Add(sqlParams[34]);
                    command.Parameters.Add(sqlParams[35]);
                    command.Parameters.Add(sqlParams[36]);
                    command.Parameters.Add(sqlParams[37]);
                    command.Parameters.Add(sqlParams[38]);
                    command.Parameters.Add(sqlParams[39]);
                    command.Parameters.Add(sqlParams[40]);

                    ProductPrice price = new ProductPrice();
                    price.RowId = item.ProductPriceId;
                    price.ProductMRP = item.ProductMRP;
                    price.Comments = item.Comments;
                    price.InclusiveSalesTax = item.InclusiveSalesTax;
                    price.RetailPrice = item.RetailPrice;
                    price.SellingPrice = item.SellingPrice;
                    price.SpecialPrice = item.SpecialPrice;
                    price.SpecialPriceEndDate = item.SpecialPriceEndDate.Value.Year == 0001 ? null : item.SpecialPriceEndDate;
                    price.SpecialPriceStartDate = item.SpecialPriceStartDate.Value.Year == 0001 ? null : item.SpecialPriceStartDate;
                    price.ProductMRP = item.ProductMRP;

                    try
                    {
                        UpdateProductPrice(price);
                        command.ExecuteNonQuery();
                        data.RowId = item.RowId;
                        data.Message = "";
                    }
                    finally
                    {
                        command.CloseConnection();
                    }
                }
                transaction.Complete();
            }
            return result;
        }
        public IList<Product> DeleteProduct(List<Product> tasks)
        {
            throw new NotImplementedException();
        }
        public List<Product> GetProduct(string QueryConditionPartParam)
        {
            List<Product> GridRecords = new List<Product>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_Product_Get";
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                int ProductId = 0;
                Product GridRecord = new Product();

                GridRecord.RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId");
                GridRecord.ProductTitle = reader.ValidateColumnExistExtractAndCastTo<string>("ProductTitle");
                GridRecord.ShortDescription = reader.ValidateColumnExistExtractAndCastTo<string>("ShortDescription");
                GridRecord.FullDescription = reader.ValidateColumnExistExtractAndCastTo<string>("FullDescription");
                GridRecord.SKUCode = reader.ValidateColumnExistExtractAndCastTo<string>("SKUCode");
                GridRecord.ModelYear = reader.ValidateColumnExistExtractAndCastTo<int>("ModelYear");
                GridRecord.ModelNumber = reader.ValidateColumnExistExtractAndCastTo<string>("ModelNumber");
                GridRecord.SellerId = reader.ValidateColumnExistExtractAndCastTo<int>("SellerId");
                GridRecord.AdminComment = reader.ValidateColumnExistExtractAndCastTo<string>("AdminComment");
                GridRecord.ShowOnHomePage = reader.ValidateColumnExistExtractAndCastTo<bool>("ShowOnHomePage");
                GridRecord.AllowCustomerReviews = reader.ValidateColumnExistExtractAndCastTo<bool>("AllowCustomerReviews");
                GridRecord.ApprovedRatingSum = reader.ValidateColumnExistExtractAndCastTo<int>("ApprovedRatingSum");
                GridRecord.NotApprovedRatingSum = reader.ValidateColumnExistExtractAndCastTo<int>("NotApprovedRatingSum");
                GridRecord.ApprovedTotalReviews = reader.ValidateColumnExistExtractAndCastTo<int>("ApprovedTotalReviews");
                GridRecord.NotApprovedTotalReviews = reader.ValidateColumnExistExtractAndCastTo<int>("NotApprovedTotalReviews");
                GridRecord.StockQuantity = reader.ValidateColumnExistExtractAndCastTo<int>("StockQuantity");
                GridRecord.DisplayStockAvailability = reader.ValidateColumnExistExtractAndCastTo<bool>("DisplayStockAvailability");
                GridRecord.DisplayStockQuantity = reader.ValidateColumnExistExtractAndCastTo<bool>("DisplayStockQuantity");
                GridRecord.MinStockQuantity = reader.ValidateColumnExistExtractAndCastTo<int>("MinStockQuantity");
                GridRecord.LowStockActivityId = reader.ValidateColumnExistExtractAndCastTo<int>("LowStockActivityId");
                GridRecord.NotifyAdminForQuantityBelow = reader.ValidateColumnExistExtractAndCastTo<int>("NotifyAdminForQuantityBelow");
                GridRecord.OrderMinimumQuantity = reader.ValidateColumnExistExtractAndCastTo<int>("OrderMinimumQuantity");
                GridRecord.OrderMaximumQuantity = reader.ValidateColumnExistExtractAndCastTo<int>("OrderMaximumQuantity");
                GridRecord.AllowedQuantities = reader.ValidateColumnExistExtractAndCastTo<string>("AllowedQuantities");
                GridRecord.IsOutOfStock = reader.ValidateColumnExistExtractAndCastTo<bool>("IsOutOfStock");
                GridRecord.DisableBuyButton = reader.ValidateColumnExistExtractAndCastTo<bool>("DisableBuyButton");
                GridRecord.DisableWishlistButton = reader.ValidateColumnExistExtractAndCastTo<bool>("DisableWishlistButton");
                GridRecord.HasDiscountsApplied = reader.ValidateColumnExistExtractAndCastTo<bool>("HasDiscountsApplied");
                GridRecord.DisplayOrder = reader.ValidateColumnExistExtractAndCastTo<int>("DisplayOrder");
                GridRecord.Published = reader.ValidateColumnExistExtractAndCastTo<bool>("Published");
                GridRecord.IsDeleted = reader.ValidateColumnExistExtractAndCastTo<bool>("IsDeleted");
                GridRecord.CreatedOnUtc = reader.ValidateColumnExistExtractAndCastTo<DateTime>("CreatedOnUtc");
                GridRecord.UpdatedOnUtc = reader.ValidateColumnExistExtractAndCastTo<DateTime>("UpdatedOnUtc");
                GridRecord.QuantityBeforeUpdate = reader.ValidateColumnExistExtractAndCastTo<int>("QuantityBeforeUpdate");
                GridRecord.QuantityUpdatedOnUtc = reader.ValidateColumnExistExtractAndCastTo<DateTime>("QuantityUpdatedOnUtc");
                GridRecord.DealProductCategoryId = reader.ValidateColumnExistExtractAndCastTo<int>("DealProductCategoryId");
                GridRecord.CommisionRateId = reader.ValidateColumnExistExtractAndCastTo<int>("CommisionRateId");
                GridRecord.DeletedOn = reader.ValidateColumnExistExtractAndCastTo<DateTime>("DeletedOn");
                GridRecord.TotalPercentOff = reader.ValidateColumnExistExtractAndCastTo<decimal>("TotalPercentOff");
                GridRecord.ProductFabricId = reader.ValidateColumnExistExtractAndCastTo<int>("ProductFabricId");
                GridRecord.FabricName = reader.ValidateColumnExistExtractAndCastTo<string>("FabricName");
                //GridRecord.SeoContentId = reader.ValidateColumnExistExtractAndCastTo<int>("SeoContentId");
                if (!reader.IsDBNull(reader.GetOrdinal("SeoContentId")))
                    GridRecord.SeoContentId = reader.GetInt32(reader.GetOrdinal("SeoContentId"));
                GridRecord.MetaTitle = reader.ValidateColumnExistExtractAndCastTo<string>("MetaTitle");
                GridRecord.MetaKeyword = reader.ValidateColumnExistExtractAndCastTo<string>("MetaKeyword");
                //GridRecord.ProductTypeId = reader.ValidateColumnExistExtractAndCastTo<int>("ProductTypeId");
                if (!reader.IsDBNull(reader.GetOrdinal("ProductTypeId")))
                    GridRecord.ProductTypeId = reader.GetInt32(reader.GetOrdinal("ProductTypeId"));
                GridRecord.ProductTypeName = reader.ValidateColumnExistExtractAndCastTo<string>("ProductTypeName");
                //GridRecord.SubCategoryId = reader.ValidateColumnExistExtractAndCastTo<int>("SubCategoryId");
                if (!reader.IsDBNull(reader.GetOrdinal("SubCategoryId")))
                    GridRecord.SubCategoryId = reader.GetInt32(reader.GetOrdinal("SubCategoryId"));
                GridRecord.SubCategoryName = reader.ValidateColumnExistExtractAndCastTo<string>("SubCategoryName");
                //GridRecord.SubCategoryParentId = reader.ValidateColumnExistExtractAndCastTo<int>("SubCategoryParentId");
                if (!reader.IsDBNull(reader.GetOrdinal("SubCategoryParentId")))
                    GridRecord.SubCategoryParentId = reader.GetInt32(reader.GetOrdinal("SubCategoryParentId"));
                GridRecord.SubCategoryParentName = reader.ValidateColumnExistExtractAndCastTo<string>("SubCategoryParentName");
                //GridRecord.CategoryId = reader.ValidateColumnExistExtractAndCastTo<int>("CategoryId");
                if (!reader.IsDBNull(reader.GetOrdinal("CategoryId")))
                    GridRecord.CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId"));
                GridRecord.CategoryName = reader.ValidateColumnExistExtractAndCastTo<string>("CategoryName");
                //GridRecord.CategoryParentId = reader.ValidateColumnExistExtractAndCastTo<int>("CategoryParentId");
                if (!reader.IsDBNull(reader.GetOrdinal("CategoryParentId")))
                    GridRecord.CategoryParentId = reader.GetInt32(reader.GetOrdinal("CategoryParentId"));
                GridRecord.CategoryParentName = reader.ValidateColumnExistExtractAndCastTo<string>("CategoryParentName");
                //GridRecord.BrandId = reader.ValidateColumnExistExtractAndCastTo<int>("BrandId");
                if (!reader.IsDBNull(reader.GetOrdinal("BrandId")))
                    GridRecord.BrandId = reader.GetInt32(reader.GetOrdinal("BrandId"));
                GridRecord.BrandShortName = reader.ValidateColumnExistExtractAndCastTo<string>("BrandShortName");
                GridRecord.BrandFullName = reader.ValidateColumnExistExtractAndCastTo<string>("BrandFullName");
                //GridRecord.ProductTagId = reader.ValidateColumnExistExtractAndCastTo<int>("ProductTagId");
                if (!reader.IsDBNull(reader.GetOrdinal("ProductTagId")))
                    GridRecord.ProductTagId = reader.GetInt32(reader.GetOrdinal("ProductTagId"));
                GridRecord.TagName = reader.ValidateColumnExistExtractAndCastTo<string>("TagName");
                //GridRecord.ProductPriceId = reader.ValidateColumnExistExtractAndCastTo<int>("ProductPriceId");
                if (!reader.IsDBNull(reader.GetOrdinal("ProductPriceId")))
                    GridRecord.ProductPriceId = reader.GetInt32(reader.GetOrdinal("ProductPriceId"));
                GridRecord.ProductMRP = reader.ValidateColumnExistExtractAndCastTo<decimal>("ProductMRP");
                GridRecord.RetailPrice = reader.ValidateColumnExistExtractAndCastTo<decimal>("RetailPrice");
                GridRecord.SellingPrice = reader.ValidateColumnExistExtractAndCastTo<decimal>("SellingPrice");
                GridRecord.SpecialPrice = reader.ValidateColumnExistExtractAndCastTo<decimal>("SpecialPrice");
                GridRecord.SpecialPriceStartDate = reader.ValidateColumnExistExtractAndCastTo<DateTime>("SpecialPriceStartDate");
                GridRecord.SpecialPriceEndDate = reader.ValidateColumnExistExtractAndCastTo<DateTime>("SpecialPriceEndDate");
                GridRecord.InclusiveSalesTax = reader.ValidateColumnExistExtractAndCastTo<bool>("InclusiveSalesTax");
                GridRecord.IsRecommended = reader.ValidateColumnExistExtractAndCastTo<bool>("IsRecommended");
                GridRecord.IsFeatured = reader.ValidateColumnExistExtractAndCastTo<bool>("IsFeatured");

                GridRecords.Add(GridRecord);
            };
            command.CloseConnection();
            return GridRecords;
        }
        public List<FilteredProduct> GetFilteredProductList(string ProductName, string year, int CategoryId, int ParentSubCategoryId, int SubCategoryId, int ProductTypeId,
            int BrandId, int PriceRangeMin, int PriceRangeMax, string SortBy, bool IsAsc, int PageNumber, int NoOfRecord)
        {
            string AscOrDsc = IsAsc == true ? "asc" : "desc";
            string QueryConditionPartParam = "";
            if (ProductName != null && ProductName != "")
            {
                QueryConditionPartParam = " AND a.ProductTitle LIKE '%" + ProductName + "%'";
            }
            if (year != null && year != "")
            {
                QueryConditionPartParam += " AND DATEPART(YYYY, a.CreatedOnUtc) = " + year + "";
            }
            if (CategoryId != 0)
            {
                QueryConditionPartParam += " AND a.CategoryId =" + CategoryId + "";
            }
            if (ParentSubCategoryId != 0)
            {
                QueryConditionPartParam += " AND a.SubCategoryParentId =" + ParentSubCategoryId + "";
            }
            if (SubCategoryId != 0)
            {
                QueryConditionPartParam += " AND a.SubCategoryId =" + SubCategoryId + "";
            }
            if (ProductTypeId != 0)
            {
                QueryConditionPartParam += " AND a.ProductTypeId =" + ProductTypeId + "";
            }
            if (BrandId != 0)
            {
                QueryConditionPartParam += " AND a.BrandId =" + BrandId + "";
            }
            if (PriceRangeMax != 0)
            {
                QueryConditionPartParam += " AND c.ProductMRP BETWEEN " + PriceRangeMin + " AND " + PriceRangeMax + "";
            }

            if (SortBy == SortByEnum.Date.ToString())
            {
                QueryConditionPartParam += " ORDER BY a.CreatedOnUtc " + AscOrDsc + "";
            }
            else if (SortBy == SortByEnum.Price.ToString())
            {
                QueryConditionPartParam += " ORDER BY c.ProductMRP " + AscOrDsc + "";
            }
            else if (SortBy == SortByEnum.Name.ToString())
            {
                QueryConditionPartParam += " ORDER BY a.ProductTitle " + AscOrDsc + "";
            }
            else
            {
                QueryConditionPartParam += " ORDER BY RowId " + AscOrDsc + "";
            }


            List<FilteredProduct> GridRecords = new List<FilteredProduct>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_FilteredProductList_Get";
            command.AddParameter("@QueryConditionPartParam", QueryConditionPartParam, DbType.String);
            command.AddParameter("@PageNumber", PageNumber, DbType.Int32);
            command.AddParameter("@NoOfRecord", NoOfRecord, DbType.Int32);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                FilteredProduct GridRecord = new FilteredProduct()
                {
                    RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
                    ProductTitle = reader.ValidateColumnExistExtractAndCastTo<string>("ProductTitle"),
                    ShortDescription = reader.ValidateColumnExistExtractAndCastTo<string>("ShortDescription"),
                    SKUCode = reader.ValidateColumnExistExtractAndCastTo<string>("SKUCode"),
                    Latitude = reader.ValidateColumnExistExtractAndCastTo<string>("Latitude"),
                    Longitude = reader.ValidateColumnExistExtractAndCastTo<string>("Longitude"),
                    ProductMRP = reader.ValidateColumnExistExtractAndCastTo<decimal>("ProductMRP"),
                    ThumbnailImage = reader.ValidateColumnExistExtractAndCastTo<string>("ThumbnailImage"),
                    Address = reader.ValidateColumnExistExtractAndCastTo<string>("Address"),
                };
                GridRecords.Add(GridRecord);
            };
            command.CloseConnection();
            return GridRecords;
        }

        public ProductRange GetMinMaxPriceRange(string year, int CategoryId, int ParentSubCategoryId, int SubCategoryId, int ProductTypeId,
            int BrandId)
        {
            string QueryConditionPartParam = "";
            if (year != null && year != "")
            {
                QueryConditionPartParam += " AND DATEPART(YYYY, a.CreatedOnUtc) = " + year + "";
            }
            if (CategoryId != 0)
            {
                QueryConditionPartParam += " AND a.CategoryId =" + CategoryId + "";
            }
            if (ParentSubCategoryId != 0)
            {
                QueryConditionPartParam += " AND a.SubCategoryParentId =" + ParentSubCategoryId + "";
            }
            if (SubCategoryId != 0)
            {
                QueryConditionPartParam += " AND a.SubCategoryId =" + SubCategoryId + "";
            }
            if (ProductTypeId != 0)
            {
                QueryConditionPartParam += " AND a.ProductTypeId =" + ProductTypeId + "";
            }
            if (BrandId != 0)
            {
                QueryConditionPartParam += " AND a.BrandId =" + BrandId + "";
            }

            ProductRange GridRecord = new ProductRange();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_MinMaxPrice_Get";
            command.AddParameter("@QueryConditionPartParam", QueryConditionPartParam, DbType.String);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                GridRecord.MinPrice = reader.ValidateColumnExistExtractAndCastTo<int>("MinPrice");
                GridRecord.MaxPrice = reader.ValidateColumnExistExtractAndCastTo<int>("MaxPrice");
            };
            command.CloseConnection();
            return GridRecord;
        }

        public ProductDetails GetProductDetailbyId(int ProductId)
        {
            ProductDetails GridRecord = new ProductDetails();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductDetailbyId_Get";
            command.AddParameter("@ProductId", ProductId, DbType.Int32);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                GridRecord.RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId");
                GridRecord.ProductTitle = reader.ValidateColumnExistExtractAndCastTo<string>("ProductTitle");
                GridRecord.ShortDescription = reader.ValidateColumnExistExtractAndCastTo<string>("ShortDescription");
                GridRecord.FullDescription = reader.ValidateColumnExistExtractAndCastTo<string>("FullDescription");
                GridRecord.ModelNumber = reader.ValidateColumnExistExtractAndCastTo<string>("ModelNumber");
                GridRecord.ModelYear = reader.ValidateColumnExistExtractAndCastTo<int>("ModelYear");
                GridRecord.SKUCode = reader.ValidateColumnExistExtractAndCastTo<string>("SKUCode");
                GridRecord.AdminComment = reader.ValidateColumnExistExtractAndCastTo<string>("AdminComment");
                GridRecord.ShowOnHomePage = reader.ValidateColumnExistExtractAndCastTo<bool>("ShowOnHomePage");
                GridRecord.Published = reader.ValidateColumnExistExtractAndCastTo<bool>("Published");
                GridRecord.TotalPercentOff = reader.ValidateColumnExistExtractAndCastTo<decimal>("TotalPercentOff");
                GridRecord.FabricName = reader.ValidateColumnExistExtractAndCastTo<string>("FabricName");
                GridRecord.ProductTypeName = reader.ValidateColumnExistExtractAndCastTo<string>("ProductTypeName");
                GridRecord.SubCategoryName = reader.ValidateColumnExistExtractAndCastTo<string>("SubCategoryName");
                GridRecord.SubCategoryParentName = reader.ValidateColumnExistExtractAndCastTo<string>("SubCategoryParentName");
                GridRecord.CategoryName = reader.ValidateColumnExistExtractAndCastTo<string>("CategoryName");
                GridRecord.CategoryParentName = reader.ValidateColumnExistExtractAndCastTo<string>("CategoryParentName");
                GridRecord.BrandShortName = reader.ValidateColumnExistExtractAndCastTo<string>("BrandShortName");
                GridRecord.BrandFullName = reader.ValidateColumnExistExtractAndCastTo<string>("BrandFullName");
                GridRecord.TagName = reader.ValidateColumnExistExtractAndCastTo<string>("TagName");
                GridRecord.ProductMRP = reader.ValidateColumnExistExtractAndCastTo<decimal>("ProductMRP");
                GridRecord.ContactPersonName = reader.ValidateColumnExistExtractAndCastTo<string>("ContactPersonName");
                GridRecord.Latitude = reader.ValidateColumnExistExtractAndCastTo<string>("Latitude");
                GridRecord.Longitude = reader.ValidateColumnExistExtractAndCastTo<string>("Longitude");
                GridRecord.ContactNumber = reader.ValidateColumnExistExtractAndCastTo<string>("ContactNumber");
                GridRecord.ThumbnailImage = reader.ValidateColumnExistExtractAndCastTo<string>("ThumbnailImage");
                GridRecord.Address = reader.ValidateColumnExistExtractAndCastTo<string>("Address");
            };

            GridRecord.AttributeLst = GetProductAttributeMapping(ProductId);
            command.CloseConnection();
            return GridRecord;
        }

        public List<FilteredProduct> GetRecommendedProducts(int ProductId, int PageNo, int RecordCount)
        {
            List<FilteredProduct> GridRecords = new List<FilteredProduct>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_RecommendedProduct_Get";
            command.AddParameter("@ProductId", ProductId, DbType.Int32);
            command.AddParameter("@PageNumber", PageNo, DbType.Int32);
            command.AddParameter("@NoOfRecord", RecordCount, DbType.Int32);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                FilteredProduct GridRecord = new FilteredProduct()
                {
                    RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
                    ProductTitle = reader.ValidateColumnExistExtractAndCastTo<string>("ProductTitle"),
                    ShortDescription = reader.ValidateColumnExistExtractAndCastTo<string>("ShortDescription"),
                    SKUCode = reader.ValidateColumnExistExtractAndCastTo<string>("SKUCode"),
                    Latitude = reader.ValidateColumnExistExtractAndCastTo<string>("Latitude"),
                    Longitude = reader.ValidateColumnExistExtractAndCastTo<string>("Longitude"),
                    ProductMRP = reader.ValidateColumnExistExtractAndCastTo<decimal>("ProductMRP"),
                    ThumbnailImage = reader.ValidateColumnExistExtractAndCastTo<string>("ThumbnailImage"),
                    Address = reader.ValidateColumnExistExtractAndCastTo<string>("Address"),
                };
                GridRecords.Add(GridRecord);
            };
            command.CloseConnection();
            return GridRecords;
        }

        public List<FilteredProduct> GetFeaturedProductList(int ProductId, int PageNo, int RecordCount)
        {
            List<FilteredProduct> GridRecords = new List<FilteredProduct>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_FeaturedProduct_Get";
            command.AddParameter("@ProductId", ProductId, DbType.Int32);
            command.AddParameter("@PageNumber", PageNo, DbType.Int32);
            command.AddParameter("@NoOfRecord", RecordCount, DbType.Int32);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                FilteredProduct GridRecord = new FilteredProduct()
                {
                    RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
                    ProductTitle = reader.ValidateColumnExistExtractAndCastTo<string>("ProductTitle"),
                    ShortDescription = reader.ValidateColumnExistExtractAndCastTo<string>("ShortDescription"),
                    SKUCode = reader.ValidateColumnExistExtractAndCastTo<string>("SKUCode"),
                    Latitude = reader.ValidateColumnExistExtractAndCastTo<string>("Latitude"),
                    Longitude = reader.ValidateColumnExistExtractAndCastTo<string>("Longitude"),
                    ProductMRP = reader.ValidateColumnExistExtractAndCastTo<decimal>("ProductMRP"),
                    ThumbnailImage = reader.ValidateColumnExistExtractAndCastTo<string>("ThumbnailImage"),
                    Address = reader.ValidateColumnExistExtractAndCastTo<string>("Address"),
                };
                GridRecords.Add(GridRecord);
            };
            command.CloseConnection();
            return GridRecords;
        }

        // Need to match all levels of category to get similer product
        // during  product add, if we have data for all five levels then we have to pass all categories.. it is mandatory
        public List<FilteredProduct> GetSimilerProductList(int ProductId, int PageNo, int RecordCount)
        {
            List<FilteredProduct> GridRecords = new List<FilteredProduct>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_SimilerProduct_Get";
            command.AddParameter("@ProductId", ProductId, DbType.Int32);
            command.AddParameter("@PageNumber", PageNo, DbType.Int32);
            command.AddParameter("@NoOfRecord", RecordCount, DbType.Int32);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                FilteredProduct GridRecord = new FilteredProduct()
                {
                    RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
                    ProductTitle = reader.ValidateColumnExistExtractAndCastTo<string>("ProductTitle"),
                    ShortDescription = reader.ValidateColumnExistExtractAndCastTo<string>("ShortDescription"),
                    SKUCode = reader.ValidateColumnExistExtractAndCastTo<string>("SKUCode"),
                    Latitude = reader.ValidateColumnExistExtractAndCastTo<string>("Latitude"),
                    Longitude = reader.ValidateColumnExistExtractAndCastTo<string>("Longitude"),
                    ProductMRP = reader.ValidateColumnExistExtractAndCastTo<decimal>("ProductMRP"),
                    ThumbnailImage = reader.ValidateColumnExistExtractAndCastTo<string>("ThumbnailImage"),
                    Address = reader.ValidateColumnExistExtractAndCastTo<string>("Address"),
                };
                GridRecords.Add(GridRecord);
            };
            command.CloseConnection();
            return GridRecords;
        }

        public List<FilteredProduct> GetHomePageProducts(int PageNo, int RecordCount)
        {
            List<FilteredProduct> GridRecords = new List<FilteredProduct>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_HomePageProduct_Get";
            command.AddParameter("@PageNumber", PageNo, DbType.Int32);
            command.AddParameter("@NoOfRecord", RecordCount, DbType.Int32);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                FilteredProduct GridRecord = new FilteredProduct()
                {
                    RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
                    ProductTitle = reader.ValidateColumnExistExtractAndCastTo<string>("ProductTitle"),
                    ShortDescription = reader.ValidateColumnExistExtractAndCastTo<string>("ShortDescription"),
                    SKUCode = reader.ValidateColumnExistExtractAndCastTo<string>("SKUCode"),
                    Latitude = reader.ValidateColumnExistExtractAndCastTo<string>("Latitude"),
                    Longitude = reader.ValidateColumnExistExtractAndCastTo<string>("Longitude"),
                    ProductMRP = reader.ValidateColumnExistExtractAndCastTo<decimal>("ProductMRP"),
                    ThumbnailImage = reader.ValidateColumnExistExtractAndCastTo<string>("ThumbnailImage"),
                    Address = reader.ValidateColumnExistExtractAndCastTo<string>("Address"),
                };
                GridRecords.Add(GridRecord);
            };
            command.CloseConnection();
            return GridRecords;
        }

        public List<ProductSuggestion> GetProductSuggestion(string Suggestion)
        {
            List<ProductSuggestion> GridRecords = new List<ProductSuggestion>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductSuggestion_Get";
            command.AddParameter("@Suggestion", Suggestion, DbType.String);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                ProductSuggestion GridRecord = new ProductSuggestion()
                {
                    ProductTitle = reader.ValidateColumnExistExtractAndCastTo<string>("ProductTitle"),
                    ProductTypeId = reader.ValidateColumnExistExtractAndCastTo<int>("ProductTypeId"),
                    SubCategoryId = reader.ValidateColumnExistExtractAndCastTo<int>("SubCategoryId"),
                    SubCategoryParentId = reader.ValidateColumnExistExtractAndCastTo<int>("SubCategoryParentId"),
                    CategoryId = reader.ValidateColumnExistExtractAndCastTo<int>("CategoryId"),
                    ProductTypeName = reader.ValidateColumnExistExtractAndCastTo<string>("ProductTypeName"),
                    SubCategoryParentName = reader.ValidateColumnExistExtractAndCastTo<string>("SubCategoryParentName"),
                    SubCategoryName = reader.ValidateColumnExistExtractAndCastTo<string>("SubCategoryName"),
                    CategoryName = reader.ValidateColumnExistExtractAndCastTo<string>("CategoryName"),
                };
                GridRecords.Add(GridRecord);
            };
            command.CloseConnection();
            return GridRecords;
        }
        #endregion

        #region ProductType Attribute Mapping
        public bool AddAttribute_ProductType(Attribute_ProductTypeMapping request)
        {
            DeleteProductAttributebyProductTypeId(request);
            bool result = true;
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductType_ProductAttribute_i";
            SqlParameter[] sqlParams = new SqlParameter[6];
            sqlParams[0] = new SqlParameter("@CategoryParentId", DbType.Int32);
            sqlParams[1] = new SqlParameter("@CategoryId", DbType.Int32);
            sqlParams[2] = new SqlParameter("@SubCategoryParentId", DbType.Int32);
            sqlParams[3] = new SqlParameter("@SubCategoryId", DbType.Int32);
            sqlParams[4] = new SqlParameter("@ProductTypeId", DbType.Int32);
            sqlParams[5] = new SqlParameter("@ProductAttributeId", DbType.Int32);
            using (var transaction = new TransactionScope())
            {
                using (command.Connection)
                {
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();
                    foreach (var item in request.AttributeIds)
                    {
                        var data = new ProductAttribute();
                        command.Parameters.Clear();
                        sqlParams[0].Value = (object)request.CategoryParentId;
                        sqlParams[1].Value = (object)request.CategoryId;
                        sqlParams[2].Value = (object)request.SubCategoryParentId;
                        sqlParams[3].Value = (object)request.SubCategoryId;
                        sqlParams[4].Value = (object)request.ProductTypeId;
                        sqlParams[5].Value = (object)item;
                        command.Parameters.Add(sqlParams[0]);
                        command.Parameters.Add(sqlParams[1]);
                        command.Parameters.Add(sqlParams[2]);
                        command.Parameters.Add(sqlParams[3]);
                        command.Parameters.Add(sqlParams[4]);
                        command.Parameters.Add(sqlParams[5]);
                        command.ExecuteNonQuery();
                    }
                }
                transaction.Complete();
            }
            return result;
        }

        private bool DeleteProductAttributebyProductTypeId(Attribute_ProductTypeMapping request)
        {
            string QueryConditionPartParam = "";
            if (request.CategoryParentId == null)
            {
                QueryConditionPartParam = " CategoryParentId IS NULL ";
            }
            else
            {
                QueryConditionPartParam = " CategoryParentId=" + request.CategoryParentId + " ";
            }
            if (request.CategoryId == null)
            {
                QueryConditionPartParam += " AND CategoryId IS NULL ";
            }
            else
            {
                QueryConditionPartParam += " AND CategoryId=" + request.CategoryId + " ";
            }
            if (request.SubCategoryParentId == null)
            {
                QueryConditionPartParam += " AND SubCategoryParentId IS NULL ";
            }
            else
            {
                QueryConditionPartParam += " AND SubCategoryParentId=" + request.SubCategoryParentId + " ";
            }
            if (request.SubCategoryId == null)
            {
                QueryConditionPartParam += " AND SubCategoryId IS NULL ";
            }
            else
            {
                QueryConditionPartParam += " AND SubCategoryId=" + request.SubCategoryId + " ";
            }
            if (request.ProductTypeId == null)
            {
                QueryConditionPartParam += " AND ProductTypeId IS NULL ";
            }
            else
            {
                QueryConditionPartParam += " AND ProductTypeId=" + request.ProductTypeId + " ";
            }
            //QueryConditionPartParam += " AND CategoryId=" + request.CategoryId + " ";
            //QueryConditionPartParam += " AND SubCategoryParentId=" + request.SubCategoryParentId + " ";
            //QueryConditionPartParam += " AND SubCategoryId=" + request.SubCategoryId + " ";
            //QueryConditionPartParam += " AND ProductTypeId=" + request.ProductTypeId + " ";

            int result;
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductAttribute_ProductType_d";
            command.AddParameter("@QueryConditionPartParam", QueryConditionPartParam, DbType.String);
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

        public List<Attribute_ProductTypeMappingDTO> GetAllAttributeProductTypeMapping()
        {
            string QueryConditionPartParam = "";

            List<Attribute_ProductTypeMappingDTO> GridRecords = new List<Attribute_ProductTypeMappingDTO>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductType_ProductAttribute_Get";
            command.AddParameter("@QueryConditionPartParam", QueryConditionPartParam, DbType.String);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                Attribute_ProductTypeMappingDTO GridRecord = new Attribute_ProductTypeMappingDTO()
                {
                    RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
                    ProductTypeId = reader.ValidateColumnExistExtractAndCastTo<int>("ProductTypeId"),
                    ProductSubCategoryId = reader.ValidateColumnExistExtractAndCastTo<int>("SubCategoryId"),
                    ProductSubCategoryParentId = reader.ValidateColumnExistExtractAndCastTo<int>("SubCategoryParentId"),
                    ProductCategoryId = reader.ValidateColumnExistExtractAndCastTo<int>("CategoryId"),
                    ProductCategoryParentId = reader.ValidateColumnExistExtractAndCastTo<int>("CategoryParentId"),
                    ProductAttributeId = reader.ValidateColumnExistExtractAndCastTo<int>("ProductAttributeId"),
                    AttributeName = reader.ValidateColumnExistExtractAndCastTo<string>("AttributeName"),
                    ProductTypeName = reader.ValidateColumnExistExtractAndCastTo<string>("ProductTypeName"),
                    CategoryParentName = reader.ValidateColumnExistExtractAndCastTo<string>("CategoryParentName"),
                    CategoryName = reader.ValidateColumnExistExtractAndCastTo<string>("CategoryName"),
                    SubCategoryParentName = reader.ValidateColumnExistExtractAndCastTo<string>("SubCategoryParentName"),
                    SubCategoryName = reader.ValidateColumnExistExtractAndCastTo<string>("SubCategoryName"),
                };
                GridRecords.Add(GridRecord);
            };
            command.CloseConnection();
            return GridRecords;
        }
        public List<Attribute_ProductTypeMappingDTO> GetAttributeProductTypeMapping(int? CategoryParentId, int? CategoryId,
            int? SubCategoryParentId, int? SubCategoryId, int? ProductTypeId)
        {
            string QueryConditionPartParam = "";
            //QueryConditionPartParam = " AND a.CategoryParentId=" + CategoryParentId + " ";
            //QueryConditionPartParam += " AND a.CategoryId=" + CategoryId + " ";
            //QueryConditionPartParam += " AND a.SubCategoryParentId=" + SubCategoryParentId + " ";
            //QueryConditionPartParam += " AND a.SubCategoryId=" + SubCategoryId + " ";
            //QueryConditionPartParam += " AND a.ProductTypeId=" + ProductTypeId + " ";
            if (CategoryParentId == null || CategoryParentId == 0)
            {
                QueryConditionPartParam = " AND CategoryParentId IS NULL ";
            }
            else
            {
                QueryConditionPartParam = " AND CategoryParentId=" + CategoryParentId + " ";
            }
            if (CategoryId == null || CategoryId == 0)
            {
                QueryConditionPartParam += " AND CategoryId IS NULL ";
            }
            else
            {
                QueryConditionPartParam += " AND CategoryId=" + CategoryId + " ";
            }
            if (SubCategoryParentId == null || SubCategoryParentId == 0)
            {
                QueryConditionPartParam += " AND SubCategoryParentId IS NULL ";
            }
            else
            {
                QueryConditionPartParam += " AND SubCategoryParentId=" + SubCategoryParentId + " ";
            }
            if (SubCategoryId == null || SubCategoryId == 0)
            {
                QueryConditionPartParam += " AND SubCategoryId IS NULL ";
            }
            else
            {
                QueryConditionPartParam += " AND SubCategoryId=" + SubCategoryId + " ";
            }
            if (ProductTypeId == null || ProductTypeId == 0)
            {
                QueryConditionPartParam += " AND ProductTypeId IS NULL ";
            }
            else
            {
                QueryConditionPartParam += " AND ProductTypeId=" + ProductTypeId + " ";
            }
            List<Attribute_ProductTypeMappingDTO> GridRecords = new List<Attribute_ProductTypeMappingDTO>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductType_ProductAttribute_Get";
            command.AddParameter("@QueryConditionPartParam", QueryConditionPartParam, DbType.String);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                Attribute_ProductTypeMappingDTO GridRecord = new Attribute_ProductTypeMappingDTO()
                {
                    RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
                    ProductTypeId = reader.ValidateColumnExistExtractAndCastTo<int>("ProductTypeId"),
                    ProductSubCategoryId = reader.ValidateColumnExistExtractAndCastTo<int>("SubCategoryId"),
                    ProductSubCategoryParentId = reader.ValidateColumnExistExtractAndCastTo<int>("SubCategoryParentId"),
                    ProductCategoryId = reader.ValidateColumnExistExtractAndCastTo<int>("CategoryId"),
                    ProductCategoryParentId = reader.ValidateColumnExistExtractAndCastTo<int>("CategoryParentId"),
                    ProductAttributeId = reader.ValidateColumnExistExtractAndCastTo<int>("ProductAttributeId"),
                    AttributeName = reader.ValidateColumnExistExtractAndCastTo<string>("AttributeName"),
                    ProductTypeName = reader.ValidateColumnExistExtractAndCastTo<string>("ProductTypeName"),
                    CategoryParentName = reader.ValidateColumnExistExtractAndCastTo<string>("CategoryParentName"),
                    CategoryName = reader.ValidateColumnExistExtractAndCastTo<string>("CategoryName"),
                    SubCategoryParentName = reader.ValidateColumnExistExtractAndCastTo<string>("SubCategoryParentName"),
                    SubCategoryName = reader.ValidateColumnExistExtractAndCastTo<string>("SubCategoryName"),
                };
                GridRecords.Add(GridRecord);
            };
            command.CloseConnection();
            return GridRecords;
        }

        private Attributehierarchy TraverseCategory(List<AttributehierarchyRawData> lstData, int level, AttributehierarchyRawData childs)
        {
            Attributehierarchy menu = null;

            Console.WriteLine(childs.Name);
            menu = new Attributehierarchy();
            menu.ID = childs.RowId;
            menu.Name = childs.Name;
            menu.Level = childs.Level;

            var child = lstData.Where(a => a.Level == level + 1 && a.ParentId == childs.RowId);

            foreach (var ch in child)
            {
                var mm = TraverseCategory(lstData, ch.Level, ch);
                if (menu._ChildMenus == null)
                    menu._ChildMenus = new List<Attributehierarchy>();
                menu._ChildMenus.Add(mm);
            }
            return menu;
        }

        public List<Attributehierarchy> GetAttributehierarchy(AttributeRequest request)
        {
            string QueryConditionPartParam = "";
            if (request.CategoryParentId == null || request.CategoryParentId == 0)
            {
                QueryConditionPartParam = " AND CategoryParentId IS NULL ";
            }
            else
            {
                QueryConditionPartParam = " AND CategoryParentId=" + request.CategoryParentId + " ";
            }
            if (request.CategoryId == null || request.CategoryId == 0)
            {
                QueryConditionPartParam += " AND CategoryId IS NULL ";
            }
            else
            {
                QueryConditionPartParam += " AND CategoryId=" + request.CategoryId + " ";
            }
            if (request.SubCategoryParentId == null || request.SubCategoryParentId == 0)
            {
                QueryConditionPartParam += " AND SubCategoryParentId IS NULL ";
            }
            else
            {
                QueryConditionPartParam += " AND SubCategoryParentId=" + request.SubCategoryParentId + " ";
            }
            if (request.SubCategoryId == null || request.SubCategoryId == 0)
            {
                QueryConditionPartParam += " AND SubCategoryId IS NULL ";
            }
            else
            {
                QueryConditionPartParam += " AND SubCategoryId=" + request.SubCategoryId + " ";
            }
            if (request.ProductTypeId == null || request.ProductTypeId == 0)
            {
                QueryConditionPartParam += " AND ProductTypeId IS NULL ";
            }
            else
            {
                QueryConditionPartParam += " AND ProductTypeId=" + request.ProductTypeId + " ";
            }

            List<Attributehierarchy> lstResult = new List<Attributehierarchy>();
            List<AttributehierarchyRawData> GridRecords = new List<AttributehierarchyRawData>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_Attributehierarchy_Get";
            command.AddParameter("@QueryConditionPartParam", QueryConditionPartParam, DbType.String);
            //command.AddParameter("@CategoryParentId", request.CategoryParentId, DbType.Int32);
            //command.AddParameter("@CategoryId", request.CategoryId, DbType.Int32);
            //command.AddParameter("@SubCategoryParentId", request.SubCategoryParentId, DbType.Int32);
            //command.AddParameter("@SubCategoryId", request.SubCategoryId, DbType.Int32);
            //command.AddParameter("@ProductTypeId", request.ProductTypeId, DbType.Int32);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                AttributehierarchyRawData GridRecord = new AttributehierarchyRawData()
                {
                    RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
                    Name = reader.ValidateColumnExistExtractAndCastTo<string>("Name"),
                    ParentId = reader.ValidateColumnExistExtractAndCastTo<int>("ParentId"),
                    Level = reader.ValidateColumnExistExtractAndCastTo<int>("Level")
                };
                GridRecords.Add(GridRecord);
            };
            command.CloseConnection();
            var parentLst = GridRecords.Where(v => v.Level == 0).ToList();
            foreach (var parent in parentLst)
            {
                lstResult.Add(TraverseCategory(GridRecords, 0, parent));
            }
            return lstResult;
        }
        #endregion


        #region Product Mapping
        private bool AddProductAttribute_Product(ProductAttribute_Product request)
        {
            int result;
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductAttribute_Product_i";
            command.AddParameter("@ProductAttributeId", request.ProductAttributeId, DbType.Int32);
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
        public List<ProductAttribute_Product> GetProductAttributeMapping(int ProductId)
        {
            List<ProductAttribute_Product> GridRecords = new List<ProductAttribute_Product>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductAttribute_Product_Get";
            command.AddParameter("@ProductId", ProductId, DbType.Int32);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                ProductAttribute_Product gridRecord = new ProductAttribute_Product()
                {
                    RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
                    ProductAttributeId = reader.ValidateColumnExistExtractAndCastTo<int>("ProductAttributeId"),
                    ProductId = reader.ValidateColumnExistExtractAndCastTo<int>("ProductId"),
                    AttributeParent = reader.ValidateColumnExistExtractAndCastTo<string>("AttributeParent"),
                    AttributeName = reader.ValidateColumnExistExtractAndCastTo<string>("AttributeName"),
                    TextPrompt = reader.ValidateColumnExistExtractAndCastTo<string>("TextPrompt"),
                };
                GridRecords.Add(gridRecord);
            };
            command.CloseConnection();
            return GridRecords;
        }

        public bool UpdateProductAttributeProductList(ProductAttribute_ProductMappingDTO request)
        {
            DeleteProductAttributebyProductId(request.ProductId);

            ProductAttribute_Product AttributeRequest = new ProductAttribute_Product();
            AttributeRequest.ProductId = request.ProductId;
            foreach (var item in request.ProductAttributes)
            {
                AttributeRequest.ProductAttributeId = item;
                AddProductAttribute_Product(AttributeRequest);
            }
            return true;
        }

        public IList<ProductAttribute_Product> UpdateProductAttributeProduct(List<ProductAttribute_Product> tasks)
        {
            var result = new List<ProductAttribute_Product>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductAttribute_Product_u";
            SqlParameter[] sqlParams = new SqlParameter[2];
            sqlParams[0] = new SqlParameter("@RowId", DbType.Int32);
            sqlParams[1] = new SqlParameter("@TextPrompt", DbType.String);
            using (var transaction = new TransactionScope())
            {
                using (command.Connection)
                {
                    if (command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();
                    foreach (var item in tasks)
                    {
                        var data = new ProductAttribute_Product();
                        command.Parameters.Clear();
                        sqlParams[0].Value = (object)item.RowId;
                        sqlParams[1].Value = (object)item.TextPrompt;
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

        private bool DeleteProductAttributebyProductId(int ProductId)
        {
            int result;
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductAttribute_Product_d";
            command.AddParameter("@ProductId", ProductId, DbType.Int32);
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

        public IList<ProductAttribute_Product> DeleteProductAttributeMapping(List<ProductAttribute_Product> tasks)
        {
            var result = new List<ProductAttribute_Product>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductAttribute_Product_d_byId";
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
                        var data = new ProductAttribute_Product();
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


        private bool AddProductColor_Mapping(ProductColor_Mapping request)
        {
            int result;
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductColor_i";
            command.AddParameter("@ColorId", request.ColorId, DbType.Int32);
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
        private List<int> GetProductColor_Mapping(int ProductId)
        {
            List<int> GridRecords = new List<int>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductColor_Get";
            command.AddParameter("@ProductId", ProductId, DbType.Int32);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                GridRecords.Add(reader.ValidateColumnExistExtractAndCastTo<int>("RowId"));
            };
            command.CloseConnection();
            return GridRecords;
        }

        public List<ProductColor_Mapping> GetProductColorMappingList(int ProductId)
        {
            List<ProductColor_Mapping> GridRecords = new List<ProductColor_Mapping>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductAttribute_Product_Get";
            command.AddParameter("@ProductId", ProductId, DbType.Int32);
            command.OpenConnection();
            var reader = command.OpenReader();
            while (reader.Read())
            {
                ProductColor_Mapping gridRecord = new ProductColor_Mapping()
                {
                    RowId = reader.ValidateColumnExistExtractAndCastTo<int>("RowId"),
                    ColorId = reader.ValidateColumnExistExtractAndCastTo<int>("ColorId"),
                    ProductId = reader.ValidateColumnExistExtractAndCastTo<int>("ProductId"),
                    ColorName = reader.ValidateColumnExistExtractAndCastTo<string>("ColorName"),
                    ColorCodeRGB = reader.ValidateColumnExistExtractAndCastTo<string>("ColorCodeRGB"),
                    ColorCodeHex = reader.ValidateColumnExistExtractAndCastTo<string>("ColorCodeHex"),
                };
                GridRecords.Add(gridRecord);
            };
            command.CloseConnection();
            return GridRecords;
        }


        private int AddProductPrice(ProductPrice request)
        {
            int result = 0;
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductPrice_i";
            command.AddParameter("@ProductMRP", request.ProductMRP, DbType.Decimal);
            command.AddParameter("@RetailPrice", request.RetailPrice, DbType.Decimal);
            command.AddParameter("@SellingPrice", request.SellingPrice, DbType.Decimal);
            command.AddParameter("@SpecialPrice", request.SpecialPrice, DbType.Decimal);
            command.AddParameter("@RangeId", request.RangeId, DbType.Int32);
            command.AddParameter("@SpecialPriceStartDate", request.SpecialPriceStartDate, DbType.DateTime);
            command.AddParameter("@SpecialPriceEndDate", request.SpecialPriceEndDate, DbType.DateTime);
            command.AddParameter("@Comments", request.Comments, DbType.String);
            command.AddParameter("@InclusiveSalesTax", request.InclusiveSalesTax, DbType.Boolean);
            command.AddParameter("@ExchangeRateId", request.ExchangeRateId, DbType.Int32);
            try
            {
                command.OpenConnection();
                var reader = command.OpenReader();
                while (reader.Read())
                {
                    result = reader.ValidateColumnExistExtractAndCastTo<int>("RowId");
                }
            }
            finally
            {
                command.CloseConnection();
            }
            return result;
        }
        private IList<ProductPrice> UpdateProductPrice(ProductPrice item)
        {
            var result = new List<ProductPrice>();
            var command = dbContext.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_ProductPrice_u";
            SqlParameter[] sqlParams = new SqlParameter[11];
            sqlParams[0] = new SqlParameter("@RowId", DbType.Int32);
            sqlParams[1] = new SqlParameter("@ProductMRP", DbType.Decimal);
            sqlParams[2] = new SqlParameter("@RetailPrice", DbType.Decimal);
            sqlParams[3] = new SqlParameter("@SellingPrice", DbType.Decimal);
            sqlParams[4] = new SqlParameter("@SpecialPrice", DbType.Decimal);
            sqlParams[5] = new SqlParameter("@RangeId", DbType.Int32);
            sqlParams[6] = new SqlParameter("@SpecialPriceStartDate", DbType.DateTime);
            sqlParams[7] = new SqlParameter("@SpecialPriceEndDate", DbType.DateTime);
            sqlParams[8] = new SqlParameter("@Comments", DbType.String);
            sqlParams[9] = new SqlParameter("@InclusiveSalesTax", DbType.Boolean);
            sqlParams[10] = new SqlParameter("@ExchangeRateId", DbType.Int32);

            command.OpenConnection();

            var data = new ProductPrice();
            command.Parameters.Clear();
            sqlParams[0].Value = (object)item.RowId;
            sqlParams[1].Value = (object)item.ProductMRP;
            sqlParams[2].Value = (object)item.RetailPrice;
            sqlParams[3].Value = (object)item.SellingPrice;
            sqlParams[4].Value = (object)item.SpecialPrice;
            sqlParams[5].Value = (object)item.RangeId;
            sqlParams[6].Value = (object)item.SpecialPriceStartDate;
            sqlParams[7].Value = (object)item.SpecialPriceEndDate;
            sqlParams[8].Value = (object)item.Comments;
            sqlParams[9].Value = (object)item.InclusiveSalesTax;
            sqlParams[10].Value = (object)item.ExchangeRateId;
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

            command.ExecuteNonQuery();
            data.RowId = item.RowId;
            return result;
        }
        #endregion
    }
}
