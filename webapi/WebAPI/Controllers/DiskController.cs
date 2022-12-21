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
    public class DiskController : ControllerBase
    {
        /// <summary>
        /// 磁碟使用概觀及檔案位置 (MB)
        /// </summary>
        [HttpGet]
        public IActionResult GetDiskOverview()
        {
            string strSql = @"select name,
                                    size / 128.0 as size,
                                    cast(fileproperty(name, 'SpaceUsed') AS INT) / 128.0 spaceUsed,
                                    size / 128.0 - cast(fileproperty(name, 'SpaceUsed') AS INT) / 128.0 spaceRemain,
                                    physical_name position
                                from
                                    sys.database_files";

            using (var db = new AppDb())
            {
                List<DiskOverview> data = db.Connection.Query<DiskOverview>(strSql).ToList();
                return Ok(new { data });
            }
        }
        /// <summary>
        /// 資料檔案空間使用 (MB)
        /// </summary>
        [HttpGet]
        [Route("DataSpace")]
        public IActionResult GetDiskDataSpace()
        {
            string strSql = @"declare @spaceUsed table (
                                    database_name varchar(255), 
                                    database_size varchar(50), 
                                    unallocated_space varchar(50),
                                    reserved varchar(50), 
                                    data varchar(50), 
                                    index_size varchar(50), 
                                    unused varchar(50)
                                )

                                insert into @spaceUsed
                                exec sp_spaceused @oneresultset = 1  

                                select 
	                                cast((select top 1 value from string_split(database_size, ' ')) as float) database_size,
	                                cast((select top 1 value from string_split(unallocated_space, ' ')) as float) unallocated_space,
	                                cast((select top 1 value from string_split(reserved, ' ')) as float) / 1024 reserved,
	                                cast((select top 1 value from string_split(data, ' ')) as float) / 1024 data,
	                                cast((select top 1 value from string_split(index_size, ' ')) as float) / 1024 index_size,
	                                cast((select top 1 value from string_split(unused, ' ')) as float) / 1024 unused
                                from @spaceUsed";

            using (var db = new AppDb())
            {
                List<DiskDataSpace> data = db.Connection.Query<DiskDataSpace>(strSql).ToList();
                return Ok(new { data });
            }
        }
        /// <summary>
        /// Table 磁碟使用
        /// </summary>
        [HttpGet]
        [Route("TableSpace")]
        public IActionResult GetDiskTableSpace()
        {
            string strSql = @"select 
                                    t.NAME AS TableName,
                                    p.[Rows],
                                    sum(a.total_pages) as TotalPages, 
                                    sum(a.used_pages) as UsedPages, 
                                    sum(a.data_pages) as DataPages,
                                    cast(sum(a.total_pages * 8) as numeric(36, 2)) as TotalSpaceKB,
                                    cast(sum(a.used_pages * 8) as numeric(36, 2)) as UsedSpaceKB,
                                    cast(sum(a.data_pages * 8) as numeric(36, 2)) as DataSpaceKB
                                from 
                                    sys.tables t
                                INNER JOIN      
                                    sys.indexes i ON t.OBJECT_ID = i.object_id
                                INNER JOIN 
                                    sys.partitions p ON i.object_id = p.OBJECT_ID AND i.index_id = p.index_id
                                INNER JOIN 
                                    sys.allocation_units a ON p.partition_id = a.container_id
                                WHERE 
                                    t.NAME NOT LIKE 'dt%' AND
                                    i.OBJECT_ID > 255 AND   
                                    i.index_id <= 1
                                GROUP BY 
                                    t.NAME, i.object_id, i.index_id, i.name, p.[Rows]
                                ORDER BY 
                                    object_name(i.object_id)";

            using (var db = new AppDb())
            {
                List<DiskTableSpace> data = db.Connection.Query<DiskTableSpace>(strSql).ToList();
                return Ok(new { data });
            }
        }
    }
}
