using System;
using System.Collections.Generic;
using System.Text;

namespace ModelProduct
{
    public class Brands
    {
        public int RowId { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public int? ProductImageId { get; set; }
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
        public string ThumbnailPath { get; set; }
        public string ImageFileStream { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Message { get; set; }
    }
}
