using System;
using System.Collections.Generic;
using System.Text;

namespace ModelProduct
{
    public class ProductPrice
    {
        public int RowId { get; set; }
        public decimal ProductMRP { get; set; }
        public decimal RetailPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal SpecialPrice { get; set; }
        public int RangeId { get; set; }
        public DateTime? SpecialPriceStartDate { get; set; }
        public DateTime? SpecialPriceEndDate { get; set; }
        public string Comments { get; set; }
        public bool InclusiveSalesTax{ get; set; }
        public int ExchangeRateId { get; set; }
    }
}
