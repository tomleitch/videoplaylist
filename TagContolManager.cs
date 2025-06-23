using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CSGeneral_x86;

namespace VideoPlaylist
{
    /// <summary>
    /// Manages operations for a CheckedListBox control containing video tags.
    /// </summary>
    public class TagControlManager
    {
        private CheckedListBox _control;
        private clsADOSupport _adoSupport;
        public ComboLoader _loader;

        /// <summary>
        /// Gets or sets the CheckedListBox control to manage.
        /// </summary>
        /// <value>The CheckedListBox control.</value>
        /// <exception cref="ArgumentNullException">Thrown when value is null.</exception>
        public CheckedListBox Control
        {
            get
            {
                return _control;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                _control = value;
            }
        }

        /// <summary>
        /// Gets or sets the ADOSupport instance for database operations.
        /// </summary>
        /// <value>The ADOSupport instance.</value>
        public clsADOSupport ADOSupport
        {
            get
            {
                return _adoSupport;
            }
            set
            {
                _adoSupport = value;
            }
        }

        /// <summary>
        /// Gets or sets the ComboLoader instance for loading tags.
        /// </summary>
        /// <value>The ComboLoader instance.</value>
        public ComboLoader Loader
        {
            get
            {
                if (_loader == null && _adoSupport != null)
                {
                    _loader = new ComboLoader(_adoSupport);
                }
                return _loader;
            }
            set
            {
                _loader = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the TagControlManager class.
        /// </summary>
        public TagControlManager()
        {
        }

        /// <summary>
        /// Loads tags into the managed CheckedListBox for the specified group.
        /// </summary>
        /// <param name="groupId">The group ID for the tags.</param>
        public void LoadTags(int groupId)
        {
            if (_control == null)
            {
                DBLogger.LogError("LoadTags failed: Control is not set.");
                return;
            }

            try
            {
                if (Loader == null)
                {
                    DBLogger.LogError("LoadTags failed: Loader is not initialized.");
                    return;
                }
                Loader.LoadTags(_control, groupId);
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"LoadTags failed for GroupId {groupId}: {ex.Message}");
            }
        }

        /// <summary>
        /// Sets the default checked states for tags based on the VideoTag.Default column.
        /// </summary>
        public void SetDefaultCheckedStates()
        {
            if (_control == null || _adoSupport == null)
            {
                DBLogger.LogError("SetDefaultCheckedStates failed: Control or ADOSupport is not set.");
                return;
            }

            try
            {
                foreach (ComboItem item in _control.Items.Cast<ComboItem>())
                {
                    ESqlCommand cmd = ADOSupport.GetESQLCommandQuery();
                    cmd.CommandText = "SELECT [Default] FROM VideoTag WHERE Id = @Id";
                    cmd.AddParameterWithValue("@Id", item.Id);
                    DataTable result = ADOSupport.GetDataTable(cmd);
                    if (result.Rows.Count > 0 && Convert.ToInt32(result.Rows[0]["Default"]) == 1)
                    {
                        _control.SetItemChecked(_control.Items.IndexOf(item), true);
                    }
                }
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"SetDefaultCheckedStates failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Sets the "Requires Review" tag (Id=14) to checked if present in the control.
        /// </summary>
        public void SetRequiresReviewTag()
        {
            if (_control == null)
            {
                DBLogger.LogError("SetRequiresReviewTag failed: Control is not set.");
                return;
            }

            try
            {
                foreach (ComboItem item in _control.Items.Cast<ComboItem>())
                {
                    if (item.Id == 14)
                    {
                        _control.SetItemChecked(_control.Items.IndexOf(item), true);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"SetRequiresReviewTag failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Combines selected tag IDs from two CheckedListBox controls.
        /// </summary>
        /// <param name="control1">The first CheckedListBox control.</param>
        /// <param name="control2">The second CheckedListBox control.</param>
        /// <returns>A list of unique selected tag IDs.</returns>
        /// <exception cref="ArgumentNullException">Thrown when control1 or control2 is null.</exception>
        public static List<int> GetCombinedSelectedTags(CheckedListBox control1, CheckedListBox control2)
        {
            if (control1 == null)
            {
                throw new ArgumentNullException(nameof(control1));
            }
            if (control2 == null)
            {
                throw new ArgumentNullException(nameof(control2));
            }

            try
            {
                return control1.CheckedItems.Cast<ComboItem>()
                    .Concat(control2.CheckedItems.Cast<ComboItem>())
                    .Select(item => item.Id)
                    .Distinct()
                    .ToList();
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"GetCombinedSelectedTags failed: {ex.Message}");
                return new List<int>();
            }
        }
    }
}