﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using TarneebClasses.Events;
using System.Security.Cryptography;

/**
 * @author  Andrew Kuo, Hoang Quoc Bao Nguyen
 * @date    2021-04-04
 */

namespace TarneebClasses
{
    /// <summary>
    /// CPUPlayerMedium class
    /// </summary>
    class CPUPlayerMedium : CPUPlayer
    {
        #region AIMemory

        /// <summary>
        /// Records the highest bid in memory.
        /// </summary>
        protected int highestBid;

        /// <summary>
        /// Records whether or not this player's team is the bid winning.
        /// </summary>
        protected bool isHighestBidTeamMine;

        /// <summary>
        /// Winning card of the trick in memory.
        /// </summary>
        protected Card winningCard;

        /// <summary>
        /// Winning Card of the trick belongs to this player's team.
        /// </summary>
        protected bool isWinningCardTeamMine;

        /// <summary>
        /// The suit of the current trick being played.
        /// </summary>
        protected Enums.CardSuit trickSuit;

        /// <summary>
        /// The tarneeb suit of the game.
        /// </summary>
        protected Enums.CardSuit tarneebSuit;

        /// <summary>
        /// Seed for Random number generator associated with the AI player.
        /// </summary>
        protected Random personalitySeed;

        /// <summary>
        /// AI Personality constants used for decision making.
        /// </summary>
        protected const int BID_PASS = -1;

        #endregion

        /// <summary>
        /// The game that this CPU player is a part of.
        /// </summary>
        private readonly Game game;

        #region Constructor
        /// <summary>
        /// Create a new Medium CPU player.
        /// </summary>
        /// <param name="playerName">The name of the player.</param>
        /// <param name="playerId">The player's ID.</param>
        /// <param name="teamNumber">The player's team number.</param>
        /// <param name="handList">The player's hand.</param>
        public CPUPlayerMedium(Game game, String playerName, int playerId, Enums.Team teamNumber, Deck handList)
            : base(game, playerName, playerId, teamNumber, handList)
        {
            // Store reference to the game, for getting valid cards in a round
            this.game = game;

            // Subscribe to events.
            game.GameActionEvent += OnGameActionEvent;
            game.PlayerTurnEvent -= base.OnPlayerTurn;
            game.PlayerTurnEvent += OnPlayerTurn;

            // Create new personality for this CPU Player based of there name so they are consistent.
            personalitySeed = new Random(
                BitConverter.ToInt32(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(playerName)), 0)
            );

            // Reset the memory;
            highestBid = 0;
            isHighestBidTeamMine = false;
            winningCard = null;
            isWinningCardTeamMine = false;
            tarneebSuit = Enums.CardSuit.Heart;
        }
        #endregion

        #region Event handlers
        /// <summary>
        /// Handles GameActionEvent raised by game and updates the player's memory.
        /// </summary>
        /// <param name="sender">Game</param>
        /// <param name="args">Information about the game.</param>
        public void OnGameActionEvent(object sender, Events.GameActionEventArgs args)
        {
            var game = sender as Game;

            // Determine action based on Game's current state.
            switch (args.State)
            {
                case Game.State.NEW_GAME:
                    highestBid = 0;
                    isHighestBidTeamMine = false;
                    winningCard = null;
                    isWinningCardTeamMine = false;
                    tarneebSuit = Enums.CardSuit.Heart;
                    break;

                case Game.State.BID_STAGE:

                    // Remember the highest bid and if it was our team.
                    if (args.Bid > highestBid)
                    {
                        highestBid = args.Bid;
                        isHighestBidTeamMine = args.Player.TeamNumber == this.TeamNumber;
                    }
                    break;

                case Game.State.BID_COMPLETE:
                    // Once bid is complete, reset the highest.
                    highestBid = 0;
                    isHighestBidTeamMine = false;
                    break;

                case Game.State.TARNEEB_SUIT:
                    // Record the TarneebSuit.
                    tarneebSuit = args.Tarneeb;
                    break;

                case Game.State.TRICK_COMPLETE:
                    // Trick has been won by someone, reset the memory and position counter.
                    // Clear the cards in memory.
                    isWinningCardTeamMine = false;
                    winningCard = null;
                    trickSuit = Enums.CardSuit.Heart;
                    break;

                case Game.State.TRICK:

                    // No previous card has been submitted, save the card and team who is winning.
                    if (winningCard == null)
                    {
                        winningCard = args.Card;
                        trickSuit = winningCard.Suit;
                        isWinningCardTeamMine = args.Player.TeamNumber == this.TeamNumber;
                    }
                    // If played card is of low suit in memory, compare the number.
                    else if (args.Card.Suit == trickSuit)
                    {
                        if (args.Card.Number > winningCard.Number)
                        {
                            winningCard = args.Card;
                            trickSuit = winningCard.Suit;
                            isWinningCardTeamMine = args.Player.TeamNumber == this.TeamNumber;
                        }
                    }
                    // If played Card is of tarneeb but winning card is not, then played card is the winning card.
                    else if (args.Card.Suit == args.Tarneeb && winningCard.Suit != args.Tarneeb)
                    {
                        winningCard = args.Card;
                        trickSuit = winningCard.Suit;
                        isWinningCardTeamMine = args.Player.TeamNumber == this.TeamNumber;
                    }

                    break;

            }
        }


