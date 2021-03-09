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
    /// Event arguments for whenever a new player joins the game.
    /// </summary>
    /// <see cref="EventArgs" />
    public class NewPlayerEventArgs : EventArgs
    {
        /// <summary>
        /// The player that joined.
        /// </summary>
        public Player Player { get; set; }
    }
}
