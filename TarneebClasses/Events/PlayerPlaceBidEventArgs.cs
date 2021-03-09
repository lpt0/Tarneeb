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
    /// Event arguments for whenever a player places a bid.
    /// Used for notifying the game what bid a player placed.
    /// </summary>
    /// <see cref="EventArgs" />
    public class PlayerPlaceBidEventArgs : EventArgs
    {
        /// <summary>
        /// The player that placed the bid.
        /// TODO: Player should be sender object
        /// </summary>
        public Player Player { get; set; }

        /// <summary>
        /// The bid placed.
        /// TODO
        /// </summary>
        public int Bid { get; set; }
    }
}
