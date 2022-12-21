###### tags: `sqlschema` `parameter`
# Parameters
> 指的是 Functions 和 Procedures 所擁有的參數。

## 所有參數
### Query
```sql=
select case par.is_output
        when 1 then 'OUT'
        when 0 then 'IN'
    end as mode,
    par.name as name,
    t.name as data_type,
    par.max_length,
    par.precision,
    case par.is_nullable
        when 0 then 1
        when 1 then 0
    end as not_nullabe
from sys.objects obj
inner join sys.parameters par
    on obj.object_id = par.object_id
left join sys.types t
    on par.system_type_id = t.system_type_id
where obj.name = [object_name]
```

### Columns
- mode : 輸入 (IN) or 輸出 (OUT)
- name : 參數名稱
- data_type : 參數的型態
- max_length：參數最大長度
- precision : 參數精準度
- not_nullable : 是否能為空值

### Sample Result
![](https://i.imgur.com/sjM1o3o.png)
