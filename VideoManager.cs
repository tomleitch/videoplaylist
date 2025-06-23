using System;
using System.Collections.Generic;
using System.Data;
using CSGeneral_x86;

namespace VideoPlaylist
{
    /// <summary>
    /// Manages video-related operations, including insertion and deletion of videos and video tags.
    /// </summary>
    public class VideoManager
    {
        private clsADOSupport _adoSupport;

        /// <summary>
        /// Gets or sets the ADOSupport instance for database operations.
        /// </summary>
        public clsADOSupport ADOSupport
        {
            get
            {
                if (_adoSupport == null)
                {
                    _adoSupport = clsADOSupport.LocalDB;
                }
                return _adoSupport;
            }
            set
            {
                _adoSupport = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the VideoManager class.
        /// </summary>
        public VideoManager()
        {
        }

        /// <summary>
        /// Inserts a video record and associated tags into the database using spInsertVideo.
        /// </summary>
        /// <param name="title">The title of the video.</param>
        /// <param name="genreId">The genre ID of the video.</param>
        /// <param name="rating">The rating of the video.</param>
        /// <param name="isFavorite">Indicates if the video is a favorite.</param>
        /// <param name="durationMinutes">The duration of the video in minutes.</param>
        /// <param name="releaseYear">The release year of the video.</param>
        /// <param name="filePath">The file path of the video.</param>
        /// <param name="videoTagIds">The list of video tag IDs to associate with the video.</param>
        /// <param name="actors">The list of actor IDs to associate with the video.</param>
        /// <param name="fileDate">The file creation date of the video.</param>
        /// <returns>The ID of the inserted video, or 0 if the operation fails.</returns>
        public int InsertVideo(string title, int genreId, decimal rating, bool isFavorite,
            int durationMinutes, int releaseYear, string filePath, List<int> videoTagIds,
            List<int> actors, DateTime fileDate)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(filePath))
            {
                DBLogger.LogError("InsertVideo failed: Title or FilePath cannot be empty.");
                return 0;
            }

            try
            {
                ESqlCommand cmd = ADOSupport.GetESQLCommand();
                cmd.CommandText = "spInsertVideo";
                cmd.AddOutputParameter("@VideoId", 0);
                cmd.AddParameterWithValue("@Title", title);
                cmd.AddParameterWithValue("@GenreId", genreId);
                cmd.AddParameterWithValue("@Rating", rating);
                cmd.AddParameterWithValue("@IsFavorite", isFavorite);
                cmd.AddParameterWithValue("@DurationMinutes", durationMinutes);
                cmd.AddParameterWithValue("@ReleaseYear", releaseYear);
                cmd.AddParameterWithAnyValue("@FilePath", filePath);
                cmd.AddParameterWithValue("@FileDate", fileDate);
                DBLogger.LogError($"Executing spInsertVideo with Title={title}, FilePath={filePath}, FileDate={fileDate}");
                cmd.ExecuteNonQuery();
                cmd.GetOutputParameter("@VideoId", out int videoId);
                DBLogger.LogError($"spInsertVideo returned VideoId: {videoId}");

                if (videoId <= 0)
                {
                    DBLogger.LogError("InsertVideo failed: Invalid or zero VideoId returned.");
                    return 0;
                }

                // Delete existing tags
                ESqlCommand deleteCmd = ADOSupport.GetESQLCommandQuery();
                deleteCmd.CommandText = "DELETE FROM VideoVideoTag WHERE VideoId = @VideoId";
                deleteCmd.AddParameterWithValue("@VideoId", videoId);
                deleteCmd.ExecuteNonQuery();

                // Insert new tags
                if (videoTagIds != null && videoTagIds.Count > 0)
                {
                    foreach (int tagId in videoTagIds)
                    {
                        ESqlCommand tagCmd = ADOSupport.GetESQLCommandQuery();
                        tagCmd.CommandText = "INSERT INTO VideoVideoTag (VideoId, VideoTagId) VALUES (@VideoId, @VideoTagId)";
                        tagCmd.AddParameterWithValue("@VideoId", videoId);
                        tagCmd.AddParameterWithValue("@VideoTagId", tagId);
                        tagCmd.ExecuteNonQuery();
                    }
                }

                // Delete existing actor tags
                ESqlCommand deleteActorsCmd = ADOSupport.GetESQLCommandQuery();
                deleteActorsCmd.CommandText = "DELETE FROM VideoActor WHERE VideoId = @VideoId";
                deleteActorsCmd.AddParameterWithValue("@VideoId", videoId);
                deleteActorsCmd.ExecuteNonQuery();

                // Insert new actors
                if (actors != null && actors.Count > 0)
                {
                    foreach (int actor in actors)
                    {
                        ESqlCommand tagCmd = ADOSupport.GetESQLCommandQuery();
                        tagCmd.CommandText = "INSERT INTO VideoActor (VideoId, ActorId) VALUES (@VideoId, @ActorId)";
                        tagCmd.AddParameterWithValue("@VideoId", videoId);
                        tagCmd.AddParameterWithValue("@ActorId", actor);
                        tagCmd.ExecuteNonQuery();
                    }
                }

                return videoId;
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"InsertVideo failed: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Inserts a test video record with predefined values and tags.
        /// </summary>
        /// <returns>The ID of the inserted video, or 0 if the operation fails.</returns>
        public int InsertTestVideo()
        {
            try
            {
                List<int> actors = null;
                var tagIds = new List<int> { 2, 4, 6 };
                DateTime fileDate = DateTime.Now; // Default test file date
                int videoId = InsertVideo(
                    title: "Test Video",
                    genreId: 1,
                    rating: 0,
                    isFavorite: false,
                    durationMinutes: 0,
                    releaseYear: 0,
                    filePath: @"C:\Videos\TestVideo.mp4",
                    videoTagIds: tagIds,
                    actors: actors,
                    fileDate: fileDate
                );
                DBLogger.LogError($"Test video insertion result: VideoId={videoId}");
                return videoId;
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"InsertTestVideo failed: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Deletes a video record from the database by its ID.
        /// </summary>
        /// <param name="videoId">The ID of the video to delete.</param>
        /// <returns>True if the video was deleted successfully; otherwise, false.</returns>
        public bool DeleteVideo(int videoId)
        {
            try
            {
                ESqlCommand checkCmd = ADOSupport.GetESQLCommandQuery();
                checkCmd.CommandText = "SELECT COUNT(*) FROM Video WHERE Id = @VideoId";
                checkCmd.AddParameterWithValue("@VideoId", videoId);
                DataTable result = ADOSupport.GetDataTable(checkCmd);
                int count = Convert.ToInt32(result.Rows[0][0]);
                if (count == 0)
                {
                    DBLogger.LogError($"DeleteVideo failed: Video with Id {videoId} does not exist.");
                    return false;
                }

                ESqlCommand deleteCmd = ADOSupport.GetESQLCommand();
                deleteCmd.CommandText = "DELETE FROM Video WHERE Id = @VideoId";
                deleteCmd.AddParameterWithValue("@VideoId", videoId);
                deleteCmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"DeleteVideo failed for Id {videoId}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Deletes a video tag record from the database by its ID.
        /// </summary>
        /// <param name="videoTagId">The ID of the video tag to delete.</param>
        /// <returns>True if the video tag was deleted successfully; otherwise, false.</returns>
        public bool DeleteVideoTag(int videoTagId)
        {
            try
            {
                ESqlCommand checkCmd = ADOSupport.GetESQLCommandQuery();
                checkCmd.CommandText = "SELECT COUNT(*) FROM VideoTag WHERE Id = @VideoTagId";
                checkCmd.AddParameterWithValue("@VideoTagId", videoTagId);
                DataTable result = ADOSupport.GetDataTable(checkCmd);
                int count = Convert.ToInt32(result.Rows[0][0]);
                if (count == 0)
                {
                    DBLogger.LogError($"DeleteVideoTag failed: VideoTag with Id {videoTagId} does not exist.");
                    return false;
                }

                ESqlCommand deleteCmd = ADOSupport.GetESQLCommand();
                deleteCmd.CommandText = "DELETE FROM VideoTag WHERE Id = @VideoTagId";
                deleteCmd.AddParameterWithValue("@VideoTagId", videoTagId);
                deleteCmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"DeleteVideoTag failed for Id {videoTagId}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Checks if a video record exists in the database based on the full pathname.
        /// </summary>
        /// <param name="filePath">The full pathname of the video.</param>
        /// <returns>True if the video exists; otherwise, false.</returns>
        public bool VideoExists(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                DBLogger.LogError("VideoExists failed: FilePath cannot be empty.");
                return false;
            }

            try
            {
                ESqlCommand cmd = ADOSupport.GetESQLCommandQuery();
                cmd.CommandText = "SELECT COUNT(*) FROM Video WHERE FilePath = @FilePath";
                cmd.AddParameterWithAnyValue("@FilePath", filePath);
                DataTable result = ADOSupport.GetDataTable(cmd);
                return Convert.ToInt32(result.Rows[0][0]) > 0;
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"VideoExists failed for FilePath {filePath}: {ex.Message}");
                return false;
            }
        }
    }
}