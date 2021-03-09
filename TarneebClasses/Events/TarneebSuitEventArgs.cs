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
    /// Event arguments for whenever a Tarneeb suit is decided upon.
    /// </summary>
    /// <see cref="EventArgs" />
    public class TarneebSuitEventArgs : EventArgs
    {
        /// <summary>
        /// The player who decided the suit.
        /// </summary>
        public Player Player { get; set; }

        /// <summary>
        /// The suit that was decided on.
        /// </summary>
        public Enums.CardSuit Suit { get; set; }
    }
}
