using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using CSGeneral_x86;

namespace VideoPlaylist
{
    /// <summary>
    /// Manages video filtering and playlist operations for panelPlaylistFilter.
    /// </summary>
    public class PlaylistFilter : IDisposable
    {
        private Panel panelPlaylistFilter;
        private clsADOSupport adoSupport;
        private PlaylistSelector playlistSelector;
        private CheckedListBox clbfvGenre;
        private CheckedListBox clbfvTagsGroup1;
        private CheckedListBox clbfvTagsGroup2;
        private TextBox txtfvActorSearch;
        private CheckedListBox clbfvActors;
        private Button btnfvAppend;
        private Button btnfvReplace;
        private TextBox txtfvCurrentMatches;
        private ComboLoader comboLoader;
        private TagControlManager tagsGroup1Manager;
        private TagControlManager tagsGroup2Manager;
        private Timer updateMatchTimer;
        private bool disposed = false;

        /// <summary>
        /// Gets or sets the Panel containing filter controls.
        /// </summary>
        public Panel Panel
        {
            get => panelPlaylistFilter;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                panelPlaylistFilter = value;
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
                comboLoader = new ComboLoader(adoSupport);
                tagsGroup1Manager = new TagControlManager { ADOSupport = adoSupport };
                tagsGroup2Manager = new TagControlManager { ADOSupport = adoSupport };
            }
        }

        /// <summary>
        /// Initializes a new instance of the PlaylistFilter class.
        /// </summary>
        public PlaylistFilter()
        {
            updateMatchTimer = new Timer
            {
                Interval = 100
            };
            updateMatchTimer.Tick += HandleUpdateMatchTimerTick;
        }

