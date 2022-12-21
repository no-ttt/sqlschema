
EXEC sys.sp_addextendedproperty 
    @name = 'DS_fn_GetDistance_', @value = '取得距離',             
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = 'FUNCTION', @level1name = 'fn_GetDistance'
        

EXEC sys.sp_addextendedproperty 
    @name = 'DS_fn_GetDistance_@GPSLng', @value = 'GPS 經度',             
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = 'FUNCTION', @level1name = 'fn_GetDistance',
    @level2type = 'PARAMETER', @level2name = '@GPSLng'
        

EXEC sys.sp_addextendedproperty 
    @name = 'DS_fn_GetDistance_@GPSLat', @value = 'GPS 緯度',             
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = 'FUNCTION', @level1name = 'fn_GetDistance',
    @level2type = 'PARAMETER', @level2name = '@GPSLat'
        

EXEC sys.sp_addextendedproperty 
    @name = 'DS_fn_GetDistance_@Lng', @value = '經度',             
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = 'FUNCTION', @level1name = 'fn_GetDistance',
    @level2type = 'PARAMETER', @level2name = '@Lng'
        

EXEC sys.sp_addextendedproperty 
    @name = 'DS_fn_GetDistance_@Lat', @value = '緯度',             
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = 'FUNCTION', @level1name = 'fn_GetDistance',
    @level2type = 'PARAMETER', @level2name = '@Lat'
        

EXEC sys.sp_addextendedproperty 
    @name = 'DS_vd_Spot_', @value = 'spot view table',             
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = 'VIEW', @level1name = 'vd_Spot'
        

EXEC sys.sp_addextendedproperty 
    @name = 'DS_vd_Spot_OID', @value = 'ID',             
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = 'VIEW', @level1name = 'vd_Spot',
    @level2type = 'COLUMN', @level2name = 'OID'
        

EXEC sys.sp_addextendedproperty 
    @name = 'DS_vd_Spot_Name', @value = '景點名稱',             
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = 'VIEW', @level1name = 'vd_Spot',
    @level2type = 'COLUMN', @level2name = 'Name'
        

EXEC sys.sp_addextendedproperty 
    @name = 'DS_vd_Spot_Type', @value = '景點種類',             
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = 'VIEW', @level1name = 'vd_Spot',
    @level2type = 'COLUMN', @level2name = 'Type'
        

EXEC sys.sp_addextendedproperty 
    @name = 'DS_vd_Spot_city', @value = '所在縣市',             
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = 'VIEW', @level1name = 'vd_Spot',
    @level2type = 'COLUMN', @level2name = 'city'
        

EXEC sys.sp_addextendedproperty 
    @name = 'DS_vd_Spot_town', @value = '所在城鎮',             
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = 'VIEW', @level1name = 'vd_Spot',
    @level2type = 'COLUMN', @level2name = 'town'
        

EXEC sys.sp_addextendedproperty 
    @name = 'DS_vd_Spot_PictureUrl', @value = '照片連結',             
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = 'VIEW', @level1name = 'vd_Spot',
    @level2type = 'COLUMN', @level2name = 'PictureUrl'
        

EXEC sys.sp_addextendedproperty 
    @name = 'DS_get_city_', @value = '取得縣市',             
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = 'PROCEDURE', @level1name = 'get_city'
        

EXEC sys.sp_addextendedproperty 
    @name = 'DS_get_city_@city', @value = '縣市資訊',             
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = 'PROCEDURE', @level1name = 'get_city',
    @level2type = 'PARAMETER', @level2name = '@city'
        

EXEC sys.sp_addextendedproperty 
    @name = 'DS_Restaurant_', @value = '全台所有餐廳',             
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = 'TABLE', @level1name = 'Restaurant'
        

EXEC sys.sp_addextendedproperty 
    @name = 'DS_Restaurant_RID', @value = 'ID',             
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = 'TABLE', @level1name = 'Restaurant',
    @level2type = 'COLUMN', @level2name = 'RID'
        

EXEC sys.sp_addextendedproperty 
    @name = 'DS_Restaurant_PictureUrl', @value = '照片連結',             
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = 'TABLE', @level1name = 'Restaurant',
    @level2type = 'COLUMN', @level2name = 'PictureUrl'
        

EXEC sys.sp_addextendedproperty 
    @name = 'DS_Restaurant_PictureDes', @value = '照片介紹',             
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = 'TABLE', @level1name = 'Restaurant',
    @level2type = 'COLUMN', @level2name = 'PictureDes'
        

EXEC sys.sp_addextendedproperty 
    @name = 'DS_Restaurant_OpenTime', @value = '營業時間',             
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = 'TABLE', @level1name = 'Restaurant',
    @level2type = 'COLUMN', @level2name = 'OpenTime'
        

EXEC sys.sp_addextendedproperty 
    @name = 'DS_Restaurant_ParkingInfo', @value = '停車資訊',             
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = 'TABLE', @level1name = 'Restaurant',
    @level2type = 'COLUMN', @level2name = 'ParkingInfo'
        
