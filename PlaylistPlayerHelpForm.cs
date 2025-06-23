using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace VideoPlaylist
{
    /// <summary>
    /// Form displaying a list of PlaylistPlayer commands with clickable or keypress interaction.
    /// </summary>
    public partial class PlaylistPlayerHelpForm : Form
    {
        private readonly Dictionary<string, string> _commandDescriptions;
        private readonly Dictionary<Keys, string> _keyMappings;

        /// <summary>
        /// Occurs when a command is selected by click or keypress.
        /// </summary>
        public event EventHandler<string> CommandSelected;

        /// <summary>
        /// Initializes a new instance of the PlaylistPlayerHelpForm class.
        /// </summary>
        public PlaylistPlayerHelpForm()
        {
            InitializeComponent();
            _commandDescriptions = new Dictionary<string, string>
            {
                { "Start", "Start or resume playback (prompts for resume position)" },
                { "Pause", "Toggle pause/unpause (Space)" },
                { "Stop", "Stop playback, saving position" },
                { "Next", "Skip to next video (Right)" },
                { "Previous", "Skip to previous video (Left)" },
                { "SkipForward10Min", "Skip forward 10 minutes (Up)" },
                { "SkipForward1Min", "Skip forward 1 minute (Down)" },
                { "SkipBackward10Min", "Skip backward 10 minutes (PageUp)" },
                { "SkipBackward1Min", "Skip backward 1 minute (PageDown)" },
                { "ReorderRandom", "Randomly reorder playlist (R)" },
                { "ReorderOrdered", "Order playlist as in database (O)" },
                { "RemoveCurrent", "Remove current video from playlist (X)" },
                { "DeleteCurrent", "Delete current video file and database entry (D)" }
            };
            _keyMappings = new Dictionary<Keys, string>
            {
                { Keys.Space, "Pause" },
                { Keys.H, "ShowHelp" },
                { Keys.Right, "Next" },
                { Keys.Left, "Previous" },
                { Keys.Up, "SkipForward10Min" },
                { Keys.Down, "SkipForward1Min" },
                { Keys.PageUp, "SkipBackward10Min" },
                { Keys.PageDown, "SkipBackward1Min" },
                { Keys.R, "ReorderRandom" },
                { Keys.O, "ReorderOrdered" },
                { Keys.X, "RemoveCurrent" },
                { Keys.D, "DeleteCurrent" }
            };
        }

        private void PlaylistPlayerHelpForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (_keyMappings.TryGetValue(e.KeyCode, out var command))
            {
                OnCommandSelected(command);
                if (command != "ShowHelp")
                {
                    this.Close();
                }
            }
        }

        private void OnCommandSelected(string command)
        {
            CommandSelected?.Invoke(this, command);
        }
    }
}