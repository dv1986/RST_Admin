using Infrastructure.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Query.FilterMaker
{
    public static class ColumnFilterExtension
    {
        public static  StringCriteria ToStringCriteria(this ColumnFilter filter,string fieldNameInDb)
        {
            return new StringCriteria(fieldNameInDb, filter.Filter, filter.GetOperator());
        }
        public static IntCriteria ToIntCriteria(this ColumnFilter filter, string fieldNameInDb)
        {
            return new IntCriteria(fieldNameInDb, filter.Filter, filter.GetOperator());
        }
        public static DecimalCriteria ToDecimalCriteria(this ColumnFilter filter, string fieldNameInDb)
        {
            return new DecimalCriteria(fieldNameInDb, filter.Filter, filter.GetOperator());
        }
        public static DateCriteria ToDateCriteria(this ColumnFilter filter, string fieldNameInDb)
        {
            return new DateCriteria(fieldNameInDb, filter.Filter, filter.GetOperator());
        }
        private static OperatorEnum GetOperator(this ColumnFilter filter)
        {
            OperatorEnum? result = null;
            switch (filter.Operation.ToLower())
            {
                case "equlas":
                case "equals":
                    result = OperatorEnum.Equals;
                    break;
                case "notEquals":
                    result = OperatorEnum.NotEquals;
                    break;
                case "lessthan":
                    result = OperatorEnum.LessThan;
                    break;
                case "greaterthan":
                    result = OperatorEnum.GreaterThan;
                    break;
                case "contains":
                    result = OperatorEnum.Contains;
                    break;
                case "notcontains":
                    result = OperatorEnum.NotContains;
                    break;
                case "startswith":
                    result = OperatorEnum.StartsWith;
                    break;
                case "endswith":
                    result = OperatorEnum.EndsWith;
                    break;
                default:
                    result = null;
                    break;
            }
            if (!result.HasValue)
            {
                throw new InvalidOperationException();
            }
            return result.Value;
        }
    }


    public static class SortColumnExtnsion
    {
        public static string Render(this List<SortColumn> lst)
        {
            StringBuilder srtb = new StringBuilder();
            if (lst != null && lst.Any())
            {                
                srtb.Append(" order by ");
                foreach (var item in lst)
                {
                    srtb.Append(item.ColId).Append(" ").Append(item.Sort).Append(",");
                }                      
            }
            var orderByStr = srtb.ToString();
            if (orderByStr.EndsWith(","))
            {
                orderByStr = orderByStr.Substring(0, orderByStr.Length - 1);
            }
            return orderByStr;

        }
    }
}
