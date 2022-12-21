###### tags: `sqlschemaInfo` `schema`
# INFORMATION_SCHEMA
[TOC]

- [Documnet](https://docs.microsoft.com/zh-tw/sql/relational-databases/system-information-schema-views/system-information-schema-views-transact-sql?view=sql-server-ver16)
- SQL Server 名稱和 SQL 標準名稱之間的關聯性
![](https://i.imgur.com/xvQQxDk.png)

## TABLES
- 所有 tables (包含 view tables)

欄位資訊

| 資料行名稱 | 描述 |
| -------- | -------- |
| TABLE_NAME | 資料表名稱|
| TABLE_TYPE |view / base table |

### TABLE_PRIVILEGES
- 資料表權限

欄位資訊

| 資料行名稱 | 描述 |
| -------- | -------- |
| GRANTOR | 權限同意授權者 |
| GRANTEE | 權限被授與者 |
| TABLE_NAME | 資料表名稱|
| PRIVILEGE_TYPE | 權限的類型 |
| IS_GRANTABLE | 被授與者是否可以將權限授與其他人 |

:::warning
Table 可以單獨開權限給指定的人看，因此每張 Table 的權限被授予者不一定相同。
:::

### TABLE_CONSTRAINTS
- 資料表條件約束

欄位資訊

| 資料行名稱 | 描述 |
| -------- | -------- |
| CONSTRAINT_NAME | 條件約束名稱 |
| TABLE_NAME | 資料表名稱 |
| CONSTRAINT_TYPE | 條件約束的類型：CHECK / UNIQUE / PRIMARY KEY / FOREIGN KEY |
| IS_DEFERRABLE | 指定條件約束檢查是否可以延後 (一律傳回 NO) |
| INITIALLY_DEFERRED | 指定一開始時是否延遲條件約束檢查 (一律傳回 NO) |

:::info
- `INITIALLY DEFERRED` : 表示僅在提交事務時檢查約束
:::

## VIEWS
- 所有 view tables

欄位資訊

| 資料行名稱 | 描述 |
| -------- | -------- |
| TABLE_NAME | 檢視表名稱 |
| VIEW_DEFINITION | 檢視定義文字 (若長度大於 nvarchar(4000)，則回傳的是 Null) |
| CHECK_OPTION | 若原始檢視是利用 WITH CHECK OPTION 所建立，則為 CASCADE，否則回傳 None |
| IS_UPDATABLE | 指定檢視是否可以更新 (一律傳回 NO) |

:::info
- `CHECK_OPTION` : view table 執行的所有數據遵循 select_statement 中設置的條件
:::

### VIEW_TABLE_USAGE
- view table 的相關資料表資訊 (base table)

欄位資訊

| 資料行名稱 | 描述 |
| -------- | -------- |
| VIEW_NAME | 檢視表名稱 |
| TABLE_NAME | base table 名稱|

### VIEW_COLUMN_USAGE
- view table 的相關資料行資訊

欄位資訊

| 資料行名稱 | 描述 |
| -------- | -------- |
| VIEW_NAME | 檢視表名稱 |
| TABLE_NAME | base table 名稱|
| COLUMN_NAME | 欄位名稱 |

## COLUMNS
- 所有資料行資訊

欄位資訊

| 資料行名稱 | 描述 |
| -------- | -------- |
| TABLE_NAME | 資料表名稱 |
| COLUMN_NAME | 資料行名稱 |
| ORDINAL_POSITION | 資料行識別碼 (流水號) |
| COLUMN_DEFAULT | 資料行的預設值 |
| IS_NULLABLE | 若資料行允許 Null，則回傳 Yes，否則回傳 No|
| DATA_TYPE| 資料類型 |+
| CHARACTER_MAXIMUM_LENGTH | 資料的最大長度 (以字元為單位)，-1 適用于 xml 和大數數值型別的資料，否則回傳 Null |
| NUMERIC_PRECISION | 數值資料有效位數，否則就回傳 Null |
| NUMERIC_SCALE | 數值資料有效小數位數，否則就回傳 Null |
| DATETIME_PRECISION | Datetime 和 ISO interval 資料類型的子類型代碼，如果是其他資料類型，就傳回 Null |
| CHARACTER_SET_NAME | 若資料行為字元資料或文字資料類型，回傳字元集名稱 (UNICODE / cp950)，否則回傳 Null |
| COLLATION_NAME | 若資料行為字元資料或文字資料類型，回傳定序名稱，否則回傳 Null (台灣地區預設為 Chinese_Taiwan_Stroke_CI_AS) |
| DOMAIN_NAME | 自定義資料類型名稱 |

:::warning
### utf8 & cp950
![](https://i.imgur.com/tugGpWU.png)

### 資料庫定序
SQL Server 在台灣的預設定序為 **Chinese_Taiwan_Stroke_CI_AS**，若用台灣縣市名稱排序，「彰化」會被排在最後一個；但若用 **Chinese_Taiwan_BOPOMOFO_CI_AI**( ㄅ, ㄆ, ㄇ...) 進行排序，「雲林」會被排在最後一個。
```sql=
select city from dbo.Citys
order by city COLLATE Chinese_Taiwan_BOPOMOFO_CI_AI
```

### DATETIME_PRECISION
![](https://i.imgur.com/M7sdW3K.png)

:::

### COLUMN_PRIVILEGES
- 資料行權限

欄位資訊

| 資料行名稱 | 描述 |
| -------- | -------- |
| GRANTOR | 權限同意授權者 |
| GRANTEE | 權限被授與者 |
| TABLE_NAME | 資料表名稱 |
| COLUMN_NAME | 資料行名稱 |
| PRIVILEGE_TYPE | 權限的類型 |
| IS_GRANTABLE | 指定被授與者是否可以將權限授與其他人 |

### COLUMN_DOMAIN_USAGE
- 具有使用者自定義資料型的資料行

欄位資訊

| 資料行名稱 | 描述 |
| -------- | -------- |
| DOMAIN_NAME | 使用者自定義資料類型名稱 |
| TABLE_NAME | 使用使用者自定義類型的資料表名稱 |
| COLUMN_NAME | 使用使用者自定義類型的資料行名稱 |

## DOMAINS
- 自定義資料類型，ex. 自定義 table 為資料類型

欄位資訊

| 資料行名稱 | 描述 |
| -------- | -------- |
| DOMAIN_NAME | 使用者自定義資料類型名稱 |
| DATA_TYPE | 系統提供的資料類型 |
| CHARACTER_MAXIMUM_LENGTH | 資料的最大長度 (以字元為單位)，-1 適用于 xml 和大數數值型別的資料，否則回傳 Null |
| COLLATION_NAME | 若資料行為字元資料或文字資料類型，回傳定序名稱，否則回傳 Null (台灣地區預設為 Chinese_Taiwan_Stroke_CI_AS) |
| CHARACTER_SET_NAME | 若資料行為字元資料或文字資料類型，回傳字元集名稱 (UNICODE / cp950)，否則回傳 Null |
| NUMERIC_PRECISION | 數值資料有效位數，否則就回傳 Null |
| NUMERIC_SCALE | 數值資料有效小數位數，否則就回傳 Null |
| DATETIME_PRECISION | Datetime 和 ISO interval 資料類型的子類型代碼，如果是其他資料類型，就傳回 Null |
| DOMAIN_DEFAULT | 定義 Transact-SQL 語句的實際文字 |

## DOMAIN_CONSTRAINTS
- 自定義資料類型的約束

欄位資訊

| 資料行名稱 | 描述 |
| -------- | -------- |
| CONSTRAINT_NAME | 規則名稱 |
| DOMAIN_NAME | 使用者自定義資料類型名稱 |
| IS_DEFERRABLE | 指定條件約束檢查是否可以延後 (一律傳回 NO) |
| INITIALLY_DEFERRED | 指定一開始時是否延遲條件約束檢查 (一律傳回 NO) |

## CONSTRAINT
### CONSTRAINT_TABLE_USAGE
- 目前具有條件約束的所有資料表

欄位資訊

| 資料行名稱 | 描述 |
| -------- | -------- |
| TABLE_NAME | 資料表名稱 |
| CONSTRAINT_NAME | 條件約束名稱 |

### CONSTRAINT_COLUMN_USAGE
- 目前具有條件約束的所有資料行

欄位資訊

| 資料行名稱 | 描述 |
| -------- | -------- |
| TABLE_NAME | 資料表名稱 |
| COLUMN_NAME | 資料行名稱 |
| CONSTRAINT_NAME | 條件約束名稱 |

## CHECK_CONSTRAINTS
- 限制可以放在列中的值範圍

欄位資訊

| 資料行名稱 | 描述 |
| -------- | -------- |
| CONSTRAINT_NAME | 條件約束名稱 |
| CHECK_CLAUSE | Transact-SQL 定義陳述式的實際內文 |

## KEY_COLUMN_USAGE
- 針對目前資料庫中被限制為索引鍵的每個資料行，各傳回一個資料列

欄位資訊

| 資料行名稱 | 描述 |
| -------- | -------- |
| CONSTRAINT_NAME | 條件約束名稱 |
| TABLE_NAME | 資料表名稱 |
| COLUMN_NAME| 資料行名稱 |
| ORDINAL_POSITION | 資料行序數位置 (流水號) |

## PARAMETERS
- 使用者自訂函數或預存程序的每個參數，各傳回一個資料列

欄位資訊

| 資料行名稱 | 描述 |
| -------- | -------- |
| SPECIFIC_NAME | function 或 sp 的名稱 |
| ORDINAL_POSITION | 參數的序數位置從 1 開始，若為函數回傳值則為 0 |
| PARAMETER_MODE | IN (輸入) / OUT (輸出) |
| IS_RESULT | 若為 function 輸出結果則回傳 YES，否則回傳 NO |
| PARAMETER_NAME | 參數名稱 |
| DATA_TYPE | 系統提供的資料類型 |
| CHARACTER_MAXIMUM_LENGTH | 資料的最大長度 (以字元為單位)，-1 適用于 xml 和大數數值型別的資料，否則回傳 Null |
| COLLATION_NAME | 若資料行為字元資料或文字資料類型，回傳定序名稱，否則回傳 Null (台灣地區預設為 Chinese_Taiwan_Stroke_CI_AS) |
| CHARACTER_SET_NAME | 若資料行為字元資料或文字資料類型，回傳字元集名稱 (UNICODE / cp950)，否則回傳 Null |
| NUMERIC_PRECISION | 數值資料有效位數，否則就回傳 Null |
| NUMERIC_SCALE | 數值資料有效小數位數，否則就回傳 Null |
| DATETIME_PRECISION | Datetime 和 ISO interval 資料類型的子類型代碼，如果是其他資料類型，就傳回 Null |

## REFERENTIAL_CONSTRAINTS
- FOREIGN KEY 的 Reference (PK)

欄位資訊

| 資料行名稱 | 描述 |
| -------- | -------- |
| CONSTRAINT_NAME | 條件約束名稱 (FK) |
| UNIQUE_CONSTRAINT_NAME | UNIQUE 條件約束名稱 (PK) |

## ROUTINES
- 預存程序和函數

欄位資訊

| 資料行名稱 | 描述 |
| -------- | -------- |
| SPECIFIC_NAME | function 或 sp 的名稱，與 ROUTINE_NAME 相同 |
| ROUTINE_NAME | function 或 sp 的名稱 |
| ROUTINE_TYPE | PROCEDURE (預存程序) / FUNCTION (函數) |
| DATA_TYPE | function 回傳值的類型，sp 則回傳 Null |
| CHARACTER_MAXIMUM_LENGTH | 資料的最大長度 (以字元為單位)，-1 適用于 xml 和大數數值型別的資料，否則回傳 Null |
| COLLATION_NAME | 若資料行為字元資料或文字資料類型，回傳定序名稱，否則回傳 Null (台灣地區預設為 Chinese_Taiwan_Stroke_CI_AS) |
| CHARACTER_SET_NAME | 若資料行為字元資料或文字資料類型，回傳字元集名稱 (UNICODE / cp950)，否則回傳 Null |
| NUMERIC_PRECISION | 數值資料有效位數，否則就回傳 Null |
| NUMERIC_SCALE | 數值資料有效小數位數，否則就回傳 Null |
| DATETIME_PRECISION | Datetime 和 ISO interval 資料類型的子類型代碼，如果是其他資料類型，就傳回 Null |
| ROUTINE_DEFINITION | 若函數或預存程序未加密，則傳回函數或預存程序之定義文字的前 4000 個字元，否則便傳回 Null |
| SQL_DATA_ACCESS | 回傳 NONE (函數不包含 SQL) / CONTAINS (函數可能包含 SQL) / READS (函數可能讀取 SQL) / MODIFIES (函數可能修改 SQL)；所有函數都傳回 READS，所有預存程序都傳回 MODIFIES |
| IS_NULL_CALL | 若 argument 是 Null，是否要呼叫 routine |
| MAX_DYNAMIC_RESULT_SETS | 回傳的最大 dynamic result set；若為函數則回傳 0 |
| CREATED | 建立時間 |
| LAST_ALTERED | 上次修改的時間 |

### ROUTINE_COLUMNS
- 資料表值函式所能回傳的資料行

欄位資訊

| 資料行名稱 | 描述 |
| -------- | -------- |
| TABLE_NAME | 資料表值函式的名稱 |
| COLUMN_NAME | 資料行名稱 |
| ORDINAL_POSITION | 資料行識別碼 (流水號) |
| COLUMN_DEFAULT | 資料行的預設值 |
| IS_NULLABLE | 若資料行允許 Null，回傳 YES，否則回傳 NO |
| DATA_TYPE | 系統提供的資料類型 |
| CHARACTER_MAXIMUM_LENGTH | 資料的最大長度 (以字元為單位)，-1 適用于 xml 和大數數值型別的資料，否則回傳 Null |
| NUMERIC_PRECISION | 數值資料有效位數，否則就回傳 Null |
| NUMERIC_SCALE | 數值資料有效小數位數，否則就回傳 Null |
| DATETIME_PRECISION | Datetime 和 ISO interval 資料類型的子類型代碼，如果是其他資料類型，就傳回 Null |
| CHARACTER_SET_NAME | 若資料行為字元資料或文字資料類型，回傳字元集名稱 (UNICODE / cp950)，否則回傳 Null |
| COLLATION_NAME | 若資料行為字元資料或文字資料類型，回傳定序名稱，否則回傳 Null (台灣地區預設為 Chinese_Taiwan_Stroke_CI_AS) |
| DOMAIN_NAME | 自定義資料類型名稱 |

## SCHEMATA
- 資料庫內的所有 schema

欄位資訊

| 資料行名稱 | 描述 |
| -------- | -------- |
| SCHEMA_NAME | 架構名稱 |
| SCHEMA_OWNER | 結構描述擁有者名稱 |
| DEFAULT_CHARACTER_SET_NAME | 回傳預設字元集的名稱 |

