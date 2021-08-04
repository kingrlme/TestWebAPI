--取交集
--SELECT @geom1 = GeomCol1 FROM Spatail_Points WHERE [PointID] = 1;  
--SELECT @geom2 = GeomCol1 FROM Spatail_Points WHERE [PointID] = 2;  
--SELECT @result = @geom1.STIntersection(@geom2);  
--SELECT @result.STAsText();  

--更新點位空間資料，方法一
UPDATE Spatail_Points
SET [GeomCol1] = geometry::STGeomFromText('POINT(' + CAST(WGS84X AS NVARCHAR(20)) 
 + ' ' + CAST(CAST(WGS84Y AS decimal(20, 8)) AS nvarchar) + ')', 0);

--更新點位空間資料，方法二
UPDATE Spatail_Points
SET [GeomCol1] = geometry::STGeomFromText([GeomCol2], 0);

 