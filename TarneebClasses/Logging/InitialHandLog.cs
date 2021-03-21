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
    /// A log representing a player's initial hand.
    /// </summary>
    public class InitialHandLog : ILog
    {
        public Type Type => Type.INITIAL_HAND;

        public DateTime DateTime => DateTime.Now;

        /// <summary>
        /// The initial hand.
        /// </summary>
        public Deck Hand;

        /// <summary>
        /// The corresponding player.
        /// </summary>
        public Player Player;

        /// <summary>
        /// Get this log as a string.
        /// </summary>
        /// <returns>The string representation of the log.</returns>
        public override string ToString()
        {
            return $"{this.Player.PlayerName}'s initial hand is {this.Hand}.";
        }
    }
}
