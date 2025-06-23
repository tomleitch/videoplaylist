using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using CSGeneral_x86;

namespace VideoPlaylist
{
    /// <summary>
    /// Manages playlist selection and creation operations for a Panel containing playlist controls.
    /// </summary>
    public class PlaylistSelector
    {
        private Panel panelPlaylist;
        private clsADOSupport adoSupport;
        private PlaylistManager playlistManager;
        private ComboBox cboppPlaylist;
        private Button butppClear;
        private Button btnppDelete;
        private Button btnppCreatePlaylist;
        private TextBox txtppNewPlaylistName;

        /// <summary>
        /// Gets or sets the Panel containing playlist controls.
        /// </summary>
        public Panel Panel
        {
            get => panelPlaylist;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                panelPlaylist = value;
            }
        }

        /// <summary>
        /// Gets or sets the ADOSupport instance for database operations.
        /// </summary>
        public clsADOSupport ADOSupport
        {
            get => adoSupport;
            set
            {
                adoSupport = value;
                playlistManager = new PlaylistManager(adoSupport);
            }
        }

        /// <summary>
        /// Initializes a new instance of the PlaylistSelector class.
        /// </summary>
        public PlaylistSelector()
        {
        }

        /// <summary>
        /// Initializes event handlers for the panel's controls and subscribes to playlist events.
        /// </summary>
        public void InitializeEvents()
        {
            if (panelPlaylist == null || adoSupport == null)
            {
                DBLogger.LogError("InitializeEvents failed: Panel or ADOSupport is not set.");
                return;
            }

            try
            {
                cboppPlaylist = panelPlaylist.Controls["cboppPlaylist"] as ComboBox;
                butppClear = panelPlaylist.Controls["butppClear"] as Button;
                btnppDelete = panelPlaylist.Controls["btnppDelete"] as Button;
                btnppCreatePlaylist = panelPlaylist.Controls["btnppCreatePlaylist"] as Button;
                txtppNewPlaylistName = panelPlaylist.Controls["txtppNewPlaylistName"] as TextBox;

                Debug.WriteLine($"Control retrieval: cboppPlaylist={cboppPlaylist != null}, butppClear={butppClear != null}, btnppDelete={btnppDelete != null}, btnppCreatePlaylist={btnppCreatePlaylist != null}, txtppNewPlaylistName={txtppNewPlaylistName != null}");

                UpdateButtonStates();

                if (cboppPlaylist != null)
                {
                    cboppPlaylist.SelectedIndexChanged += HandlePlaylistSelectionChanged;
                    Refresh();
                    cboppPlaylist.SelectedIndex = 0;
                }
                if (butppClear != null)
                {
                    butppClear.Click += HandleClearClick;
                }
                if (btnppDelete != null)
                {
                    btnppDelete.Click += HandleDeleteClick;
                }
                if (btnppCreatePlaylist != null)
                {
                    btnppCreatePlaylist.Click += HandleCreatePlaylistClick;
                }
                if (txtppNewPlaylistName != null)
                {
                    txtppNewPlaylistName.TextChanged += HandleNewPlaylistNameTextChanged;
                }

                VideoEvents.PlaylistRowAdded += HandlePlaylistRowAdded;
                VideoEvents.PlaylistRowDeleted += HandlePlaylistRowDeleted;

                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"InitializeEvents failed: {ex.Message}");
                MessageBox.Show($"Error initializing playlist controls: {ex.Message}", "Error");
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event for the playlist ComboBox.
        /// </summary>
        private void HandlePlaylistSelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboppPlaylist != null && cboppPlaylist.SelectedItem is ComboItem item)
                {
                    Debug.WriteLine($"SelectedIndexChanged: Id={item.Id}, Name={item.Name}");
                    if (item.Id > 0)
                    {
                        Debug.WriteLine($"PlaylistSelected raised for PlaylistId: {item.Id}");
                        VideoEvents.OnPlaylistSelected(item.Id);
                    }
                    UpdateNumEntries();
                    UpdateButtonStates();
                }
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"HandlePlaylistSelectionChanged failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles the Click event for the Clear button.
        /// </summary>
        private void HandleClearClick(object sender, EventArgs e)
        {
            Debug.WriteLine("butppClear clicked");
            Clear();
        }

        /// <summary>
        /// Handles the Click event for the Delete button.
        /// </summary>
        private void HandleDeleteClick(object sender, EventArgs e)
        {
            Debug.WriteLine("btnppDelete clicked");
            Delete();
        }

