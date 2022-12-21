using System.Collections.Generic;
using System.Linq;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Lib;
using WebAPI.model;
using DocxProcessor;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TableController : ControllerBase
    {
        /// <summary>
        /// 取得資料庫所有 Table
        /// </summary>
        [HttpGet]
        public IActionResult GetTable(string sortWay)
        {
            
             string strSql = @"select t.name as name,
                                        t.create_date as created,
                                        t.modify_date as last_modified,
		                                ISNULL(p.value, '') as remark
                                from sys.tables t
                                left join sys.extended_properties as p
		                                on t.object_id = p.major_id and p.minor_id = 0
                                order by 
		                                case @sortWay when 'name' then t.name
			                                when 'created' then t.create_date
			                                when 'last_modified' then t.modify_date
			                                when 'remark' then p.value
		                                end";


            //string strSql = @"select t.name,
            //                            t.create_date as created,
            //                            t.modify_date as last_modified,
            //                      ISNULL(p.value, '') as remark
            //                    from sys.tables t
            //                    left join sys.extended_properties as p
            //                      on t.object_id = p.major_id and p.minor_id = 0
            //                    order by name";

            var p = new DynamicParameters();
            p.Add("@sortWay", sortWay);

            using (var db = new AppDb())
            {
                List<Table> data = db.Connection.Query<Table>(strSql, p).ToList();
                return Ok(new { data });
            }
        }
        /// <summary>
        /// 指定 Table 的欄位
        /// </summary>
        [HttpGet]
        [Route("Column")]
        public IActionResult GetTabColumn(string Tab, string sortWay)
        {
            string strSql = @"select col.column_id as id,
                                    col.name,
                                    t.name as data_type,
                                    col.max_length,
                                    col.precision,
	                                case col.is_nullable
		                                when 0 then 1
		                                when 1 then 0
	                                end as not_nullable,
	                                ISNULL(p.value, '') as remark
                                from sys.tables as tab
                                    inner join sys.columns as col
                                        on tab.object_id = col.object_id
                                    left join sys.types as t
		                                on col.user_type_id = t.user_type_id
	                                left join sys.extended_properties as p
		                                on tab.object_id = p.major_id and col.column_id = p.minor_id
                                where tab.name = @Tab
                                order by 
		                                case @sortWay when 'name' then col.name
			                                when 'id' then col.column_id
			                                when 'data_type' then t.name
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
        /// 指定 Table 的相依資訊
        /// </summary>
        [HttpGet]
        [Route("Relation")]
        public IActionResult GetRel(string Tab)
        {
            string strSql = @"select fk_tab.name as foreign_table,
                                    pk_tab.name as primary_table,
	                                fk_tab.name + '.' + fk_col.name + ' → ' + pk_tab.name + '.' + pk_col.name as rel,
                                    fk.name as fk_constraint_name
                                from sys.foreign_keys fk
                                    inner join sys.tables fk_tab
                                        on fk_tab.object_id = fk.parent_object_id
                                    inner join sys.tables pk_tab
                                        on pk_tab.object_id = fk.referenced_object_id
                                    inner join sys.foreign_key_columns fk_cols
                                        on fk_cols.constraint_object_id = fk.object_id
                                    inner join sys.columns fk_col
                                        on fk_col.column_id = fk_cols.parent_column_id
                                        and fk_col.object_id = fk_tab.object_id
                                    inner join sys.columns pk_col
                                        on pk_col.column_id = fk_cols.referenced_column_id
                                        and pk_col.object_id = pk_tab.object_id
                                where fk_tab.name = @Tab or pk_tab.name = @Tab
                                order by fk_tab.name, pk_tab.name";

            var p = new DynamicParameters();
            p.Add("@Tab", Tab);

            using (var db = new AppDb())
            {
                List<Rel> data = db.Connection.Query<Rel>(strSql, p).ToList();
                return Ok(new { data });
            }
        }
        /// <summary>
        /// 指定 Table 的 unique key
        /// </summary>
        [HttpGet]
        [Route("Unique")]
        public IActionResult GetUnique(string Tab)
        {
            string strSql = @"select i.name as key_name,
	                                substring(column_names, 1, len(column_names)-1) as columns,
                                    case when c.type = 'PK' then 'Primary key'
                                        when c.type = 'UQ' then 'Unique constraint'
                                        when i.type = 1 then 'Unique clustered index'
                                        when i.type = 2 then 'Unique index'
                                        end as constraint_type
                                from sys.objects t
                                    left outer join sys.indexes i
                                        on t.object_id = i.object_id
                                    left outer join sys.key_constraints c
                                        on i.object_id = c.parent_object_id 
                                        and i.index_id = c.unique_index_id
                                   cross apply (select col.name + ', '
                                                    from sys.index_columns ic
                                                        inner join sys.columns col
                                                            on ic.object_id = col.object_id
                                                            and ic.column_id = col.column_id
                                                    where ic.object_id = t.object_id
                                                        and ic.index_id = i.index_id
                                                            order by col.column_id
                                                            for xml path ('') ) D (column_names)
                                where is_unique = 1
                                and t.is_ms_shipped <> 1
                                and t.name = @Tab
                                order by t.name";

            var p = new DynamicParameters();
            p.Add("@Tab", Tab);

            using (var db = new AppDb())
            {
                List<Unique> data = db.Connection.Query<Unique>(strSql, p).ToList();
                return Ok(new { data });
            }
        }
        /// <summary>
        /// 指定 Table 引用的表 (uses)
        /// </summary>
        [HttpGet]
        [Route("Uses")]
        public IActionResult GetUses(string Tab)
        {
            string strSql = @"select distinct pk_tab.name as table_name
                                from sys.foreign_keys fk
                                    inner join sys.tables fk_tab
                                        on fk_tab.object_id = fk.parent_object_id
                                    inner join sys.tables pk_tab
                                        on pk_tab.object_id = fk.referenced_object_id
                                where fk_tab.name = @Tab
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
        /// 引用指定 Table 的表 (used)
        /// </summary>
        [HttpGet]
        [Route("Used")]
        public IActionResult GetUsed(string Tab)
        {
            string strSql = @"select distinct fk_tab.name as table_name
                                from sys.foreign_keys fk
                                    inner join sys.tables fk_tab
                                        on fk_tab.object_id = fk.parent_object_id
                                    inner join sys.tables pk_tab
                                        on pk_tab.object_id = fk.referenced_object_id
                                where pk_tab.name = @Tab
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
        /// 指定 Table 的 index
        /// </summary>
        [HttpGet]
        [Route("Index")]
        public IActionResult GetIndex(string Tab)
        {
            string strSql = @"select i.name as index_name,
                                case when i.is_primary_key = 1 then 'Primary key'
                                    when i.is_unique = 1 then 'Unique'
                                    else 'Not unique' end as type,
                                substring(column_names, 1, len(column_names)-1) as columns,
                                case when i.type = 1 then 'Clustered index'
                                    when i.type = 2 then 'Nonclustered unique index'
                                    when i.type = 3 then 'XML index'
                                    when i.type = 4 then 'Spatial index'
                                    when i.type = 5 then 'Clustered columnstore index'
                                    when i.type = 6 then 'Nonclustered columnstore index'
                                    when i.type = 7 then 'Nonclustered hash index'
                                    end as index_type
                            from sys.objects t
                                inner join sys.indexes i
                                    on t.object_id = i.object_id
                                cross apply (select col.name + ', '
                                                from sys.index_columns ic
                                                    inner join sys.columns col
                                                        on ic.object_id = col.object_id
                                                        and ic.column_id = col.column_id
                                                where ic.object_id = t.object_id
                                                    and ic.index_id = i.index_id
                                                        order by col.column_id
                                                        for xml path ('') ) D (column_names)
                            where t.is_ms_shipped <> 1 and index_id > 0 and t.name = @Tab
                            order by t.name";

            var p = new DynamicParameters();
            p.Add("@Tab", Tab);

            using (var db = new AppDb())
            {
                List<Index> data = db.Connection.Query<Index>(strSql, p).ToList();
                return Ok(new { data });
            }
        }
    }
}