        /// <summary>
        /// Initializes event handlers for filter controls and loads data.
        /// </summary>
        public void InitializeEvents()
        {
            if (panelPlaylistFilter == null || adoSupport == null)
            {
                DBLogger.LogError("InitializeEvents failed: Panel or ADOSupport is not set.");
                return;
            }

            try
            {
                clbfvGenre = panelPlaylistFilter.Controls["clbfvGenre"] as CheckedListBox;
                clbfvTagsGroup1 = panelPlaylistFilter.Controls["clbfvTagsGroup1"] as CheckedListBox;
                clbfvTagsGroup2 = panelPlaylistFilter.Controls["clbfvTagsGroup2"] as CheckedListBox;
                txtfvActorSearch = panelPlaylistFilter.Controls["txtfvActorSearch"] as TextBox;
                clbfvActors = panelPlaylistFilter.Controls["clbfvActors"] as CheckedListBox;
                btnfvAppend = panelPlaylistFilter.Controls["btnfvAppend"] as Button;
                btnfvReplace = panelPlaylistFilter.Controls["btnfvReplace"] as Button;
                txtfvCurrentMatches = panelPlaylistFilter.Controls["txtfvCurrentMatches"] as TextBox;

                Debug.WriteLine($"Control retrieval: clbfvGenre={clbfvGenre != null}, clbfvTagsGroup1={clbfvTagsGroup1 != null}, clbfvTagsGroup2={clbfvTagsGroup2 != null}, txtfvActorSearch={txtfvActorSearch != null}, clbfvActors={clbfvActors != null}, btnfvAppend={btnfvAppend != null}, btnfvReplace={btnfvReplace != null}, txtfvCurrentMatches={txtfvCurrentMatches != null}");

                if (clbfvGenre != null)
                {
                    clbfvGenre.ItemCheck += HandleGenreItemCheck;
                }
                if (clbfvTagsGroup1 != null)
                {
                    clbfvTagsGroup1.ItemCheck += HandleTagsGroup1ItemCheck;
                    tagsGroup1Manager.Control = clbfvTagsGroup1;
                }
                if (clbfvTagsGroup2 != null)
                {
                    clbfvTagsGroup2.ItemCheck += HandleTagsGroup2ItemCheck;
                    tagsGroup2Manager.Control = clbfvTagsGroup2;
                }
                if (txtfvActorSearch != null)
                {
                    txtfvActorSearch.TextChanged += HandleActorSearchTextChanged;
                }
                if (clbfvActors != null)
                {
                    clbfvActors.ItemCheck += HandleActorsItemCheck;
                }
                if (btnfvAppend != null)
                {
                    btnfvAppend.Click += HandleAppendClick;
                }
                if (btnfvReplace != null)
                {
                    btnfvReplace.Click += HandleReplaceClick;
                }

                LoadControls();
                UpdateMatchCount();
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"InitializeEvents failed: {ex.Message}");
                MessageBox.Show($"Error initializing filter controls: {ex.Message}", "Error");
            }
        }

        /// <summary>
        /// Loads filter controls with genres, tags, and actors.
        /// </summary>
        private void LoadControls()
        {
            try
            {
                comboLoader.LoadGenres(clbfvGenre);
                tagsGroup1Manager.LoadTags(1);
                tagsGroup2Manager.LoadTags(2);
                UpdateActorList("");
                UpdateMatchCount();
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"LoadControls failed: {ex.Message}");
                MessageBox.Show($"Error loading controls: {ex.Message}", "Error");
            }
        }

        /// <summary>
        /// Handles the ItemCheck event for Genre CheckedListBox.
        /// </summary>
        private void HandleGenreItemCheck(object sender, ItemCheckEventArgs e)
        {
            updateMatchTimer.Start();
            UpdateButtonStates();
        }

        /// <summary>
        /// Handles the ItemCheck event for Tags Group 1 CheckedListBox.
        /// </summary>
        private void HandleTagsGroup1ItemCheck(object sender, ItemCheckEventArgs e)
        {
            updateMatchTimer.Start();
            UpdateButtonStates();
        }

        /// <summary>
        /// Handles the ItemCheck event for Tags Group 2 CheckedListBox.
        /// </summary>
        private void HandleTagsGroup2ItemCheck(object sender, ItemCheckEventArgs e)
        {
            updateMatchTimer.Start();
            UpdateButtonStates();
        }

        /// <summary>
        /// Handles the TextChanged event for Actor search TextBox.
        /// </summary>
        private void HandleActorSearchTextChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateActorList(txtfvActorSearch.Text);
                UpdateMatchCount();
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"HandleActorSearchTextChanged failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles the ItemCheck event for Actors CheckedListBox.
        /// </summary>
        private void HandleActorsItemCheck(object sender, ItemCheckEventArgs e)
        {
            updateMatchTimer.Start();
            UpdateButtonStates();
        }

        /// <summary>
        /// Handles the Click event for the Append button.
        /// </summary>
        private void HandleAppendClick(object sender, EventArgs e)
        {
            try
            {
                FilterAndUpdatePlaylist(false);
                Debug.WriteLine("VideosAppended raised for PlaylistId: " + GetSelectedPlaylistId());
                VideoEvents.OnVideosAppended(GetSelectedPlaylistId());
                UpdateMatchCount();
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"HandleAppendClick failed: {ex.Message}");
                MessageBox.Show($"Error appending videos: {ex.Message}", "Error");
            }
        }

        /// <summary>
        /// Handles the Click event for the Replace button.
        /// </summary>
        private void HandleReplaceClick(object sender, EventArgs e)
        {
            try
            {
                FilterAndUpdatePlaylist(true);
                Debug.WriteLine("VideosReplaced raised for PlaylistId: " + GetSelectedPlaylistId());
                VideoEvents.OnVideosReplaced(GetSelectedPlaylistId());
                UpdateMatchCount();
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"HandleReplaceClick failed: {ex.Message}");
                MessageBox.Show($"Error replacing videos: {ex.Message}", "Error");
            }
        }

        /// <summary>
        /// Handles the Tick event for the update match timer.
        /// </summary>
        private void HandleUpdateMatchTimerTick(object sender, EventArgs e)
        {
            updateMatchTimer.Stop();
            UpdateMatchCount();
        }

        /// <summary>
        /// Handles the PlaylistSelected event from VideoEvents.
        /// </summary>
        private void HandlePlaylistSelected(object sender, int playlistId)
        {
            if (GetSelectedPlaylistId() == playlistId || GetSelectedPlaylistId() == 0)
            {
                UpdateMatchCount();
                UpdateButtonStates();
                DBLogger.Log($"PlaylistSelected handled for PlaylistId={playlistId}");
            }
        }

        /// <summary>
        /// Updates the Actors CheckedListBox based on search text.
        /// </summary>
        private void UpdateActorList(string searchText)
        {
            if (clbfvActors == null)
            {
                return;
            }

            try
            {
                clbfvActors.Items.Clear();
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    UpdateMatchCount();
                    return;
                }

                string[] searchTerms = searchText.Trim().Split(' ');
                string query = "SELECT Id, Forename, Surname FROM Actor WHERE ";
                if (searchTerms.Length == 1)
                {
                    query += "Forename LIKE @Search OR Surname LIKE @Search";
                }
                else if (searchTerms.Length >= 2)
                {
                    query += "Forename LIKE @Search1 AND Surname LIKE @Search2";
                }

                ESqlCommand cmd = adoSupport.GetESQLCommandQuery();
                cmd.CommandText = query;
                if (searchTerms.Length == 1)
                {
                    cmd.AddParameterWithValue("@Search", $"%{searchTerms[0]}%");
                }
                else if (searchTerms.Length >= 2)
                {
                    cmd.AddParameterWithValue("@Search1", $"%{searchTerms[0]}%");
                    cmd.AddParameterWithValue("@Search2", $"%{searchTerms[1]}%");
                }

                DataTable result = adoSupport.GetDataTable(cmd);

                foreach (DataRow row in result.Rows)
                {
                    int id = Convert.ToInt32(row["Id"]);
                    string name = $"{row["Forename"]} {row["Surname"]}".Trim();
                    clbfvActors.Items.Add(new ComboItem(id, name), false);
                }
                Debug.WriteLine($"Actor search found {result.Rows.Count} matches for: {searchText}");
                DBLogger.Log($"Actor search found {result.Rows.Count} matches for: {searchText}");
                UpdateMatchCount();
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"UpdateActorList failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the current matches count based on filter criteria.
        /// </summary>
        private void UpdateMatchCount()
        {
            if (txtfvCurrentMatches == null)
            {
                return;
            }

            try
            {
                int playlistId = GetSelectedPlaylistId();
                DataTable criteria = BuildCriteriaTable();

                // Detailed logging of Criteria table
                string criteriaLog = $"UpdateMatchCount Criteria: {criteria.Rows.Count} rows";
                foreach (DataRow row in criteria.Rows)
                {
                    int typeId = Convert.ToInt32(row["TypeId"]);
                    int id = Convert.ToInt32(row["Id"]);
                    criteriaLog += $"\nTypeId: {typeId}, Id: {id}";
                }
                Debug.WriteLine(criteriaLog);
                DBLogger.Log(criteriaLog);

                ESqlCommand cmd = adoSupport.GetESQLCommandQuery();
                cmd.CommandText = "spCountFilteredVideos";
                cmd.AddTableParameter("@Criteria", criteria, "dbo.IdList");
                cmd.AddParameterWithValue("@PlaylistId", playlistId == 0 ? (object)DBNull.Value : playlistId);

                DataTable result = adoSupport.GetDataTable(cmd);
                int matchCount = result.Rows.Count > 0 ? Convert.ToInt32(result.Rows[0]["MatchCount"]) : 0;
                txtfvCurrentMatches.Text = matchCount.ToString();
                Debug.WriteLine($"Match count updated: {matchCount}");
                DBLogger.Log($"Match count updated: {matchCount}");
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"UpdateMatchCount failed: {ex.Message}");
                txtfvCurrentMatches.Text = "0";
            }
        }

        /// <summary>
        /// Builds a DataTable with filter criteria for stored procedures.
        /// </summary>
        private DataTable BuildCriteriaTable()
        {
            DataTable criteria = new DataTable();
            criteria.Columns.Add("TypeId", typeof(int));
            criteria.Columns.Add("Id", typeof(int));

            // Add Genre IDs (ComboItem)
            if (clbfvGenre != null)
            {
                foreach (object item in clbfvGenre.CheckedItems)
                {
                    if (item is ComboItem comboItem)
                    {
                        criteria.Rows.Add(1, comboItem.Id);
                    }
                    else
                    {
                        DBLogger.LogError($"Unexpected item type in clbfvGenre.CheckedItems: {item?.GetType().Name}");
                    }
                }
            }

            // Add Tags Group 1 IDs (ComboItem)
            if (clbfvTagsGroup1 != null)
            {
                foreach (object item in clbfvTagsGroup1.CheckedItems)
                {
                    if (item is ComboItem comboItem)
                    {
                        criteria.Rows.Add(2, comboItem.Id);
                    }
                }
            }

            // Add Tags Group 2 IDs (ComboItem)
            if (clbfvTagsGroup2 != null)
            {
                foreach (object item in clbfvTagsGroup2.CheckedItems)
                {
                    if (item is ComboItem comboItem)
                    {
                        criteria.Rows.Add(3, comboItem.Id);
                    }
                }
            }

            // Add Actor IDs (ComboItem)
            if (clbfvActors != null)
            {
                foreach (object item in clbfvActors.CheckedItems)
                {
                    if (item is ComboItem comboItem)
                    {
                        criteria.Rows.Add(4, comboItem.Id);
                    }
                }
            }

            // Log Criteria table before returning
            string criteriaLog = $"BuildCriteriaTable: {criteria.Rows.Count} rows";
            foreach (DataRow row in criteria.Rows)
            {
                int typeId = Convert.ToInt32(row["TypeId"]);
                int id = Convert.ToInt32(row["Id"]);
                criteriaLog += $"\nTypeId: {typeId}, Id: {id}";
            }
            Debug.WriteLine(criteriaLog);
            DBLogger.Log(criteriaLog);

            return criteria;
        }

        /// <summary>
        /// Filters videos and updates the selected playlist.
        /// </summary>
        /// <param name="replace">True to replace existing videos; false to append.</param>
        private void FilterAndUpdatePlaylist(bool replace)
        {
            int playlistId = GetSelectedPlaylistId();
            if (playlistId <= 0)
            {
                MessageBox.Show("Please select a valid playlist.", "Error");
                return;
            }

            DataTable criteria = BuildCriteriaTable();
            if (criteria.Rows.Count == 0)
            {
                MessageBox.Show("Please select at least one filter criterion.", "Error");
                return;
            }

            try
            {
                ESqlCommand cmd = adoSupport.GetESQLCommand();
                cmd.CommandText = "spAppendOrReplaceVideos";
                cmd.AddTableParameter("@Criteria", criteria, "dbo.IdList");
                cmd.AddParameterWithValue("@PlaylistId", playlistId);
                cmd.AddParameterWithValue("@Replace", replace ? 1 : 0);
                cmd.ExecuteNonQuery();

                Debug.WriteLine($"Videos {(replace ? "replaced" : "appended")} for PlaylistId: {playlistId}");
                DBLogger.Log($"Videos {(replace ? "replaced" : "appended")} for PlaylistId: {playlistId}");
                playlistSelector.Refresh();
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"FilterAndUpdatePlaylist failed: {ex.Message}");
                MessageBox.Show($"Error {(replace ? "replacing" : "appending")} videos: {ex.Message}", "Error");
            }
        }

        /// <summary>
        /// Gets the ID of the selected playlist from PlaylistSelector.
        /// </summary>
        private int GetSelectedPlaylistId()
        {
            if (playlistSelector == null || !(playlistSelector.Panel.Controls["cboppPlaylist"] is ComboBox cboppPlaylist))
            {
                return 0;
            }

            return cboppPlaylist.SelectedItem is ComboItem item ? item.Id : 0;
        }

        /// <summary>
        /// Sets the PlaylistSelector instance for playlist operations.
        /// </summary>
        public void SetPlaylistSelector(PlaylistSelector selector)
        {
            if (playlistSelector != null)
            {
                VideoEvents.PlaylistSelected -= HandlePlaylistSelected;
            }

            playlistSelector = selector;
            VideoEvents.PlaylistSelected += HandlePlaylistSelected;
            DBLogger.Log($"Subscribed to VideoEvents.PlaylistSelected for PlaylistId={GetSelectedPlaylistId()}");
            UpdateButtonStates();
            UpdateMatchCount();
        }

        /// <summary>
        /// Updates the enabled state of Append/Replace buttons.
        /// </summary>
        private void UpdateButtonStates()
        {
            try
            {
                bool hasValidSelection = GetSelectedPlaylistId() > 0;
                bool hasCriteria = (clbfvGenre?.CheckedItems.Count > 0 ||
                                   clbfvTagsGroup1?.CheckedItems.Count > 0 ||
                                   clbfvTagsGroup2?.CheckedItems.Count > 0 ||
                                   clbfvActors?.CheckedItems.Count > 0);

                if (btnfvAppend != null)
                {
                    btnfvAppend.Enabled = hasValidSelection && hasCriteria;
                }
                if (btnfvReplace != null)
                {
                    btnfvReplace.Enabled = hasValidSelection && hasCriteria;
                }
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"UpdateButtonStates failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Disposes of the PlaylistFilter resources and unsubscribes from events.
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                updateMatchTimer.Tick -= HandleUpdateMatchTimerTick;
                updateMatchTimer.Dispose();
                VideoEvents.PlaylistSelected -= HandlePlaylistSelected;
                DBLogger.Log("Unsubscribed from VideoEvents.PlaylistSelected");
                disposed = true;
            }
        }
    }
}