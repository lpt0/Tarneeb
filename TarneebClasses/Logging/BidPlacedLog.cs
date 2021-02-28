using System;
using System.Collections.Generic;
using System.Text;

/**
 * Author: Haran
 * Date: 2021-02-08
 */
namespace TarneebClasses.Logging
{
    /// <summary>
    /// A log representing a bid being placed.
    /// </summary>
    public class BidPlacedLog : ILog
    {
        public Type Type => Type.NEW_GAME;

        public DateTime DateTime => DateTime.Now;

        /// <summary>
        /// The bid that was played.
        /// </summary>
        public Bid Card;

        /// <summary>
        /// The player that placed the bid.
        /// </summary>
        public Player Player;

        public override string ToString()
        {
            // TODO
            return $"[{this.DateTime}] {this.Player.PlayerName} placed a bid of TODO.";
        }
    }
}
