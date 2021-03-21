using System;
using System.Collections.Generic;
using System.Text;

/**
 * Author: Haran
 * Date: 2021-02-08
 */
namespace TarneebClasses.Logging
{
    /// <summary>
    /// A log representing a bid being placed.
    /// </summary>
    public class BidPlacedLog : ILog
    {
        public Type Type => Type.BID_PLACED;

        public DateTime DateTime => DateTime.Now;

        /// <summary>
        /// The bid that was played.
        /// </summary>
        public int Bid;

        /// <summary>
        /// The player that placed the bid.
        /// </summary>
        public Player Player;

        /// <summary>
        /// Get this log as a string.
        /// </summary>
        /// <returns>The string representation of the log.</returns>
        public override string ToString()
        {
            // TODO
            return $"{this.Player.PlayerName} placed a bid of {this.Bid}.";
        }
    }
}
