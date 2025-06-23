using System;
using System.Data;
using System.Windows.Forms;
using CSGeneral_x86;

namespace VideoPlaylist
{
    /// <summary>
    /// Loads data into ComboBox and CheckedListBox controls for genres and tags.
    /// </summary>
    public class ComboLoader
    {
        private clsADOSupport _adoSupport;

        /// <summary>
        /// Gets or sets the ADOSupport instance for database operations.
        /// </summary>
        public clsADOSupport ADOSupport
        {
            get => _adoSupport ?? (_adoSupport = clsADOSupport.LocalDB);
            set => _adoSupport = value;
        }

        /// <summary>
        /// Initializes a new instance of the ComboLoader class.
        /// </summary>
        /// <param name="adoSupport">The ADOSupport instance for database operations.</param>
        public ComboLoader(clsADOSupport adoSupport = null)
        {
            _adoSupport = adoSupport;
        }

        /// <summary>
        /// Loads genres into a ComboBox from the Genre table.
        /// </summary>
        /// <param name="comboBox">The ComboBox to populate.</param>
        public void LoadGenres(ComboBox comboBox)
        {
            try
            {
                // Check Genres table existence
                int tableExists;
                ADOSupport.GetScalerValue(out tableExists, "SELECT COUNT(*) FROM sys.tables WHERE name = 'Genre'");
                if (tableExists == 0)
                {
                    DBLogger.LogError("Genre table does not exist.");
                    throw new InvalidOperationException("Genre table does not exist.");
                }

                // Fetch DataTable
                string sql = "SELECT Id, Name FROM [dbo].[Genre] ORDER BY Name";
                DataTable dt = ADOSupport.GetDataTable(sql);
                DBLogger.LogError($"Loading genres: Found {dt.Rows.Count} records.");

                // Bind to ComboBox
                comboBox.DataSource = dt;
                comboBox.DisplayMember = "Name";
                comboBox.ValueMember = "Id";
                if (comboBox.Items.Count > 0)
                {
                    comboBox.SelectedIndex = 0;
                    DBLogger.LogError("Genres ComboBox bound successfully.");
                }
                else
                {
                    DBLogger.LogError("No genres found to bind to ComboBox.");
                }
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"Error loading genres: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Loads genres into a CheckedListBox from the Genre table.
        /// </summary>
        /// <param name="checkedListBox">The CheckedListBox to populate.</param>
        public void LoadGenres(CheckedListBox checkedListBox)
        {
            try
            {
                // Check Genres table existence
                int tableExists;
                ADOSupport.GetScalerValue(out tableExists, "SELECT COUNT(*) FROM sys.tables WHERE name = 'Genre'");
                if (tableExists == 0)
                {
                    DBLogger.LogError("Genre table does not exist.");
                    throw new InvalidOperationException("Genre table does not exist.");
                }

                // Fetch DataTable
                string sql = "SELECT Id, Name FROM [dbo].[Genre] ORDER BY Name";
                DataTable dt = ADOSupport.GetDataTable(sql);
                DBLogger.LogError($"Loading genres: Found {dt.Rows.Count} records.");

                // Populate CheckedListBox
                checkedListBox.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    checkedListBox.Items.Add(new ComboItem((int)row["Id"], row["Name"].ToString()), false);
                }
                DBLogger.LogError($"Genres CheckedListBox populated with {checkedListBox.Items.Count} items.");
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"Error loading genres: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Loads tags into a CheckedListBox from the VideoTag table for the specified group.
        /// </summary>
        /// <param name="checkedListBox">The CheckedListBox to populate.</param>
        /// <param name="groupId">The group ID for tags (e.g., 1 or 2).</param>
        public void LoadTags(CheckedListBox checkedListBox, int groupId)
        {
            bool isChecked = false;
            try
            {
                // Check VideoTag table existence
                int tableExists;
                ADOSupport.GetScalerValue(out tableExists, "SELECT COUNT(*) FROM sys.tables WHERE name = 'VideoTag'");
                if (tableExists == 0)
                {
                    DBLogger.LogError("VideoTag table does not exist.");
                    throw new InvalidOperationException("VideoTag table does not exist.");
                }

                // Fetch DataTable
                string sql = "SELECT * FROM [dbo].[VideoTag] WHERE GroupId = <GroupId> ORDER BY Name";
                string errorCode = "";
                ADOSupport.ExpandAmericanSQLStatement(ref errorCode, ref sql, "<GroupId>", groupId);
                if (!string.IsNullOrEmpty(errorCode))
                {
                    DBLogger.LogError($"LoadTags failed: Error expanding GroupId {groupId} - {errorCode}");
                    throw new InvalidOperationException($"Error expanding SQL: {errorCode}");
                }

                DataTable dt = ADOSupport.GetDataTable(sql);
                DBLogger.LogError($"Loading tags for GroupId {groupId}: Found {dt.Rows.Count} records.");

                // Populate CheckedListBox
                checkedListBox.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    var comboItem = new ComboItem((int)row["Id"], row["Name"].ToString());
                    isChecked = row[3].ToBoolean() || comboItem.Id == 14; // Check Default flag or Requires Review
                    checkedListBox.Items.Add(comboItem, isChecked);
                }
                DBLogger.LogError($"Tags CheckedListBox populated with {checkedListBox.Items.Count} items for GroupId {groupId}.");
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"Error loading tags for GroupId {groupId}: {ex.Message}");
                throw;
            }
        }
    }

    internal class ComboItem
    {
        public int Id { get; }
        public string Name { get; }

        public ComboItem(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString() => Name;
    }
}