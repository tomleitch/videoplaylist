using System;
using System.Windows.Forms;
using CSGeneral_x86;

namespace VideoPlaylist
{
    public partial class PlaylistPanel : Form
    {
        private PlaylistSelector _playlistSelector;

        public PlaylistPanel()
        {
            InitializeComponent();
        }

        private void PlaylistPanel_Load(object sender, EventArgs e)
        {
            try
            {
                _playlistSelector = new PlaylistSelector
                {
                    Panel = panelPlaylist,
                    ADOSupport = clsADOSupport.LocalDB
                };
                _playlistSelector.InitializeEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing PlaylistSelector: {ex.Message}", "Error");
                DBLogger.LogError($"PlaylistPanel_Load failed: {ex.Message}");
            }
        }

    }
}