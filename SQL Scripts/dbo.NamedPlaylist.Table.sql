USE [VideoPlayerDebug]
GO
/****** Object:  Table [dbo].[NamedPlaylist]    Script Date: 24/06/2025 21:17:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NamedPlaylist](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[LastPlayedVideoId] [int] NOT NULL,
	[LastPlayedPosition] [int] NOT NULL,
	[SortPreference] [nvarchar](20) NOT NULL,
	[FilterPreference] [nvarchar](max) NULL,
 CONSTRAINT [PK_NamedPlaylist] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[NamedPlaylist] ADD  DEFAULT ((0)) FOR [LastPlayedVideoId]
GO
ALTER TABLE [dbo].[NamedPlaylist] ADD  DEFAULT ((0)) FOR [LastPlayedPosition]
GO
ALTER TABLE [dbo].[NamedPlaylist] ADD  CONSTRAINT [DF_NamedPlaylist_SortPreference]  DEFAULT ('Name') FOR [SortPreference]
GO
ALTER TABLE [dbo].[NamedPlaylist] ADD  CONSTRAINT [DF_NamedPlaylist_FilterPreference]  DEFAULT ('None') FOR [FilterPreference]
GO
