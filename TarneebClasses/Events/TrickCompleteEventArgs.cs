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
    /// Event arguments for whenever a trick is completed.
    /// </summary>
    /// <see cref="EventArgs" />
    public class TrickCompleteEventArgs : EventArgs
    {
        /// <summary>
        /// The cards played in the trick.
        /// </summary>
        public Card[] CardsPlayed { get; set; }

        /// <summary>
        /// The winner of the trick.
        /// </summary>
        public Player Winner { get; set; }
    }
}
