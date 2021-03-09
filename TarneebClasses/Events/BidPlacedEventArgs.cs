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
    /// </summary>
    /// <see cref="EventArgs" />
    public class BidPlacedEventArgs : EventArgs
    {
        /// <summary>
        /// The player who placed the bid.
        /// </summary>
        public Player Player { get; set; }

        /// <summary>
        /// The bid that was placed. TODO
        /// </summary>
        public Bid Bid { get; set; }
    }
}
