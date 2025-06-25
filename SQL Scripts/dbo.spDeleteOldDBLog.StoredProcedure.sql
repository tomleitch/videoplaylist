USE [VideoPlayerDebug]
GO
/****** Object:  StoredProcedure [dbo].[spDeleteOldDBLog]    Script Date: 24/06/2025 21:17:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spDeleteOldDBLog]
                           AS
                           BEGIN
                               DECLARE @id int;
                               SELECT @id = MAX(DBLogId) FROM DBLog;
                               DELETE FROM DBLog WHERE DBLogId < (@id-300);
                           END
GO
