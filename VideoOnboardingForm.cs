using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CSGeneral_x86;

namespace VideoPlaylist
{
    /// <summary>
    /// Form for onboarding videos, allowing directory selection and video insertion with metadata.
    /// </summary>
    public partial class VideoOnboardingForm : Form
    {
        private readonly ComboLoader _comboLoader;
        private readonly VideoManager _videoManager;
        private readonly PlaylistManager _playlistManager;
        private readonly TagControlManager tagsGroup1Manager;
        private readonly TagControlManager tagsGroup2Manager;
        private string _selectedDirectory;
        private List<string> _videoFiles;
        private bool _isProcessing;
        private DateTime _runTime;
        private CleanUnwantedText cleanText = new CleanUnwantedText();
        private DBProperty dbpSelectedDirectory;
        private clsADOSupport _ado = null;
        private ActorManager _actorManager = null;

        /// <summary>
        /// Gets the ActorManager instance for actor operations.
        /// </summary>
        public ActorManager ActorManager
        {
            get
            {
                if (_actorManager == null)
                {
                    _actorManager = new ActorManager();
                }
                return _actorManager;
            }
        }

        /// <summary>
        /// Gets or sets the ADOSupport instance for database operations.
        /// </summary>
        public clsADOSupport ADOSupport
        {
            get => _ado ?? clsADOSupport.LocalDB;
            set => _ado = value;
        }

        /// <summary>
        /// Initializes a new instance of the VideoOnboardingForm class.
        /// </summary>
        /// <param name="adoSupport">The ADOSupport instance for database operations.</param>
        public VideoOnboardingForm(clsADOSupport adoSupport)
        {
            InitializeComponent();
            _comboLoader = new ComboLoader(ADOSupport);
            _videoManager = new VideoManager();
            _playlistManager = new PlaylistManager(ADOSupport);
            _videoFiles = new List<string>();
            dbpSelectedDirectory = new DBProperty(clsADOSupport.LocalDB, "videoOnboardDir", "L:\\Media\\P\\Single\\Hair");
            txtDirectory.Text = dbpSelectedDirectory.Value;
            tagsGroup1Manager = new TagControlManager();
            tagsGroup1Manager.Control = clbTagsGroup1;
            tagsGroup1Manager.ADOSupport = ADOSupport;
            tagsGroup2Manager = new TagControlManager();
            tagsGroup2Manager.Control = clbTagsGroup2;
            tagsGroup2Manager.ADOSupport = ADOSupport;
        }

