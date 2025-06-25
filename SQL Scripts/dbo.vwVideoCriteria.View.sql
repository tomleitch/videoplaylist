USE [VideoPlayerDebug]
GO
/****** Object:  View [dbo].[vwVideoCriteria]    Script Date: 24/06/2025 21:17:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
    CREATE   VIEW [dbo].[vwVideoCriteria]
    AS
    SELECT 
        v.Id AS VideoId,
        v.GenreId,
        vvt.VideoTagId AS TagId,
        va.ActorId
    FROM Video v
    LEFT JOIN VideoVideoTag vvt ON vvt.VideoId = v.Id
    LEFT JOIN VideoActor va ON va.VideoId = v.Id
GO
