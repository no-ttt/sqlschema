ð è³æåº«è³å®å¥æª¢èåæç³»çµ±

ð å¯¦ä½ç­è¨ï¼https://hackmd.io/@nooo/Hy8oHbgjc

# ç³»çµ±ä½¿ç¨èªªæ
## å®è£ç°å¢
ä¸è¼ [.Net Core 3.1](https://dotnet.microsoft.com/en-us/download/dotnet/3.1)

![](https://i.imgur.com/bQuDYvL.png)


## å¦ä½å·è¡ ?
1. ä¸è¼å°æ¡
2. æé `appsettings.json`ï¼è¼¸å¥è³æåº«è³è¨

![](https://i.imgur.com/Taoh5v2.png)

3. å·è¡ `WebAPI.exe`ï¼é£ç· http://localhost:5000


# èªååæ·åè¨»è§£
> execute: regex.py

## æ³¨æäºé 
### 1. Table / View / Function / Procedure çè¨»è§£è«å¯«å¨ `create` é£ä¸è¡

example. 
```sql=
CREATE TABLE Restaurant ( -- å¨å°ææé¤å»³
    ...
);
```
### 2. æ¬ä½è¨»è§£è«è·è¦è¨»è§£çæ¬ä½åç¨±å¯«å¨åä¸è¡

example. 
```sql=
CREATE TABLE Restaurant (
    RID int, -- ID
    PictureUrl varchar(400), -- ç§çé£çµ
    PictureDes nvarchar(8000), -- ç§çä»ç´¹
    OpenTime nvarchar(1000), -- çæ¥­æé
    ParkingInfo nvarchar(1000) -- åè»è³è¨
);
```

### 3. Function / Procedure çåæ¸å½åä¸è¦å  `AS`
example.

```sql=
CREATE PROCEDURE get_city  -- åå¾ç¸£å¸
	@city VARCHAR(20) -- ç¸£å¸è³è¨
    -- @city AS VARCHAR(20)  (X)
AS
BEGIN
...
```

## View Table ä½¿ç¨ä¾å¤
view table å¯«æ³è¼çºå¤åï¼æä»¥æå¯è½ææä¸å°è¨»è§£ ... QQ
ç®ååªæéæ¬¾å¯«æ³æéï¼

```sql=
CREATE VIEW vd_Spot AS  -- spot view table
SELECT 
    o.OID as OID,  -- ID
    o.CName as Name, -- æ¯é»åç¨±
    o.Type as Type,  -- æ¯é»ç¨®é¡
    c2.CName as city, -- æå¨ç¸£å¸
    c.CName as town,   -- æå¨åé®
    s.PictureUrl as PictureUrl  -- ç§çé£çµ
FROM Object o, Class c, Class c2, CO, ScenicSpot s, Inheritance i
...
```

### è§£æ±ºæ¹æ³
1. å¤å¯«ä¸é» Regex expression
2. ç´æ¥éåç«¯å¯«è¨»è§£ (æ¨ ðð»)

## å¦ä½å·è¡
1. æé `regex.py` å¡«ä¸è¼¸å¥è³æå¤¾åè¼¸åºæªæ¡çè·¯å¾

![](https://i.imgur.com/umahVc4.png)

:::warning
ð è«æææè¦é²è¡æªåè¨»è§£ç `.sql` æªæ¾å°åä¸åè³æå¤¾å§ !!
:::

2. å·è¡ `python3 regex.py`


