--新增數筆資料
--LINESTRING
DECLARE @NEXT INT SELECT @NEXT = MAX([PointID]) FROM [Spatail_Points]
SET @NEXT = CASE WHEN @NEXT IS NULL THEN 1 ELSE @NEXT + 1 END 
INSERT INTO [Spatail_Points]
           ([PointID],[PointName],
		   [WGS84X],[WGS84Y],[TWD97X],[TWD97Y],
		   [UID],[CreateTime],[GeomCol1])
     VALUES
           (@NEXT, '點位'+ CAST(@NEXT as nvarchar(10)) , 
		   121.350034637712, 24.9533949910592, 285345.95, 2760661.08 , 
		   '', convert(varchar,getdate(),112)+replace(convert(varchar,getdate(),108),':',''),
		   geometry::STGeomFromText('LINESTRING (100 100, 20 180, 180 180)', 0));
GO

--Point
DECLARE @NEXT INT SELECT @NEXT = MAX([PointID]) FROM [Spatail_Points]
SET @NEXT = CASE WHEN @NEXT IS NULL THEN 1 ELSE @NEXT + 1 END 
INSERT INTO [Spatail_Points]
           ([PointID],[PointName],
		   [WGS84X],[WGS84Y],[TWD97X],[TWD97Y],
		   [UID],[CreateTime],[GeomCol1])
     VALUES
           (@NEXT, '點位'+ CAST(@NEXT as nvarchar(10)) , 
		   121.353188646307, 24.9539004050855, 285664.3, 2760717.88 , 
		   '', convert(varchar,getdate(),112)+replace(convert(varchar,getdate(),108),':',''),
		   geometry::STGeomFromText('POINT(121.353188646307 24.9539004050855)', 0));
GO


--POLYGON
DECLARE @NEXT INT SELECT @NEXT = MAX([PointID]) FROM [Spatail_Points]
SET @NEXT = CASE WHEN @NEXT IS NULL THEN 1 ELSE @NEXT + 1 END 
INSERT INTO [Spatail_Points]
           ([PointID],[PointName],
		   [WGS84X],[WGS84Y],[TWD97X],[TWD97Y],
		   [UID],[CreateTime],[GeomCol1])
     VALUES
           (@NEXT, '點位'+ CAST(@NEXT as nvarchar(10)) , 
		   121.351048208332, 24.9533817709754, 285448.31, 2760659.88 , 
		   '', convert(varchar,getdate(),112)+replace(convert(varchar,getdate(),108),':',''),
		   geometry::STGeomFromText('POLYGON ((0 0, 150 0, 150 150, 0 150, 0 0))', 0));
GO


