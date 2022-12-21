###### tags: `sqlschema` `function`
# Functions
## 所有 Functions
### Query
```sql=
select schema_name(obj.schema_id) as schema_name,
    obj.name as function_name,
    case type
        when 'FN' then 'SQL scalar function'
        when 'TF' then 'SQL table-valued-function'
        when 'IF' then 'SQL inline table-valued function'
    end as type,
    substring(par.parameters, 0, len(par.parameters)) as parameters,
    TYPE_NAME(ret.user_type_id) as return_type,
    mod.definition
from sys.objects obj
join sys.sql_modules mod
     on mod.object_id = obj.object_id
cross apply (select p.name + ' ' + TYPE_NAME(p.user_type_id) + ', ' 
        from sys.parameters p
        where p.object_id = obj.object_id 
            and p.parameter_id != 0 
    for xml path ('') ) par (parameters)
left join sys.parameters ret
    on obj.object_id = ret.object_id
    and ret.parameter_id = 0
where obj.type in ('FN', 'TF', 'IF')
order by schema_name, function_name;
```

### Columns
- schema_name : schema 名稱
- function_name : function 名稱
- type : function 的型態
    - SQL scalar function
    - SQL inline table-valued function
    - SQL table-valued-function
- parameters : 參數名稱及其型態
- return_type : 回傳值的型態
- definition : 建立 function 的 sql 語法

### Sample Result
![](https://i.imgur.com/IVKOltp.png)

## 