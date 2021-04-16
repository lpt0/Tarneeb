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
    /// Event arguments for when it is a player's turn.
    /// </summary>
    /// <see cref="EventArgs" />
    public class PlayerTurnEventArgs : EventArgs
    {
        /// <summary>
        /// The player who's turn it is.
        /// </summary>
        public Player Player { get; set; }

        /// <summary>
        /// The current stage for this turn, such as bid or trick.
        /// </summary>
        public Game.State State { get; set; }
    }
}
