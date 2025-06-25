USE [VideoPlayerDebug]
GO
/****** Object:  StoredProcedure [dbo].[spFilterVideos]    Script Date: 24/06/2025 21:17:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spFilterVideos]
    @Criteria IdList READONLY,
    @PlaylistId INT = NULL
AS
BEGIN
    SET NOCOUNT OFF;

    DECLARE @GenreIds TABLE (Id INT);
    DECLARE @TagIds1 TABLE (Id INT);
    DECLARE @TagIds2 TABLE (Id INT);
    DECLARE @ActorIds TABLE (Id INT);

    -- Extract criteria by TypeId
    INSERT INTO @GenreIds (Id) SELECT Id FROM @Criteria WHERE TypeId = 1;
    INSERT INTO @TagIds1 (Id) SELECT Id FROM @Criteria WHERE TypeId = 2;
    INSERT INTO @TagIds2 (Id) SELECT Id FROM @Criteria WHERE TypeId = 3;
    INSERT INTO @ActorIds (Id) SELECT Id FROM @Criteria WHERE TypeId = 4;

    -- Filter videos, excluding those in the specified playlist
    SELECT DISTINCT v.Id, v.Title, v.FilePath, v.LastPlayed
    FROM Video v
    LEFT JOIN VideoVideoTag vvt ON v.Id = vvt.VideoId
    LEFT JOIN VideoActor va ON v.Id = va.VideoId
    LEFT JOIN NamedPlaylistVideo npv ON v.Id = npv.VideoId AND npv.PlaylistId = @PlaylistId
    WHERE (EXISTS (SELECT 1 FROM @GenreIds g WHERE g.Id = v.GenreId) OR NOT EXISTS (SELECT 1 FROM @GenreIds))
    AND (EXISTS (SELECT 1 FROM @TagIds1 t1 WHERE t1.Id = vvt.VideoTagId) OR NOT EXISTS (SELECT 1 FROM @TagIds1))
    AND (EXISTS (SELECT 1 FROM @TagIds2 t2 WHERE t2.Id = vvt.VideoTagId) OR NOT EXISTS (SELECT 1 FROM @TagIds2))
    AND (EXISTS (SELECT 1 FROM @ActorIds a WHERE a.Id = va.ActorId) OR NOT EXISTS (SELECT 1 FROM @ActorIds))
    AND (@PlaylistId IS NULL OR npv.VideoId IS NULL)
    ORDER BY v.LastPlayed DESC;

    PRINT 'Filtered videos based on provided criteria, excluding PlaylistId: ' + ISNULL(CAST(@PlaylistId AS NVARCHAR(10)), 'NULL');
END;
GO
