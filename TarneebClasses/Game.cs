using System;
using System.Collections.Generic;
using System.Text;

/**
 * Author: Haran
 * Date: 2021-02-08
 */
namespace TarneebClasses
{
    /// <summary>
    /// Class Game handles core game logic and logging.
    /// </summary>
    class Game
    {
        /* FIELDS */

        /// <summary>
        /// Singleton for game.
        /// </summary>
        public static Game GameInstance { get; }

        /// <summary>
        /// The players of the current game.
        /// </summary>
        public Player[] Players { get; set; }

        /// <summary>
        /// The game's deck.
        /// </summary>
        public Deck Deck { get; set; }

        /// <summary>
        /// The last card that was played.
        /// </summary>
        public Card LastCardPlayed { get; set; }

        /// <summary>
        /// The trump suit.
        /// </summary>
        public Enums.CardSuit TarneebSuit { get; } //TODO: Initialize in constructor

        /* CONSTRUCTOR */

        /// <summary>
        /// Create a new game.
        /// </summary>
        public Game()
        {
            if (Game.GameInstance == null)
            {
                //TODO
                // Game flow:
                // Initialize state
                // Get player name (need to prompt and wait)
                // Generate CPU names
                // Get player team colour (player num % 2)
                // Set CPU team colours (see above)
                // Bid stage
                // P1 starts bid
                // P2 bids
                // P3 bids
                // ... until 13
                // Can only bid once (i++ until i == 4 - counter clockwise though)
                // whoever wins the bid picks trump suit (tarneeb)
                // then he plays low suit 
                // people can only play one card (one trick)
                // round consists of playerNum tricks (4 tricks)
                // after a round, increment score
                // winner then gets to play next round's low suit
                // play until all cards are out (13 rounds)
                // whoever wins their bet, wins the points and wins the game
                // whoever LOSES the bet, loses the amount of points
                // whoever reaches 41 final score, wins
            }
        }

        /* METHODS */
        /// <summary>
        /// Initialize the game state.
        /// </summary>
        public void Initialize()
        {
            // Create deck
            this.Deck = new Deck();

            // Create players
            this.Players = new Player[4];
        }

        /// <summary>
        /// TODO
        /// </summary>
        public void GetPlayerName()
        {
            
        }

        
    }
}
