CREATE FUNCTION fn_GetDistance  -- 取得距離
(   
@GPSLng DECIMAL(38,35),  -- GPS 經度
@GPSLat DECIMAL(38,35),  -- GPS 緯度
@Lng DECIMAL(38,35),  -- 經度
@Lat DECIMAL(38,35)    -- 緯度
)  
RETURNS DECIMAL(38,35)  
as  
BEGIN  
   DECLARE @result DECIMAL(38,35)  
   SELECT @result = 6378137.0*ACOS(SIN(@GPSLat/180*PI())*SIN(@Lat/180*PI())+COS(@GPSLat/180*PI())*COS(@Lat/180*PI())*COS((@GPSLng-@Lng)/180*PI()))  
   RETURN @result  
END