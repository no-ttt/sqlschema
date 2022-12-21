###### tags: `sqlschema` `database` `schema`
# Databases and schemas
> 比較少會再創 Schema 做使用，通常都直接用預設的 `dbo`。

## 所有資料庫
![](https://i.imgur.com/M4fdfaO.png)

### Query
```sql=
select name as database_name, 
    database_id, 
    create_date
from sys.databases
order by name
```

### Columns
- database_name : 資料庫名稱
- database_id : 數據庫 ID (唯一的)
- created_date : 創建或重命名資料庫的日期

### Sample Result
![](https://i.imgur.com/3VWDFiI.png)

## 資料庫架構
![](https://i.imgur.com/IGOd8pL.png)

### Query
```sql=
select s.name as schema_name, 
    s.schema_id,
    u.name as schema_owner
from sys.schemas s
    inner join sys.sysusers u
        on u.uid = s.principal_id
order by s.name
```
:::warning
### Schema vs User
- 相關的 sys
    - sys.schemas (as s) : schema 目錄檢視
        - principal_id : 擁有這個結構描述之主體的識別碼
    - sys.sysusers (as u) : 系統使用者
        - uid : 使用者識別碼，在這個資料庫中是唯一的
- 架構目錄 ⇔ 使用者 : u.uid = s.principal_id
:::

### Columns
- schema_name : schema 名稱
- schema_id : schema ID (唯一的)
- schema_owner :  schema 所有者

### Sample Result
![](https://i.imgur.com/A8vxpSF.png)


## 用戶創建的 Schema
### Query
```sql=
select s.name as schema_name, 
    s.schema_id,
    u.name as schema_owner
from sys.schemas s
    inner join sys.sysusers u
        on u.uid = s.principal_id
where u.issqluser = 1
    and u.name not in ('sys', 'guest', 'INFORMATION_SCHEMA')
```

### Columns
- schema_name : schema 名稱
- schema_id : schema ID (唯一的)
- schema_owner :  schema 所有者

### Sample Result
![](https://i.imgur.com/xjVEV7F.png)


:::info
### schema 指派所有者
- user 右鍵 → 屬性 → 擁有的結構描述

![](https://i.imgur.com/ZpBUpFu.png)
:::

## 所有資料庫的架構
### Query
```sql=
declare @query nvarchar(max);

set @query =
(select 'select ''' + name + ''' as database_name,
                name COLLATE DATABASE_DEFAULT as schema_name 
         from ['+ name + '].sys.schemas union all '
  from sys.databases 
  where database_id > 4
  for xml path(''), type).value('.', 'nvarchar(max)');

--- 把最後一個 union all 字串刪除，並加上排序條件
set @query = left(@query,len(@query)-10) + ' order by database_name, schema_name';
execute (@query);
```

> @query 文字輸出：
```xml=
<!-- xml -->
select 'TDX' as database_name,
                name COLLATE DATABASE_DEFAULT as schema_name 
         from [TDX].sys.schemas union all 
		 
select 'UD' as database_name,
                name COLLATE DATABASE_DEFAULT as schema_name 
         from [UD].sys.schemas union all 
		 
select 'Travel_test' as database_name,
                name COLLATE DATABASE_DEFAULT as schema_name 
         from [Travel_test].sys.schemas union all
```

:::warning
### for xml path(''), type).value('.', 'nvarchar(max)')
![](https://i.imgur.com/n6IhZFH.png)
:::

### Columns
- database_name : 資料庫名稱
- schema_name : schema 名稱

### Sample Result
![](https://i.imgur.com/qtUEToj.png)


## 所有資料庫的用戶 schema
### Query
```sql=
declare @query nvarchar(max)

set @query = 
(select 'select ''' + name + ''' as database_name,
              s.Name COLLATE DATABASE_DEFAULT as schema_name,
              u.name COLLATE DATABASE_DEFAULT as schema_owner 
        FROM ['+ name + '].sys.schemas s
        JOIN ['+ name + '].sys.sysusers u on u.uid = s.principal_id
        where u.issqluser = 1
              and u.name not in (''sys'', ''guest'', ''INFORMATION_SCHEMA'')
        union all '
    from sys.databases 
    where database_id > 4
    for xml path(''), type).value('.', 'nvarchar(max)')

set @query = left(@query,len(@query)-10) 
                        + ' order by database_name, schema_name'

execute (@query)
```

### Columns
- database_name : 資料庫名稱
- schema_name : schema 名稱
- schema_owner : schema 所有者

### Result
![](https://i.imgur.com/H4N9zLK.png)
