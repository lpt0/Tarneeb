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
    /// Event arguments for whenever a player plays a card.
    /// Used for notifying the game what card a player was played.
    /// </summary>
    /// <see cref="EventArgs" />
    public class PlayerPlayCardEventArgs : EventArgs
    {
        /// <summary>
        /// The player that played the card.
        /// </summary>
        public Player Player { get; set; }

        /// <summary>
        /// The card that they played.
        /// </summary>
        public Card Card { get; set; }
    }
}
