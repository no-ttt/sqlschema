###### tags: `sqlschema` `userInfo`
# User Infomation
## 資料庫內所有用戶
### Query
```sql=
select name as username,
    create_date,
    modify_date,
    type_desc as type,
    authentication_type_desc as authentication_type
from sys.database_principals
where type not in ('A', 'G', 'R', 'X')
    and sid is not null
    and name != 'guest'
order by username;
```

### Columns
- `username`：用戶名稱
- `create_date`：添加帳戶的日期
- `modify_date`：上次更新帳戶的日期
- `type_desc`：主體類型
- `authentication_type`：用戶身份驗證類型

### Sample Result
![](https://i.imgur.com/8WIjiwr.png)
