using System;
using System.Windows.Forms;
using CSGeneral_x86;

namespace VideoPlaylist
{
    /// <summary>
    /// Form for filtering videos and adding them to playlists.
    /// </summary>
    public partial class PlaylistFilterPanel : Form
    {
        private PlaylistFilter playlistFilter;
        private PlaylistSelector playlistSelector;

        /// <summary>
        /// Initializes a new instance of the PlaylistFilterPanel class.
        /// </summary>
        public PlaylistFilterPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Load event of the PlaylistFilterPanel form.
        /// </summary>
        private void PlaylistFilterPanel_Load(object sender, EventArgs e)
        {
            try
            {
                playlistSelector = new PlaylistSelector
                {
                    Panel = panelPlaylist,
                    ADOSupport = clsADOSupport.LocalDB
                };
                playlistSelector.InitializeEvents();

                playlistFilter = new PlaylistFilter
                {
                    Panel = panelPlaylistFilter,
                    ADOSupport = clsADOSupport.LocalDB
                };
                playlistFilter.SetPlaylistSelector(playlistSelector);
                playlistFilter.InitializeEvents();

                // Mock selection of "New Additions" playlist
                if (cboppPlaylist != null)
                {
                    foreach (ComboItem item in cboppPlaylist.Items)
                    {
                        if (item.Name == "New Additions")
                        {
                            cboppPlaylist.SelectedItem = item;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing form: {ex.Message}", "Error");
                DBLogger.LogError($"PlaylistFilterPanel_Load failed: {ex.Message}");
            }
        }
    }
}