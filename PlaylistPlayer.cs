using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AxWMPLib;
using CSGeneral_x86;

namespace VideoPlaylist
{
    /// <summary>
    /// Manages video playback for a playlist using axWindowsMediaPlayer.
    /// </summary>
    public class PlaylistPlayer : IDisposable
    {
        private readonly clsADOSupport _adoSupport;
        private AxWindowsMediaPlayer _player;
        private PlaylistSelector _playlistSelector;
        private List<VideoItem> _currentPlaylist;
        private int _currentIndex;
        private bool _isPlaying;
        private bool _disposed;
        private const string SEQUENCE_ORDERED = "Ordered";
        private const string SEQUENCE_RANDOM = "Random";

        private class VideoItem
        {
            public int VideoId { get; set; }
            public string FilePath { get; set; }
            public int OrderIndex { get; set; }
            public decimal Position { get; set; }
            public bool IsPlayed { get; set; }
            public string SequenceType { get; set; } // Added to fix CS1061
        }

        /// <summary>
        /// Gets or sets the axWindowsMediaPlayer control.
        /// </summary>
        public AxWindowsMediaPlayer Player
        {
            get => _player;
            set => _player = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Initializes a new instance of the PlaylistPlayer class.
        /// </summary>
        public PlaylistPlayer(clsADOSupport adoSupport)
        {
            _adoSupport = adoSupport ?? throw new ArgumentNullException(nameof(adoSupport));
            _currentPlaylist = new List<VideoItem>();
            _currentIndex = -1;
            _isPlaying = false;
        }

        /// <summary>
        /// Initializes event handlers and subscriptions.
        /// </summary>
        public void Initialize()
        {
            if (_adoSupport == null)
            {
                DBLogger.LogError("Initialize failed: ADOSupport is not set.");
                return;
            }

            try
            {
                VideoEvents.PlaylistSelected += HandlePlaylistSelected;
                VideoEvents.PlaylistRowAdded += HandlePlaylistRowChanged;
                VideoEvents.PlaylistRowDeleted += HandlePlaylistRowChanged;
                VideoEvents.PlaylistCleared += HandlePlaylistCleared;
                VideoEvents.PlaylistDeleted += HandlePlaylistDeleted;
                if (_player != null)
                {
                    _player.PlayStateChange += HandlePlayStateChange;
                }
                DBLogger.Log("PlaylistPlayer initialized and subscribed to events.");
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"Initialize failed: {ex.Message}");
                MessageBox.Show($"Error initializing player: {ex.Message}", "Error");
            }
        }

        /// <summary>
        /// Sets the PlaylistSelector instance.
        /// </summary>
        public void SetPlaylistSelector(PlaylistSelector selector)
        {
            _playlistSelector = selector;
            DBLogger.Log("PlaylistSelector set for PlaylistPlayer.");
        }

        /// <summary>
        /// Handles keypresses for playback control.
        /// </summary>
        public void KeyPressed(Keys key)
        {
            try
            {
                switch (key)
                {
                    case Keys.Enter: Start(); break;
                    case Keys.Space: Pause(); break;
                    case Keys.H: ShowHelp(); break;
                    case Keys.N: Next(); break;
                    case Keys.P: Previous(); break;
                    case Keys.Up: SkipForward10Min(); break;
                    case Keys.Down: SkipForward1Min(); break;
                    case Keys.PageUp: SkipBackward10Min(); break;
                    case Keys.PageDown: SkipBackward1Min(); break;
                    case Keys.R: Reorder(SEQUENCE_RANDOM); break;
                    case Keys.O: Reorder(SEQUENCE_ORDERED); break;
                    case Keys.D: DeleteCurrentFile(); break;
                    case Keys.X: RemoveCurrentFromPlaylist(); break;
                    default: DBLogger.Log($"Unhandled key: {key}"); break;
                }
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"KeyPressed failed: {ex.Message}");
                VideoEvents.OnVideoPlaybackError($"Keypress error: {ex.Message}");
            }
        }

