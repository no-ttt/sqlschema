namespace WebAPI.model
{
    public class DocTable
    {
        public string tableName { get; set; }
    }
    public class DocColumn
    {
        public string tableName { get; set; }
        public string name { get; set; }
        public string data_type { get; set; }
        public string des { get; set;  }
    }
    public class DocTableDes
    {
        public string tbName { get; set; }
        public string tbDes { get; set; }
    }
}
