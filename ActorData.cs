using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoPlaylist
{
    /// <summary>
    /// Represents an actor record with match count for token-based searches.
    /// </summary>
    public class ActorData
    {
        /// <summary>
        /// Gets or sets the actor's unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the actor's full name.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the actor's forename.
        /// </summary>
        public string Forename { get; set; }

        /// <summary>
        /// Gets or sets the actor's surname.
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the actor is included.
        /// </summary>
        public bool Include { get; set; }

        /// <summary>
        /// Gets or sets the number of matches for search tokens.
        /// </summary>
        public int MatchCount { get; set; }
    }

}

