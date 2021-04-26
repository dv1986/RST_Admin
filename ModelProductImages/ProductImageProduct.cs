using System;

namespace ModelProductImages
{
    public class ProductImageProduct
    {
        public int RowId { get; set; }
        public int ProductId { get; set; }
        public int ProductImageId { get; set; }
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
        public string ThumbnailPath { get; set; }
        public string ImageFileStream { get; set; }
        public string Message { get; set; }
    }
}
