USE [VideoPlayerDebug]
GO
/****** Object:  StoredProcedure [dbo].[spInsertPlaylist]    Script Date: 24/06/2025 21:17:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spInsertPlaylist] @PlaylistId INT OUTPUT, @Name NVARCHAR(100) AS BEGIN SET NOCOUNT OFF; IF EXISTS (SELECT 1 FROM [dbo].[NamedPlaylist] WHERE Name = @Name) BEGIN SELECT @PlaylistId = Id FROM [dbo].[NamedPlaylist] WHERE Name = @Name; RETURN @PlaylistId; END

INSERT INTO [dbo].[NamedPlaylist] (Name, LastPlayedVideoId, LastPlayedPosition, SortPreference, FilterPreference)
VALUES (@Name, 0, 0, 'Name', 'None');
SET @PlaylistId = SCOPE_IDENTITY();
RETURN @PlaylistId;

END; 


GO
