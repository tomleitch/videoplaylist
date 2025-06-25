USE [VideoPlayerDebug]
GO
/****** Object:  StoredProcedure [dbo].[spAppendOrReplaceVideos]    Script Date: 24/06/2025 21:17:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spAppendOrReplaceVideos]
    @Criteria IdList READONLY,
    @PlaylistId INT,
    @Replace BIT = 0
AS
BEGIN
    SET NOCOUNT OFF;

    -- Log Criteria table contents for debugging
    DECLARE @CriteriaLog NVARCHAR(MAX) = 'Criteria received: ';
    SELECT @CriteriaLog += 'TypeId: ' + CAST(TypeId AS NVARCHAR(10)) + ', Id: ' + CAST(Id AS NVARCHAR(10)) + '; '
    FROM @Criteria;
    PRINT @CriteriaLog;

    -- Validate PlaylistId
    IF @PlaylistId <= 0
    BEGIN
        RAISERROR ('Invalid PlaylistId.', 16, 1);
        RETURN;
    END

    -- Count criteria by TypeId
    DECLARE @GenreCount INT = (SELECT COUNT(*) FROM @Criteria WHERE TypeId = 1);
    DECLARE @TagCount INT = (SELECT COUNT(*) FROM @Criteria WHERE TypeId IN (2, 3));
    DECLARE @ActorCount INT = (SELECT COUNT(*) FROM @Criteria WHERE TypeId = 4);

    -- Begin transaction
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Delete existing videos if replacing
        IF @Replace = 1
        BEGIN
            DELETE FROM NamedPlaylistVideo WHERE PlaylistId = @PlaylistId;
            PRINT 'Deleted existing videos for PlaylistId: ' + CAST(@PlaylistId AS NVARCHAR(10));
        END

        -- Insert matching videos
        INSERT INTO NamedPlaylistVideo (PlaylistId, VideoId, OrderIndex)
        SELECT DISTINCT @PlaylistId, vc.VideoId, 0
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
        HAVING 
            COUNT(DISTINCT CASE WHEN c.TypeId = 1 AND vc.GenreId = c.Id THEN vc.GenreId END) = @GenreCount
            AND COUNT(DISTINCT CASE WHEN c.TypeId IN (2, 3) AND vc.TagId = c.Id THEN vc.TagId END) = @TagCount
            AND COUNT(DISTINCT CASE WHEN c.TypeId = 4 AND vc.ActorId = c.Id THEN vc.ActorId END) = @ActorCount;

        -- Save search criteria
        EXEC spSaveSearchCriteria @PlaylistId, @Criteria;

        PRINT 'Inserted videos for PlaylistId: ' + CAST(@PlaylistId AS NVARCHAR(10));
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        PRINT 'Error: ' + @ErrorMessage;
        RAISERROR (@ErrorMessage, 16, 1);
    END CATCH
END;
GO