        /// <summary>
        /// On Player event handler.
        /// </summary>
        /// <param name="sender">Game raising event</param>
        /// <param name="args">Event parameters</param>
        public new void OnPlayerTurn(object sender, Events.PlayerTurnEventArgs args)
        {
            // Sender is the Game object; logs can be read from there
            // Is it this player's turn?
            if (args.Player == this)
            {
                // Retrieve the current game state.
                var game = sender as Game;

                switch (args.State)
                {
                    case Game.State.BID_STAGE:
                        // Perform bid logic
                        //var previousBid = game.Logs.Where(log => log.Type == Logging.Type.BID_PLACED).ToList();

                        this.PerformAction(new Events.PlayerActionEventArgs() { Bid = getAiBid(highestBid) });


                        // For now, CPU players will pass
                        //this.PerformAction(new Events.PlayerActionEventArgs() { Bid = -1 });
                        break;
                    case Game.State.BID_WON:
                        // Perform Tarneeb logic
                        // noop
                        break;
                    case Game.State.TRICK:
                        // Perform trick logic

                        //var previousPlayed = game.Logs.Where(log => log.Type == Logging.Type.CARD_PLAYED).ToList();

                        //var relatedTrick = previousPlayed.GetRange(previousPlayed.Count-game.CurrentCards.GetLength(0), previousPlayed.Count-1);

                        Card card = calculateAiCard();
                        this.PerformAction(new Events.PlayerActionEventArgs() { CardPlayed = card });

                        //// For now, just draw a random card
                        //int cardIdx = new Random().Next(this.game.GetValidCards(this).Count);
                        //Card card = this.HandList.Pick(cardIdx);
                        //this.PerformAction(new Events.PlayerActionEventArgs() { CardPlayed = card });
                        break;
                    default:
                        throw new Exception("Unknown state!");
                }
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// AI will pass biding, will not join into the bidding stage 
        /// </summary>
        /// <param name="currentBid">The current bid to be considered</param>
        /// <returns>A bid number</returns>
        public int getAiBid(int currentBid)
        {
            // The bid the AI will submit.
            int bid = currentBid;

            bid = BID_PASS;

            return bid;
        }

        /// <summary>
        /// Play valid cards only 
        /// </summary>
        /// <returns>Card from the HandList</returns>
        public Card calculateAiCard()
        {

            // Holds trick suit options.
            List<Card> trickSuitCards;

            // Holds tarneeb suit options.
            List<Card> tarneebSuitCards;

            // Card to return
            Card toPick = null;

            // If the player's team is not winning, 
            if (!isWinningCardTeamMine)
            {
                // Determine trick suit options.
                trickSuitCards = this.game.GetValidCards(this)
                    .ToList();

                // Determine tarneeb suit options.
                tarneebSuitCards = this.game.GetValidCards(this)
                    .ToList();

                // Card to return
                toPick = null;

                // If there are choices available, determine a valid card.
                if (trickSuitCards.Count > 0)
                {
                    // Pick the trick suit over tarneeb.
                    toPick = trickSuitCards.First();
                }
                // If no trick suit card is available then check if there are an tarneeb.
                else if (tarneebSuitCards.Count > 0)
                {
                    toPick = tarneebSuitCards.First();
                }
                // Pick the lowest valued card to throw, order by the number and prioritize non-tarneeb cards
                else
                {
                    toPick = this.game.GetValidCards(this)
                        .OrderBy(card => card.Number)
                        .OrderBy(card => {
                            int value = -1;
                            if (card.Suit == tarneebSuit) value = 1;
                            return value;
                        })
                        .FirstOrDefault();
                }
            }

            return toPick;
        }
        #endregion
    }
}
