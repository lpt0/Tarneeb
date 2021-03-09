using System;
using System.Collections.Generic;
using System.Text;

/**
 * @author: Haran
 * @date: 2021-03-08
 */

namespace TarneebClasses
{
    /// <summary>
    /// A computer-controlled player.
    /// </summary>
    public class CPUPlayer : Player
    {
        #region Constructor
        /// <summary>
        /// Create a new CPU player.
        /// </summary>
        /// <param name="playerName">The name of the player.</param>
        /// <param name="playerId">The player's ID.</param>
        /// <param name="teamNumber">The player's team number.</param>
        /// <param name="handList">The player's hand.</param>
        public CPUPlayer(Game game, String playerName, int playerId, Enums.Team teamNumber, Deck handList) 
            : base(playerName, playerId, teamNumber, handList)
        {
            // Setup the event listener
            game.PlayerTurnEvent += OnPlayerTurn;
        }
        #endregion

        #region Event handlers
        public void OnPlayerTurn(object sender, Events.PlayerTurnEventArgs args)
        {
            // Sender is the Game object; logs can be read from there
            // Is it this player's turn?
            if (args.Player == this)
            {
                switch (args.State)
                {
                    case Game.State.BID_STAGE:
                        // Perform bid logic
                        // For now, CPU players will pass
                        this.PerformAction(new Events.PlayerActionEventArgs() { Bid = -1 });
                        break;
                    case Game.State.BID_WON:
                        // Perform Tarneeb logic
                        // noop
                        break;
                    case Game.State.TRICK:
                        // Perform trick logic
                        // For now, just draw a random card
                        int cardIdx = new Random().Next(this.HandList.Cards.Count);
                        Card card = this.HandList.Pick(cardIdx);
                        this.PerformAction(new Events.PlayerActionEventArgs() { CardPlayed = card });
                        break;
                    default:
                        throw new Exception("Unknown state!");
                        break;
                }
            }
        }
        #endregion
    }
}
