
using ModelSpecifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceSpecification
{
    public interface ISpecificationService
    {
		#region Colors
		List<Colors> GetColors(string QueryConditionPartParam);
		bool AddColors(Colors request);
		IList<Colors> UpdateColors(List<Colors> tasks);
		IList<Colors> DeleteColors(List<Colors> tasks);
		#endregion


		#region MeasureDimension
		List<MeasureDimension> GetMeasureDimension(string QueryConditionPartParam);
		bool AddMeasureDimension(MeasureDimension request);
		IList<MeasureDimension> UpdateMeasureDimension(List<MeasureDimension> tasks);
		IList<MeasureDimension> DeleteMeasureDimension(List<MeasureDimension> tasks);
		#endregion


		#region ProductFabric
		List<ProductFabric> GetProductFabric(string QueryConditionPartParam);
		bool AddProductFabric(ProductFabric request);
		IList<ProductFabric> UpdateProductFabric(List<ProductFabric> tasks);
		IList<ProductFabric> DeleteProductFabric(List<ProductFabric> tasks);
		#endregion


		#region ProductTag
		List<ProductTag> GetProductTag(string QueryConditionPartParam);
		bool AddProductTag(ProductTag request);
		IList<ProductTag> UpdateProductTag(List<ProductTag> tasks);
		IList<ProductTag> DeleteProductTag(List<ProductTag> tasks);
		#endregion


		#region Size Type
		List<ProductSizeType> GetProductSizeType(string QueryConditionPartParam);
		bool AddProductSizeType(ProductSizeType request);
		IList<ProductSizeType> UpdateProductSizeType(List<ProductSizeType> tasks);
		IList<ProductSizeType> DeleteProductSizeType(List<ProductSizeType> tasks);
		#endregion
	}
}
