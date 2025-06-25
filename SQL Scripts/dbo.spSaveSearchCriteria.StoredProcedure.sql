USE [VideoPlayerDebug]
GO
/****** Object:  StoredProcedure [dbo].[spSaveSearchCriteria]    Script Date: 24/06/2025 21:17:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spSaveSearchCriteria]
    @PlaylistId INT = NULL,
    @Criteria IdList READONLY
AS
BEGIN
    SET NOCOUNT OFF;

    -- Delete existing search criteria
    DELETE FROM [dbo].[SearchCriteriaItem];
    DELETE FROM [dbo].[SearchCriteria];

    -- Insert new search criteria
    DECLARE @SearchCriteriaId INT;

    INSERT INTO [dbo].[SearchCriteria] (SearchDate, PlaylistId)
    VALUES (GETDATE(), @PlaylistId);
    SET @SearchCriteriaId = SCOPE_IDENTITY();

    INSERT INTO [dbo].[SearchCriteriaItem] (SearchCriteriaId, TypeId, Id)
    SELECT @SearchCriteriaId, TypeId, Id
    FROM @Criteria;

    PRINT 'Saved SearchCriteriaId: ' + CAST(@SearchCriteriaId AS NVARCHAR(10));
END;
GO
