--���涰
--SELECT @geom1 = GeomCol1 FROM Spatail_Points WHERE [PointID] = 1;  
--SELECT @geom2 = GeomCol1 FROM Spatail_Points WHERE [PointID] = 2;  
--SELECT @result = @geom1.STIntersection(@geom2);  
--SELECT @result.STAsText();  

--��s�I��Ŷ���ơA��k�@
UPDATE Spatail_Points
SET [GeomCol1] = geometry::STGeomFromText('POINT(' + CAST(WGS84X AS NVARCHAR(20)) 
 + ' ' + CAST(CAST(WGS84Y AS decimal(20, 8)) AS nvarchar) + ')', 0);

--��s�I��Ŷ���ơA��k�G
UPDATE Spatail_Points
SET [GeomCol1] = geometry::STGeomFromText([GeomCol2], 0);

 