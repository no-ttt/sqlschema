using System.Collections.Generic;

namespace WebAPI
{
    public class CountViolationUse
    {
        public int Violation { get; set; }
    }
    public class Result
    {
        public string Owner { get; set; }
        public string Principal { get; set; }
        public string Role { get; set; }
        public string Principal_Type { get; set; }
        public string Violation { get; set; }
        public string Member { get; set; }
        public string Login { get; set; }
        public string Permission_Class { get; set; }
        public string Permission { get; set; }
        public string Module { get; set; }
        public string Signing_Object { get; set; }
        public string Signing_Object_Owner { get; set; }
        public string Signing_Object_Thumbprint { get; set; }
        public string Last_Definition_Modify_Date { get; set; }
        public string Signing_Object_Type { get; set; }
        public string Object { get; set; }
        public string Schema { get; set; }
        public string Class_Separator { get; set; }
        public string database_user { get; set; }
        public string sid { get; set; }
        public string Endpoint_Name { get; set; }
        public string Endpoint_Type { get; set; }
        public string Database { get; set; }
        public int Encryption_State { get; set; }
        public string Key_Algorithm { get; set; }
        public int Key_Length { get; set; }
        public string Encryptor_Type { get; set; }
        public string Key_Name { get; set; }
        public string Certificate_Name { get; set; }
        public string Thumbprint { get; set; }
        public string name { get; set; }
        public string pvt_key_encryption_type_desc { get; set; }
        public string algorithm_desc { get; set; }
    }

    public class TermResult 
    {
        public string ID { get;  set; }
        public string Check { get; set; }
        public string Category { get; set; }
        public string Risk { get; set; }
        public string Des { get; set; }
        public string Impact { get; set; }
        public string Query { get; set; }
        public List<object> Result { get; set; }
        public List<CountViolationUse> ViolationUse { get; set; }
        public string Remediation { get; set; }
        public string RemediationScript { get; set; }
    }

    public class RiskRank
    {
        public int High { get; set; }
        public int Medium { get; set; }
        public int Low { get; set; }
    }

    public class ResultCount
    {
        public int passed { get; set; } 
        public int failed { get; set; }
        public List<RiskRank> Risk { get; set; }
    }

    public class Output
    {
        public List<TermResult> passed { get; set; }
        public List<TermResult> failed { get; set; }

    }
}

