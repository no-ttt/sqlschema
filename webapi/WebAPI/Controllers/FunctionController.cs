using System.Collections.Generic;
using System.Linq;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Lib;
using WebAPI.model;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FunctionController : ControllerBase
    {
        /// <summary>
        /// 取得資料庫所有 Function
        /// </summary>
        [HttpGet]
        public IActionResult GetFunction(string sortWay)
        {
            string strSql = @"select obj.name as name,
	                                    obj.create_date as created,
	                                    obj.modify_date as last_modified,
		                                ISNULL(p.value, '') as remark
                                from sys.objects obj
                                left join sys.extended_properties as p
                                        on obj.object_id = p.major_id and p.minor_id = 0
                                where obj.type in ('FN', 'TF', 'IF')
                                order by 
		                                case @sortWay when 'name' then obj.name
			                                when 'created' then obj.create_date
			                                when 'last_modified' then obj.modify_date
			                                when 'remark' then p.value
		                                end";

            var p = new DynamicParameters();
            p.Add("@sortWay", sortWay);

            using (var db = new AppDb())
            {
                List<Table> data = db.Connection.Query<Table>(strSql, p).ToList();
                return Ok(new { data });
            }
        }
        /// <summary>
        /// 指定 Function 使用的 Object (uses)
        /// </summary>
        [HttpGet]
        [Route("Uses")]
        public IActionResult GetFunctionUses(string func)
        {
            string strSql = @"select dep_obj.name as object_name,
                                    dep_obj.type_desc as object_type
                                from sys.objects obj
                                left join sys.sql_expression_dependencies dep
                                    on dep.referencing_id = obj.object_id
                                left join sys.objects dep_obj
                                    on dep_obj.object_id = dep.referenced_id
                                where obj.type in ('AF', 'FN', 'FS', 'FT', 'IF', 'TF')
                                    and obj.name = @func and dep_obj.name is not null
                                order by object_name;";

            var p = new DynamicParameters();
            p.Add("@func", func);

            using (var db = new AppDb())
            {
                List<objUse> data = db.Connection.Query<objUse>(strSql, p).ToList();
                return Ok(new { data });
            }
        }
        /// <summary>
        /// 有使用到指定 Function 的 Object (used)
        /// </summary>
        [HttpGet]
        [Route("Used")]
        public IActionResult GetFunctionUsed(string func)
        {
            string strSql = @"select ref_o.name as object_name,
                                    ref_o.type_desc as object_type
                                from sys.objects o
                                join sys.sql_expression_dependencies dep
                                     on o.object_id = dep.referenced_id
                                join sys.objects ref_o
                                     on dep.referencing_id = ref_o.object_id
                                where o.type in ('FN', 'TF', 'IF')
                                      and o.name = @func and ref_o.name is not null
                                order by object_name";

            var p = new DynamicParameters();
            p.Add("@func", func);

            using (var db = new AppDb())
            {
                List<objUse> data = db.Connection.Query<objUse>(strSql, p).ToList();
                return Ok(new { data });
            }
        }
        /// <summary>
        /// 指定 Function 的 Type 和 Script
        /// </summary>
        [HttpGet]
        [Route("Info")]
        public IActionResult GetFunctionInfo(string func)
        {
            string strSql = @"select case type
                                            when 'FN' then 'SQL scalar function'
                                            when 'TF' then 'SQL table-valued-function'
                                            when 'IF' then 'SQL inline table-valued function'
                                        end as type,
                                        mod.definition as script
                                from sys.objects obj
                                join sys.sql_modules mod
                                     on mod.object_id = obj.object_id
                                where obj.type in ('FN', 'TF', 'IF') and obj.name = @func";

            var p = new DynamicParameters();
            p.Add("@func", func);

            using (var db = new AppDb())
            {
                List<Info> data = db.Connection.Query<Info>(strSql, p).ToList();
                return Ok(new { data });
            }
        }
    }
}
