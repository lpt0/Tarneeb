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
    /// Event arguments for when a game action has taken place.
    /// TODO: Could be renamed to "GameStateChange"
    /// </summary>
    /// <see cref="EventArgs" />
    public class GameActionEventArgs : EventArgs
    {
        /// <summary>
        /// The state at the time that the event was raised.
        /// </summary>
        public Game.State State { get; set; }

        // All other arguments depend on the state above
        /// <summary>
        /// The player that performed the action.
        /// </summary>
        public Player Player { get; set; }

        /// <summary>
        /// The card that was played, if a card was played.
        /// </summary>
        public Card Card { get; set; }

        /// <summary>
        /// The number of currently played in the trick/round.
        /// </summary>
        public int CardsPlayedInRound { get; set; }

        /// <summary>
        /// The bid that was placed.
        /// </summary>
        public int Bid { get; set; }

        /// <summary>
        /// The Tarneeb suit that was decided on.
        /// </summary>
        public Enums.CardSuit Tarneeb { get; set; }

        /// <summary>
        /// The score to add/remove, when a bid has been won/lost.
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

        /// <summary>
        /// The total scores for each team.
        /// </summary>
        public int[] TeamScores { get; set; }

        /// <summary>
        /// The scores for each team during a round/trick.
        /// </summary>
        public int[] BidScores { get; set; }
    }
}
