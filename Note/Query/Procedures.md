###### tags: `sqlschema` `procedure`
# Procedures
## 所有 Procedures
### Query
```sql=
select schema_name(obj.schema_id) as schema_name,
    obj.name as procedure_name,
    case type
        when 'P' then 'SQL Stored Procedure'
        when 'X' then 'Extended stored procedure'
    end as type,
    substring(par.parameters, 0, len(par.parameters)) as parameters,
    mod.definition
from sys.objects obj
join sys.sql_modules mod
     on mod.object_id = obj.object_id
cross apply (select p.name + ' ' + TYPE_NAME(p.user_type_id) + ', ' 
            from sys.parameters p
            where p.object_id = obj.object_id 
                and p.parameter_id != 0 
            for xml path ('') ) par (parameters)
where obj.type in ('P', 'X')
order by schema_name, procedure_name;
```

### Columns
- schema_name : schema 名稱
- procedure_name : procedure 名稱
- type : procedure 的型態
    - SQL Stored Procedure
    - Extended stored procedure
- parameters : 參數名稱及其型態
- definition : 建立 procedure 的 sql 語法

### Sample Result
![](https://i.imgur.com/LEVAJCo.png)
