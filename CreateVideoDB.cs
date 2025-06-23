using System;
using System.Collections.Generic;
using CSGeneral_x86;

namespace VideoPlaylist
{
    internal class CreateVideoDB
    {
        private readonly clsADOSupport _adoSupport;

        public CreateVideoDB(clsADOSupport adoSupport)
        {
            if (adoSupport == null) throw new ArgumentNullException(nameof(adoSupport));
            if (string.IsNullOrWhiteSpace(adoSupport.ConnectionString)) throw new ArgumentException("ConnectionString cannot be empty.");
            _adoSupport = adoSupport;
        }

        public bool SchemaExists()
        {
            try
            {
                string sqlTableCount = "SELECT COUNT(*) FROM sys.tables WHERE name IN ('DBProperties', 'DBLog', 'DBLogError', 'Genre', 'Video', 'Actor', 'VideoActor', 'VideoTag', 'VideoVideoTag', 'ActorTag', 'ActorActorTag', 'AnonymousPlaylist', 'NamedPlaylist', 'NamedPlaylistVideo', 'UnwantedWord')";
                int tableCount;
                _adoSupport.GetScalerValue(out tableCount, sqlTableCount);
                if (tableCount != 15)
                {
                    DBLogger.LogError($"Schema check failed: Only {tableCount} of 15 tables exist.");
                    return false;
                }

                string sqlColumns = @"SELECT COUNT(*) FROM sys.columns 
                                   WHERE object_id = OBJECT_ID('[dbo].[Video]') 
                                   AND name IN ('Id', 'Title', 'GenreId', 'Rating', 'IsFavorite', 'DurationMinutes', 'ReleaseYear', 'FilePath', 'CreatedDate')";
                int columnCount;
                _adoSupport.GetScalerValue(out columnCount, sqlColumns);
                if (columnCount != 9)
                {
                    DBLogger.LogError($"Video table check failed: Only {columnCount} of 9 columns exist.");
                    return false;
                }

                string sqlConstraint = @"SELECT COUNT(*) FROM sys.indexes 
                                       WHERE object_id = OBJECT_ID('[dbo].[Video]') 
                                       AND name = 'UK_Video_FilePath'";
                int constraintCount;
                _adoSupport.GetScalerValue(out constraintCount, sqlConstraint);
                if (constraintCount != 1)
                {
                    DBLogger.LogError("Video table check failed: UNIQUE constraint on FilePath missing.");
                    return false;
                }

                string sqlProc = "SELECT COUNT(*) FROM sys.procedures WHERE name = 'spInsertVideo'";
                int procCount;
                _adoSupport.GetScalerValue(out procCount, sqlProc);
                if (procCount != 1)
                {
                    DBLogger.LogError("Schema check failed: Stored procedure spInsertVideo missing.");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                DBLogger.LogError("Error checking schema: " + ex.Message);
                return false;
            }
        }

        public void UpdateSchema()
        {
            try
            {
                var expectedSchema = new Dictionary<string, List<(string ColumnName, string Definition)>>()
                {
                    {"DBProperties", new List<(string, string)>
                        {("PropertyName", "NVARCHAR(256) NOT NULL"), ("PropertyValue", "NVARCHAR(MAX) NULL"), ("Memo", "NVARCHAR(MAX) NULL")}},
                    {"DBLog", new List<(string, string)>
                        {("DBLogId", "INT IDENTITY(1,1) NOT NULL"), ("Message", "NVARCHAR(MAX) NULL"), ("Status", "NVARCHAR(256) NOT NULL"), ("TS", "DATETIME NOT NULL DEFAULT GETDATE()")}},
                    {"DBLogError", new List<(string, string)>
                        {("DBLogErrorId", "INT IDENTITY(1,1) NOT NULL"), ("Message", "NVARCHAR(MAX) NULL"), ("Status", "NVARCHAR(256) NOT NULL"), ("TS", "DATETIME NOT NULL DEFAULT GETDATE()")}},
                    {"Genre", new List<(string, string)>
                        {("Id", "INT IDENTITY(1,1) NOT NULL"), ("Name", "NVARCHAR(50) NOT NULL UNIQUE")}},
                    {"Video", new List<(string, string)>
                        {("Id", "INT IDENTITY(1,1) NOT NULL"), ("Title", "NVARCHAR(200) NOT NULL"), ("GenreId", "INT NOT NULL DEFAULT 1"),
                         ("Rating", "DECIMAL(3,1) NOT NULL DEFAULT 0 CHECK (Rating >= 0 AND Rating <= 10)"), ("IsFavorite", "BIT NOT NULL DEFAULT 1"),
                         ("DurationMinutes", "INT NOT NULL DEFAULT 0"), ("ReleaseYear", "INT NOT NULL DEFAULT 0"),
                         ("FilePath", "NVARCHAR(500) NOT NULL CONSTRAINT UK_Video_FilePath UNIQUE"),
                         ("CreatedDate", "DATETIME NOT NULL DEFAULT GETDATE()")}},
                    {"Actor", new List<(string, string)>
                        {("Id", "INT IDENTITY(1,1) NOT NULL"), ("Name", "NVARCHAR(100) NOT NULL UNIQUE")}},
                    {"VideoActor", new List<(string, string)>
                        {("VideoId", "INT NOT NULL"), ("ActorId", "INT NOT NULL")}},
                    {"VideoTag", new List<(string, string)>
                        {("Id", "INT IDENTITY(1,1) NOT NULL"), ("Name", "NVARCHAR(50) NOT NULL UNIQUE"),
                         ("GroupId", "INT NOT NULL DEFAULT 0"), ("Default", "BIT NOT NULL DEFAULT 1")}},
                    {"VideoVideoTag", new List<(string, string)>
                        {("VideoId", "INT NOT NULL"), ("VideoTagId", "INT NOT NULL")}},
                    {"ActorTag", new List<(string, string)>
                        {("Id", "INT IDENTITY(1,1) NOT NULL"), ("Name", "NVARCHAR(50) NOT NULL UNIQUE")}},
                    {"ActorActorTag", new List<(string, string)>
                        {("ActorId", "INT NOT NULL"), ("ActorTagId", "INT NOT NULL")}},
                    {"AnonymousPlaylist", new List<(string, string)>
                        {("Id", "INT NOT NULL DEFAULT 1"), ("VideoIds", "NVARCHAR(MAX) NULL"), ("LastPlayedVideoId", "INT NOT NULL DEFAULT 0"),
                         ("LastPlayedPosition", "INT NOT NULL DEFAULT 0"), ("SortPreference", "NVARCHAR(20) NOT NULL"), ("FilterPreference", "NVARCHAR(MAX) NULL")}},
                    {"NamedPlaylist", new List<(string, string)>
                        {("Id", "INT IDENTITY(1,1) NOT NULL"), ("Name", "NVARCHAR(100) NOT NULL"), ("LastPlayedVideoId", "INT NOT NULL DEFAULT 0"),
                         ("LastPlayedPosition", "INT NOT NULL DEFAULT 0"), ("SortPreference", "NVARCHAR(20) NOT NULL"), ("FilterPreference", "NVARCHAR(MAX) NULL")}},
                    {"NamedPlaylistVideo", new List<(string, string)>
                        {("PlaylistId", "INT NOT NULL"), ("VideoId", "INT NOT NULL"), ("OrderIndex", "INT NOT NULL DEFAULT 0")}},
                    {"UnwantedWord", new List<(string, string)>
                        {("MatchingWord", "VARCHAR(200) NOT NULL CONSTRAINT UK_UnwantedWord_MatchingWord UNIQUE")}}
                };

                foreach (var table in expectedSchema)
                {
                    string tableName = table.Key;
                    foreach (var column in table.Value)
                    {
                        try
                        {
                            string sqlCheckColumn = $@"SELECT COUNT(*) FROM sys.columns 
                                                     WHERE object_id = OBJECT_ID('dbo.{tableName}') 
                                                     AND name = '{column.ColumnName}'";
                            int columnExists;
                            _adoSupport.GetScalerValue(out columnExists, sqlCheckColumn);
                            if (columnExists == 0)
                            {
                                string sqlAlter = $@"ALTER TABLE [dbo].[{tableName}] ADD [{column.ColumnName}] {column.Definition}";
                                _adoSupport.ExecuteSQL(sqlAlter);
                                DBLogger.LogError($"Successfully added {column.ColumnName} to {tableName}.");
                            }
                        }
                        catch (Exception ex)
                        {
                            DBLogger.LogError($"Error adding {column.ColumnName} to {tableName}: {ex.Message}");
                        }
                    }
                }

                try
                {
                    string sqlCheckConstraint = @"SELECT COUNT(*) FROM sys.indexes 
                                                 WHERE object_id = OBJECT_ID('[dbo].[Video]') 
                                                 AND name = 'UK_Video_FilePath'";
                    int constraintExists;
                    _adoSupport.GetScalerValue(out constraintExists, sqlCheckConstraint);
                    if (constraintExists == 0)
                    {
                        string sqlAddConstraint = @"ALTER TABLE [dbo].[Video] 
                                                   ADD CONSTRAINT [UK_Video_FilePath] UNIQUE ([FilePath])";
                        _adoSupport.ExecuteSQL(sqlAddConstraint);
                        DBLogger.LogError("Successfully added UNIQUE constraint to Video.FilePath.");
                    }
                }
                catch (Exception ex)
                {
                    DBLogger.LogError($"Error adding UNIQUE constraint to Video.FilePath: {ex.Message}");
                }

                CreateSpInsertVideo();
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"Error updating schema: {ex.Message}");
                throw;
            }
        }

