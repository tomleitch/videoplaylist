USE [VideoPlayerDebug]
GO
/****** Object:  UserDefinedTableType [dbo].[IdList]    Script Date: 24/06/2025 21:17:59 ******/
CREATE TYPE [dbo].[IdList] AS TABLE(
	[TypeId] [int] NOT NULL,
	[Id] [int] NOT NULL
)
GO
