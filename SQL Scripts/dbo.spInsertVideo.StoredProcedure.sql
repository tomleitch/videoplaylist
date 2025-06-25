USE [VideoPlayerDebug]
GO
/****** Object:  StoredProcedure [dbo].[spInsertVideo]    Script Date: 24/06/2025 21:17:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[spInsertVideo] (
    @VideoId INT OUTPUT,
    @Title NVARCHAR(200),
    @GenreId INT,
    @Rating DECIMAL(3,1),
    @IsFavorite BIT,
    @DurationMinutes INT,
    @ReleaseYear INT,
    @FilePath NVARCHAR(500),
    @FileDate DATETIME
) AS
BEGIN
    SET NOCOUNT OFF;

    -- Validate FileDate
    IF @FileDate <= '1900-01-01'
    BEGIN
        RAISERROR ('FileDate must be after January 1, 1900.', 16, 1);
        RETURN;
    END

    PRINT 'Checking FilePath...';
    SELECT @VideoId = Id FROM Video WHERE FilePath = @FilePath;
    PRINT 'FilePath check result: ' + ISNULL(CAST(@VideoId AS NVARCHAR(10)), 'NULL');
    
    IF @VideoId IS NOT NULL
    BEGIN
        PRINT 'Updating existing video...';
        UPDATE Video
        SET Title = @Title,
            GenreId = @GenreId,
            Rating = @Rating,
            IsFavorite = @IsFavorite,
            DurationMinutes = @DurationMinutes,
            ReleaseYear = @ReleaseYear,
            FilePath = @FilePath,
            FileDate = @FileDate
        WHERE Id = @VideoId;
        PRINT 'Updated VideoId: ' + CAST(@VideoId AS NVARCHAR(10));
        RETURN @VideoId;
    END
    
    PRINT 'Inserting new video...';
    INSERT INTO Video (Title, GenreId, Rating, IsFavorite, DurationMinutes, ReleaseYear, FilePath, FileDate)
    VALUES (@Title, @GenreId, @Rating, @IsFavorite, @DurationMinutes, @ReleaseYear, @FilePath, @FileDate);
    SET @VideoId = SCOPE_IDENTITY();
    PRINT 'Inserted VideoId: ' + CAST(@VideoId AS NVARCHAR(10));
    RETURN @VideoId;
END;
GO
