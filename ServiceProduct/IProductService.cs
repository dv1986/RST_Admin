using ModelProduct;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceProduct
{
    public interface IProductService
    {


        #region Brand
        bool AddBrands(Brands request);
        IList<Brands> UpdateBrands(List<Brands> tasks);
        IList<Brands> DeleteBrands(List<Brands> tasks);
        List<Brands> GetBrands(string QueryConditionPartParam);
        #endregion


        #region Product Attribute Parent
        List<ProductAttributeParent> GetProductAttributeParent(string QueryConditionPartParam);
        List<ProductAttributeParent> GetProductAttributeParentbyProdcutType(int ProductTypeId);
        bool AddProductAttributeParent(ProductAttributeParent request);
        IList<ProductAttributeParent> UpdateProductAttributeParent(List<ProductAttributeParent> tasks);
        IList<ProductAttributeParent> DeleteProductAttributeParent(List<ProductAttributeParent> tasks);
        #endregion


        #region Product Attribute
        List<ProductAttribute> GetProductAttribute(string QueryConditionPartParam);
        bool AddProductAttribute(ProductAttribute request);
        IList<ProductAttribute> UpdateProductAttribute(List<ProductAttribute> tasks);
        IList<ProductAttribute> DeleteProductAttribute(List<ProductAttribute> tasks);
        #endregion


        #region Product
        int AddProduct(Product product);
        IList<Product> UpdateProduct(List<Product> tasks);
        IList<Product> DeleteProduct(List<Product> tasks);
        List<Product> GetProduct(string QueryConditionPartParam);

        List<FilteredProduct> GetFilteredProductList(string ProductName, string year, int CategoryId, int ParentSubCategoryId, int SubCategoryId, int ProductTypeId,
            int BrandId, int PriceRangeMin, int PriceRangeMax, string SortBy, bool IsAsc, int PageNumber, int NoOfRecord);

        ProductRange GetMinMaxPriceRange(string year, int CategoryId, int ParentSubCategoryId, int SubCategoryId, int ProductTypeId, int BrandId);

        ProductDetails GetProductDetailbyId(int ProductId);

        List<FilteredProduct> GetRecommendedProducts(int ProductId, int PageNo, int RecordCount);

        List<FilteredProduct> GetFeaturedProductList(int ProductId, int PageNo, int RecordCount);

        List<FilteredProduct> GetSimilerProductList(int ProductId, int PageNo, int RecordCount);
        List<FilteredProduct> GetHomePageProducts(int PageNo, int RecordCount);
        List<ProductSuggestion> GetProductSuggestion(string Suggestion);
        #endregion

        #region ProductType Attribute Mapping
        bool AddAttribute_ProductType(Attribute_ProductTypeMapping request);
        List<Attribute_ProductTypeMappingDTO> GetAllAttributeProductTypeMapping();
        List<Attribute_ProductTypeMappingDTO> GetAttributeProductTypeMapping(int? CategoryParentId, int? CategoryId,
            int? SubCategoryParentId, int? SubCategoryId, int? ProductTypeId);
        List<Attributehierarchy> GetAttributehierarchy(AttributeRequest request);
        #endregion

        #region Product Mapping
        List<ProductAttribute_Product> GetProductAttributeMapping(int ProductId);
        bool UpdateProductAttributeProductList(ProductAttribute_ProductMappingDTO request);
        IList<ProductAttribute_Product> UpdateProductAttributeProduct(List<ProductAttribute_Product> tasks);
        IList<ProductAttribute_Product> DeleteProductAttributeMapping(List<ProductAttribute_Product> tasks);
        #endregion
    }
}
