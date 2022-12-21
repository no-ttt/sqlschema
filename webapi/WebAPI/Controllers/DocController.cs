using System.Collections.Generic;
using System.Linq;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Lib;
using WebAPI.model;
using DocxProcessor;
using System.IO;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocController : ControllerBase
    {
        /// <summary>
        /// 結案文件生成
        /// </summary>
        [HttpGet]
        public IActionResult GetDocx()
        {
            List<DocTable> table = new List<DocTable>();
            List<DocColumn> column = new List<DocColumn>();
            List<DocTableDes> tableDes = new List<DocTableDes>();
            List<Stream> docxs = new List<Stream>();

            // 所有 table
            string tableSql = @"select t.name as tableName from sys.tables t order by name";

            // 所有欄位註解
            string columnSql = @"select tab.name as tableName,
                                        col.name,
                                        t.name as data_type,
	                                    ISNULL(p.value, '') as des
                                    from sys.tables as tab
                                        inner join sys.columns as col
                                            on tab.object_id = col.object_id
                                        left join sys.types as t
		                                    on col.user_type_id = t.user_type_id
	                                    left join sys.extended_properties as p
		                                    on tab.object_id = p.major_id and col.column_id = p.minor_id
                                    order by tab.name, column_id";

            // 所有 table 的註解
            string tableDesSql = @"select t.name as tbName,
                                        ISNULL(p.value, '') as tbDes
                                    from sys.tables t
                                        left join sys.extended_properties as p
                                            on t.object_id = p.major_id and p.minor_id = 0
                                    order by tbName";

            using (var db = new AppDb())
            {
                table = db.Connection.Query<DocTable>(tableSql).ToList();
                column = db.Connection.Query<DocColumn>(columnSql).ToList();
                tableDes = db.Connection.Query<DocTableDes>(tableDesSql).ToList();
            }

            // 從實體路徑讀檔案
            //byte[] docx = System.IO.File.ReadAllBytes(".\\Templates\\tableList.docx");
            //byte[] docx2 = System.IO.File.ReadAllBytes(".\\Templates\\tableDetailList.docx");

            byte[] docx = System.IO.File.ReadAllBytes(Path.GetRelativePath(".", "Templates/tableList.docx"));
            byte[] docx2 = System.IO.File.ReadAllBytes(Path.GetRelativePath(".", "Templates/tableDetailList.docx"));

            // 取代
            var wordProcessor = new ReplaceWordTemplate();

            byte[] tmp;
            tmp = wordProcessor.Replace(docx, tableDes);
            docxs.Add(new MemoryStream(tmp));

            foreach (var tb in table)
            {
                var col = column.Where(c => c.tableName == tb.tableName)
                    .Select(c => new { c.name, c.data_type, c.des })
                    .ToList();
                
                tmp = wordProcessor.Replace(docx2, col);
                tmp = wordProcessor.Replace(tmp, tb);

                docxs.Add(new MemoryStream(tmp));
            }
            

            // 取套件
            MergeWordTemplate WordMerger = new MergeWordTemplate();

            // 合併文件
            byte[] final = WordMerger.MergeDocxsIntoOne(docxs);

            // 設定檔名
            string fileName = "document";

            // 回傳合併檔案
            return File(
                fileContents: final,
                contentType: "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                fileDownloadName: $"{fileName}.docx"
            );
        }
        /// <summary>
        /// 交接文件生成
        /// </summary>
        [HttpGet]
        [Route("Handover")]
        public IActionResult GetHandoverDocx()
        {
            // table
            List<DocTable> table = new List<DocTable>();
            List<DocColumn> column = new List<DocColumn>();
            List<DocTableDes> tableDes = new List<DocTableDes>();

            // view
            List<DocTable> view = new List<DocTable>();
            List<DocColumn> viewColumn = new List<DocColumn>();
            List<DocTableDes> viewDes = new List<DocTableDes>();

            // func
            List<DocTable> func = new List<DocTable>();
            List<DocColumn> funcParam = new List<DocColumn>();
            List<DocTableDes> funcDes = new List<DocTableDes>();

            // proc
            List<DocTable> proc = new List<DocTable>();
            List<DocColumn> procParam = new List<DocColumn>();
            List<DocTableDes> procDes = new List<DocTableDes>();

            List<Stream> docxs = new List<Stream>();

            // table
            string tableSql = @"select t.name as tableName from sys.tables t order by name";
            string columnSql = @"select tab.name as tableName,
                                        col.name,
                                        t.name as data_type,
	                                    ISNULL(p.value, '') as des
                                    from sys.tables as tab
                                        inner join sys.columns as col
                                            on tab.object_id = col.object_id
                                        left join sys.types as t
		                                    on col.user_type_id = t.user_type_id
	                                    left join sys.extended_properties as p
		                                    on tab.object_id = p.major_id and col.column_id = p.minor_id
                                    order by tab.name, column_id";
            string tableDesSql = @"select t.name as tbName,
                                        ISNULL(p.value, '') as tbDes
                                    from sys.tables t
                                        left join sys.extended_properties as p
                                            on t.object_id = p.major_id and p.minor_id = 0
                                    order by tbName";

            // view
            string viewSql = @"select v.name as tableName from sys.views v order by name";
            string viewColumnSql = @"select tab.name as tableName,
                                            col.name,
                                            t.name as data_type,
                                            ISNULL(p.value, '') as des
                                        from sys.views as tab
                                            inner join sys.columns as col
                                                on tab.object_id = col.object_id
                                            left join sys.types as t
                                                on col.user_type_id = t.user_type_id
                                            left join sys.extended_properties as p
                                                on tab.object_id = p.major_id and col.column_id = p.minor_id
                                        order by tab.name, col.column_id";
            string viewDesSql = @"select v.name as tbName,
                                        ISNULL(p.value, '') as tbDes
                                    from sys.views v
                                        left join sys.extended_properties as p
                                            on v.object_id = p.major_id and p.minor_id = 0
                                    order by tbName";

            // func
            string funcSql = @"select obj.name as tableName
                                from sys.objects obj
                                where obj.type in ('FN', 'TF', 'IF') 
                                order by name";
            string funcParamSql = @"select obj.name as tableName,
                                            par.name,
                                            t.name as data_type,
                                        ISNULL(p.value, '') as des
                                        from sys.objects obj
                                            inner join sys.parameters par
                                                on obj.object_id = par.object_id
                                            left join sys.extended_properties p
                                                on par.object_id = p.major_id and par.parameter_id = p.minor_id 
                                            left join sys.types t
                                                on par.system_type_id = t.system_type_id
                                        where obj.type in ('FN', 'TF', 'IF') and par.parameter_id != 0
                                        order by obj.name, par.parameter_id";
            string funcDesSql = @"select obj.name as tbName,
                                        ISNULL(p.value, '') as tbDes
                                    from sys.objects obj
                                    left join sys.extended_properties as p
                                        on obj.object_id = p.major_id and p.minor_id = 0
                                    where obj.type in ('FN', 'TF', 'IF') 
                                    order by tbName";

            // proc
            string procSql = @"select obj.name as tableName
                                from sys.objects obj
                                where obj.type in ('P', 'X')
                                order by name";
            string procParamSql = @"select obj.name as tableName,
                                        par.name,
                                        t.name as data_type,
                                    ISNULL(p.value, '') as des
                                    from sys.objects obj
                                        inner join sys.parameters par
                                            on obj.object_id = par.object_id
                                        left join sys.extended_properties p
                                            on par.object_id = p.major_id and par.parameter_id = p.minor_id
                                        left join sys.types t
                                            on par.system_type_id = t.system_type_id
                                    where obj.type in ('P', 'X') and par.parameter_id != 0
                                    order by obj.name, par.parameter_id";
            string procDesSql = @"select obj.name as tbName,
                                        ISNULL(p.value, '') as tbDes
                                    from sys.objects obj
                                        left join sys.extended_properties as p
                                            on obj.object_id = p.major_id and p.minor_id = 0
                                    where obj.type in ('P', 'X')
                                    order by tbName";

            using (var db = new AppDb())
            {
                // table
                table = db.Connection.Query<DocTable>(tableSql).ToList();
                column = db.Connection.Query<DocColumn>(columnSql).ToList();
                tableDes = db.Connection.Query<DocTableDes>(tableDesSql).ToList();

                // view
                view = db.Connection.Query<DocTable>(viewSql).ToList();
                viewColumn = db.Connection.Query<DocColumn>(viewColumnSql).ToList();
                viewDes = db.Connection.Query<DocTableDes>(viewDesSql).ToList();

                // func
                func = db.Connection.Query<DocTable>(funcSql).ToList();
                funcParam = db.Connection.Query<DocColumn>(funcParamSql).ToList();
                funcDes = db.Connection.Query<DocTableDes>(funcDesSql).ToList();

                // proc
                proc = db.Connection.Query<DocTable>(procSql).ToList();
                procParam = db.Connection.Query<DocColumn>(procParamSql).ToList();
                procDes = db.Connection.Query<DocTableDes>(procDesSql).ToList();

            }

            byte[] docx = System.IO.File.ReadAllBytes(Path.GetRelativePath(".", "Templates/tableList.docx"));
            byte[] docx2 = System.IO.File.ReadAllBytes(Path.GetRelativePath(".", "Templates/tableDetailList.docx"));

            // 取代
            var wordProcessor = new ReplaceWordTemplate();
            byte[] tmp;

            // table
            tmp = wordProcessor.Replace(docx, tableDes);
            docxs.Add(new MemoryStream(tmp));

            foreach (var tb in table)
            {
                var col = column.Where(c => c.tableName == tb.tableName)
                    .Select(c => new { c.name, c.data_type, c.des })
                    .ToList();

                tmp = wordProcessor.Replace(docx2, col);
                tmp = wordProcessor.Replace(tmp, tb);

                docxs.Add(new MemoryStream(tmp));
            }

            // view
            tmp = wordProcessor.Replace(docx, viewDes);
            docxs.Add(new MemoryStream(tmp));

            foreach (var tb in view)
            {
                var col = viewColumn.Where(c => c.tableName == tb.tableName)
                    .Select(c => new { c.name, c.data_type, c.des })
                    .ToList();

                tmp = wordProcessor.Replace(docx2, col);
                tmp = wordProcessor.Replace(tmp, tb);

                docxs.Add(new MemoryStream(tmp));
            }

            // func
            tmp = wordProcessor.Replace(docx, funcDes);
            docxs.Add(new MemoryStream(tmp));

            foreach (var tb in func)
            {
                var col = funcParam.Where(c => c.tableName == tb.tableName)
                    .Select(c => new { c.name, c.data_type, c.des })
                    .ToList();

                tmp = wordProcessor.Replace(docx2, col);
                tmp = wordProcessor.Replace(tmp, tb);

                docxs.Add(new MemoryStream(tmp));
            }

            // proc
            tmp = wordProcessor.Replace(docx, procDes);
            docxs.Add(new MemoryStream(tmp));

            foreach (var tb in proc)
            {
                var col = procParam.Where(c => c.tableName == tb.tableName)
                    .Select(c => new { c.name, c.data_type, c.des })
                    .ToList();

                tmp = wordProcessor.Replace(docx2, col);
                tmp = wordProcessor.Replace(tmp, tb);

                docxs.Add(new MemoryStream(tmp));
            }


            // 取套件
            MergeWordTemplate WordMerger = new MergeWordTemplate();

            // 合併文件
            byte[] final = WordMerger.MergeDocxsIntoOne(docxs);

            // 設定檔名
            string fileName = "Handover";

            // 回傳合併檔案
            return File(
                fileContents: final,
                contentType: "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                fileDownloadName: $"{fileName}.docx"
            );
        }
    }
}
