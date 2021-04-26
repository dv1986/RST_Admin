namespace Infrastructure.Grid
{
    public class SortColumn
    {
        public string ColId { get; set; }

        public string Sort { get; set; }

        public override string ToString()
        {
            return $"{ColId} {Sort}";
        }
    }
}