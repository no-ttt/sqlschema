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
    public class ColumnValueController : ControllerBase
    {
        /// <summary>
        /// 取得資料庫欄位值分布狀態
        /// </summary>
        [HttpGet]
        public IActionResult GetColumnValue(string Tab)
        {
            string strSql = @"drop table if exists #colAnalysis
                                create table #colAnalysis (
	                                I int, 
	                                Col nvarchar(20), 
	                                K int, 
	                                N int
                                )

                                declare @name nvarchar(100), @i int, @n int
                                declare @sqlStr varchar(1000)

                                declare aCursor cursor for 
                                select c.name as ColumnName, c.column_id, p.rows as N
                                from sys.all_columns c, sys.tables t 
                                left join sys.partitions p 
                                    on t.object_id = p.object_id and p.index_id = 1
                                where t.object_id = c.object_id
                                and t.name = @Tab

                                open aCursor  
	                                fetch next from aCursor into @name, @i, @n　while @@FETCH_STATUS = 0  
	                                begin
		                                select @sqlStr = 'insert #colAnalysis values(' + convert(varchar(20), @i) +
			                                ', ''' + @name +''', ( select count(distinct ' + @name + ') ' +
			                                '+ count(distinct case when ' + @name + ' is null then 1 end) from ' + @Tab + '), ' +
			                                convert(varchar(20), @n) + ' )'
		                                EXEC ( @sqlStr ) fetch next from aCursor into @name, @i, @n
	                                end
                                close aCursor
                                deallocate aCursor

                                select *, iif(N < 1, '', (convert(real, K) / N)) as 'Ratio' from #colAnalysis order by I";

            var p = new DynamicParameters();
            p.Add("@Tab", Tab);

            using (var db = new AppDb())
            {
                List<ColumnValue> data = db.Connection.Query<ColumnValue>(strSql, p).ToList();
                return Ok(new { data });
            }
        }
        /// <summary>
        /// 取得資料庫欄位值型態分布
        /// </summary>
        [HttpGet]
        [Route("DataType")]
        public IActionResult GetDataType()
        {
            string strSql = @"select t.name as data_type,
                                    count(*) as [columns],
                                    cast(100.0 * count(*) /
                                    (select count(*) from sys.tables as tab inner join
                                        sys.columns as col on tab.object_id = col.object_id)
                                            as numeric(36, 1)) as percent_columns,
                                      count(distinct tab.object_id) as [tables],
                                      cast(100.0 * count(distinct tab.object_id) /
                                      (select count(*) from sys.tables) as numeric(36, 1)) as percent_tables
                                  from sys.tables as tab
                                       inner join sys.columns as col
                                        on tab.object_id = col.object_id
                                       left join sys.types as t
                                        on col.user_type_id = t.user_type_id
                                group by t.name
                                order by percent_tables desc";

            string tbCount = @"select count(*) as tableCount from sys.tables";

            using (var db = new AppDb())
            {
                List<DataType> data = db.Connection.Query<DataType>(strSql).ToList();
                List<TbCount> count = db.Connection.Query<TbCount>(tbCount).ToList();
                return Ok(new { data, count });
            }
        }
    }
}
