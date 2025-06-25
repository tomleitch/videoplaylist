USE [VideoPlayerDebug]
GO
/****** Object:  StoredProcedure [dbo].[spGetPlayerState]    Script Date: 24/06/2025 21:17:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spGetPlayerState]
    @PlaylistId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT PlaylistId, VideoId, OrderIndex, Position, IsPlayed, SequenceType, LastUpdated
    FROM [dbo].[PlaylistPlayerState]
    WHERE PlaylistId = @PlaylistId
    ORDER BY OrderIndex;
    DECLARE @LogMsg NVARCHAR(1000) = 'Retrieved PlaylistPlayerState for PlaylistId: ' + CAST(@PlaylistId AS NVARCHAR(10));
    EXEC [dbo].[spInsertDBLog] @Msg = @LogMsg, @Status = 'INFO';
END;
GO
