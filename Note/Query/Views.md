###### tags: `sqlschema` `view`
# Views
## 所有 View Tables
### Query
```sql=
select schema_name(v.schema_id) as schema_name,
   v.name as view_name,
   v.create_date as created,
   v.modify_date as last_modified,
   m.definition
from sys.views v
join sys.sql_modules m 
     on m.object_id = v.object_id
 order by schema_name, view_name;
```

### Columns
- schema_name : schema 名稱
- view_name : view table 名稱
- created : 創建日期和時間
- last_modified : 查看上次修改日期和時間
- definition : 建立 view table 的 sql 語法

### Sample Result
![](https://i.imgur.com/dmiaW2y.png)

## 所有 View Tables 的欄位
### Query
```sql=
select schema_name(v.schema_id) as schema_name,
   object_name(c.object_id) as view_name,
   c.column_id,
   c.name as column_name,
   type_name(user_type_id) as data_type,
   c.max_length,
   c.precision
from sys.columns c
join sys.views v 
    on v.object_id = c.object_id
order by schema_name, view_name, column_id;
```

### Columns
- schema_name : schema 名稱
- view_name : view table 名稱
- column_id : 欄位 id，每張資料表都從 1 開始
- column_name : 欄位名稱
- data_type : 數據類型
- max_length : 數據最大長度
- precision : 數據精準度

### Sample Result
![](https://i.imgur.com/wVGaQ46.png)

## 相關的 Tables
View tables 的相依性。

### Query
```sql=
select distinct schema_name(v.schema_id) as schema_name,
    v.name as view_name,
    schema_name(o.schema_id) as referenced_schema_name,
    o.name as referenced_entity_name,
    o.type_desc as entity_type
from sys.views v
join sys.sql_expression_dependencies d
    on d.referencing_id = v.object_id
    and d.referenced_id is not null
join sys.objects o
    on o.object_id = d.referenced_id
 order by schema_name, view_name;
```

### Columns
- schema_name : schema 名稱
- view_name : view table 名稱
- referenced_schema_name : 被引用的 table 的 schema 名稱
- referenced_entity_name : 被引用的 table 名稱
- entity_type : 被引用對象的類型
    - USER_TABLE
    - VIEW

### Sample Result
![](https://i.imgur.com/rrVDmcb.png)
