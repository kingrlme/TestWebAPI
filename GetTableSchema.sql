--取出資料庫內所有資料表的名稱
SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_CATALOG='EmmaGeoDB'

--顯示某個資料庫中的每個資料表的每個欄位描述
SELECT COLUMN_NAME,ORDINAL_POSITION,DATA_TYPE,CHARACTER_MAXIMUM_LENGTH 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'Organization'

--依資料表名稱找出此表的PK欄位
SELECT COLUMN_NAME 
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE TABLE_NAME = 'Organization'

--顯示某個資料庫中的每個資料表的每個欄位描述
SELECT a.Table_name       as 表格名稱
	  ,b.COLUMN_NAME                          as 欄位名稱
	  ,b.DATA_TYPE                            as 資料型別
	  ,isnull(b.CHARACTER_MAXIMUM_LENGTH,'')  as 長度
	  ,isnull(b.COLUMN_DEFAULT,'')            as 預設值
	  ,b.IS_NULLABLE                          as 允許空值
	  ,( SELECT value
			   FROM fn_listextendedproperty (NULL, 'schema', a.Table_schema, 'table', a.TABLE_NAME, 'column', default)
			  WHERE name='MS_Description' 
				and objtype='COLUMN' 
				and objname Collate Chinese_Taiwan_Stroke_CI_AS = b.COLUMN_NAME
		) as 欄位備註
FROM 
	INFORMATION_SCHEMA.TABLES  a
	LEFT JOIN INFORMATION_SCHEMA.COLUMNS b ON a.TABLE_NAME = b.TABLE_NAME
WHERE a.Table_name='Organization'
ORDER BY a.TABLE_NAME, b.ORDINAL_POSITION