using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSGeneral_x86;

namespace VideoPlaylist
{
    public partial class Form1 : Form
    {
        private clsADOSupport ADOSupport;
        private PlaylistSelector _playlistSelector; // Added
        private PlaylistFilter _playlistFilter; // Added
        private PlaylistPlayer _playlistPlayer; // Added

        public string Server
        {
            get
            {
                return @"MAXIPC\SQLDEV";
            }
        }
        public string Catalog
        {
            get
            {
                if (System.Diagnostics.Debugger.IsAttached)
                    return "VideoPlayerDebug";
                else
                    return "VideoPlayer";
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CreateVideoDB db = new CreateVideoDB(ADOSupport);
            
                //db.CreateSchema();
                //db.UpdateSchema();
            
            PlaylistPanel playlistPanel = new PlaylistPanel();
            playlistPanel.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MainThread.SetMainThread();
            ADOSupport = new clsADOSupport();
            ADOSupport.ConnectToSQLServer("", "", Server, Catalog, true);
            clsADOSupport.LocalDB = ADOSupport;
            clsADOSupport.RemoteDB = ADOSupport;

            // Initialize PlaylistSelector and PlaylistFilter
            _playlistSelector = new PlaylistSelector();
            _playlistSelector.Panel = panelPlaylist;
            _playlistSelector.ADOSupport = ADOSupport;
            _playlistSelector.InitializeEvents();

            _playlistFilter = new PlaylistFilter();
            _playlistFilter.Panel = panelPlaylistFilter;
            _playlistFilter.ADOSupport = ADOSupport;
            _playlistFilter.SetPlaylistSelector(_playlistSelector);
            _playlistFilter.InitializeEvents();

            // Initialize PlaylistPlayer
            _playlistPlayer = new PlaylistPlayer(ADOSupport);
            _playlistPlayer.Player = axWindowsMediaPlayer1;
            _playlistPlayer.SetPlaylistSelector(_playlistSelector);
            _playlistPlayer.Initialize();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            VideoOnboardingForm frm = new VideoOnboardingForm(ADOSupport);
            frm.Show();
        }

        private void btnSelectPlaylist_Click(object sender, EventArgs e)
        {
            PlaylistPanel playlistPanel = new PlaylistPanel();
            playlistPanel.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PlaylistFilterPanel fp = new PlaylistFilterPanel();
            fp.Show();
        }

        private void tabVideo_Click(object sender, EventArgs e)
        {
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Console.WriteLine(e.KeyChar);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine(e.KeyValue);
            _playlistPlayer.KeyPressed(e.KeyCode); // Pass keypress to PlaylistPlayer
        }
    }
}