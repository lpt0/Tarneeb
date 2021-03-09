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
    /// Event arguments for whenever it is a player's turn to bid.
    /// </summary>
    /// <see cref="EventArgs" />
    public class PlayerBidTurnEventArgs : EventArgs
    {
        /// <summary>
        /// The player that is currently playing. TODO reword
        /// </summary>
        public Player Player { get; set; }
    }
}
