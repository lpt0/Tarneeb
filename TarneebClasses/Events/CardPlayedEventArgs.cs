using System;
using System.Collections.Generic;
using System.Text;

/**
 * @author: Haran
 * @date: 2021-02-28
 */
namespace TarneebClasses.Events
{
    /// <summary>
    /// Event arguments for whenever a card is played.
    /// Used for notifying listeners that a card has been played.
    /// </summary>
    /// <see cref="EventArgs" />
    public class CardPlayedEventArgs : EventArgs
    {
        /// <summary>
        /// The player that played a card.
        /// </summary>
        public Player Player { get; set; }

        /// <summary>
        /// The card that was played.
        /// </summary>
        public Card Card { get; set; }
    }
}
