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
    /// Event arguments for whenever a bid is finished.
    /// </summary>
    /// <see cref="EventArgs" />
    public class BidFinishedEventArgs : EventArgs
    {
        /// <summary>
        /// TODO
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// The team that won the bid.
        /// </summary>
        public Enums.Team WinningTeam { get; set; }

        /// <summary>
        /// The team that lost the bid.
        /// </summary>
        public Enums.Team LosingTeam { get; set; }
    }
}
