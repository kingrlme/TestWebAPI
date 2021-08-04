USE [EmmaDB]
GO

/****** Object:  Table [dbo].[Organization]    Script Date: 2021/3/25 �W�� 08:24:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Organization](
	[SID] [bigint] NOT NULL,
	[OrgID] [nvarchar](50) NULL,
	[OrgName] [nvarchar](50) NULL,
	[OrgStatus] [nvarchar](50) NULL,
	[OrgCreateYear] [nvarchar](50) NULL,
	[OrgCreateMonth] [nvarchar](50) NULL,
	[CreateTime] [nvarchar](50) NULL,
	[WGS84X] [decimal](20, 5) NULL,
	[WGS84Y] [decimal](20, 5) NULL,
	[Geom] [geometry] NULL,
 CONSTRAINT [PK_Organization] PRIMARY KEY CLUSTERED 
(
	[SID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�۰ʼW�[���y����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Organization', @level2type=N'COLUMN',@level2name=N'SID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��´ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Organization', @level2type=N'COLUMN',@level2name=N'OrgID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��´�W��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Organization', @level2type=N'COLUMN',@level2name=N'OrgName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��´���A' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Organization', @level2type=N'COLUMN',@level2name=N'OrgStatus'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��´�إߦ褸�~��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Organization', @level2type=N'COLUMN',@level2name=N'OrgCreateYear'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��´�إߤ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Organization', @level2type=N'COLUMN',@level2name=N'OrgCreateMonth'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�إ߮ɶ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Organization', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'WGS84X' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Organization', @level2type=N'COLUMN',@level2name=N'WGS84X'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'WGS84Y' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Organization', @level2type=N'COLUMN',@level2name=N'WGS84Y'
GO


