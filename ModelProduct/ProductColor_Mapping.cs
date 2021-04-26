using System;
using System.Collections.Generic;
using System.Text;

namespace ModelProduct
{
    public class ProductColor_Mapping
    {
        public int RowId { get; set; }
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public string ColorCodeRGB { get; set; }
        public string ColorCodeHex { get; set; }
        public int ProductId { get; set; }
    }
}
