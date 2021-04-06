using System;
using System.Collections.Generic;
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
        protected const int MIN_AI_SAFE_BID = 7;
        protected const int MAX_AI_SAFE_BID = 9;
        protected const int MIN_AI_SAFE_SWING = 1;
        protected const int MAX_AI_SAFE_SWING = 3;
        protected const int MIN_AI_RISK_BID = 10;
        protected const int MAX_AI_RISK_BID = 11;
        protected const int MIN_AI_RISK_SWING = 1;
        protected const int MAX_AI_RISK_SWING = 2;
        protected const int MIN_AI_BID_THROW = 6;   // Possibility that the AI player will throw. X/10
        protected const int MIN_BID = 7;
        protected const int MAX_BID = 13;
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
            //var game = sender as Game;

            // Determine action based on Game's current state.
            switch (args.State)
            {
                case Game.State.NEW_GAME:
                    highestBid = MIN_BID;
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
                    highestBid = MIN_BID;
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
                //var game = sender as Game;

                switch (args.State)
                {
                    case Game.State.BID_STAGE:
                        // Perform bid logic
                        this.PerformAction(new Events.PlayerActionEventArgs() { Bid = getAiBid(highestBid) });
                        break;
                    case Game.State.BID_WON:
                        // Perform Tarneeb logic
                        this.PerformAction(new Events.PlayerActionEventArgs() { Tarneeb = decideOnTarneeb() });
                        break;
                    case Game.State.TRICK:
                        // Perform trick logic
                        Card card = calculateAiCard();
                        this.PerformAction(new Events.PlayerActionEventArgs() { CardPlayed = card });
                        break;
                    default:
                        throw new Exception("Unknown state!");
                }
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Custom logic for AI to decide what their bid should be. It considers it's constants 
        /// and what it gathers from the GameActionEvents it receives.
        /// </summary>
        /// <param name="currentBid">The current bid to be considered</param>
        /// <returns>A bid number</returns>
        public int getAiBid(int currentBid)
        {
            // The bid the AI will submit.
            int bid = currentBid;
            // Variation based on the AI's personality.
            int swing = personalitySeed.Next(MIN_AI_SAFE_SWING, MAX_AI_SAFE_SWING);
            // The cpu has decided to throw.
            bool throwBid = false;

            // The first bid therefore, minimum should be choosen
            if (bid <= MIN_BID)
            {
                bid = MIN_BID + swing;
            }
            // Current bid is maximum, pass.
            else if (bid >= MAX_BID)
            {
                bid = BID_PASS;
            }
            //// 10% chance that if AI might just bump the current bid to max if its at 12, regardless if their team is winning.
            //else if (bid + 1 == MAX_BID && personalitySeed.Next(0, 10) > 8) { bid = MAX_BID; }
            else
            {
                // The AI's Team is not winning, therefore motivated to be the bid winner.
                if (!isWinningCardTeamMine)
                {
                    // TODO: consider Cards in hand.

                    // Check if the AI wants to throw
                    if (personalitySeed.Next(0, 10) > MIN_AI_BID_THROW)
                    {
                        throwBid = true;

                    }
                    // CPU attempt to figure out if it is a good idea to bid..
                    else
                    {
                        // If the bid is less than their safe bid amount.
                        if ((bid + swing) < MAX_AI_SAFE_BID)
                        {
                            bid += swing;
                        }
                        // If the bid is risky
                        else if ((bid + swing) < MAX_AI_RISK_BID)
                        {
                            bid += swing;
                        }
                        else
                        {
                            throwBid = true;
                        }
                    }
                }
                // The AI knows it winning.
                // TODO: The AI can decide whether or not it should go first versus his teammate
                else
                {
                    // No special action if it wants to take lead, pass if team is winning.
                    throwBid = true;
                }
            }

            // Constraint in case the random generator goes off course.
            if (bid > MAX_BID) bid = BID_PASS;
            // If CPU has decide to throw, set bid to pass.
            else if (throwBid) bid = BID_PASS;

            return bid;
        }

        /// <summary>
        /// AI decision making for selecting a tarneeb.
        /// This decision is based off of the number of cards the users has of a card suit.
        /// </summary>
        /// <returns></returns>
        public Enums.CardSuit decideOnTarneeb()
        {
            // Suit to return.
            Enums.CardSuit newTarneeb;

            // List of counts.
            List<int> counts = new List<int>();
            // Get the cards by suit.
            List<Card> clubs = this.HandList.Cards.Where(card => card.Suit == Enums.CardSuit.Club).OrderBy(card => card.Number).ToList();
            List<Card> diamonds = this.HandList.Cards.Where(card => card.Suit == Enums.CardSuit.Diamond).OrderBy(card => card.Number).ToList();
            List<Card> hearts = this.HandList.Cards.Where(card => card.Suit == Enums.CardSuit.Heart).OrderBy(card => card.Number).ToList();
            List<Card> spades = this.HandList.Cards.Where(card => card.Suit == Enums.CardSuit.Spades).OrderBy(card => card.Number).ToList();

            // Tally all the counts
            counts.Add(clubs.Count);
            counts.Add(diamonds.Count);
            counts.Add(hearts.Count);
            counts.Add(spades.Count);

            // Check for more clubs than diamonds
            switch (counts.IndexOf(counts.Max()))
            {
                case 0: newTarneeb = Enums.CardSuit.Club; break;
                case 1: newTarneeb = Enums.CardSuit.Diamond; break;
                case 2: newTarneeb = Enums.CardSuit.Heart; break;
                case 3: newTarneeb = Enums.CardSuit.Spades; break;
                default: newTarneeb = Enums.CardSuit.Club; break;
            }

            return newTarneeb;
        }

        /// <summary>
        /// Calculates the best card for the AI to play.
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

            // The AI first to play.
            if (winningCard == null)
            {
                // Pick their strongest of trick suit.
                trickSuitCards = this.game.GetValidCards(this)
                    .OrderByDescending(card => card.Number)
                    .OrderBy(card => {
                        // Priorities non-tarneeb suit cards
                        int value = -1;
                        if (card.Suit == tarneebSuit) value = 1;
                        return value;
                    })
                    .ToList();
                //
                if (trickSuitCards.Count > 0)
                {
                    // Pick the trick suit over tarneeb.
                    toPick = trickSuitCards.First();
                }
                // No card of the choosen tarneeb (shouldn't be but maybe)
                else
                {
                    tarneebSuitCards = this.game.GetValidCards(this)
                        .Where(card => card.Suit == tarneebSuit)
                        .OrderBy(card => card.Number)
                        .ToList();
                    if (tarneebSuitCards.Count > 0)
                    {
                        toPick = tarneebSuitCards.First();
                    }
                }

            }
            // If the player's team is not winning, 
            else if (!isWinningCardTeamMine)
            {
                // Determine trick suit options.
                trickSuitCards = this.game.GetValidCards(this)
                    .Where(card => (card.Suit == winningCard.Suit) && card.Number > winningCard.Number)
                    .OrderBy(card => card.Number)
                    .ToList();

                // Determine tarneeb suit options.
                tarneebSuitCards = this.game.GetValidCards(this)
                    .Where(card => card.Suit == tarneebSuit)
                    .OrderBy(card => card.Number)
                    .ToList();

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
                // toPick is null, play any low card.
            }

            // If toPick is null (no good card to play), then any low card is fine.
            if (toPick == null)
            {
                // Pick the lowest valued card to throw, order by the number and prioritize non-tarneeb cards
                toPick = this.game.GetValidCards(this)
                       .OrderBy(card => card.Number)
                       .OrderBy(card => {
                           // Priorities non-tarneeb suit cards
                           int value = -1;
                           if (card.Suit == tarneebSuit) value = 1;
                           return value;
                       })
                       .FirstOrDefault();
            }

            return toPick;
        }

        #endregion
    }
}
