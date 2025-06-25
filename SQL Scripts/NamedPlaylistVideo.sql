
/****** Object:  Table [dbo].[NamedPlaylistVideo]    Script Date: 24/06/2025 19:39:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NamedPlaylistVideo](
	[PlaylistId] [int] NOT NULL,
	[VideoId] [int] NOT NULL,
	[OrderIndex] [int] NOT NULL,
 CONSTRAINT [PK_NamedPlaylistVideo] PRIMARY KEY CLUSTERED 
(
	[PlaylistId] ASC,
	[VideoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[NamedPlaylistVideo] ADD  DEFAULT ((0)) FOR [OrderIndex]
GO


