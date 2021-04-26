namespace Infrastructure.Grid
{
    public class ColumnMetaData
    {
        public string TableName { get; set; }

        public string ColumnName { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Key
        {
            set
            {
                var pair = value.Split('$');
                if (pair.Length > 1)
                {
                    TableName = pair[0];
                    ColumnName = pair[1];
                }
            }
            get => $"{TableName}${ColumnName}";
        }

        public string DataType { get; set; }
        public string ColumnStyle { get; set; }
        public string Aggregate { get; set; }

        public string ColumnFormat { get; set; }
        public string Field { get; set; }
        public string AllowZero { get; set; }
    }
}