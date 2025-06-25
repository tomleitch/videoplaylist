USE [VideoPlayerDebug]
GO
/****** Object:  StoredProcedure [dbo].[spDeletePlayerState]    Script Date: 24/06/2025 21:17:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spDeletePlayerState]
    @PlaylistId INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM [dbo].[PlaylistPlayerState]
    WHERE PlaylistId = @PlaylistId;
    DECLARE @LogMsg NVARCHAR(1000) = 'Deleted PlaylistPlayerState for PlaylistId: ' + CAST(@PlaylistId AS NVARCHAR(10));
    EXEC [dbo].[spInsertDBLog] @Msg = @LogMsg, @Status = 'INFO';
END;
GO
