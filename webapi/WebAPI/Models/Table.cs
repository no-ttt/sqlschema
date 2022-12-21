namespace WebAPI.model
{
    public class Table
    {
        public string name { get; set; }
        public string created { get; set; }
        public string last_modified { get; set; }
        public string remark { get; set; }
    }
    public class Column
    {
        public int id { get; set; }
        public string name { get; set; }
        public string data_type { get; set; }
        public int max_length { get; set; }
        public int precision { get; set; }
        public bool not_nullable { get; set; }
        public string remark { get; set; }
    }
    public class Rel
    {
        public string foreign_table { get; set; }
        public string primary_table { get; set; }
        public string rel { get; set; }
        public string fk_constraint_name { get; set; }
    }
    public class Unique
    {
        public string key_name { get; set; }
        public string columns { get; set; }
        public string constraint_type { get; set; }
    }
    public class Use
    {
        public string table_name { get; set; }
    }
    public class Index
    {
        public string index_name { get; set; }
        public string type { get; set; }
        public string columns { get; set; }
        public string index_type { get; set; }

    }
}
