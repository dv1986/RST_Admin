using ModelCategories;
using ModelCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCategories
{
    public interface ICategoriesService
    {
        #region Category Parent
        bool AddProductCategoryParent(ProductCategoryParent productCategoryParent);
        IList<ProductCategoryParent> UpdateProductCategoryParent(List<ProductCategoryParent> tasks);
        IList<ProductCategoryParent> DeleteProductCategoryParent(List<ProductCategoryParent> tasks);
        List<ProductCategoryParent> GetProductCategoryParent(string QueryConditionPartParam);
        #endregion

        #region Category
        bool AddProductCategory(ProductCategory productCategory);
        IList<ProductCategory> UpdateProductCategory(List<ProductCategory> tasks);
        IList<ProductCategory> DeleteProductCategory(List<ProductCategory> tasks);
        List<ProductCategory> GetProductCategory(string SearchStr);
        List<ProductCategory> GetCategoryLookup(int CategoryParentId);
        #endregion

        #region Product-Type
        bool AddProductType(ProductType Productype);
        IList<ProductType> UpdateProductType(List<ProductType> tasks);
        IList<ProductType> DeleteProductType(List<ProductType> tasks);
        List<ProductType> GetProductType(string SearchStr);
        List<ProductType> GetProductTypeLookup(int SubCategoryParentId);
        #endregion

        #region Sub-Category-Parent
        bool AddProductSubCategoryParent(ProductSubCategoryParent request);
        IList<ProductSubCategoryParent> UpdateProductSubCategoryParent(List<ProductSubCategoryParent> tasks);
        IList<ProductSubCategoryParent> DeleteProductSubCategoryParent(List<ProductSubCategoryParent> tasks);
        List<ProductSubCategoryParent> GetProductSubCategoryParent(string QueryConditionPartParam);
        List<ProductSubCategoryParent> GetSubCategoryParentLookup(int CategoryId);
        #endregion

        #region Sub-Category
        bool AddProductSubCategory(ProductSubCategory productSubCategory);
        IList<ProductSubCategory> UpdateProductSubCategory(List<ProductSubCategory> tasks);
        IList<ProductSubCategory> DeleteProductSubCategory(List<ProductSubCategory> tasks);
        List<ProductSubCategory> GetProductSubCategory(string QueryConditionPartParam);
        List<ProductSubCategory> GetSubCategoryLookup(int SubCategoryParentId);
        #endregion


        #region Product-Feature-Category
        List<ProductsFeaturesCategory> GetProductsFeaturesCategory(string QueryConditionPartParam);
        bool AddProductsFeaturesCategory(ProductsFeaturesCategory request);
        IList<ProductsFeaturesCategory> UpdateProductsFeaturesCategory(List<ProductsFeaturesCategory> tasks);
        IList<ProductsFeaturesCategory> DeleteProductsFeaturesCategory(List<ProductsFeaturesCategory> tasks);
        #endregion


        #region Product-Feature
        List<ProductFeatures> GetProductFeatures(string QueryConditionPartParam);
        bool AddProductFeatures(ProductFeatures request);
        IList<ProductFeatures> UpdateProductFeatures(List<ProductFeatures> tasks);
        IList<ProductFeatures> DeleteProductFeatures(List<ProductFeatures> tasks);
        #endregion

        Categoryhierarchy GetCategoryhierarchy();

    }
}
