using System;
using System.Collections.Generic;
using System.Text;

/**
 * Author: Haran
 * Date: 2021-03-16
 */
namespace TarneebClasses.Logging
{
    /// <summary>
    /// A log representing a bid being placed.
    /// </summary>
    public class BidCompleteLog : ILog
    {
        public Type Type => Type.BID_PLACED;

        public DateTime DateTime => DateTime.Now;

        /// <summary>
        /// The winning team.
        /// </summary>
        public Enums.Team WinningTeam;

        /// <summary>
        /// The losing team.
        /// </summary>
        public Enums.Team LosingTeam;

        /// <summary>
        /// The score.
        /// </summary>
        public int Score;

        /// <summary>
        /// Get this log as a string.
        /// </summary>
        /// <returns>The string representation of the log.</returns>
        public override string ToString()
        {
            // TODO
            return $"{this.WinningTeam} won with a score of {this.Score}.";
        }
    }
}
