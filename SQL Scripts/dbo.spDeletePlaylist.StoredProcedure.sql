USE [VideoPlayerDebug]
GO
/****** Object:  StoredProcedure [dbo].[spDeletePlaylist]    Script Date: 24/06/2025 21:17:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spDeletePlaylist] @PlaylistId INT 
AS 
BEGIN 
   SET NOCOUNT OFF; 
   DELETE FROM [dbo].[NamedPlaylistVideo] 
   WHERE PlaylistId = @PlaylistId; 
   
   DELETE FROM [dbo].[NamedPlaylist] 
   WHERE Id = @PlaylistId; 
END;


GO
