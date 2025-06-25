USE [VideoPlayerDebug]
GO
/****** Object:  Table [dbo].[Video]    Script Date: 24/06/2025 21:17:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Video](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](200) NOT NULL,
	[GenreId] [int] NOT NULL,
	[Rating] [decimal](3, 1) NOT NULL,
	[IsFavorite] [bit] NOT NULL,
	[DurationMinutes] [int] NOT NULL,
	[ReleaseYear] [int] NOT NULL,
	[FilePath] [nvarchar](500) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[FileDate] [datetime] NULL,
	[LastPlayed] [datetime] NULL,
 CONSTRAINT [PK_Video] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_Video_FilePath] UNIQUE NONCLUSTERED 
(
	[FilePath] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Video] ADD  DEFAULT ((1)) FOR [GenreId]
GO
ALTER TABLE [dbo].[Video] ADD  DEFAULT ((0)) FOR [Rating]
GO
ALTER TABLE [dbo].[Video] ADD  DEFAULT ((1)) FOR [IsFavorite]
GO
ALTER TABLE [dbo].[Video] ADD  DEFAULT ((0)) FOR [DurationMinutes]
GO
ALTER TABLE [dbo].[Video] ADD  DEFAULT ((0)) FOR [ReleaseYear]
GO
ALTER TABLE [dbo].[Video] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Video]  WITH CHECK ADD CHECK  (([Rating]>=(0) AND [Rating]<=(10)))
GO
