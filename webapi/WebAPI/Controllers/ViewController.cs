using System.Collections.Generic;
using System.Linq;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Lib;
using WebAPI.model;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ViewController : ControllerBase
    {
        /// <summary>
        /// 取得資料庫所有 View Table
        /// </summary>
        [HttpGet]
        public IActionResult GetTable(string sortWay)
        {
            string strSql = @"select v.name,
                                    v.create_date as created,
                                    v.modify_date as last_modified,
                                    ISNULL(p.value, '') as remark
                                from sys.views v
                                    left join sys.extended_properties as p
                                        on v.object_id = p.major_id and p.minor_id = 0
                                order by 
		                                case @sortWay when 'name' then v.name
			                                when 'created' then v.create_date
			                                when 'last_modified' then v.modify_date
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
        /// 指定 View Table 的欄位
        /// </summary>
        [HttpGet]
        [Route("Column")]
        public IActionResult GetViewColumn(string Tab, string sortWay)
        {
            string strSql = @"select c.column_id as id,
                                    c.name as name,
                                    type_name(user_type_id) as data_type,
                                    c.max_length,
                                    c.precision,
	                                case c.is_nullable
		                                when 0 then 1
		                                when 1 then 0
	                                end as not_nullable,
	                                ISNULL(p.value, '') as remark
                                from sys.columns c
                                join sys.views v 
                                    on v.object_id = c.object_id
                                left join sys.extended_properties as p
                                    on v.object_id = p.major_id and c.column_id = p.minor_id
                                where object_name(c.object_id) = @Tab
                                order by 
		                                case @sortWay when 'name' then c.name
			                                when 'id' then c.column_id
			                                when 'data_type' then type_name(user_type_id)
			                                when 'remark' then p.value
		                                end";

            var p = new DynamicParameters();
            p.Add("@Tab", Tab);
            p.Add("@sortWay", sortWay);

            using (var db = new AppDb())
            {
                List<Column> data = db.Connection.Query<Column>(strSql, p).ToList();
                return Ok(new { tab = Tab, data });
            }
        }
        /// <summary>
        /// 指定 View table 引用的表 (uses)
        /// </summary>
        [HttpGet]
        [Route("Uses")]
        public IActionResult GetUses(string Tab)
        {
            string strSql = @"select distinct o.name as table_name
                                from sys.views v
                                join sys.sql_expression_dependencies d
                                     on d.referencing_id = v.object_id
                                     and d.referenced_id is not null
                                join sys.objects o
                                     on o.object_id = d.referenced_id
                                where v.name = @Tab
                                order by table_name";

            var p = new DynamicParameters();
            p.Add("@Tab", Tab);

            using (var db = new AppDb())
            {
                List<Use> data = db.Connection.Query<Use>(strSql, p).ToList();
                return Ok(new { data });
            }
        }
        /// <summary>
        /// 指定 View table 的 script
        /// </summary>
        [HttpGet]
        [Route("Script")]
        public IActionResult GetScript(string Tab)
        {
            string strSql = @"select m.definition as script
                                from sys.views v
                                join sys.sql_modules m 
                                        on m.object_id = v.object_id
                                where v.Name = @Tab";

            var p = new DynamicParameters();
            p.Add("@Tab", Tab);

            using (var db = new AppDb())
            {
                List<View> data = db.Connection.Query<View>(strSql, p).ToList();
                return Ok(new { data });
            }
        }
    }
}
