using System;
using System.Collections.Generic;
using System.Text;

/**
 * @author: Haran
 * @date: 2021-03-08
 */
namespace TarneebClasses.Events
{
    /// <summary>
    /// Event arguments for a player action, such as a player playing a card
    /// or placing a bid.
    /// </summary>
    /// <see cref="EventArgs" />
    public class PlayerActionEventArgs : EventArgs
    {
        /// <summary>
        /// The card that was played.
        /// </summary>
        public Card CardPlayed { get; set; }

        /// <summary>
        /// The bid that was placed.
        /// </summary>
        public int Bid { get; set; }

        /// <summary>
        /// The Tarneeb suit that was decided.
        /// </summary>
        public Enums.CardSuit Tarneeb { get; set; }
    }
}
