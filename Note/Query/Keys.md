###### tags: `sqlschema` `key`
# Keys
![](https://i.imgur.com/guqed0o.png)

## 所有 Primary Keys 的欄位
### Query
```sql=
select schema_name(tab.schema_id) as [schema_name], 
    pk.[name] as pk_name,
    substring(column_names, 1, len(column_names)-1) as [columns],
    tab.[name] as table_name
from sys.tables tab
    inner join sys.indexes pk
        on tab.object_id = pk.object_id 
        and pk.is_primary_key = 1
   cross apply (select col.[name] + ', '
                    from sys.index_columns ic
                        inner join sys.columns col
                            on ic.object_id = col.object_id
                            and ic.column_id = col.column_id
                    where ic.object_id = tab.object_id
                        and ic.index_id = pk.index_id
                            order by col.column_id
                            for xml path ('') ) D (column_names)
order by schema_name(tab.schema_id), pk.[name]
```

### Columns
- schema_name : schema 名稱
- pk_name : PK constraint name
- columns : 欄位名稱 (若 1 個以上，以 ',' 分隔)
- table_name : 資料表名稱

### Sample Result
![](https://i.imgur.com/xVlPHth.png)


## 所有 Foreign keys 的欄位
### Query
```sql=
select schema_name(fk_tab.schema_id) + '.' + fk_tab.name as foreign_table,
    '>-' as rel,
    schema_name(pk_tab.schema_id) + '.' + pk_tab.name as primary_table,
    fk_cols.constraint_column_id as no, 
    fk_col.name as fk_column_name,
    ' = ' as [join],
    pk_col.name as pk_column_name,
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
order by schema_name(fk_tab.schema_id) + '.' + fk_tab.name,
    schema_name(pk_tab.schema_id) + '.' + pk_tab.name, 
    fk_cols.constraint_column_id
```

### Columns
- foreign_table : 帶有 schema 名稱的外部表名稱
- rel : 方向的關係符號
- primary_table : 帶有 schema 名稱的主(引用)表名稱
- no : key 的 id， 單列鍵為 1，複合鍵對於鍵的每一列為 1, 2, ... n
- fk_column_name : 外部表欄位名稱
- join : 對列的連接操作
- pk_column_name : 主要（引用）表列
- fk_constraint_name : FK constraint name

### Sample Result
![](https://i.imgur.com/LSgL2gm.png)

## 整個資料庫的 FK 和 PK 關係
### Query
```sql=
select schema_name(tab.schema_id) + '.' + tab.name as [table],
    col.column_id,
    col.name as column_name,
    case when fk.object_id is not null then '>-' else null end as rel,
    schema_name(pk_tab.schema_id) + '.' + pk_tab.name as primary_table,
    pk_col.name as pk_column_name,
    fk_cols.constraint_column_id as no,
    fk.name as fk_constraint_name
from sys.tables tab
    inner join sys.columns col 
        on col.object_id = tab.object_id
    left outer join sys.foreign_key_columns fk_cols
        on fk_cols.parent_object_id = tab.object_id
        and fk_cols.parent_column_id = col.column_id
    left outer join sys.foreign_keys fk
        on fk.object_id = fk_cols.constraint_object_id
    left outer join sys.tables pk_tab
        on pk_tab.object_id = fk_cols.referenced_object_id
    left outer join sys.columns pk_col
        on pk_col.column_id = fk_cols.referenced_column_id
        and pk_col.object_id = fk_cols.referenced_object_id
order by schema_name(tab.schema_id) + '.' + tab.name,
    col.column_id
```

### Columns
- table : 帶有 schema 名稱的資料表名稱
- column_id : 欄位的 id
- column_name : 欄位名稱
- rel : 方向的關係符號
- primary_table : 帶有 schema 名稱的主(引用)表名稱
- pk_column_name : 主要（引用）表列
- no : key 的 id， 單列鍵為 1，複合鍵對於鍵的每一列為 1, 2, ... n
- fk_constraint_name : FK constraint name

### Sample Result
![](https://i.imgur.com/V6CviuX.png)

## Unique keys & Indexes
### Query
```sql=
select schema_name(t.schema_id) + '.' + t.[name] as table_view, 
    case when t.[type] = 'U' then 'Table'
        when t.[type] = 'V' then 'View'
        end as [object_type],
    case when c.[type] = 'PK' then 'Primary key'
        when c.[type] = 'UQ' then 'Unique constraint'
        when i.[type] = 1 then 'Unique clustered index'
        when i.type = 2 then 'Unique index'
        end as constraint_type, 
    c.[name] as constraint_name,
    substring(column_names, 1, len(column_names)-1) as [columns],
    i.[name] as index_name,
    case when i.[type] = 1 then 'Clustered index'
        when i.type = 2 then 'Index'
        end as index_type
from sys.objects t
    left outer join sys.indexes i
        on t.object_id = i.object_id
    left outer join sys.key_constraints c
        on i.object_id = c.parent_object_id 
        and i.index_id = c.unique_index_id
   cross apply (select col.[name] + ', '
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
order by schema_name(t.schema_id) + '.' + t.[name]
```

### Columns
- table_view : 帶有 schema 名稱的 table / view table 名稱
- object_type : 在其上創建 constraint/index 的類型
    -  Table
    -  View
- constraint_type : 約束 (constraint) 類型
    - Primary key 
    - Unique constraint
    - Unique clustered index
    - Unique index 
- constraint_name : primary/unique key constraint，無約束則為 null
- columns : 用 ',' 分隔的 index 欄位
- index_name : index 名稱
- index_type : index 類型
    - Clustered index
    - Index

### Sample Result
![](https://i.imgur.com/Melv10F.png)


