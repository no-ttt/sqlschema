###### tags: `sqlschema` `sys` `object`
# Object
:::warning
參考資料：[物件目錄檢視 (Transact-SQL)](https://learn.microsoft.com/zh-tw/sql/relational-databases/system-catalog-views/object-catalog-views-transact-sql?view=sql-server-ver16)
:::

## sys.tables
- 系統內所有 tables 的相關資訊
- https://learn.microsoft.com/zh-tw/sql/relational-databases/system-catalog-views/sys-tables-transact-sql?view=sql-server-ver16

## sys.views
- 系統內所有 view tables 的相關資訊
- https://learn.microsoft.com/zh-tw/sql/relational-databases/system-catalog-views/sys-all-views-transact-sql?view=sql-server-ver16

## sys.columns
- 系統內所有欄位
- https://learn.microsoft.com/zh-tw/sql/relational-databases/system-catalog-views/sys-columns-transact-sql?view=sql-server-ver16

## sys.types
- 系統內所有資料型態
- https://learn.microsoft.com/zh-tw/sql/relational-databases/system-catalog-views/sys-types-transact-sql?view=sql-server-ver16

## sys.objects
- 資料庫中建立的每個使用者定義架構範圍物件
    - ex. table, view, function, procedure ...
- https://learn.microsoft.com/zh-tw/sql/relational-databases/system-catalog-views/sys-objects-transact-sql?view=sql-server-ver16

## sys.sql_modules
- 建立 view、function、procedure 的原生程式碼
- https://learn.microsoft.com/zh-tw/sql/relational-databases/system-catalog-views/sys-sql-modules-transact-sql?view=sql-server-ver16

## sys.sql_expression_dependencies
- 檢視相依性
- https://learn.microsoft.com/zh-tw/sql/relational-databases/system-catalog-views/sys-sql-expression-dependencies-transact-sql?view=sql-server-ver16

## sys.foreign_keys
- 系統內所有外部索引
- https://learn.microsoft.com/zh-tw/sql/relational-databases/system-catalog-views/sys-foreign-keys-transact-sql?view=sql-server-ver16

## sys.foreign_key_columns
- 系統內所有外部索引及相關欄位 (References & Referenced)
- https://learn.microsoft.com/zh-tw/sql/relational-databases/system-catalog-views/sys-foreign-key-columns-transact-sql?view=sql-server-ver16

## sys.indexes
- 每個表格式物件 (如資料表、檢視或資料表值函式) 的索引
- https://learn.microsoft.com/zh-tw/sql/relational-databases/system-catalog-views/sys-indexes-transact-sql?view=sql-server-ver16

## sys.index_columns
- 每個欄位物件 (如資料表、檢視或資料表值函式) 的索引
- https://learn.microsoft.com/zh-tw/sql/relational-databases/system-catalog-views/sys-index-columns-transact-sql?view=sql-server-ver16

## sys.key_constraints
- 針對每個本身是主索引鍵或唯一條件約束的物件，各包含一個資料列 (PK, UQ)
- https://learn.microsoft.com/zh-tw/sql/relational-databases/system-catalog-views/sys-key-constraints-transact-sql?view=sql-server-ver16

## sys.database_principals
- 資料庫用戶資訊
- https://learn.microsoft.com/zh-tw/sql/relational-databases/system-catalog-views/sys-database-principals-transact-sql?view=sql-server-ver16