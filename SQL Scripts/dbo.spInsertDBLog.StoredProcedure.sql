USE [VideoPlayerDebug]
GO
/****** Object:  StoredProcedure [dbo].[spInsertDBLog]    Script Date: 24/06/2025 21:17:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spInsertDBLog](
                               @Msg varchar(1000),
                               @Status varchar(200)
                           )
                           AS
                           BEGIN
                               INSERT INTO DBLog(Message, Status, TS)
                               VALUES (@Msg, @Status, GETDATE());
                               IF @Status = 'ERROR'
                               BEGIN
                                   INSERT INTO DBLogError(Message, Status, TS)
                                   VALUES (@Msg, @Status, GETDATE());
                               END
                           END
GO
