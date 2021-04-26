
using ModelProductImages;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceProductImage
{
    public interface IProductImageService
    {
        #region Upload Image
        ProductImages AddProductImages(ProductImages request);
        int GetMaxProductImageId();
        bool UpdateImage(int ImageId, string ModuleName, int RowId);
        #endregion

        #region ProductImage_Product
        List<ProductImageProduct> GetProductImage_Product(int ProductId);

        bool AddProductImage_Product(ProductImageProduct request);

        IList<ProductImageProduct> DeleteProductImage_Product_dyId(List<ProductImageProduct> tasks);

        bool DeleteProductImage_Product_dyProductId(ProductImageProduct request);
        #endregion

    }
}
