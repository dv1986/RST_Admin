using System;

namespace Infrastructure.Grid
{
    public class DataGridUpdateDataRequest
    {
        public dynamic DataToUpdate { get; set; }

        public string GridGuid { get; set; }

        public string KeyField { get; set; }

    }
}