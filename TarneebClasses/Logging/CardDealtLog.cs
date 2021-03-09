using System;
using System.Collections.Generic;
using System.Text;

/**
 * Author: Haran
 * Date: 2021-03-08
 */
namespace TarneebClasses.Logging
{
    /// <summary>
    /// A log representing a card being dealt to a player.
    /// </summary>
    public class CardDealtLog : ILog
    {
        public Type Type => Type.CARD_DEALT;

        public DateTime DateTime => DateTime.Now;

        /// <summary>
        /// The card that was dealt.
        /// </summary>
        public Card Card;

        /// <summary>
        /// The player that received the card.
        /// </summary>
        public Player Player;

        /// <summary>
        /// Get this log as a string.
        /// </summary>
        /// <returns>The string representation of the log.</returns>
        public override string ToString()
        {
            return $"[{this.DateTime}] {this.Player.PlayerName} was dealt a {this.Card}.";
        }
    }
}
