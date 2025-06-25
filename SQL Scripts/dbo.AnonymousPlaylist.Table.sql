USE [VideoPlayerDebug]
GO
/****** Object:  Table [dbo].[AnonymousPlaylist]    Script Date: 24/06/2025 21:17:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AnonymousPlaylist](
	[Id] [int] NOT NULL,
	[VideoIds] [nvarchar](max) NULL,
	[LastPlayedVideoId] [int] NOT NULL,
	[LastPlayedPosition] [int] NOT NULL,
	[SortPreference] [nvarchar](20) NOT NULL,
	[FilterPreference] [nvarchar](max) NULL,
 CONSTRAINT [PK_AnonymousPlaylist] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[AnonymousPlaylist] ADD  DEFAULT ((1)) FOR [Id]
GO
ALTER TABLE [dbo].[AnonymousPlaylist] ADD  DEFAULT ((0)) FOR [LastPlayedVideoId]
GO
ALTER TABLE [dbo].[AnonymousPlaylist] ADD  DEFAULT ((0)) FOR [LastPlayedPosition]
GO
