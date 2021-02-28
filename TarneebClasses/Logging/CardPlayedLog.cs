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
        public Type Type => Type.NEW_GAME;

        public DateTime DateTime => DateTime.Now;

        /// <summary>
        /// The card that was played.
        /// </summary>
        public Card Card;

        /// <summary>
        /// The player that played the card.
        /// </summary>
        public Player Player;

        public override string ToString()
        {
            return $"[{this.DateTime}] {this.Player.PlayerName} played {this.Card}.";
        }
    }
}
