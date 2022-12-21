using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebAPI.Lib;
using WebAPI.model;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExtendedpropertyController : ControllerBase
    {
        /// <summary>
        /// 新增 Table / View / Function / Procedure 備註 (type : TABLE / VIEW / FUNCTION / PROCEDURE)
        /// </summary>
        [HttpPost]
        [Route("{level1type}")]
        public IActionResult PostRemark(Extendedproperty Info, string level1type)
        {
            //string strSql = @"EXEC sp_addextendedproperty 
            //                        @name = 'MS_Description',  @value = @value, 
            //                        @level0type = 'SCHEMA', @level0name = 'dbo',  
            //                        @level1type = 'TABLE', @level1name = @tableName,
            //                        @level2type = 'COLUMN', @level2name = @columnName
            //                    GO";
            // 如果存在進行更新

            string description_name = $"DS_{Info.tableName}_{Info.columnName}";

            // 如果不存在進行新增
            using (var db = new AppDb())
            {
                // 存在搜尋
                string sqlstr = @"select top 1 1 from sys.extended_properties where name = @description_name";

                var p1 = new DynamicParameters();
                p1.Add("@description_name", description_name);

                bool flag;
                flag = db.Connection.QueryFirstOrDefault<bool>(sqlstr, p1);

                string sp = flag == false ? @"sp_addextendedproperty" : @"sp_updateextendedproperty";         
                
                var p2 = new DynamicParameters();

                p2.Add("@name", $"DS_{Info.tableName}_{Info.columnName}");
                p2.Add("@value", Info.value);
                p2.Add("@level0type", "SCHEMA");
                p2.Add("@level0name", "dbo");
                p2.Add("@level1type", level1type);
                p2.Add("@level1name", Info.tableName);

                if (Info.columnName != "")
                {
                    if (level1type == "TABLE" || level1type == "VIEW")
                        p2.Add("@level2type", "COLUMN");

                    if (level1type == "FUNCTION" || level1type == "PROCEDURE")
                        p2.Add("@level2type", "PARAMETER");
                    
                    p2.Add("@level2name", Info.columnName);
                }

                //List<Extendedproperty> data = db.Connection.Query<Extendedproperty>(strSql).ToList();
                db.Connection.Execute(sp, p2, commandType: System.Data.CommandType.StoredProcedure);                
            }

            return Ok();
        }
    }
}
