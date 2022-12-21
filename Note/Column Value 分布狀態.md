###### tags: `sqlschemaInfo` `columns`
# Column Value 分布狀態 
## Query
```sql=
if exists ( select * from dbo.sysobjects where id = OBJECT_ID('SampleColAnalysis') )
    begin
        drop table SampleColAnalysis
    end
go
create table SampleColAnalysis (
    I int, Col nvarchar(20), K int, N int
)
go

declare @name nvarchar(100), @i int, @n int
declare @sqlStr varchar(1000)
select @n = count(*) from Object
declare aCursor cursor for 
select COLUMN_NAME, ORDINAL_POSITION 
from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'Object'
order by ORDINAL_POSITION
open aCursor  

fetch next from aCursor into @name, @i
while @@FETCH_STATUS = 0  
    begin
        select @sqlStr = 'insert SampleColAnalysis values(' + convert(varchar(20), @i) +
            ', ''' + @name +''', ( select count(distinct ' + @name + ') ' +
            '+ count(distinct case when ' + @name + ' is null then 1 end) from Object ), ' +
        convert(varchar(20), @n) + ' )'
        EXEC ( @sqlStr )
            fetch next from aCursor into @name, @i
    end

close aCursor
deallocate aCursor

select *, convert(real, K) / N as 'Ratio' 
from SampleColAnalysis order by K
```

## Columns
- `I` : 編號
- `Col` : 欄位
- `K` : 不同值個數
- `N` : 資料表總欄位個數

## Sample Result
![](https://i.imgur.com/VZhzWKv.png)
