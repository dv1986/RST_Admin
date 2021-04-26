using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO
{
    public static class ADOExtensions
    {
        public static DataTable ToTpStringList(this IList<string> data)
        {

            DataTable table = new DataTable();
            table.Columns.Add("Value", typeof(string));
            foreach (var item in data)
            {
                DataRow row = table.NewRow();
                row["Value"] = item;
                table.Rows.Add(row);
            }
            return table;
        }

    }
}
