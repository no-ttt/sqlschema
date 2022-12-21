namespace WebAPI.model
{
    public class ColumnValue
    {
        public int I { get; set; }
        public string Col { get; set; }
        public int K { get; set; }
        public int N { get; set; }
        public float Ratio { get; set; }
    }
    public class DataType
    {
        public string data_type { get; set; }
        public int columns { get; set; }
        public float percent_columns { get; set; }
        public int tables { get; set; }
        public float percent_tables { get; set; }
    }
    public class TbCount
    {
        public string tableCount { get; set; }
    }
}
