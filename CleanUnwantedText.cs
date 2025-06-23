using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using  CSGeneral_x86;

namespace VideoPlaylist
{
    public class CleanUnwantedText
    {
        private static HashSet<string> _unwantedWords;
        private clsADOSupport _adoSupport;

        public clsADOSupport ADOSupport
        {
            get => _adoSupport ?? (_adoSupport = clsADOSupport.LocalDB);
            set => _adoSupport = value;
        }

        public CleanUnwantedText()
        {
            
        }

        private void InitializeUnwantedWords()
        {
            if (_unwantedWords != null) return;

            try
            {
                _unwantedWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                string query = "SELECT MatchingWord FROM UnwantedWord";
                DataTable table = ADOSupport.GetDataTable(query);
                foreach (DataRow row in table.Rows)
                {
                    string word = row["MatchingWord"]?.ToString();
                    if (!string.IsNullOrEmpty(word))
                    {
                        _unwantedWords.Add(word);
                    }
                }
            }
            catch (Exception ex)
            {
                DBLogger.LogError($"Failed to load UnwantedWords: {ex.Message}");
                throw new InvalidOperationException("Unable to initialize unwanted words from database.", ex);
            }
        }

        public string CleanText(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            InitializeUnwantedWords();

            // Tokenize on whitespace and punctuation
            string[] tokens = Regex.Split(input.Trim(), @"\s+|[,\.;:?!()-]");
            List<string> cleanedTokens = tokens
                .Where(token => !string.IsNullOrEmpty(token) && !_unwantedWords.Contains(token))
                .ToList();

            // Recombine with single space
            return string.Join(" ", cleanedTokens).Trim();
        }
    }
}