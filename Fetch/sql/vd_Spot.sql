CREATE VIEW vd_Spot AS  -- spot view table
SELECT 
    o.OID as OID,  -- ID
    o.CName as Name, -- 景點名稱
    o.Type as Type,  -- 景點種類
    c2.CName as city, -- 所在縣市
    c.CName as town,   -- 所在城鎮
    s.PictureUrl as PictureUrl  -- 照片連結
FROM Object o, Class c, Class c2, CO, ScenicSpot s, Inheritance i
where c.CID = co.CID and co.OID = o.OID and o.OID = s.SID and c.CID = i.CCID and i.PCID = c2.CID
union
SELECT o.OID, o.CName as Name, o.Type, c2.CName as city, c.CName as town, r.PictureUrl
FROM Object o, Class c, Class c2, CO, Restaurant r, Inheritance i
where c.CID = co.CID and co.OID = o.OID and o.OID = r.RID and c.CID = i.CCID and i.PCID = c2.CID
union
SELECT o.OID, o.CName as Name, o.Type, c2.CName as city, c.CName as town, h.PictureUrl
FROM Object o, Class c, Class c2, CO, Hotel h, Inheritance i
where c.CID = co.CID and co.OID = o.OID and o.OID = h.HID and c.CID = i.CCID and i.PCID = c2.CID
union
SELECT o.OID, o.CName as Name, o.Type, c2.CName as city, c.CName as town, a.PictureUrl
FROM Object o, Class c, Class c2, CO, Activity a, Inheritance i
where c.CID = co.CID and co.OID = o.OID and o.OID = a.aID and c.CID = i.CCID and i.PCID = c2.CID