        /// <summary>
        /// Starts or resumes playback, prompting user choice.
        /// </summary>
        public void Start()
        {
            if (_player == null || _currentIndex < 0) return;
            try
            {
                if (_currentPlaylist[_currentIndex].Position > 0)
                {
                    var result = MessageBox.Show("Resume from last position?", "Resume Playback", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        _player.Ctlcontrols.currentPosition = (double)_currentPlaylist[_currentIndex].Position;
                    }
                    else
                    {
                        _currentPlaylist[_currentIndex].Position = 0;
                        SavePlayerState();
                    }
                }
                _player.URL = _currentPlaylist[_currentIndex].FilePath;
                _player.Ctlcontrols.play();
                _player.settings.mute = true;
                _player.stretchToFit = true;
                _player.uiMode = "full";
                _isPlaying = true;
                UpdateVideoLastPlayed();
                DBLogger.Log($"Started playback for VideoId: {_currentPlaylist[_currentIndex].VideoId}");
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"Start failed: {ex.Message}");
                MessageBox.Show($"Error starting playback: {ex.Message}", "Error");
                VideoEvents.OnVideoPlaybackError($"Start error: {ex.Message}");
            }
        }

        /// <summary>
        /// Toggles pause/unpause of the current video.
        /// </summary>
        public void Pause()
        {
            if (_player == null || !_isPlaying) return;
            try
            {
                _player.Ctlcontrols.pause();
                _isPlaying = false;
                _currentPlaylist[_currentIndex].Position = (decimal)_player.Ctlcontrols.currentPosition;
                SavePlayerState();
                DBLogger.Log($"Paused playback for VideoId: {_currentPlaylist[_currentIndex].VideoId}");
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"Pause failed: {ex.Message}");
                VideoEvents.OnVideoPlaybackError($"Pause error: {ex.Message}");
            }
        }

        /// <summary>
        /// Stops playback and saves position.
        /// </summary>
        public void Stop()
        {
            if (_player == null || !_isPlaying) return;
            try
            {
                _player.Ctlcontrols.stop();
                _isPlaying = false;
                _currentPlaylist[_currentIndex].Position = (decimal)_player.Ctlcontrols.currentPosition;
                SavePlayerState();
                DBLogger.Log($"Stopped playback for VideoId: {_currentPlaylist[_currentIndex].VideoId}");
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"Stop failed: {ex.Message}");
                VideoEvents.OnVideoPlaybackError($"Stop error: {ex.Message}");
            }
        }

        /// <summary>
        /// Skips to the next video in the playlist.
        /// </summary>
        public void Next()
        {
            if (_currentIndex >= _currentPlaylist.Count - 1) return;
            try
            {
                _currentPlaylist[_currentIndex].IsPlayed = true;
                _currentPlaylist[_currentIndex].Position = 0;
                SavePlayerState();
                _currentIndex++;
                Start();
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"Next failed: {ex.Message}");
                VideoEvents.OnVideoPlaybackError($"Next error: {ex.Message}");
            }
        }

        /// <summary>
        /// Skips to the previous video in the playlist.
        /// </summary>
        public void Previous()
        {
            if (_currentIndex <= 0) return;
            try
            {
                _currentPlaylist[_currentIndex].Position = 0;
                SavePlayerState();
                _currentIndex--;
                Start();
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"Previous failed: {ex.Message}");
                VideoEvents.OnVideoPlaybackError($"Previous error: {ex.Message}");
            }
        }

        /// <summary>
        /// Skips forward 10 minutes in the current video.
        /// </summary>
        public void SkipForward10Min()
        {
            Skip(600); // 10 minutes in seconds
        }

        /// <summary>
        /// Skips forward 1 minute in the current video.
        /// </summary>
        public void SkipForward1Min()
        {
            Skip(60); // 1 minute in seconds
        }

        /// <summary>
        /// Skips backward 10 minutes in the current video.
        /// </summary>
        public void SkipBackward10Min()
        {
            Skip(-600); // 10 minutes in seconds
        }

        /// <summary>
        /// Skips backward 1 minute in the current video.
        /// </summary>
        public void SkipBackward1Min()
        {
            Skip(-60); // 1 minute in seconds
        }

        private void Skip(double seconds)
        {
            if (_player == null || !_isPlaying) return;
            try
            {
                double newPosition = _player.Ctlcontrols.currentPosition + seconds;
                if (newPosition < 0) newPosition = 0;
                _player.Ctlcontrols.currentPosition = newPosition;
                _currentPlaylist[_currentIndex].Position = (decimal)newPosition;
                SavePlayerState();
                DBLogger.Log($"Skipped {seconds} seconds for VideoId: {_currentPlaylist[_currentIndex].VideoId}");
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"Skip failed: {ex.Message}");
                VideoEvents.OnVideoPlaybackError($"Skip error: {ex.Message}");
            }
        }

        /// <summary>
        /// Reorders the playlist (ordered or random).
        /// </summary>
        public void Reorder(string sequenceType)
        {
            try
            {
                if (sequenceType != SEQUENCE_ORDERED && sequenceType != SEQUENCE_RANDOM) return;
                if (sequenceType == SEQUENCE_RANDOM)
                {
                    var random = new Random();
                    _currentPlaylist = _currentPlaylist.OrderBy(x => random.Next()).ToList();
                    for (int i = 0; i < _currentPlaylist.Count; i++)
                    {
                        _currentPlaylist[i].OrderIndex = i;
                        _currentPlaylist[i].SequenceType = SEQUENCE_RANDOM;
                    }
                }
                else
                {
                    LoadPlaylist(GetCurrentPlaylistId(), SEQUENCE_ORDERED);
                }
                SavePlayerState();
                DBLogger.Log($"Reordered playlist to {sequenceType}");
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"Reorder failed: {ex.Message}");
                VideoEvents.OnVideoPlaybackError($"Reorder error: {ex.Message}");
            }
        }

        /// <summary>
        /// Removes the current video from the playlist.
        /// </summary>
        public void RemoveCurrentFromPlaylist()
        {
            if (_currentIndex < 0) return;
            try
            {
                int playlistId = GetCurrentPlaylistId();
                int videoId = _currentPlaylist[_currentIndex].VideoId;
                ESqlCommand cmd = _adoSupport.GetESQLCommand();
                cmd.CommandText = "DELETE FROM [dbo].[NamedPlaylistVideo] WHERE PlaylistId = @PlaylistId AND VideoId = @VideoId";
                cmd.AddParameterWithValue("@PlaylistId", playlistId);
                cmd.AddParameterWithValue("@VideoId", videoId);
                cmd.ExecuteNonQuery();
                _currentPlaylist.RemoveAt(_currentIndex);
                if (_currentPlaylist.Count == 0)
                {
                    Stop();
                    _currentIndex = -1;
                }
                else if (_currentIndex >= _currentPlaylist.Count)
                {
                    _currentIndex--;
                }
                SavePlayerState();
                VideoEvents.OnPlaylistRowDeleted(playlistId);
                DBLogger.Log($"Removed VideoId: {videoId} from PlaylistId: {playlistId}");
                if (_isPlaying) Start();
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"RemoveCurrentFromPlaylist failed: {ex.Message}");
                VideoEvents.OnVideoPlaybackError($"Remove error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes the current video file and removes it from the database.
        /// </summary>
        public void DeleteCurrentFile()
        {
            if (_currentIndex < 0) return;
            try
            {
                Stop();
                int videoId = _currentPlaylist[_currentIndex].VideoId;
                string filePath = _currentPlaylist[_currentIndex].FilePath;
                if (!File.Exists(filePath))
                {
                    DBLogger.LogError($"DeleteCurrentFile failed: File not found: {filePath}");
                    MessageBox.Show($"File not found: {filePath}", "Error");
                    return;
                }
                File.Delete(filePath);
                ESqlCommand cmd = _adoSupport.GetESQLCommand();
                cmd.CommandText = "DELETE FROM [dbo].[Video] WHERE Id = @VideoId";
                cmd.AddParameterWithValue("@VideoId", videoId);
                cmd.ExecuteNonQuery();
                _currentPlaylist.RemoveAt(_currentIndex);
                if (_currentPlaylist.Count == 0)
                {
                    _currentIndex = -1;
                }
                else if (_currentIndex >= _currentPlaylist.Count)
                {
                    _currentIndex--;
                }
                SavePlayerState();
                VideoEvents.OnPlaylistRowDeleted(GetCurrentPlaylistId());
                DBLogger.Log($"Deleted file and VideoId: {videoId}");
                if (_isPlaying) Start();
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"DeleteCurrentFile failed: {ex.Message}");
                MessageBox.Show($"Error deleting file: {ex.Message}", "Error");
                VideoEvents.OnVideoPlaybackError($"Delete error: {ex.Message}");
            }
        }

        /// <summary>
        /// Shows the help form with command list.
        /// </summary>
        public void ShowHelp()
        {
            try
            {
                var helpForm = new PlaylistPlayerHelpForm();
                helpForm.CommandSelected += (s, command) =>
                {
                    switch (command)
                    {
                        case "Start": Start(); break;
                        case "Pause": Pause(); break;
                        case "Stop": Stop(); break;
                        case "Next": Next(); break;
                        case "Previous": Previous(); break;
                        case "SkipForward10Min": SkipForward10Min(); break;
                        case "SkipForward1Min": SkipForward1Min(); break;
                        case "SkipBackward10Min": SkipBackward10Min(); break;
                        case "SkipBackward1Min": SkipBackward1Min(); break;
                        case "ReorderRandom": Reorder(SEQUENCE_RANDOM); break;
                        case "ReorderOrdered": Reorder(SEQUENCE_ORDERED); break;
                        case "RemoveCurrent": RemoveCurrentFromPlaylist(); break;
                        case "DeleteCurrent": DeleteCurrentFile(); break;
                    }
                };
                helpForm.Show();
                DBLogger.Log("ShowHelp opened PlaylistPlayerHelpForm.");
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"ShowHelp failed: {ex.Message}");
                VideoEvents.OnVideoPlaybackError($"Help error: {ex.Message}");
            }
        }

        private void HandlePlaylistSelected(object sender, int playlistId)
        {
            if (!_isPlaying)
            {
                LoadPlaylist(playlistId, SEQUENCE_ORDERED);
                _currentIndex = 0;
                DBLogger.Log($"PlaylistSelected handled for PlaylistId: {playlistId}");
            }
        }

        private void HandlePlaylistRowChanged(object sender, int playlistId)
        {
            if (!_isPlaying && playlistId == GetCurrentPlaylistId())
            {
                LoadPlaylist(playlistId, _currentPlaylist.Any() ? _currentPlaylist[0].SequenceType : SEQUENCE_ORDERED);
                DBLogger.Log($"PlaylistRowChanged handled for PlaylistId: {playlistId}");
            }
        }

        private void HandlePlaylistCleared(object sender, int playlistId)
        {
            if (playlistId == GetCurrentPlaylistId())
            {
                Stop();
                _currentPlaylist.Clear();
                _currentIndex = -1;
                SavePlayerState();
                DBLogger.Log($"PlaylistCleared handled for PlaylistId: {playlistId}");
            }
        }

        private void HandlePlaylistDeleted(object sender, int playlistId)
        {
            if (playlistId == GetCurrentPlaylistId())
            {
                Stop();
                _currentPlaylist.Clear();
                _currentIndex = -1;
                ESqlCommand cmd = _adoSupport.GetESQLCommand();
                cmd.CommandText = "spDeletePlayerState";
                cmd.AddParameterWithValue("@PlaylistId", playlistId);
                cmd.ExecuteNonQuery();
                DBLogger.Log($"PlaylistDeleted handled for PlaylistId: {playlistId}");
            }
        }

        private void HandlePlayStateChange(object sender, _WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (e.newState == 8 && _isPlaying) // MediaEnded
            {
                _currentPlaylist[_currentIndex].IsPlayed = true;
                SavePlayerState();
                if (_currentIndex < _currentPlaylist.Count - 1)
                {
                    Next();
                }
                else
                {
                    Stop();
                    _isPlaying = false;
                    DBLogger.Log("Playlist playback completed.");
                }
            }
        }

        private void LoadPlaylist(int playlistId, string sequenceType)
        {
            try
            {
                ESqlCommand cmd = _adoSupport.GetESQLCommandQuery();
                cmd.CommandText = "SELECT npv.VideoId, v.FilePath, npv.OrderIndex FROM [dbo].[NamedPlaylistVideo] npv INNER JOIN [dbo].[Video] v ON npv.VideoId = v.Id WHERE npv.PlaylistId = @PlaylistId ORDER BY npv.OrderIndex";
                cmd.AddParameterWithValue("@PlaylistId", playlistId);
                DataTable result = _adoSupport.GetDataTable(cmd);

                _currentPlaylist.Clear();
                foreach (DataRow row in result.Rows)
                {
                    _currentPlaylist.Add(new VideoItem
                    {
                        VideoId = Convert.ToInt32(row["VideoId"]),
                        FilePath = row["FilePath"].ToString(),
                        OrderIndex = Convert.ToInt32(row["OrderIndex"]),
                        Position = 0,
                        IsPlayed = false,
                        SequenceType = sequenceType
                    });
                }

                cmd.CommandText = "spGetPlayerState";
                cmd.Parameters.Clear();
                cmd.AddParameterWithValue("@PlaylistId", playlistId);
                result = _adoSupport.GetDataTable(cmd);

                foreach (DataRow row in result.Rows)
                {
                    var item = _currentPlaylist.FirstOrDefault(x => x.VideoId == Convert.ToInt32(row["VideoId"]));
                    if (item != null)
                    {
                        item.Position = Convert.ToDecimal(row["Position"]);
                        item.IsPlayed = Convert.ToBoolean(row["IsPlayed"]);
                        item.OrderIndex = Convert.ToInt32(row["OrderIndex"]);
                        item.SequenceType = row["SequenceType"].ToString();
                    }
                }

                if (sequenceType == SEQUENCE_RANDOM)
                {
                    Reorder(SEQUENCE_RANDOM);
                }
                else
                {
                    _currentPlaylist = _currentPlaylist.OrderBy(x => x.OrderIndex).ToList();
                }
                SavePlayerState();
                DBLogger.Log($"Loaded playlist for PlaylistId: {playlistId}, SequenceType: {sequenceType}");
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"LoadPlaylist failed: {ex.Message}");
                VideoEvents.OnVideoPlaybackError($"Load playlist error: {ex.Message}");
            }
        }

        private void SavePlayerState()
        {
            try
            {
                int playlistId = GetCurrentPlaylistId();
                foreach (var item in _currentPlaylist)
                {
                    ESqlCommand cmd = _adoSupport.GetESQLCommand();
                    cmd.CommandText = "spSavePlayerState";
                    cmd.AddParameterWithValue("@PlaylistId", playlistId);
                    cmd.AddParameterWithValue("@VideoId", item.VideoId);
                    cmd.AddParameterWithValue("@OrderIndex", item.OrderIndex);
                    cmd.AddParameterWithValue("@Position", item.Position);
                    cmd.AddParameterWithValue("@IsPlayed", item.IsPlayed);
                    cmd.AddParameterWithValue("@SequenceType", item.SequenceType ?? SEQUENCE_ORDERED);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"SavePlayerState failed: {ex.Message}");
                VideoEvents.OnVideoPlaybackError($"Save state error: {ex.Message}");
            }
        }

        private void UpdateVideoLastPlayed()
        {
            try
            {
                ESqlCommand cmd = _adoSupport.GetESQLCommand();
                cmd.CommandText = "spUpdateVideoLastPlayed";
                cmd.AddParameterWithValue("@VideoId", _currentPlaylist[_currentIndex].VideoId);
                cmd.ExecuteNonQuery();
                DBLogger.Log($"Updated LastPlayed for VideoId: {_currentPlaylist[_currentIndex].VideoId}");
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"UpdateVideoLastPlayed failed: {ex.Message}");
                VideoEvents.OnVideoPlaybackError($"Update LastPlayed error: {ex.Message}");
            }
        }

        private int GetCurrentPlaylistId()
        {
            if (_playlistSelector == null || !(_playlistSelector.Panel.Controls["cboppPlaylist"] is ComboBox cboppPlaylist))
            {
                return 0;
            }
            return cboppPlaylist.SelectedItem is ComboItem item ? item.Id : 0;
        }

        /// <summary>
        /// Disposes of the PlaylistPlayer resources.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                VideoEvents.PlaylistSelected -= HandlePlaylistSelected;
                VideoEvents.PlaylistRowAdded -= HandlePlaylistRowChanged;
                VideoEvents.PlaylistRowDeleted -= HandlePlaylistRowChanged;
                VideoEvents.PlaylistCleared -= HandlePlaylistCleared;
                VideoEvents.PlaylistDeleted -= HandlePlaylistDeleted;
                if (_player != null)
                {
                    _player.PlayStateChange -= HandlePlayStateChange;
                }
                _disposed = true;
                DBLogger.Log("PlaylistPlayer disposed.");
            }
        }
    }
}