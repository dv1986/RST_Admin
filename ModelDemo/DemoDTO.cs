using System;
using System.Reflection;

namespace ModelDemo
{
    public class DemoDTO
    {
        public int RowId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime DateField { get; set; }
        public decimal Decimal4Digit { get; set; }
        public decimal Decimal2Digit { get; set; }
        public int NumberField { get; set; }
        public string Phone { get; set; }
        public decimal CurrencyField { get; set; }
        public decimal Percentage { get; set; }
        public decimal PercentageDecimal { get; set; }
    }
}
