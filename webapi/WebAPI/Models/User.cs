namespace WebAPI.model
{
    public class User
    {
        public string name { get; set; }
        public string create_date { get; set; }
        public string modify_date { get; set; }
        public string type { get; set; }
        public string authentication_type { get; set; }
        public string not_disabled { get; set; }
        public string is_policy_checked { get; set; }
        public string is_expiration_checked { get; set; }

    }
    public class UserTable
    {
        public string user_name { get; set; }
        public string table_name { get; set; }
        public string privilege_type { get; set; }
        public string is_grantable { get; set; }
    }
    public class UserColumn
    {
        public string user_name { get; set; }
        public string table_name { get; set; }
        public string column_name { get; set; }
        public string privilege_type { get; set; }
        public string is_grantable { get; set; }
    }
}
