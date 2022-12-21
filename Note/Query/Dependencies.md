###### tags: `sqlschema` `dependencies`
# Dependencies
## Relation
### Query
```sql=
select schema_name(fk_tab.schema_id) + '.' + fk_tab.name as foreign_table,
    '>-' as rel,
    schema_name(pk_tab.schema_id) + '.' + pk_tab.name as primary_table,
    substring(column_names, 1, len(column_names)-1) as [fk_columns],
    fk.name as fk_constraint_name
from sys.foreign_keys fk
    inner join sys.tables fk_tab
        on fk_tab.object_id = fk.parent_object_id
    inner join sys.tables pk_tab
        on pk_tab.object_id = fk.referenced_object_id
    cross apply (select col.[name] + ', '
        from sys.foreign_key_columns fk_c
            inner join sys.columns col
                on fk_c.parent_object_id = col.object_id
                and fk_c.parent_column_id = col.column_id
        where fk_c.parent_object_id = fk_tab.object_id
            and fk_c.constraint_object_id = fk.object_id
                order by col.column_id
                for xml path ('') ) D (column_names)
order by schema_name(fk_tab.schema_id) + '.' + fk_tab.name,
    schema_name(pk_tab.schema_id) + '.' + pk_tab.name
```

### Columns
- `foreign_table`：帶有 schema 名稱的外部表名
- `rel`：暗示方向的關係符號
- `primary_table`：帶有 schema 名稱的主表名
- `fk_columns`：FK 關係欄位名稱
- `fk_constraint_name`：外見約束名稱

### Sample Result
![](https://i.imgur.com/ZlroHm1.png)


## References
> 指定 Object 引用的所有表。

### Query
#### Tables

```sql=
select distinct 
    schema_name(fk_tab.schema_id) + '.' + fk_tab.name as foreign_table,
    '>-' as rel,
    schema_name(pk_tab.schema_id) + '.' + pk_tab.name as primary_table
from sys.foreign_keys fk
    inner join sys.tables fk_tab
        on fk_tab.object_id = fk.parent_object_id
    inner join sys.tables pk_tab
        on pk_tab.object_id = fk.referenced_object_id
where fk_tab.[name] = 'Your table' -- enter table name here
--  and schema_name(fk_tab.schema_id) = 'Your table schema name'
order by schema_name(fk_tab.schema_id) + '.' + fk_tab.name,
    schema_name(pk_tab.schema_id) + '.' + pk_tab.name
```

#### Functions
```sql=
select schema_name(obj.schema_id) as schema_name,
    obj.name as function_name,
    schema_name(dep_obj.schema_id) as referenced_object_schema,
    dep_obj.name as referenced_object_name,
    dep_obj.type_desc as object_type
from sys.objects obj
left join sys.sql_expression_dependencies dep
    on dep.referencing_id = obj.object_id
left join sys.objects dep_obj
    on dep_obj.object_id = dep.referenced_id
where obj.type in ('AF', 'FN', 'FS', 'FT', 'IF', 'TF')
    -- and obj.name = 'function_name'  -- put function name here
order by schema_name, function_name;
```

#### Procedures
```sql=
select schema_name(obj.schema_id) as schema_name,
    obj.name as procedure_name,
    schema_name(dep_obj.schema_id) as referenced_object_schema,
    dep_obj.name as referenced_object_name,
    dep_obj.type_desc as object_type
from sys.objects obj
left join sys.sql_expression_dependencies dep
    on dep.referencing_id = obj.object_id
left join sys.objects dep_obj
    on dep_obj.object_id = dep.referenced_id
where obj.type in ('P', 'X', 'PC', 'RF')
    --  and obj.name = 'procedure_name'  -- put procedure name here
order by schema_name, procedure_name;
```



### Columns
#### Tables
- `foreign_table`：帶有 schema 名稱的外部表名
- `rel`：暗示方向的關係符號
- `primary_table`：帶有 schema 名稱的主表名

#### Functions / Procedures
- `schema_name`：schema 名稱
- `function_name`：function 名稱
- `referenced_object_schema`：被引用對象的 schema 名稱
- `referenced_object_name`：被引用對象的名稱
- `object_type`：引用對象的型態 

### Sample Result
#### Tables
![](https://i.imgur.com/APz6xsg.png)

#### Functions / Procedures

![](https://i.imgur.com/2MTyJeF.png)

## Referenced by
> 列出所有引用指定 Object 的表。

### Query
#### Tables
```sql=
select distinct 
    schema_name(fk_tab.schema_id) + '.' + fk_tab.name as foreign_table,
    '>-' as rel,
    schema_name(pk_tab.schema_id) + '.' + pk_tab.name as primary_table
from sys.foreign_keys fk
    inner join sys.tables fk_tab
        on fk_tab.object_id = fk.parent_object_id
    inner join sys.tables pk_tab
        on pk_tab.object_id = fk.referenced_object_id
where pk_tab.[name] = 'Your table' -- enter table name here
--  and schema_name(pk_tab.schema_id) = 'Your table schema name'
order by schema_name(fk_tab.schema_id) + '.' + fk_tab.name,
    schema_name(pk_tab.schema_id) + '.' + pk_tab.name
```
#### Functions
```sql=
select schema_name(o.schema_id) + '.' + o.name as [function],
    'is used by' as ref,
    schema_name(ref_o.schema_id) + '.' + ref_o.name as [object],
    ref_o.type_desc as object_type
from sys.objects o
join sys.sql_expression_dependencies dep
    on o.object_id = dep.referenced_id
join sys.objects ref_o
    on dep.referencing_id = ref_o.object_id
where o.type in ('FN', 'TF', 'IF')
    and schema_name(o.schema_id) = 'dbo'  -- put schema name here
    and o.name = 'ufnLeadingZeros'  -- put function name here
order by [object];
```

#### Procedures
```sql=
select schema_name(o.schema_id) + '.' + o.name as [procedure],
    'is used by' as ref,
    schema_name(ref_o.schema_id) + '.' + ref_o.name as [object],
    ref_o.type_desc as object_type
from sys.objects o
join sys.sql_expression_dependencies dep
    on o.object_id = dep.referenced_id
join sys.objects ref_o
    on dep.referencing_id = ref_o.object_id
where o.type in ('P', 'X')
    and schema_name(o.schema_id) = 'dbo'  -- put schema name here
    and o.name = 'uspPrintError'  -- put procedure name here
order by [object]; 
```

### Columns
#### Tables
- `foreign_table`：帶有 schema 名稱的外部表名
- `rel`：暗示方向的關係符號
- `primary_table`：帶有 schema 名稱的主表名

#### Functions / Procedures
- `function`：schema 和 function 的名稱
- `ref`：「被使用」
- `object`：有使用指定 object 的名稱
- `object_type`：有使用指定 object 的型態


### Sample Result
#### Tables

![](https://i.imgur.com/2QYqPMa.png)

#### Functions / Procedures
![](https://i.imgur.com/r2LSFCl.png)

