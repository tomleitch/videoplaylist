using System;
using System.Collections.Generic;
using CSGeneral_x86;

namespace VideoPlaylist
{
    internal class TaskSQL
    {
        private readonly clsADOSupport _adoSupport;

        public TaskSQL(clsADOSupport adoSupport)
        {
            _adoSupport = adoSupport ?? throw new ArgumentNullException(nameof(adoSupport));
        }

        public long AddVideo(string title, int genreId, string filePath, DateTime createdDate, List<int> tagIds)
        {
            try
            {
                string sanitizedTitle = title.Replace("'", "''");
                string sanitizedFilePath = filePath.Replace("'", "''");
                string sql = $@"INSERT INTO [dbo].[Videos] (Title, GenreId, FilePath, CreatedDate, LastModifiedDate)
                              OUTPUT INSERTED.Id
                              VALUES ('{sanitizedTitle}', {genreId}, '{sanitizedFilePath}', '{createdDate:yyyy-MM-dd HH:mm:ss}', '{createdDate:yyyy-MM-dd HH:mm:ss}')";
                long videoId;
                _adoSupport.GetScalerValue(out videoId, sql);

                foreach (int tagId in tagIds)
                {
                    sql = $@"INSERT INTO [dbo].[VideoVideoTags] (VideoId, VideoTagId) VALUES ({videoId}, {tagId})";
                    _adoSupport.ExecuteSQL(sql);
                }

                return videoId;
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"Error adding video {title}: {ex.Message}");
                throw;
            }
        }
    }
}