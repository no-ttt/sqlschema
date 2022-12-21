namespace WebAPI.model
{
    public class DiskOverview
    {
        public string name { get; set; }
        public float size { get; set; }
        public float spaceUsed { get; set; }
        public float spaceRemain { get; set; }
        public string position { get; set; }
    }
    public class DiskDataSpace
    {
        public float database_size { get; set; }
        public float unallocated_space { get; set; }
        public float reserved { get; set; }
        public float data { get; set; }
        public float index_size { get; set; }
        public float unused { get; set; }
    }
    public class DiskTableSpace
    {
        public string TableName { get; set; }
        public int Rows { get; set; }
        public int TotalPages { get; set; }
        public int UsedPages { get; set; }
        public int DataPages { get; set; }
        public float TotalSpaceKB { get; set; }
        public float UsedSpaceKB { get; set; }
        public float DataSpaceKB { get; set; }
    }
}
