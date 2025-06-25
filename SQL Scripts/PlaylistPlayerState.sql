
/****** Object:  Table [dbo].[PlaylistPlayerState]    Script Date: 24/06/2025 19:40:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PlaylistPlayerState](
	[PlaylistId] [int] NOT NULL,
	[VideoId] [int] NOT NULL,
	[OrderIndex] [int] NOT NULL,
	[Position] [decimal](10, 2) NOT NULL,
	[IsPlayed] [bit] NOT NULL,
	[SequenceType] [varchar](10) NOT NULL,
	[LastUpdated] [datetime] NOT NULL,
 CONSTRAINT [PK_PlaylistPlayerState] PRIMARY KEY CLUSTERED 
(
	[PlaylistId] ASC,
	[VideoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PlaylistPlayerState] ADD  DEFAULT ((0.00)) FOR [Position]
GO

ALTER TABLE [dbo].[PlaylistPlayerState] ADD  DEFAULT ((0)) FOR [IsPlayed]
GO

ALTER TABLE [dbo].[PlaylistPlayerState] ADD  DEFAULT ('Ordered') FOR [SequenceType]
GO

ALTER TABLE [dbo].[PlaylistPlayerState] ADD  DEFAULT (getdate()) FOR [LastUpdated]
GO

ALTER TABLE [dbo].[PlaylistPlayerState]  WITH CHECK ADD  CONSTRAINT [CHK_SequenceType] CHECK  (([SequenceType]='Random' OR [SequenceType]='Ordered'))
GO

ALTER TABLE [dbo].[PlaylistPlayerState] CHECK CONSTRAINT [CHK_SequenceType]
GO


