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
    /// A log representing a trick being completed, and won.
    /// </summary>
    public class TrickCompletedLog : ILog
    {
        public Type Type => Type.CARD_PLAYED;

        public DateTime DateTime => DateTime.Now;

        /// <summary>
        /// The player that won the trick.
        /// </summary>
        public Player Player;

        /// <summary>
        /// Get this log as a string.
        /// </summary>
        /// <returns>The string representation of the log.</returns>
        public override string ToString()
        {
            return $"{this.Player.PlayerName} won the trick.";
        }
    }
}
