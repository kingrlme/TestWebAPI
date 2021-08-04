USE [EmmaGeoDB]
GO

/****** Object:  Table [dbo].[Spatail_Points]    Script Date: 2021/3/23 下午 01:51:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Spatail_Points](
	[PointID] [bigint] NOT NULL,
	[PointName] [nvarchar](50) NULL,
	[WGS84X] [decimal](20, 5) NULL,
	[WGS84Y] [decimal](20, 5) NULL,
	[TWD97X] [decimal](20, 5) NULL,
	[TWD97Y] [decimal](20, 5) NULL,
	[UID] [nvarchar](50) NULL,
	[CreateTime] [nvarchar](50) NULL,
	[GeomCol1] [geometry] NULL,
	[GeomCol2]  AS ([GeomCol1].[STAsText]())
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'點位ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Spatail_Points', @level2type=N'COLUMN',@level2name=N'PointID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'點位名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Spatail_Points', @level2type=N'COLUMN',@level2name=N'PointName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'WGS84X' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Spatail_Points', @level2type=N'COLUMN',@level2name=N'WGS84X'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'WGS84Y' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Spatail_Points', @level2type=N'COLUMN',@level2name=N'WGS84Y'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'TWD97X' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Spatail_Points', @level2type=N'COLUMN',@level2name=N'TWD97X'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'TWD97Y' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Spatail_Points', @level2type=N'COLUMN',@level2name=N'TWD97Y'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建立者帳號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Spatail_Points', @level2type=N'COLUMN',@level2name=N'UID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'資料建立時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Spatail_Points', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'空間資料' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Spatail_Points', @level2type=N'COLUMN',@level2name=N'GeomCol1'
GO