        public void CreateSchema()
        {
            try
            {
                CreateDBPropertiesTable();
                CreateDBLogTable();
                CreateDBLogErrorTable();
                CreateSpAddLogError();
                CreateSpAllLogError();
                CreateSpDeleteOldDBLog();
                CreateSpInsertDBLog();
                CreateGenreTable();
                CreateVideoTable();
                CreateActorTable();
                CreateVideoActorTable();
                CreateVideoTagTable();
                CreateVideoVideoTagTable();
                CreateActorTagTable();
                CreateActorActorTagTable();
                CreateAnonymousPlaylistTable();
                CreateNamedPlaylistTable();
                CreateNamedPlaylistVideoTable();
                CreateUnwantedWordTable();
                CreateSpInsertVideo();
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"Error creating schema: {ex.Message}");
                throw;
            }
        }

        private void CreateDBPropertiesTable()
        {
            string sql = @"IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'DBProperties')
                           CREATE TABLE [dbo].[DBProperties](
                               [PropertyName] [nvarchar](256) NOT NULL,
                               [PropertyValue] [nvarchar](MAX) NULL,
                               [Memo] [nvarchar](MAX) NULL,
                               CONSTRAINT [PK_DBProperties] PRIMARY KEY CLUSTERED 
                               ([PropertyName] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF,
                               ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                           ) ON [PRIMARY]";
            _adoSupport.ExecuteSQL(sql);
        }

        private void CreateDBLogTable()
        {
            string sql = @"IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'DBLog')
                           CREATE TABLE [dbo].[DBLog](
                               [DBLogId] [int] IDENTITY(1,1) NOT NULL,
                               [Message] [nvarchar](MAX) NULL,
                               [Status] [nvarchar](256) NOT NULL,
                               [TS] [datetime] NOT NULL DEFAULT GETDATE(),
                               CONSTRAINT [PK_DBLog] PRIMARY KEY CLUSTERED 
                               ([DBLogId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF,
                               ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                           ) ON [PRIMARY]";
            _adoSupport.ExecuteSQL(sql);
        }

        private void CreateDBLogErrorTable()
        {
            string sql = @"IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'DBLogError')
                           CREATE TABLE [dbo].[DBLogError](
                               [DBLogErrorId] [int] IDENTITY(1,1) NOT NULL,
                               [Message] [nvarchar](MAX) NULL,
                               [Status] [nvarchar](256) NOT NULL,
                               [TS] [datetime] NOT NULL DEFAULT GETDATE(),
                               CONSTRAINT [PK_DBLogError] PRIMARY KEY CLUSTERED 
                               ([DBLogErrorId] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF,
                               ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                           ) ON [PRIMARY]";
            _adoSupport.ExecuteSQL(sql);
        }

        private void CreateSpAddLogError()
        {
            string sql = @"IF NOT EXISTS (SELECT 1 FROM sys.procedures WHERE name = 'spAddLogError')
                           EXEC('CREATE PROC [dbo].[spAddLogError](@Message varchar(1000), @InsertedID int output)
                           AS
                           BEGIN
                               INSERT INTO DBLogError(Message, Status, TS) VALUES(@Message, ''ERROR'', GETDATE());
                               SET @InsertedID = SCOPE_IDENTITY();
                           END')";
            _adoSupport.ExecuteSQL(sql);
        }

        private void CreateSpAllLogError()
        {
            string sql = @"IF NOT EXISTS (SELECT 1 FROM sys.procedures WHERE name = 'spAllLogError')
                           EXEC('CREATE PROC [dbo].[spAllLogError](@Message varchar(1000), @InsertedID int output)
                           AS
                           BEGIN
                               INSERT INTO DBLog(Message, Status, TS) VALUES(@Message, ''ERROR'', GETDATE());
                               SET @InsertedID = SCOPE_IDENTITY();
                           END')";
            _adoSupport.ExecuteSQL(sql);
        }

        private void CreateSpDeleteOldDBLog()
        {
            string sql = @"IF NOT EXISTS (SELECT 1 FROM sys.procedures WHERE name = 'spDeleteOldDBLog')
                           EXEC('CREATE PROC [dbo].[spDeleteOldDBLog]
                           AS
                           BEGIN
                               DECLARE @id int;
                               SELECT @id = MAX(DBLogId) FROM DBLog;
                               DELETE FROM DBLog WHERE DBLogId < (@id-300);
                           END')";
            _adoSupport.ExecuteSQL(sql);
        }

        private void CreateSpInsertDBLog()
        {
            string sql = @"IF NOT EXISTS (SELECT 1 FROM sys.procedures WHERE name = 'spInsertDBLog')
                           EXEC('CREATE PROC [dbo].[spInsertDBLog](
                               @Msg varchar(1000),
                               @Status varchar(200)
                           )
                           AS
                           BEGIN
                               INSERT INTO DBLog(Message, Status, TS)
                               VALUES (@Msg, @Status, GETDATE());
                               IF @Status = ''ERROR''
                               BEGIN
                                   INSERT INTO DBLogError(Message, Status, TS)
                                   VALUES (@Msg, @Status, GETDATE());
                               END
                           END')";
            _adoSupport.ExecuteSQL(sql);
        }

        private void CreateSpInsertVideo()
        {
            string sql = @"IF NOT EXISTS (SELECT 1 FROM sys.procedures WHERE name = 'spInsertVideo')
                           EXEC('CREATE PROC [dbo].[spInsertVideo] (
                               @VideoId INT OUTPUT,
                               @Title NVARCHAR(200),
                               @GenreId INT,
                               @Rating DECIMAL(3,1),
                               @IsFavorite BIT,
                               @DurationMinutes INT,
                               @ReleaseYear INT,
                               @FilePath NVARCHAR(500)
                           ) AS
                           BEGIN
                               SET NOCOUNT ON;
                               SELECT @VideoId = Id FROM Video WHERE FilePath = @FilePath;
                               IF @VideoId IS NOT NULL
                                   RETURN @VideoId;
                               INSERT INTO Video (Title, GenreId, Rating, IsFavorite, DurationMinutes, ReleaseYear, FilePath)
                               VALUES (@Title, @GenreId, @Rating, @IsFavorite, @DurationMinutes, @ReleaseYear, @FilePath);
                               SET @VideoId = SCOPE_IDENTITY();
                               RETURN @VideoId;
                           END')";
            _adoSupport.ExecuteSQL(sql);
        }

        private void CreateGenreTable()
        {
            string sql = @"IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Genre')
                           CREATE TABLE [dbo].[Genre](
                               Id INT IDENTITY(1,1) NOT NULL,
                               Name NVARCHAR(50) NOT NULL UNIQUE,
                               CONSTRAINT PK_Genre PRIMARY KEY CLUSTERED (Id ASC)
                           ) ON [PRIMARY]";
            _adoSupport.ExecuteSQL(sql);
        }

        private void CreateVideoTable()
        {
            string sql = @"IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Video')
                           CREATE TABLE [dbo].[Video](
                               Id INT IDENTITY(1,1) NOT NULL,
                               Title NVARCHAR(200) NOT NULL,
                               GenreId INT NOT NULL DEFAULT 1,
                               Rating DECIMAL(3,1) NOT NULL DEFAULT 0 CHECK (Rating >= 0 AND Rating <= 10),
                               IsFavorite BIT NOT NULL DEFAULT 1,
                               DurationMinutes INT NOT NULL DEFAULT 0,
                               ReleaseYear INT NOT NULL DEFAULT 0,
                               FilePath NVARCHAR(500) NOT NULL,
                               CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
                               CONSTRAINT PK_Video PRIMARY KEY CLUSTERED (Id ASC),
                               CONSTRAINT UK_Video_FilePath UNIQUE (FilePath)
                           ) ON [PRIMARY]";
            _adoSupport.ExecuteSQL(sql);
        }

        private void CreateActorTable()
        {
            string sql = @"IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Actor')
                           CREATE TABLE [dbo].[Actor](
                               Id INT IDENTITY(1,1) NOT NULL,
                               Name NVARCHAR(100) NOT NULL UNIQUE,
                               CONSTRAINT PK_Actor PRIMARY KEY CLUSTERED (Id ASC)
                           ) ON [PRIMARY]";
            _adoSupport.ExecuteSQL(sql);
        }

        private void CreateVideoActorTable()
        {
            string sql = @"IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'VideoActor')
                           CREATE TABLE [dbo].[VideoActor](
                               VideoId INT NOT NULL,
                               ActorId INT NOT NULL,
                               CONSTRAINT PK_VideoActor PRIMARY KEY CLUSTERED (VideoId, ActorId)
                           ) ON [PRIMARY]";
            _adoSupport.ExecuteSQL(sql);
        }

        private void CreateVideoTagTable()
        {
            string sql = @"IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'VideoTag')
                           CREATE TABLE [dbo].[VideoTag](
                               Id INT IDENTITY(1,1) NOT NULL,
                               Name NVARCHAR(50) NOT NULL UNIQUE,
                               GroupId INT NOT NULL DEFAULT 0,
                               [Default] BIT NOT NULL DEFAULT 1,
                               CONSTRAINT PK_VideoTag PRIMARY KEY CLUSTERED (Id ASC)
                           ) ON [PRIMARY]";
            _adoSupport.ExecuteSQL(sql);
        }

        private void CreateVideoVideoTagTable()
        {
            string sql = @"IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'VideoVideoTag')
                           CREATE TABLE [dbo].[VideoVideoTag](
                               VideoId INT NOT NULL,
                               VideoTagId INT NOT NULL,
                               CONSTRAINT PK_VideoVideoTag PRIMARY KEY CLUSTERED (VideoId, VideoTagId)
                           ) ON [PRIMARY]";
            _adoSupport.ExecuteSQL(sql);
        }

        private void CreateActorTagTable()
        {
            string sql = @"IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ActorTag')
                           CREATE TABLE [dbo].[ActorTag](
                               Id INT IDENTITY(1,1) NOT NULL,
                               Name NVARCHAR(50) NOT NULL UNIQUE,
                               CONSTRAINT PK_ActorTag PRIMARY KEY CLUSTERED (Id ASC)
                           ) ON [PRIMARY]";
            _adoSupport.ExecuteSQL(sql);
        }

        private void CreateActorActorTagTable()
        {
            string sql = @"IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ActorActorTag')
                           CREATE TABLE [dbo].[ActorActorTag](
                               ActorId INT NOT NULL,
                               ActorTagId INT NOT NULL,
                               CONSTRAINT PK_ActorActorTag PRIMARY KEY CLUSTERED (ActorId, ActorTagId)
                           ) ON [PRIMARY]";
            _adoSupport.ExecuteSQL(sql);
        }

        private void CreateAnonymousPlaylistTable()
        {
            string sql = @"IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'AnonymousPlaylist')
                           CREATE TABLE [dbo].[AnonymousPlaylist](
                               Id INT NOT NULL DEFAULT 1,
                               VideoIds NVARCHAR(MAX) NULL,
                               LastPlayedVideoId INT NOT NULL DEFAULT 0,
                               LastPlayedPosition INT NOT NULL DEFAULT 0,
                               SortPreference NVARCHAR(20) NOT NULL,
                               FilterPreference NVARCHAR(MAX) NULL,
                               CONSTRAINT PK_AnonymousPlaylist PRIMARY KEY CLUSTERED (Id ASC)
                           ) ON [PRIMARY]";
            _adoSupport.ExecuteSQL(sql);
        }

        private void CreateNamedPlaylistTable()
        {
            string sql = @"IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'NamedPlaylist')
                           CREATE TABLE [dbo].[NamedPlaylist](
                               Id INT IDENTITY(1,1) NOT NULL,
                               Name NVARCHAR(100) NOT NULL,
                               LastPlayedVideoId INT NOT NULL DEFAULT 0,
                               LastPlayedPosition INT NOT NULL DEFAULT 0,
                               SortPreference NVARCHAR(20) NOT NULL,
                               FilterPreference NVARCHAR(MAX) NULL,
                               CONSTRAINT PK_NamedPlaylist PRIMARY KEY CLUSTERED (Id ASC)
                           ) ON [PRIMARY]";
            _adoSupport.ExecuteSQL(sql);
        }

        private void CreateNamedPlaylistVideoTable()
        {
            string sql = @"IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'NamedPlaylistVideo')
                           CREATE TABLE [dbo].[NamedPlaylistVideo](
                               PlaylistId INT NOT NULL,
                               VideoId INT NOT NULL,
                               OrderIndex INT NOT NULL DEFAULT 0,
                               CONSTRAINT PK_NamedPlaylistVideo PRIMARY KEY CLUSTERED (PlaylistId, VideoId)
                           ) ON [PRIMARY]";
            _adoSupport.ExecuteSQL(sql);
        }

        private void CreateUnwantedWordTable()
        {
            string sql = @"IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'UnwantedWord')
                           CREATE TABLE [dbo].[UnwantedWord](
                               MatchingWord VARCHAR(200) NOT NULL,
                               CONSTRAINT UK_UnwantedWord_MatchingWord UNIQUE (MatchingWord)
                           ) ON [PRIMARY]";
            _adoSupport.ExecuteSQL(sql);
        }
    }
}