USE [VideoPlayerDebug]
GO
/****** Object:  StoredProcedure [dbo].[spUpdateVideoLastPlayed]    Script Date: 24/06/2025 21:17:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spUpdateVideoLastPlayed]
    @VideoId INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE [dbo].[Video]
    SET LastPlayed = GETDATE()
    WHERE Id = @VideoId;
    DECLARE @LogMsg NVARCHAR(1000) = 'Updated LastPlayed for VideoId: ' + CAST(@VideoId AS NVARCHAR(10));
    EXEC [dbo].[spInsertDBLog] @Msg = @LogMsg, @Status = 'INFO';
END;
GO
