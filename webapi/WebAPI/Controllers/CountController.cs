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
    public class CountController : ControllerBase
    {
        /// <summary>
        /// 取得資料庫 table/view/function/procedure 數量
        /// </summary>
        [HttpGet]
        public IActionResult GetCount()
        {
            string strSql = @"select
                                (select count(*) from sys.tables t) as table_count,
                                (select count(*) from sys.views)  as view_count,
                                (select count(*) from sys.sql_modules m 
	                                inner join sys.objects o
	                                on m.object_id = o.object_id
                                where type_desc like '%function%') as function_count,
                                (select count(*) from sys.sql_modules m 
	                                inner join sys.objects o
	                                on m.object_id = o.object_id
                                where type_desc like '%procedure%') as procedure_count";

            using (var db = new AppDb())
            {
                List<Count> data = db.Connection.Query<Count>(strSql).ToList();
                return Ok(new { data });
            }
        }
    }
}
