###### tags: `sqlschema` `table`
# Tables
## 所有資料表
> 在同個資料庫下的資料表。

### Query
```sql=
select schema_name(t.schema_id) as schema_name,
   t.name as table_name,
   t.create_date,
   t.modify_date
from sys.tables t
order by schema_name, table_name;
```

### Columns
- schema_name : schema 名稱
- table_name : 資料表名稱
- create_date : 創建日期
- modify_date : 上次的更改日期 (alter)

### Sample Result
![](https://i.imgur.com/h6MmlGJ.png)

## 所有 Tables 的欄位
### Query
```sql=
select schema_name(tab.schema_id) as schema_name,
    tab.name as table_name, 
    col.column_id,
    col.name as column_name, 
    t.name as data_type,    
    col.max_length,
    col.precision
from sys.tables as tab
    inner join sys.columns as col
        on tab.object_id = col.object_id
    left join sys.types as t
    on col.user_type_id = t.user_type_id
order by schema_name, table_name, column_id;
```

### Columns
- schema_name : schema 名稱
- table_name : 資料表名稱
- column_id : 欄位 id，每張資料表都從 1 開始
- column_name : 欄位名稱
- data_type : 數據類型
- max_length : 數據最大長度
- precision : 數據精準度

### Sample Result
![](https://i.imgur.com/kZWzr3S.png)

## 最近創建的資料表
過去 30 天內創建的資料表。

### Query
```sql=
select schema_name(schema_id) as schema_name,
   name as table_name,
   create_date,
   modify_date
from sys.tables
where create_date > DATEADD(DAY, -30, CURRENT_TIMESTAMP)
order by create_date desc;
```

### Columns
- schema_name : schema 名稱
- table_name : 資料表名稱
- create_date : 創建日期
- modify_date : 上次的更改日期 (alter)

### Sample Result
![](https://i.imgur.com/X1zVTAc.png)

## 最近修改的資料表
過去 30 天內由 ALTER 語句修改的所有資料表。

### Query
```sql=
select schema_name(schema_id) as schema_name,
   name as table_name,
   create_date,
   modify_date
from sys.tables
where modify_date > DATEADD(DAY, -30, CURRENT_TIMESTAMP)
order by modify_date desc;
```

### Columns
- schema_name : schema 名稱
- table_name : 資料表名稱
- create_date : 創建日期
- modify_date : 上次的更改日期 (alter)

### Sample Result
![](https://i.imgur.com/oQNfg2R.png)

## 所有資料庫中的資料表
### Query
```sql=
declare @sql nvarchar(max);

select @sql = 
    (select ' UNION ALL
        SELECT ' +  + quotename(name,'''') + ' as database_name,
           s.name COLLATE DATABASE_DEFAULT
                AS schema_name,
           t.name COLLATE DATABASE_DEFAULT as table_name 
           FROM '+ quotename(name) + '.sys.tables t
           JOIN '+ quotename(name) + '.sys.schemas s
                on s.schema_id = t.schema_id'
    from sys.databases 
    where state=0
    order by [name] for xml path(''), type).value('.', 'nvarchar(max)');

set @sql = stuff(@sql, 1, 12, '') + ' order by database_name, 
                                               schema_name,
                                               table_name';

execute (@sql);
```
### Columns
- database_name : 資料庫名稱
- schema_name : schema 名稱
- table_name : 資料表名稱

### Sample Result
![](https://i.imgur.com/7OSAVz2.png)