        /// <summary>
        /// Loads form controls with genres and tags from the database.
        /// </summary>
        private void LoadControls()
        {
            try
            {
                _comboLoader.LoadGenres(cmbGenre);
                tagsGroup1Manager.LoadTags(1);
                tagsGroup1Manager.SetDefaultCheckedStates();
                tagsGroup1Manager.SetRequiresReviewTag();
                tagsGroup2Manager.LoadTags(2);
                tagsGroup2Manager.SetDefaultCheckedStates();
                tagsGroup2Manager.SetRequiresReviewTag();
                ValidateInputs();
            }
            catch (Exception ex)
            {
                lblError.Text = $"Error loading controls: {ex.Message}";
                lblError.Visible = true;
                DBLogger.LogError($"Error loading controls: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles the Browse button click to select a directory.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _selectedDirectory = dialog.SelectedPath;
                    txtDirectory.Text = _selectedDirectory;
                    try
                    {
                        dbpSelectedDirectory.Value = _selectedDirectory;
                        LoadVideoFiles();
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = $"Error saving directory: {ex.Message}";
                        lblError.Visible = true;
                        DBLogger.LogError($"Error saving directory: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Loads video files from the selected directory into the grid.
        /// </summary>
        private void LoadVideoFiles()
        {
            try
            {
                _videoFiles.Clear();
                dgvVideos.Rows.Clear();
                if (!Directory.Exists(_selectedDirectory))
                {
                    lblError.Text = "Invalid directory.";
                    lblError.Visible = true;
                    return;
                }

                var extensions = new[] { ".mp4", ".mkv", ".avi", ".mov", ".wmv", ".mp3", ".wav", ".flac", ".aac", ".ogg" };
                _videoFiles.AddRange(Directory.GetFiles(_selectedDirectory)
                    .Where(file => extensions.Contains(Path.GetExtension(file).ToLower())));

                foreach (var file in _videoFiles)
                {
                    dgvVideos.Rows.Add(file);
                }

                lblError.Visible = false;
                ValidateInputs();
            }
            catch (Exception ex)
            {
                lblError.Text = $"Error loading files: {ex.Message}";
                lblError.Visible = true;
                DBLogger.LogError($"Error loading files: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles the Go button click to process and insert videos.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void btnGo_Click(object sender, EventArgs e)
        {
            decimal rating = 5.0m;

            try
            {
                rating = setupProcessing(rating, true);
                int successCount = AddVideoList(_videoFiles);

                if (_isProcessing)
                {
                    MessageBox.Show($"Successfully added {successCount} videos.", "Success");
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch (Exception ex)
            {
                lblError.Text = $"Error processing videos: {ex.Message}";
                lblError.Visible = true;
                DBLogger.LogError($"Error processing videos: {ex.Message}");
            }
            finally
            {
                stopProcessing();
            }
        }

        /// <summary>
        /// Stops video processing and resets control states.
        /// </summary>
        private void stopProcessing()
        {
            _isProcessing = false;
            btnGo.Enabled = true;
            btnStop.Enabled = false;
        }

        /// <summary>
        /// Adds videos and associated video tags. Now inserts actors as well.
        /// </summary>
        /// <param name="videoFiles">The list of video file paths to add.</param>
        /// <param name="singleShot">Indicates if processing a single video.</param>
        /// <returns>The number of successfully added videos.</returns>
        private int AddVideoList(List<string> videoFiles, bool singleShot = false)
        {
            int successCount = 0;
            decimal rating = 5.0m;

            foreach (var file in videoFiles)
            {
                rating = setupProcessing(rating, false, singleShot);

                try
                {
                    int id = (int)((DataRowView)cmbGenre.SelectedItem)["Id"];
                    var tagIds = TagControlManager.GetCombinedSelectedTags(clbTagsGroup1, clbTagsGroup2);
                    var actors = clbActors.CheckedItems.Cast<ActorData>()
                        .Select(item => item.Id)
                        .ToList();

                    var title = Path.GetFileNameWithoutExtension(file);
                    title = cleanText.CleanText(title);
                    DateTime fileDate = File.GetCreationTime(file);

                    var videoId = _videoManager.InsertVideo(title, id, rating, false, 0, 0, file, tagIds, actors, fileDate);
                    _playlistManager.AddToNewAdditionsPlaylist(videoId);
                    successCount++;

                    if (!singleShot)
                    {
                        progressBar.Value++;
                        progressBar.Refresh();
                    }
                }
                catch (Exception ex)
                {
                    DBLogger.LogError($"Error adding video {file}: {ex.Message}");
                    lblError.Text = $"Error adding video {Path.GetFileName(file)}: {ex.Message}";
                    lblError.Visible = true;
                }
            }

            return successCount;
        }

        /// <summary>
        /// Sets up processing state and returns the rating value.
        /// </summary>
        /// <param name="rating">The current rating value.</param>
        /// <param name="forceSetting">Indicates if processing state should be forced.</param>
        /// <param name="singleShot">Indicates if processing a single video.</param>
        /// <returns>The updated rating value.</returns>
        private decimal setupProcessing(decimal rating, bool forceSetting = false, bool singleShot = false)
        {
            if (forceSetting)
            {
                forceSetting = false;
                _isProcessing = false;
            }

            if (!_isProcessing)
            {
                _isProcessing = true;
                rating = 5.0m;
                btnGo.Enabled = false;
                btnStop.Enabled = true;
                if (!singleShot)
                {
                    progressBar.Value = 0;
                    progressBar.Maximum = _videoFiles.Count;
                }
                lblError.Visible = false;
                _runTime = DateTime.Now;
                _playlistManager.ClearNewAdditionsPlaylist();
            }

            return rating;
        }

        /// <summary>
        /// Handles the Stop button click to cancel video processing.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void btnStop_Click(object sender, EventArgs e)
        {
            _isProcessing = false;
        }

        /// <summary>
        /// Validates input controls to enable or disable the Go button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ValidateInputs(object sender, EventArgs e)
        {
            ValidateInputs();
        }

        /// <summary>
        /// Validates input controls to enable or disable the Go button.
        /// </summary>
        private void ValidateInputs()
        {
            btnGo.Enabled = !string.IsNullOrEmpty(_selectedDirectory) &&
                           Directory.Exists(_selectedDirectory) &&
                           _videoFiles.Count > 0 &&
                           cmbGenre.SelectedItem != null &&
                           (clbTagsGroup1.CheckedItems.Count > 0 || clbTagsGroup2.CheckedItems.Count > 0);
        }

        /// <summary>
        /// Handles the ProgressBar click event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void progressBar_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the form load event to initialize controls and load saved directory.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void VideoOnboardingForm_Load(object sender, EventArgs e)
        {
            try
            {
                string savedDirectory = dbpSelectedDirectory.Value;
                if (!string.IsNullOrEmpty(savedDirectory) && Directory.Exists(savedDirectory))
                {
                    _selectedDirectory = savedDirectory;
                    txtDirectory.Text = _selectedDirectory;
                    LoadVideoFiles();
                }
                LoadControls();
            }
            catch (Exception ex)
            {
                lblError.Text = $"Error loading form: {ex.Message}";
                lblError.Visible = true;
                DBLogger.LogError($"Error loading form: {ex.Message}");
            }
            this.WindowState = FormWindowState.Maximized;
        }

        /// <summary>
        /// Handles the DataGridView cell click event to populate filename details.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The cell event arguments.</param>
        private void dgvVideos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            populateFilename(dgvVideos.CurrentRow.Cells[0].Value.ToString());
        }

        /// <summary>
        /// Populates filename details and updates actor and existence indicators.
        /// </summary>
        /// <param name="filename">The filename to process.</param>
        private void populateFilename(string filename)
        {
            var title = Path.GetFileNameWithoutExtension(filename);
            title = cleanText.CleanText(title);
            txtFilename.Text = title;
            List<string> tokens = ActorManager.TokeniseFilename(title);
            List<ActorData> actors = ActorManager.FindMatchingEntries(ActorManager.TokeniseFilename(title));
            ActorManager.PopulateChecklistBox(tokens, clbActors, 5);
            labRecordExists.Visible = _videoManager.VideoExists(filename);
        }

        /// <summary>
        /// Handles the DataGridView cell content click event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The cell event arguments.</param>
        private void dgvVideos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        /// <summary>
        /// Handles the ItemCheck event for validation.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The item check event arguments.</param>
        private void ValidateInputs(object sender, ItemCheckEventArgs e)
        {
        }

        /// <summary>
        /// Handles the Refresh button click to repopulate filename details.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void htnRefresh_Click(object sender, EventArgs e)
        {
            populateFilename(txtFilename.Text);
        }

        /// <summary>
        /// Handles the Add Actor button click to insert a new actor and refresh details.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void btnAddActor_Click(object sender, EventArgs e)
        {
            ActorManager.InsertQuickActor(txtAddActor.Text);
            txtAddActor.Text = "";
            populateFilename(txtFilename.Text);
        }

        /// <summary>
        /// Handles the Add Video button click to insert a single video and move to the next row.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void btnAddVideo_Click(object sender, EventArgs e)
        {
            List<string> videoList = new List<string>();

            videoList.Add(dgvVideos.CurrentRow.Cells[0].Value.ToString());
            int successCount = AddVideoList(videoList, true);

            if (successCount == 1)
            {
                int idx = dgvVideos.CurrentRow.Index;
                if (idx < (dgvVideos.Rows.Count - 1))
                {
                    dgvVideos.CurrentCell = dgvVideos.Rows[idx + 1].Cells[0];
                    dgvVideos.Rows[idx + 1].Selected = true;
                }

                System.Threading.Thread.Sleep(500);
                populateFilename(dgvVideos.CurrentRow.Cells[0].Value.ToString());
            }
        }

        /// <summary>
        /// Handles the DataGridView row enter event to populate filename details.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The cell event arguments.</param>
        private void dgvVideos_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            populateFilename(Path.GetFileNameWithoutExtension(dgvVideos.Rows[e.RowIndex].Cells[0].Value.ToString()));
        }

        /// <summary>
        /// Handles the Stop Run button click to stop processing.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void btnStopRun_Click(object sender, EventArgs e)
        {
            stopProcessing();
        }
    }
}