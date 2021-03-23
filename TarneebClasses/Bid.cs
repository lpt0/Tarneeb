using System;
using System.Collections.Generic;
using System.Linq;

/**
 * @author  Duy Tan Vu.
 * @date    2021-02-26.
 */

namespace TarneebClasses
{
    /// <summary>
    /// Represents Bidding stage.
    /// </summary>
    public class Bid
    {
        /// <summary>
        /// A list of available bidding values.
        /// </summary>
        private static List<int> bidValues = new List<int>();

        /// <summary>
        /// A list of Players passed in the constructor.
        /// </summary>
        private List<Player> MyPlayers { get; set; }

        /// <summary>
        /// Original players when the Bid object is created.
        /// </summary>
        private List<Player> originalPlayers;

        /// <summary>
        /// Player who wins the bidding stage.
        /// </summary>
        public Player WinningPlayer { get; private set; }

        /// <summary>
        /// The higest bid value.
        /// </summary>
        public int HighestBid { get; private set; } = -1;

        /// <summary>
        /// The trump suit determined by winning player.
        /// </summary>
        public Enums.CardSuit TarneebSuit { get; private set; }

        /// <summary>
        /// Parameterized constructor that takes an array of players.
        /// </summary>
        public Bid(Player[] listOfPlayers)
        {
            bidValues.AddRange(new List<int>() { 7, 8, 9, 10, 11, 12, 13 });
            MyPlayers = listOfPlayers.ToList();

            // Clone the original list in case all player passed, we need the original list to reset MyPlayer property.
            originalPlayers = MyPlayers.ToList();
        }

        /// <summary>
        /// Bids method to decide the winner of the bid.
        /// </summary>
        /// <param name="currentPlayer">A Player object.</param>
        /// <param name="bid">An int represents their bid.</param>
        /// <returns></returns>
        public Player Bids(Player currentPlayer, int bid)
        {
            var currentIdx = MyPlayers.IndexOf(currentPlayer);

            if (bid == -1)
            {
                MyPlayers.Remove(currentPlayer);

                if (MyPlayers.Count() == 0)
                {
                    MyPlayers = originalPlayers.ToList();
                    return MyPlayers[0];
                }
                else if (MyPlayers.Count() == 1 && HighestBid != -1)
                {
                    return null;
                }
                else
                {
                    return MyPlayers[currentIdx];
                }
            }
            else if (!bidValues.Contains(bid))
            {
                Console.WriteLine("Invalid Bid. Please try again.");
                return currentPlayer;
            }
            else
            {
                bidValues.RemoveAll(item => item <= bid);
                WinningPlayer = currentPlayer;
                HighestBid = bid;

                if (bid == 13) return null;
            }

            var nextIndex = currentIdx + 1 == MyPlayers.Count() ? 0 : currentIdx + 1;
            return MyPlayers[nextIndex];
        }

        /// <summary>
        /// The bid winer decides the Tarneeb Suit.
        /// </summary>
        /// <param name="tarneebSuit">An enum CardSuit.</param>
        public void DecideTarneebSuit(Enums.CardSuit tarneebSuit)
        {
            TarneebSuit = tarneebSuit;
        }
    }
}
