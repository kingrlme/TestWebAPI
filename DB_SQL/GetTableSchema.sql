--���X��Ʈw���Ҧ���ƪ��W��
SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_CATALOG='EmmaGeoDB'

--��ܬY�Ӹ�Ʈw�����C�Ӹ�ƪ��C�����y�z
SELECT COLUMN_NAME,ORDINAL_POSITION,DATA_TYPE,CHARACTER_MAXIMUM_LENGTH 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'Organization'

--�̸�ƪ�W�٧�X����PK���
SELECT COLUMN_NAME 
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE TABLE_NAME = 'Organization'

--��ܬY�Ӹ�Ʈw�����C�Ӹ�ƪ��C�����y�z
SELECT a.Table_name       as ���W��
	  ,b.COLUMN_NAME                          as ���W��
	  ,b.DATA_TYPE                            as ��ƫ��O
	  ,isnull(b.CHARACTER_MAXIMUM_LENGTH,'')  as ����
	  ,isnull(b.COLUMN_DEFAULT,'')            as �w�]��
	  ,b.IS_NULLABLE                          as ���\�ŭ�
	  ,( SELECT value
			   FROM fn_listextendedproperty (NULL, 'schema', a.Table_schema, 'table', a.TABLE_NAME, 'column', default)
			  WHERE name='MS_Description' 
				and objtype='COLUMN' 
				and objname Collate Chinese_Taiwan_Stroke_CI_AS = b.COLUMN_NAME
		) as ���Ƶ�
FROM 
	INFORMATION_SCHEMA.TABLES  a
	LEFT JOIN INFORMATION_SCHEMA.COLUMNS b ON a.TABLE_NAME = b.TABLE_NAME
WHERE a.Table_name='Organization'
ORDER BY a.TABLE_NAME, b.ORDINAL_POSITION