        /// <summary>
        /// Handles the Click event for the Create Playlist button.
        /// </summary>
        private void HandleCreatePlaylistClick(object sender, EventArgs e)
        {
            Debug.WriteLine("btnppCreatePlaylist clicked");
            Save();
        }

        /// <summary>
        /// Handles the TextChanged event for the New Playlist Name TextBox.
        /// </summary>
        private void HandleNewPlaylistNameTextChanged(object sender, EventArgs e)
        {
            Debug.WriteLine($"txtppNewPlaylistName.TextChanged: Text={txtppNewPlaylistName?.Text}");
            UpdateButtonStates();
        }

        /// <summary>
        /// Handles the PlaylistRowAdded event from VideoEvents.
        /// </summary>
        private void HandlePlaylistRowAdded(object sender, int playlistId)
        {
            Debug.WriteLine($"PlaylistRowAdded raised for PlaylistId: {playlistId}");
            UpdateNumEntries();
        }

        /// <summary>
        /// Handles the PlaylistRowDeleted event from VideoEvents.
        /// </summary>
        private void HandlePlaylistRowDeleted(object sender, int playlistId)
        {
            Debug.WriteLine($"PlaylistRowDeleted raised for PlaylistId: {playlistId}");
            UpdateNumEntries();
        }

        /// <summary>
        /// Saves a new playlist and raises the NewPlaylistCreated event.
        /// </summary>
        public void Save()
        {
            if (panelPlaylist == null || adoSupport == null)
            {
                DBLogger.LogError("Save failed: Panel or ADOSupport is not set.");
                return;
            }

            try
            {
                if (txtppNewPlaylistName == null)
                {
                    DBLogger.LogError("Save failed: Playlist name control not found.");
                    MessageBox.Show("Playlist name control not found.", "Error");
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtppNewPlaylistName.Text))
                {
                    DBLogger.LogError("Save failed: Playlist name is empty.");
                    MessageBox.Show("Please enter a playlist name.", "Error");
                    return;
                }

                int playlistId = playlistManager.CreatePlaylist(txtppNewPlaylistName.Text);
                if (playlistId > 0)
                {
                    Debug.WriteLine($"NewPlaylistCreated raised for PlaylistId: {playlistId}");
                    VideoEvents.OnNewPlaylistCreated(playlistId);
                    txtppNewPlaylistName.Text = "";
                    Refresh(playlistId);
                    UpdateButtonStates();
                }
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"Save failed: {ex.Message}");
                MessageBox.Show($"Error saving playlist: {ex.Message}", "Error");
            }
        }

        /// <summary>
        /// Deletes the selected playlist and raises the PlaylistDeleted and PlaylistSelected events.
        /// </summary>
        public void Delete()
        {
            if (panelPlaylist == null || adoSupport == null)
            {
                DBLogger.LogError("Delete failed: Panel or ADOSupport is not set.");
                return;
            }

            try
            {
                if (cboppPlaylist == null || !(cboppPlaylist.SelectedItem is ComboItem item) || item.Id <= 0)
                {
                    DBLogger.LogError("Delete failed: No valid playlist selected or control not found.");
                    MessageBox.Show("Please select a valid playlist to delete.", "Error");
                    return;
                }

                int playlistId = item.Id;
                playlistManager.DeletePlaylist(playlistId);
                Debug.WriteLine($"PlaylistDeleted raised for PlaylistId: {playlistId}");
                VideoEvents.OnPlaylistDeleted(playlistId);
                Debug.WriteLine("PlaylistSelected raised for PlaylistId: 0");
                VideoEvents.OnPlaylistSelected(0);
                Refresh(); // Ensure no playlist is selected after deletion
                if (cboppPlaylist != null)
                {
                    cboppPlaylist.SelectedIndex = 0;
                }
                UpdateButtonStates();
                DBLogger.Log($"Deleted playlist and refreshed selector panel for PlaylistId: {playlistId}");
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"Delete failed: {ex.Message}");
                MessageBox.Show($"Error deleting playlist: {ex.Message}", "Error");
            }
        }

        /// <summary>
        /// Clears the videos in the selected playlist and raises the PlaylistCleared event.
        /// </summary>
        public void Clear()
        {
            if (panelPlaylist == null || adoSupport == null)
            {
                DBLogger.LogError("Clear failed: Panel or ADOSupport is not set.");
                return;
            }

            try
            {
                if (cboppPlaylist == null || !(cboppPlaylist.SelectedItem is ComboItem item) || item.Id <= 0)
                {
                    DBLogger.LogError("Clear failed: No valid playlist selected or control not found.");
                    MessageBox.Show("Please select a valid playlist to clear.", "Error");
                    return;
                }

                int playlistId = item.Id;
                playlistManager.ClearPlaylistVideos(playlistId);
                Debug.WriteLine($"PlaylistCleared raised for PlaylistId: {playlistId}");
                VideoEvents.OnPlaylistCleared(playlistId);
                Refresh(playlistId); // Retain current playlist selection after clearing
                UpdateNumEntries();
                UpdateButtonStates();
                DBLogger.Log($"Cleared videos and refreshed selector panel for PlaylistId: {playlistId}");
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"Clear failed: {ex.Message}");
                MessageBox.Show($"Error clearing playlist: {ex.Message}", "Error");
            }
        }

        /// <summary>
        /// Refreshes the playlist ComboBox and updates the number of entries.
        /// </summary>
        public void Refresh(int selectPlaylistId = -1)
        {
            if (panelPlaylist == null || adoSupport == null)
            {
                DBLogger.LogError("Refresh failed: Panel or ADOSupport is not set.");
                return;
            }

            try
            {
                if (cboppPlaylist == null)
                {
                    DBLogger.LogError("Refresh failed: ComboBox control not found.");
                    MessageBox.Show("ComboBox control not found.", "Error");
                    return;
                }

                // Store current selection if not specified
                int currentPlaylistId = selectPlaylistId > -1 ? selectPlaylistId :
                    (cboppPlaylist.SelectedItem is ComboItem item && item.Id > 0) ? item.Id : -1;

                cboppPlaylist.Items.Clear();
                cboppPlaylist.Items.Add(new ComboItem(-1, "-- Select Playlist --"));
                List<ComboItem> playlists = playlistManager.GetPlaylists();
                if (playlists.Count == 0)
                {
                    DBLogger.LogError("Refresh: No playlists found in NamedPlaylist table.");
                }

                foreach (ComboItem playlist in playlists)
                {
                    cboppPlaylist.Items.Add(playlist);
                }

                // Restore selection
                bool selectionRestored = false;
                foreach (ComboItem playlist in cboppPlaylist.Items)
                {
                    if (playlist.Id == currentPlaylistId)
                    {
                        cboppPlaylist.SelectedItem = playlist;
                        selectionRestored = true;
                        break;
                    }
                }

                if (!selectionRestored)
                {
                    cboppPlaylist.SelectedIndex = 0;
                }

                UpdateNumEntries();
                UpdateButtonStates();
                DBLogger.Log($"Refreshed playlists, selection {(selectionRestored ? "restored" : "reset")} to PlaylistId: {currentPlaylistId}");
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"Refresh failed: {ex.Message}");
                MessageBox.Show($"Error refreshing playlists: {ex.Message}", "Error");
            }
        }

        /// <summary>
        /// Updates the number of entries TextBox based on the selected playlist.
        /// </summary>
        private void UpdateNumEntries()
        {
            if (panelPlaylist == null)
            {
                DBLogger.LogError("UpdateNumEntries failed: Panel is not set.");
                return;
            }

            try
            {
                TextBox txtppNumEntries = panelPlaylist.Controls["txtppNumEntries"] as TextBox;
                if (txtppNumEntries == null || cboppPlaylist == null)
                {
                    DBLogger.LogError("UpdateNumEntries failed: Controls not found.");
                    return;
                }

                txtppNumEntries.Text = (cboppPlaylist.SelectedItem is ComboItem item && item.Id > 0)
                    ? playlistManager.GetVideoCount(item.Id).ToString()
                    : "0";
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"UpdateNumEntries failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the enabled state of buttons based on control states.
        /// </summary>
        private void UpdateButtonStates()
        {
            if (panelPlaylist == null)
            {
                DBLogger.LogError("UpdateButtonStates failed: Panel is not set.");
                return;
            }

            try
            {
                bool hasValidSelection = cboppPlaylist?.SelectedItem is ComboItem item && item.Id > 0;
                bool hasValidName = txtppNewPlaylistName != null && !string.IsNullOrWhiteSpace(txtppNewPlaylistName.Text);

                if (butppClear != null)
                {
                    butppClear.Enabled = hasValidSelection;
                }
                if (btnppDelete != null)
                {
                    btnppDelete.Enabled = hasValidSelection;
                }
                if (btnppCreatePlaylist != null)
                {
                    btnppCreatePlaylist.Enabled = hasValidName;
                }
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"UpdateButtonStates failed: {ex.Message}");
            }
        }
    }
}