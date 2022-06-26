USE [YFS]
GO

/****** Object:  Table [dbo].[USERS]    Script Date: 21.06.2022 9:17:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[USERS](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Email] [varchar](100) NULL,
	[Password] [varchar](100) NOT NULL,
	[Created] [datetime] NOT NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[USERS] ADD  CONSTRAINT [DF_USERS_Password]  DEFAULT ('') FOR [Password]
GO

ALTER TABLE [dbo].[USERS] ADD  CONSTRAINT [DF_USERS_Created]  DEFAULT (getdate()) FOR [Created]
GO



