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

            // Test Deck Class.
            DeckTest();

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

    }
}
