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
        /// <summary>
        /// Reference to the current game.
        /// </summary>
        private Game _game;

        #region Constructor
        /// <summary>
        /// Create a new CPU player.
        /// </summary>
        /// <param name="game">The game to use to listen for events.</param>
        /// <param name="playerName">The name of the player.</param>
        /// <param name="playerId">The player's ID.</param>
        /// <param name="teamNumber">The player's team number.</param>
        /// <param name="handList">The player's hand.</param>
        public CPUPlayer(Game game, String playerName, int playerId, Enums.Team teamNumber, Deck handList) 
            : base(playerName, playerId, teamNumber, handList)
        {
            this._game = game;
            // Setup the event listener
            this._game.PlayerTurnEvent += OnPlayerTurn;
        }
        #endregion

        #region Event handlers
        /// <summary>
        /// Handles logic for the player's turn.
        /// </summary>
        /// <param name="sender">The game.</param>
        /// <param name="args">Event arguments. Only contains the Player object.</param>
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
                        // Perform Tarneeb logic; doesn't happen for this CPU player
                        // noop
                        break;
                    case Game.State.TRICK:
                        // Perform trick logic
                        // For now, just draw a random card
                        List<Card> validCards = this._game.GetValidCards(this);
                        int cardIdx = new Random().Next(validCards.Count);
                        Card card = validCards[cardIdx];
                        this.PerformAction(new Events.PlayerActionEventArgs() { CardPlayed = card });
                        break;
                    default:
                        throw new Exception("Unknown state!");
                }
            }
        }
        #endregion
    }
}
