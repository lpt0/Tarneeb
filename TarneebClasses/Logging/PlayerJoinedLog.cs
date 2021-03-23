using System;
using System.Collections.Generic;
using System.Text;

/**
 * Author: Haran
 * Date: 2021-03-08
 */
namespace TarneebClasses.Logging
{
    /// <summary>
    /// A log representing a player joining.
    /// </summary>
    public class PlayerJoinedLog : ILog
    {
        public Type Type => Type.PLAYER_JOINED;

        public DateTime DateTime => DateTime.Now;

        /// <summary>
        /// The player that joined.
        /// </summary>
        public Player Player;

        public override string ToString()
        {
            return $"{this.Player.PlayerName} joined.";
        }
    }
}
