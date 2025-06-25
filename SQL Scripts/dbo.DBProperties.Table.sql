USE [VideoPlayerDebug]
GO
/****** Object:  Table [dbo].[DBProperties]    Script Date: 24/06/2025 21:17:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DBProperties](
	[PropertyName] [nvarchar](256) NOT NULL,
	[PropertyValue] [nvarchar](max) NULL,
	[Memo] [nvarchar](max) NULL,
 CONSTRAINT [PK_DBProperty] PRIMARY KEY CLUSTERED 
(
	[PropertyName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
