using System;
using System.Collections.Generic;
using System.Text;

namespace ModelProduct
{
    public class Product
    {
		public int RowId { get; set; }
		public string ProductTitle { get; set; }
		public string ShortDescription { get; set; }
		public string FullDescription { get; set; }
		public string SKUCode { get; set; }
		public int? SellerId { get; set; }
		public string AdminComment { get; set; }
		public bool ShowOnHomePage { get; set; }
		public bool AllowCustomerReviews { get; set; }
		public int? ApprovedRatingSum { get; set; }
		public int? NotApprovedRatingSum { get; set; }
		public int? ApprovedTotalReviews { get; set; }
		public int? NotApprovedTotalReviews { get; set; }
		public int StockQuantity { get; set; }
		public bool DisplayStockAvailability { get; set; }
		public bool DisplayStockQuantity { get; set; }
		public int MinStockQuantity { get; set; }
		public int? LowStockActivityId { get; set; }
		public int NotifyAdminForQuantityBelow { get; set; }
		public int OrderMinimumQuantity { get; set; }
		public int OrderMaximumQuantity { get; set; }
		public string AllowedQuantities { get; set; }
		public bool IsOutOfStock { get; set; }
		public bool DisableBuyButton { get; set; }
		public bool DisableWishlistButton { get; set; }
		public bool HasDiscountsApplied { get; set; }
		public int DisplayOrder { get; set; }
		public bool Published { get; set; }
		public bool IsDeleted { get; set; }
        public bool IsRecommended { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime CreatedOnUtc { get; set; }
		public DateTime UpdatedOnUtc { get; set; }
		public int QuantityBeforeUpdate { get; set; }
		public DateTime QuantityUpdatedOnUtc { get; set; }
		public int? DealProductCategoryId { get; set; }
		public int? CommisionRateId { get; set; }
		public DateTime DeletedOn { get; set; }
		public decimal TotalPercentOff { get; set; }
		public int? ProductFabricId { get; set; }
		public string FabricName { get; set; }
		public int? SeoContentId { get; set; }
		public string MetaTitle { get; set; }
		public string MetaKeyword { get; set; }
		public int? ProductTypeId { get; set; }
		public string ProductTypeName { get; set; }
		public int? SubCategoryId { get; set; }
		public string SubCategoryName { get; set; }
		public int? SubCategoryParentId { get; set; }
		public string SubCategoryParentName { get; set; }
		public int? CategoryId { get; set; }
		public string CategoryName { get; set; }
		public int CategoryParentId { get; set; }
		public string CategoryParentName { get; set; }
		public int? BrandId { get; set; }
		public string BrandShortName { get; set; }
		public string BrandFullName { get; set; }
		public int ProductTagId { get; set; }
		public string TagName { get; set; }
		public int ProductPriceId { get; set; }
		public decimal ProductMRP { get; set; }
		public decimal RetailPrice { get; set; }
		public decimal SellingPrice { get; set; }
		public decimal SpecialPrice { get; set; }
		public DateTime? SpecialPriceStartDate { get; set; }
		public DateTime? SpecialPriceEndDate { get; set; }
		public bool InclusiveSalesTax { get; set; }
        public string Comments { get; set; }
        public List<int> AttributeIds { get; set; }
        public List<int?> ColorIds { get; set; }
        public string Message { get; set; }
        public int ModelYear { get; set; }
        public string ModelNumber { get; set; }
        public int CreatedBy { get; set; }
    }
}
