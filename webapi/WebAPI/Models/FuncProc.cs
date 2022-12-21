namespace WebAPI.model
{
    public class Params
    {
        public string mode { get; set; }
        public string name { get; set; }
        public string data_type { get; set; }
        public int max_length { get; set; }
        public int precision { get; set; }
        public bool not_nullable { get; set; }
        public string remark { get; set; }

    }
    public class objUse
    {
        public string object_name { get; set; }
        public string object_type { get; set; }
    }
    public class Info
    {
        public string type { get; set; }
        public string script { get; set; }
    }
}
