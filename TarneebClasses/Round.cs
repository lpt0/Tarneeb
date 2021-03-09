using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarneebClasses
{
    /**
    * @Author  Hoang Quoc Bao Nguyen.
    * @Date    2021-01-29.
    */

    /// <summary>
    /// Round class represents a round of the Tarneeb game 
    /// </summary>
    public class Round 
    {
        /// <summary>
        /// Trump Suit of the whole game 
        /// </summary>
        public Enums.CardSuit TrumpSuit { get; set; }

        /// <summary>
        /// Card 1 played by the first player 
        /// </summary>
        public Card CardOne { get; set; }

        /// <summary>
        /// Card 2 played by the second player
        /// </summary>
        public Card CardTwo { get; set; }

        /// <summary>
        /// Card 3 played by the third player
        /// </summary>
        public Card CardThree { get; set; }

        /// <summary>
        /// Card 4 played by the fouth player
        /// </summary>
        public Card CardFour { get; set; }

        /// <summary>
        /// List of card played in a round 
        /// </summary>
        static List<Card> CardList = new List<Card>() { }; 

        /// <summary>
        /// Parameterized Constructor 
        /// </summary>
        /// <param name="trumpSuit">Trump suit of the game</param>
        /// <param name="card1">Card 1 played by the first player</param>
        /// <param name="card2">Card 2 played by the second player</param>
        /// <param name="card3">Card 3 played by the third player</param>
        /// <param name="card4">Card 4 played by the fourth player</param>
        public Round (Enums.CardSuit trumpSuit, Card card1, Card card2, Card card3, Card card4)
        {
            TrumpSuit = trumpSuit; 
            CardOne = card1;
            CardTwo = card2;
            CardThree = card3;
            CardFour = card4;
            List<Card> CardList = new List<Card>() { CardOne, CardTwo, CardThree, CardFour };
        }

        /// <summary>
        ///  WinCard determines the winning card of the round
        /// </summary>
        /// <param name="round">4 cards played in a round</param>
        /// <returns>The winning card</returns>
        public Card WinCard (Round round)
        {
            // Initialize variables
            // trumpSuitCard to store the trump suit value 
            // cardSuit to get the suit of the first played card
            var trumpSuitCard = CardList.Select(x => x)
                                        .Where(x => x.Suit == TrumpSuit).ToString();
            Enums.CardSuit card1Suit = CardList.Select(x => x.Suit).FirstOrDefault();
            Card cardMax; 

            // If there is one or multiple trump suit cards have been played then 
            // Focus on the trump suit cards to find the card contains the highest number to be the win card 
            // Else if there is no trump suit card has been played then
            // Using the Suit from the first card played to find the win card 
            if (trumpSuitCard != "")
            {
                // sorting the number from largest to smallest
                // Take the largest number by select the first value in the list 
                var highestNumber = CardList.OrderByDescending(x => x.Number)
                                            .Where(y => y.Suit == TrumpSuit).FirstOrDefault(); 
                cardMax = highestNumber; 
            }
            else
            {
                var highestNumber = CardList.OrderByDescending(x => x.Number)
                                            .Where(y => y.Suit == TrumpSuit).FirstOrDefault();

                cardMax = highestNumber; 
            }

            return cardMax; 
        }

        /// <summary>
        /// WinRoundCounter represents how many round the team has won 
        /// </summary>
        /// <returns>Number of win rounds</returns>
        public int WinRoundCounter()
        {
            int numberOfWinTrick = 0;

            numberOfWinTrick++; 
            return numberOfWinTrick; 
        }

        /// <summary>
        /// RoundScore represents the score the teams receive after a round 
        /// </summary>
        /// <param name="bid">The round bid number</param>
        /// <param name="numberOfWinTrick">Win trick counter</param>
        /// <returns></returns>
        public int RoundScore (int bid, int numberOfWinTrick)
        {
            int score = 0; 

            if (numberOfWinTrick < bid)
            {
                score = -bid; 
            }
            else
            {
                score = numberOfWinTrick; 
            }

            return score; 
        }
        
        /// <summary>
        /// Reset the Card list for the new round 
        /// </summary>
        private static void Reset()
        {
            CardList.Clear(); 
        }
    }
}
