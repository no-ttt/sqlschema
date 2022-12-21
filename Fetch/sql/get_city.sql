CREATE PROCEDURE get_city  -- 取得縣市
	-- Add the parameters for the stored procedure here
	@city VARCHAR(20) -- 縣市資訊
AS
BEGIN
	DECLARE @search_c AS VARCHAR(20);
	SET @search_c = '%'+@city+'%';

	select o.CName as Spot, o.CDes as Des, O.Telephone, O.Address, O.PositionLat, O.PositionLon, c2.CName as city
	from Class c1, co, Object o, Inheritance i, Class c2
	WHERE O.Address like @search_c AND O.OID = CO.OID AND CO.CID = C1.CID and c1.CID = i.CCID and i.PCID = c2.CID
END