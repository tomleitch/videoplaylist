USE [VideoPlayerDebug]
GO
/****** Object:  StoredProcedure [dbo].[spCountFilteredVideos]    Script Date: 24/06/2025 21:17:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spCountFilteredVideos]
    @Criteria IdList READONLY,
    @PlaylistId INT = NULL
AS
BEGIN
    SET NOCOUNT OFF;

    -- Log Criteria table contents for debugging
    DECLARE @CriteriaLog NVARCHAR(MAX) = 'Criteria received: ';
    SELECT @CriteriaLog += 'TypeId: ' + CAST(TypeId AS NVARCHAR(10)) + ', Id: ' + CAST(Id AS NVARCHAR(10)) + '; '
    FROM @Criteria;
    PRINT @CriteriaLog;

    -- Count criteria by TypeId
    DECLARE @GenreCount INT = (SELECT COUNT(*) FROM @Criteria WHERE TypeId = 1);
    DECLARE @TagCount INT = (SELECT COUNT(*) FROM @Criteria WHERE TypeId IN (2, 3));
    DECLARE @ActorCount INT = (SELECT COUNT(*) FROM @Criteria WHERE TypeId = 4);

    -- Count matching videos
    WITH VideoMatches AS (
        SELECT 
            vc.VideoId,
            COUNT(DISTINCT CASE WHEN c.TypeId = 1 AND vc.GenreId = c.Id THEN vc.GenreId END) AS GenreMatchCount,
            COUNT(DISTINCT CASE WHEN c.TypeId IN (2, 3) AND vc.TagId = c.Id THEN vc.TagId END) AS TagMatchCount,
            COUNT(DISTINCT CASE WHEN c.TypeId = 4 AND vc.ActorId = c.Id THEN vc.ActorId END) AS ActorMatchCount
        FROM vwVideoCriteria vc
        LEFT JOIN @Criteria c ON 
            (c.TypeId = 1 AND vc.GenreId = c.Id) OR
            (c.TypeId IN (2, 3) AND vc.TagId = c.Id) OR
            (c.TypeId = 4 AND vc.ActorId = c.Id)
        WHERE NOT EXISTS (
            SELECT 1 
            FROM NamedPlaylistVideo npv 
            WHERE npv.PlaylistId = @PlaylistId 
            AND npv.VideoId = vc.VideoId
        )
        GROUP BY vc.VideoId
    )
    SELECT COUNT(*) AS MatchCount
    FROM VideoMatches
    WHERE GenreMatchCount = @GenreCount
      AND TagMatchCount = @TagCount
      AND ActorMatchCount = @ActorCount;

    PRINT 'Counted unique videos for PlaylistId: ' + ISNULL(CAST(@PlaylistId AS NVARCHAR(10)), 'NULL');
END;
GO
