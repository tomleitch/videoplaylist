using System;

namespace VideoPlaylist
{
    /// <summary>
    /// Centralized event definitions for playlist-related operations.
    /// </summary>
    public static class VideoEvents
    {
        /// <summary>
        /// Occurs when a playlist is selected.
        /// </summary>
        public static event EventHandler<int> PlaylistSelected;

        /// <summary>
        /// Occurs when a new playlist is created.
        /// </summary>
        public static event EventHandler<int> NewPlaylistCreated;

        /// <summary>
        /// Occurs when a playlist is deleted.
        /// </summary>
        public static event EventHandler<int> PlaylistDeleted;

        /// <summary>
        /// Occurs when the playlist selection is cleared.
        /// </summary>
        public static event EventHandler<int> PlaylistCleared;

        /// <summary>
        /// Occurs when one or more videos are added to a playlist.
        /// </summary>
        public static event EventHandler<int> PlaylistRowAdded;

        /// <summary>
        /// Occurs when one or more videos are deleted from a playlist.
        /// </summary>
        public static event EventHandler<int> PlaylistRowDeleted;

        /// <summary>
        /// Occurs when videos are appended to a playlist.
        /// </summary>
        public static event EventHandler<int> VideosAppended;

        /// <summary>
        /// Occurs when videos replace existing ones in a playlist.
        /// </summary>
        public static event EventHandler<int> VideosReplaced;

        /// <summary>
        /// Occurs when a playback error occurs (e.g., file not found).
        /// </summary>
        public static event EventHandler<string> VideoPlaybackError;

        /// <summary>
        /// Raises the PlaylistSelected event.
        /// </summary>
        /// <param name="playlistId">The ID of the selected playlist.</param>
        public static void OnPlaylistSelected(int playlistId)
        {
            PlaylistSelected?.Invoke(null, playlistId);
        }

        /// <summary>
        /// Raises the NewPlaylistCreated event.
        /// </summary>
        /// <param name="playlistId">The ID of the created playlist.</param>
        public static void OnNewPlaylistCreated(int playlistId)
        {
            NewPlaylistCreated?.Invoke(null, playlistId);
        }

        /// <summary>
        /// Raises the PlaylistDeleted event.
        /// </summary>
        /// <param name="playlistId">The ID of the deleted playlist.</param>
        public static void OnPlaylistDeleted(int playlistId)
        {
            PlaylistDeleted?.Invoke(null, playlistId);
        }

        /// <summary>
        /// Raises the PlaylistCleared event.
        /// </summary>
        /// <param name="playlistId">The ID of the cleared playlist (0 for no selection).</param>
        public static void OnPlaylistCleared(int playlistId)
        {
            PlaylistCleared?.Invoke(null, playlistId);
        }

        /// <summary>
        /// Raises the PlaylistRowAdded event.
        /// </summary>
        /// <param name="playlistId">The ID of the playlist with added videos.</param>
        public static void OnPlaylistRowAdded(int playlistId)
        {
            PlaylistRowAdded?.Invoke(null, playlistId);
        }

        /// <summary>
        /// Raises the PlaylistRowDeleted event.
        /// </summary>
        /// <param name="playlistId">The ID of the playlist with deleted videos.</param>
        public static void OnPlaylistRowDeleted(int playlistId)
        {
            PlaylistRowDeleted?.Invoke(null, playlistId);
        }

        /// <summary>
        /// Raises the VideosAppended event.
        /// </summary>
        /// <param name="playlistId">The ID of the playlist with appended videos.</param>
        public static void OnVideosAppended(int playlistId)
        {
            VideosAppended?.Invoke(null, playlistId);
        }

        /// <summary>
        /// Raises the VideosReplaced event.
        /// </summary>
        /// <param name="playlistId">The ID of the playlist with replaced videos.</param>
        public static void OnVideosReplaced(int playlistId)
        {
            VideosReplaced?.Invoke(null, playlistId);
        }

        /// <summary>
        /// Raises the VideoPlaybackError event.
        /// </summary>
        /// <param name="errorMessage">The error message describing the playback issue.</param>
        public static void OnVideoPlaybackError(string errorMessage)
        {
            VideoPlaybackError?.Invoke(null, errorMessage);
        }
    }
}