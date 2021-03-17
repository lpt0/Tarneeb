using System;
using System.Collections.Generic;
using System.Text;

/**
 * Author: Haran
 * Date: 2021-02-16
 */
namespace TarneebClasses.Logging
{
    /// <summary>
    /// A log representing a bid being placed.
    /// </summary>
    public class TarneebSuitLog : ILog
    {
        public Type Type => Type.TARNEEB_SUIT;

        public DateTime DateTime => DateTime.Now;

        /// <summary>
        /// The tarneeb suit.
        /// </summary>
        public Enums.CardSuit Suit;

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
            return $"[{this.DateTime}] {this.Player.PlayerName} decided on the suit {this.Suit}.";
        }
    }
}
