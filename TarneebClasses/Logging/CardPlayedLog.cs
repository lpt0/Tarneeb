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
    /// A log representing a card being played.
    /// </summary>
    public class CardPlayedLog : ILog
    {
        public Type Type => Type.CARD_PLAYED;

        public DateTime DateTime => DateTime.Now;

        /// <summary>
        /// The card that was played.
        /// </summary>
        public Card Card;

        /// <summary>
        /// The player that played the card.
        /// </summary>
        public Player Player;

        /// <summary>
        /// Get this log as a string.
        /// </summary>
        /// <returns>The string representation of the log.</returns>
        public override string ToString()
        {
            return $"{this.Player.PlayerName} played {this.Card}.";
        }
    }
}
