USE [VideoPlayerDebug]
GO
/****** Object:  StoredProcedure [dbo].[spSavePlayerState]    Script Date: 24/06/2025 21:17:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spSavePlayerState]
    @PlaylistId INT,
    @VideoId INT,
    @OrderIndex INT,
    @Position DECIMAL(10, 2) = 0.00,
    @IsPlayed BIT = 0,
    @SequenceType VARCHAR(10) = 'Ordered'
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM [dbo].[PlaylistPlayerState] WHERE PlaylistId = @PlaylistId AND VideoId = @VideoId)
    BEGIN
        UPDATE [dbo].[PlaylistPlayerState]
        SET OrderIndex = @OrderIndex, 
            Position = @Position, 
            IsPlayed = @IsPlayed, 
            SequenceType = @SequenceType, 
            LastUpdated = GETDATE()
        WHERE PlaylistId = @PlaylistId AND VideoId = @VideoId;
    END
    ELSE
    BEGIN
        INSERT INTO [dbo].[PlaylistPlayerState] (PlaylistId, VideoId, OrderIndex, Position, IsPlayed, SequenceType, LastUpdated)
        VALUES (@PlaylistId, @VideoId, @OrderIndex, @Position, @IsPlayed, @SequenceType, GETDATE());
    END
END;
GO
