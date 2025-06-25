using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CSGeneral_x86;

namespace VideoPlaylist
{


    /// <summary>
    /// Manages actor-related operations, including searching and UI population.
    /// </summary>
    public class ActorManager
    {
        private clsADOSupport _adoSupport;

        /// <summary>
        /// Gets or sets the ADOSupport instance for database operations.
        /// </summary>
        public clsADOSupport ADOSupport
        {
            get
            {
                if (_adoSupport == null)
                {
                    _adoSupport = clsADOSupport.LocalDB;
                }
                return _adoSupport;
            }
            set
            {
                _adoSupport = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the ActorManager class.
        /// </summary>
        public ActorManager()
        {
        }

        /// <summary>
        /// Finds actors matching the provided word tokens in FullName, Forename, or Surname.
        /// </summary>
        /// <param name="tokens">The list of word tokens to match.</param>
        /// <returns>A list of matching ActorData objects, sorted by MatchCount descending.</returns>
        public List<ActorData> FindMatchingEntries(List<string> tokens)
        {
            if (tokens == null || tokens.Count == 0)
            {
                return new List<ActorData>();
            }

            try
            {
                var actors = new List<ActorData>();
                string sql = "SELECT Id, FullName, Forename, Surname, Include FROM Actor";
                ESqlCommand cmd = ADOSupport.GetESQLCommandQuery();
                cmd.CommandText = sql;
                DataTable result = ADOSupport.GetDataTable(cmd);

                foreach (DataRow row in result.Rows)
                {
                    int matchCount = 0;
                    string fullName = row["FullName"].ToString().ToLower();
                    string forename = row["Forename"]?.ToString().Trim().ToLower() ?? "";
                    string surname = row["Surname"]?.ToString().Trim().ToLower() ?? "";

                    foreach (string token in tokens.Select(t => t.ToLower()))
                    {
                        if (fullName.Contains(token))
                        {
                            matchCount++;
                        }
                        if (!string.IsNullOrEmpty(forename) && forename.Contains(token))
                        {
                            matchCount++;
                        }
                        if (!string.IsNullOrEmpty(surname) && surname == token)
                        {
                            matchCount++;
                        }
                    }

                    for (int i = 1; i < tokens.Count; i++)
                    {
                        if (tokens[i].ToLower() == surname && !string.IsNullOrEmpty(forename) &&
                            tokens[i - 1].ToLower().Contains(forename))
                        {
                            matchCount++;
                            break;
                        }
                    }

                    if (matchCount > 0)
                    {
                        actors.Add(new ActorData
                        {
                            Id = Convert.ToInt32(row["Id"]),
                            FullName = row["FullName"].ToString(),
                            Forename = row["Forename"]?.ToString().Trim(),
                            Surname = row["Surname"]?.ToString().Trim(),
                            Include = Convert.ToBoolean(row["Include"]),
                            MatchCount = matchCount
                        });
                    }
                }

                return actors.OrderByDescending(a => a.MatchCount).ToList();
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"FindMatchingEntries failed: {ex.Message}");
                return new List<ActorData>();
            }
        }

        /// <summary>
        /// Populates a CheckedListBox with actors from FindMatchingEntries.
        /// </summary>
        /// <param name="tokens">The list of word tokens to match.</param>
        /// <param name="checkListBox">The CheckedListBox to populate.</param>
        /// <param name="maxMatches">The maximum number of matches to display.</param>
        public void PopulateChecklistBox(List<string> tokens, CheckedListBox checkListBox, int maxMatches)
        {
            if (checkListBox == null)
            {
                DBLogger.LogError("PopulateChecklistBox failed: CheckListBox is null.");
                return;
            }

            try
            {
                checkListBox.Items.Clear();
                List<ActorData> actors = FindMatchingEntries(tokens);
                if (actors.Count > 0)
                {
                    checkListBox.ValueMember = "Id";
                    checkListBox.DisplayMember = "FullName";
                    foreach (ActorData actor in actors)
                    {
                        if ( actor.MatchCount >= maxMatches )
                        checkListBox.Items.Add(actor, true);
                    }
                }
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"PopulateChecklistBox failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Splits a name into tokens using common delimiters, removing empty entries.
        /// </summary>
        /// <param name="name">The name to split.</param>
        /// <returns>A list of non-empty tokens.</returns>
        public List<string> SplitNameIntoTokens(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return new List<string>();
            }

            try
            {
                return System.Text.RegularExpressions.Regex.Split(name.Trim(), @"[\s,.-;:_]+")
                    .Where(p => !string.IsNullOrEmpty(p))
                    .ToList();
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"SplitNameIntoTokens failed: {ex.Message}");
                return new List<string>();
            }
        }

        /// <summary>
        /// Inserts an actor with the provided full name, splitting into Forename and Surname, and sets Include to true.
        /// </summary>
        /// <param name="fullName">The full name of the actor.</param>
        /// <returns>The ID of the inserted actor, or 0 if the operation fails.</returns>
        public int InsertQuickActor(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                DBLogger.LogError("InsertQuickActor failed: FullName cannot be empty.");
                return 0;
            }

            try
            {
                var parts = SplitNameIntoTokens(fullName);
                string forename = parts.Count > 0 ? parts[0] : "";
                string surname = parts.Count > 1 ? parts[parts.Count - 1] : "";

                ESqlCommand cmd = ADOSupport.GetESQLCommandQuery();
                cmd.CommandText = "INSERT INTO Actor (FullName, Forename, Surname, Include) OUTPUT INSERTED.Id VALUES (@FullName, @Forename, @Surname, @Include)";
                cmd.AddParameterWithValue("@FullName", fullName);
                cmd.AddParameterWithAnyValue("@Forename", forename);
                cmd.AddParameterWithAnyValue("@Surname", surname);
                cmd.AddParameterWithValue("@Include", true);

                DataTable result = ADOSupport.GetDataTable(cmd);
                if (result.Rows.Count > 0)
                {
                    return Convert.ToInt32(result.Rows[0]["Id"]);
                }

                DBLogger.LogError("InsertQuickActor failed: No ID returned.");
                return 0;
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"InsertQuickActor failed: {ex.Message}");
                return 0;
            }
        }

        public List<string> TokeniseFilename(string fname)

        {

            try
            {
                return System.Text.RegularExpressions.Regex.Split(fname.Trim(), @"[\s,.-;:_]+")
                    .Where(p => !string.IsNullOrEmpty(p))
                    .ToList();
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"SplitNameIntoTokens failed: {ex.Message}");
                return new List<string>();
            }
        }

    }
}
