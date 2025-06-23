using System;
using System.Collections.Generic;
using System.Data;
using CSGeneral_x86;

namespace VideoPlaylist
{
    /// <summary>
    /// Manages playlist-related operations, including creation, deletion, and video additions.
    /// </summary>
    internal class PlaylistManager
    {
        private readonly clsADOSupport _adoSupport;

        /// <summary>
        /// Initializes a new instance of the PlaylistManager class.
        /// </summary>
        /// <param name="adoSupport">The ADOSupport instance for database operations.</param>
        /// <exception cref="ArgumentNullException">Thrown when adoSupport is null.</exception>
        public PlaylistManager(clsADOSupport adoSupport)
        {
            _adoSupport = adoSupport ?? throw new ArgumentNullException(nameof(adoSupport));
        }

        /// <summary>
        /// Clears and recreates the 'New Additions' playlist.
        /// </summary>
        public void ClearNewAdditionsPlaylist()
        {
            try
            {
                string sql = @"IF EXISTS (SELECT 1 FROM [dbo].[NamedPlaylist] WHERE Name = 'New Additions')
                              BEGIN
                                  DECLARE @PlaylistId INT;
                                  SELECT @PlaylistId = Id FROM [dbo].[NamedPlaylist] WHERE Name = 'New Additions';
                                  DELETE FROM [dbo].[NamedPlaylistVideo] WHERE PlaylistId = @PlaylistId;
                                  DELETE FROM [dbo].[NamedPlaylist] WHERE Id = @PlaylistId;
                              END
                              INSERT INTO [dbo].[NamedPlaylist] (Name) VALUES ('New Additions')";
                _adoSupport.ExecuteSQL(sql);
                DBLogger.Log("Cleared and recreated New Additions playlist");
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"Error clearing New Additions playlist: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Adds a video to the 'New Additions' playlist.
        /// </summary>
        /// <param name="videoId">The ID of the video to add.</param>
        public void AddToNewAdditionsPlaylist(long videoId)
        {
            try
            {
                string sql = $@"DECLARE @PlaylistId INT;
                               SELECT @PlaylistId = Id FROM [dbo].[NamedPlaylist] WHERE Name = 'New Additions';

                               IF NOT EXISTS (SELECT 1 FROM [dbo].[NamedPlaylistVideo] WHERE PlaylistId = @PlaylistId AND VideoId = {videoId})
                                   INSERT INTO [dbo].[NamedPlaylistVideo] (PlaylistId, VideoId, OrderIndex)
                                   VALUES (@PlaylistId, {videoId}, 0)";
                _adoSupport.ExecuteSQL(sql);
                DBLogger.Log($"Added video {videoId} to New Additions playlist");
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"Error adding video {videoId} to New Additions playlist: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Creates a new playlist.
        /// </summary>
        /// <param name="name">The name of the new playlist.</param>
        /// <returns>The ID of the created playlist, or 0 if the operation fails.</returns>
        public int CreatePlaylist(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                DBLogger.LogError("CreatePlaylist failed: Name cannot be empty.");
                return 0;
            }

            try
            {
                ESqlCommand cmd = _adoSupport.GetESQLCommand();
                cmd.CommandText = "spInsertPlaylist";
                cmd.AddOutputParameter("@PlaylistId", 0);
                cmd.AddParameterWithValue("@Name", name);
                cmd.ExecuteNonQuery();
                cmd.GetOutputParameter("@PlaylistId", out int playlistId);

                if (playlistId == 0)
                {
                    DBLogger.LogError("CreatePlaylist failed: No PlaylistId returned.");
                }
                else
                {
                    DBLogger.Log($"Created playlist {name} with PlaylistId: {playlistId}");
                }

                return playlistId;
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"CreatePlaylist failed: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Deletes a playlist and its associated videos.
        /// </summary>
        /// <param name="playlistId">The ID of the playlist to delete.</param>
        public void DeletePlaylist(int playlistId)
        {
            if (playlistId <= 0)
            {
                DBLogger.LogError("DeletePlaylist failed: Invalid PlaylistId.");
                return;
            }

            try
            {
                ESqlCommand cmd = _adoSupport.GetESQLCommand();
                cmd.CommandText = "spDeletePlaylist";
                cmd.AddParameterWithValue("@PlaylistId", playlistId);
                cmd.ExecuteNonQuery();
                DBLogger.Log($"Deleted playlist with PlaylistId: {playlistId}");
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"DeletePlaylist failed for PlaylistId {playlistId}: {ex.Message}");
            }
        }

        /// <summary>
        /// Clears all videos from a playlist.
        /// </summary>
        /// <param name="playlistId">The ID of the playlist to clear.</param>
        public void ClearPlaylistVideos(int playlistId)
        {
            if (playlistId <= 0)
            {
                DBLogger.LogError("ClearPlaylistVideos failed: Invalid PlaylistId.");
                return;
            }

            try
            {
                ESqlCommand cmd = _adoSupport.GetESQLCommandQuery();
                cmd.CommandText = "DELETE FROM [dbo].[NamedPlaylistVideo] WHERE PlaylistId = @PlaylistId";
                cmd.AddParameterWithValue("@PlaylistId", playlistId);
                int rowsAffected = cmd.ExecuteNonQuery();
                DBLogger.Log($"Cleared {rowsAffected} videos from PlaylistId: {playlistId}");
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"ClearPlaylistVideos failed for PlaylistId {playlistId}: {ex.Message}");
                throw; // Rethrow to surface error in UI
            }
        }

        /// <summary>
        /// Retrieves a list of all playlists for populating a ComboBox.
        /// </summary>
        /// <returns>A list of ComboItem objects representing playlists.</returns>
        public List<ComboItem> GetPlaylists()
        {
            try
            {
                ESqlCommand cmd = _adoSupport.GetESQLCommandQuery();
                cmd.CommandText = "SELECT Id, Name FROM [dbo].[NamedPlaylist] ORDER BY Name";
                DataTable result = _adoSupport.GetDataTable(cmd);
                List<ComboItem> playlists = new List<ComboItem>();

                foreach (DataRow row in result.Rows)
                {
                    playlists.Add(new ComboItem(Convert.ToInt32(row["Id"]), row["Name"].ToString()));
                }

                DBLogger.Log($"Retrieved {playlists.Count} playlists");
                return playlists;
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"GetPlaylists failed: {ex.Message}");
                return new List<ComboItem>();
            }
        }

        /// <summary>
        /// Retrieves the number of videos in a playlist.
        /// </summary>
        /// <param name="playlistId">The ID of the playlist.</param>
        /// <returns>The number of videos in the playlist.</returns>
        public int GetVideoCount(int playlistId)
        {
            if (playlistId <= 0)
            {
                return 0;
            }

            try
            {
                ESqlCommand cmd = _adoSupport.GetESQLCommandQuery();
                cmd.CommandText = "SELECT COUNT(*) FROM [dbo].[NamedPlaylistVideo] WHERE PlaylistId = @PlaylistId";
                cmd.AddParameterWithValue("@PlaylistId", playlistId);
                DataTable result = _adoSupport.GetDataTable(cmd);
                int count = Convert.ToInt32(result.Rows[0][0]);
                DBLogger.Log($"Video count for PlaylistId {playlistId}: {count}");
                return count;
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"GetVideoCount failed for PlaylistId {playlistId}: {ex.Message}");
                return 0;
            }
        }
    }
}