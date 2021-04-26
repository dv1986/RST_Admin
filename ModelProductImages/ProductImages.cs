using System;
using System.Collections.Generic;
using System.Text;

namespace ModelProductImages
{
    public class ProductImages
    {
        public int RowId { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
        public bool IsDisplay { get; set; }
        public int DisplayOrder { get; set; }
        public string ThumbnailPath { get; set; }

    }
}
