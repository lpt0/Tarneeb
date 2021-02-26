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
        static List<int> bidValues = new List<int>();

        /// <summary>
        /// A list of players passing their turn.
        /// </summary>
        static List<Player> passedPlayers = new List<Player>();

        /// <summary>
        /// Player who wins the bidding stage.
        /// </summary>
        public Player WinningPlayer { get; set; }

        /// <summary>
        /// The higest bid value.
        /// </summary>
        public int HighestBid { get; set; } = 0;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Bid()
        {
            Reset();
        }

        /// <summary>
        /// Reset the bidValues list and passedPlayer list.
        /// </summary>
        private static void Reset()
        {
            bidValues.AddRange(new List<int>() { 7, 8, 9, 10, 11, 12, 13 });
            passedPlayers.Clear();
        }

        /// <summary>
        /// Bidding stage.
        /// </summary>
        /// <param name="player1">Player one.</param>
        /// <param name="player2">Player two.</param>
        /// <param name="player3">Player three.</param>
        /// <param name="player4">Player four.</param>
        public void BidStage(Player player1, Player player2, Player player3, Player player4)
        {
            // Create a list of players.
            List<Player> myPlayers = new List<Player>()
            { player1, player2, player3, player4 };

            // Temp index to do the bidding loop.
            int i = 0;
            do
            {
                // If all four people pass, clear the list and continue asking for other bids. 
                // Otherwise, the program will hang because of the next if statement.
                if (passedPlayers.Count() == 4) passedPlayers.Clear();

                // If a player passed their turn, they cannot bid anymore, the loop will go to the next available player.
                if (passedPlayers.Contains(myPlayers[i]))
                {
                    i++;
                    i %= 4;
                    continue;
                }

                // Print available bidding values.
                Console.Write($"\nAvailable bids: ");
                foreach (var item in bidValues)
                {
                    Console.Write($"{item} ");
                }

                // Asking for a bid from player.
                Console.WriteLine($"\n{myPlayers[i].PlayerName}'s turn - press number key or P | p to pass:");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int value))
                {
                    // If the input is a number but is not a valid number of bid values, display error message.
                    if (!bidValues.Contains(value))
                    {
                        Console.WriteLine("Please choose number from available bids.");
                    }
                    // If the input is valid, set highest bid and winning player. Remove bid values than and equal to the highest bid.
                    else
                    {
                        HighestBid = value;
                        WinningPlayer = myPlayers[i];
                        bidValues.RemoveAll(item => item <= HighestBid);

                        // Break the loop if higest bid is the largest possible number.
                        if (HighestBid == 13) break;

                        // Increase i vairable to move to the next available player.
                        i++;
                    }
                }
                else
                {
                    // Add player to passedPlayers list if user enters [p|P].
                    if (input.Length != 0 && input.ToLower()[0] == 'p')
                    {
                        passedPlayers.Add(myPlayers[i]);

                        // Increase i vairable to move to the next available player.
                        i++;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please try again.");
                    }
                }

                // If there is one player already bid and the other three passed. Break the loop.
                if (passedPlayers.Count() == 3 && HighestBid != 0) break;

                i %= 4; // Keep the i variable a valid index to loop through the list of four players.
            } while (HighestBid != 13);

            Console.WriteLine($"\n{WinningPlayer.PlayerName} has the highest bid: {HighestBid}");
        }
    }
}
