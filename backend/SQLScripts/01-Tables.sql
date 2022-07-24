USE [YFS]
GO

/****** Object:  Table [dbo].[AccountGroup]    Script Date: 22.07.2022 6:26:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AccountGroup](
	[AccountGroupId] [int] IDENTITY(1,1) NOT NULL,
	[AccountGroupNameEn] [varchar](255) NOT NULL,
	[AccountGroupNameRu] [varchar](255) NOT NULL,
	[AccountGroupNameUa] [varchar](255) NOT NULL,
	[GroupOrderBy] [int] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[UserId] [int] NULL,
 CONSTRAINT [PK_AccountGroup] PRIMARY KEY CLUSTERED 
(
	[AccountGroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AccountGroup] ADD  CONSTRAINT [DF_AccountGroup_AccountGroupNameEn]  DEFAULT ('') FOR [AccountGroupNameEn]
GO

ALTER TABLE [dbo].[AccountGroup] ADD  CONSTRAINT [DF_AccountGroup_AccountGroupNameRu]  DEFAULT ('') FOR [AccountGroupNameRu]
GO

ALTER TABLE [dbo].[AccountGroup] ADD  CONSTRAINT [DF_AccountGroup_AccountGroupNameUa]  DEFAULT ('') FOR [AccountGroupNameUa]
GO

ALTER TABLE [dbo].[AccountGroup] ADD  CONSTRAINT [DF_ACCOUNT_GROUP_Acc_grp_orderby]  DEFAULT ((0)) FOR [GroupOrderBy]
GO

ALTER TABLE [dbo].[AccountGroup] ADD  CONSTRAINT [DF_ACCOUNT_GROUP_Acc_created_time]  DEFAULT (getdate()) FOR [CreatedTime]
GO


/****** Object:  Table [dbo].[AccountGroupDefault]    Script Date: 22.07.2022 6:26:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AccountGroupDefault](
	[AccountGroupDefaultId] [int] IDENTITY(1,1) NOT NULL,
	[AccountGroupDefaultNameEn] [varchar](255) NOT NULL,
	[AccountGroupDefaultNameRu] [varchar](255) NOT NULL,
	[AccountGroupDefaultNameUa] [varchar](255) NOT NULL,
	[GroupOrderBy] [int] NOT NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AccountGroupDefault] ADD  CONSTRAINT [DF_AccountGroupDefault_OrderBy]  DEFAULT ((0)) FOR [GroupOrderBy]
GO

/****** Object:  Table [dbo].[Accounts]    Script Date: 22.07.2022 6:27:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Accounts](
	[AccountId] [int] IDENTITY(1,1) NOT NULL,
	[AccountName] [varchar](255) NOT NULL,
	[AccountGroupId] [int] NOT NULL,
	[CurrencyId] [int] NOT NULL
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[AccountsDefault]    Script Date: 22.07.2022 6:27:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AccountsDefault](
	[AccountDefaultId] [int] IDENTITY(1,1) NOT NULL,
	[AccountGroupDefaultId] [int] NOT NULL,
	[AccountDefaultNameEn] [varchar](255) NOT NULL,
	[AccountDefaultNameRu] [varchar](255) NOT NULL,
	[AccountDefaultNameUa] [varchar](255) NOT NULL
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Currency]    Script Date: 22.07.2022 6:28:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Currency](
	[CurrencyKey] [int] NOT NULL,
	[CurrencyNameUs] [varchar](100) NOT NULL,
	[CurrencyNameRu] [varchar](100) NOT NULL,
	[CurrencyNameUa] [varchar](100) NOT NULL,
	[CurrencyShortNameUs] [varchar](10) NOT NULL,
	[CurrencyShortNameRu] [varchar](10) NOT NULL,
	[CurrencyShortNameUa] [varchar](10) NOT NULL
) ON [PRIMARY]
GO

USE [YFS]
GO

/****** Object:  Table [dbo].[Users]    Script Date: 22.07.2022 6:28:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[Email] [varchar](100) NULL,
	[Password] [varchar](100) NOT NULL,
	[Created_Time] [datetime] NOT NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_USERS_Password]  DEFAULT ('') FOR [Password]
GO

ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_USERS_Created]  DEFAULT (getdate()) FOR [Created_Time]
GO







