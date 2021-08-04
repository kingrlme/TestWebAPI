UPDATE [Organization] SET [WGS84X] = 121.500954,[WGS84Y] = 23.580796 WHERE SID=1;
UPDATE [Organization] SET [WGS84X] = 121.520653,[WGS84Y] = 23.919065 WHERE SID=2;
UPDATE [Organization] SET [WGS84X] = 121.409947,[WGS84Y] = 23.222814 WHERE SID=3;
UPDATE [Organization] SET [WGS84X] = 121.109262,[WGS84Y] = 22.80994 WHERE SID=4;
UPDATE [Organization] SET [WGS84X] = 121.491632,[WGS84Y] = 23.869805 WHERE SID=5;
UPDATE [Organization] SET [WGS84X] = 121.259067,[WGS84Y] = 22.991364 WHERE SID=6;
UPDATE [Organization] SET [WGS84X] = 121.504002,[WGS84Y] = 23.483253 WHERE SID=7;
UPDATE [Organization] SET [WGS84X] = 121.425585,[WGS84Y] = 23.675106 WHERE SID=8;
UPDATE [Organization] SET [WGS84X] = 121.586864,[WGS84Y] = 23.887746 WHERE SID=9;
UPDATE [Organization] SET [WGS84X] = 121.467737,[WGS84Y] = 23.815739 WHERE SID=10;
UPDATE [Organization] SET [WGS84X] = 121.616361,[WGS84Y] = 24.05001 WHERE SID=11;
UPDATE [Organization] SET [WGS84X] = 120.877771,[WGS84Y] = 22.406431 WHERE SID=12;

--更新點位空間資料，方法一
UPDATE [Organization]
SET [Geom] = geometry::STGeomFromText('POINT(' + CAST(WGS84X AS NVARCHAR(20)) + ' ' + CAST(CAST(WGS84Y AS decimal(20, 8)) AS nvarchar) + ')', 0);
	