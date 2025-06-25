USE [VideoPlayerDebug]
GO
/****** Object:  StoredProcedure [dbo].[spAddLogError]    Script Date: 24/06/2025 21:17:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spAddLogError](@Message varchar(1000), @InsertedID int output)
                           AS
                           BEGIN
                               INSERT INTO DBLogError(Message, Status, TS) VALUES(@Message, 'ERROR', GETDATE());
                               SET @InsertedID = SCOPE_IDENTITY();
                           END
GO
