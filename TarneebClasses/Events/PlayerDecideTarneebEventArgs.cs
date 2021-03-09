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
    /// Event arguments for whenever a player decides the Tarneeb suit.
    /// Used for notifying the game of the trump suit.
    /// </summary>
    /// <see cref="EventArgs" />
    public class PlayerDecideTarneebEventArgs : EventArgs
    {
        /// <summary>
        /// The player that decided the Tarneeb (trump) suit.
        /// </summary>
        public Player Player { get; set; }

        /// <summary>
        /// The suit that the player decided on.
        /// </summary>
        public Enums.CardSuit Suit { get; set; }
    }
}
