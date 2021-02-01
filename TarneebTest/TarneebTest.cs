using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TarneebClasses;

namespace TarneebTest
{
    /// <summary>
    /// Test class for the Tarneeb classes.
    /// </summary>
    class TarneebTest
    {
        /// <summary>
        /// Main method for testing.
        /// </summary>
        /// <param name="args">No used.</param>
        static void Main(string[] args)
        {

            // Test Card Class and Deck Class.
            // DeckTest();

            // Test Player Class.
            PlayerTest();

            // Prompt user for any key to quit.
            Console.WriteLine("\n\nPress any key to quit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Test various Deck methods.
        /// </summary>
        static void DeckTest()
        {
            // New Deck
            Deck d = new Deck();

            // List Cards
            Console.WriteLine("Testing Deck:");
            d.Cards.ForEach(card => Console.WriteLine("\t" + card));

            d.Shuffle();
            Console.WriteLine("Shuffled Deck:");
            d.Cards.ForEach(card => Console.WriteLine("\t" + card));

            d.Sort();
            Console.WriteLine("Sorted Deck:");
            d.Cards.ForEach(card => Console.WriteLine("\t" + card));
        }

        /// <summary>
        /// Test Player class instances and its methods.
        /// </summary>
        static void PlayerTest()
        {
            Deck deck = new Deck();
            deck.Shuffle();

            Player player1 = new Player("Player One", 1, Enums.Team.Blue, new Deck
            {
                Cards = deck.Draw(13).ToList()
            });
            Player player2 = new Player("Player Two", 2, Enums.Team.Blue, new Deck
            {
                Cards = deck.Draw(13).ToList()
            });
            Player player3 = new Player("Player Three", 3, Enums.Team.Red, new Deck
            {
                Cards = deck.Draw(13).ToList()
            });
            Player player4 = new Player("Player Four", 4, Enums.Team.Red, new Deck
            {
                Cards = deck.Draw(13).ToList()
            });

            Console.WriteLine($"{player1}");
            Console.WriteLine("Player One's cards:");
            player1.HandList.Cards.ForEach(card => Console.WriteLine($"\t{card}"));

            Console.WriteLine($"\n{player2}");
            Console.WriteLine("Player Two's cards:");
            player2.HandList.Cards.ForEach(card => Console.WriteLine($"\t{card}"));

            Console.WriteLine($"\n{player3}");
            Console.WriteLine("Player Three's cards:");
            player3.HandList.Cards.ForEach(card => Console.WriteLine($"\t{card}"));

            Console.WriteLine($"\n{player4}");
            Console.WriteLine("Player Four's cards:");
            player4.HandList.Cards.ForEach(card => Console.WriteLine($"\t{card}"));

            Console.WriteLine($"\nNumer of cards left in the deck: {deck.Cards.Count()}");
        }
    }
}
