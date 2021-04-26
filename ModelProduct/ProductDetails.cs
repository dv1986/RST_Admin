using System;
using System.Collections.Generic;
using System.Text;

namespace ModelProduct
{
    public class ProductDetails
    {
		public int RowId { get; set; }
		public string ProductTitle { get; set; }
		public string ShortDescription { get; set; }
		public string FullDescription { get; set; }
		public string SKUCode { get; set; }
		public string AdminComment { get; set; }
		public bool ShowOnHomePage { get; set; }
		public bool Published { get; set; }
		public decimal TotalPercentOff { get; set; }
		public string FabricName { get; set; }
		public string ProductTypeName { get; set; }
		public string SubCategoryName { get; set; }
		public string SubCategoryParentName { get; set; }
		public string CategoryName { get; set; }
		public string CategoryParentName { get; set; }
		public string BrandShortName { get; set; }
		public string BrandFullName { get; set; }
		public string TagName { get; set; }
		public decimal ProductMRP { get; set; }
		public string ContactPersonName { get; set; }
		public string Latitude { get; set; }
		public string Longitude { get; set; }
		public string ContactNumber { get; set; }
		public string ThumbnailImage { get; set; }
		public int ModelYear { get; set; }
		public string ModelNumber { get; set; }
		public string Address { get; set; }
        public List<ProductAttribute_Product> AttributeLst { get; set; }
    }
}
