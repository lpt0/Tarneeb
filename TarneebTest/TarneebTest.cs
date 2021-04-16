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
            //DeckTest();

            // Test Player Class.
            //PlayerTest();

            // Test Bid Class.
            BidTest();

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
            d.Shuffle();

            //// List Cards
            //Console.WriteLine("Testing Deck:");
            //d.Cards.ForEach(card => Console.WriteLine("\t" + card));

            //d.Shuffle();
            //Console.WriteLine("Shuffled Deck:");
            //d.Cards.ForEach(card => Console.WriteLine("\t" + card));

            //d.Sort();
            //Console.WriteLine("Sorted Deck:");
            //d.Cards.ForEach(card => Console.WriteLine("\t" + card));
            //d.Sort();

            //List<Card> selection;
            //List<Card> selection2;

            //selection = handList.Cards.Where(card => (card.Suit == trickSuit))
            //            .OrderBy(card => card.Number)
            //            .Where(card => card.Number > winningCard.Number).ToList();

            //Console.WriteLine(selection.Count);

            //selection2 = handList.Cards.Where(card => (card.Suit == tarneebSuit))
            //           .OrderBy(card => card.Number)
            //           .Where(card => card.Number > winningCard.Number).ToList();

            //Console.WriteLine(selection2.Count);

            //selection.ForEach(card => Console.Write($"{card}, "));
            //Console.WriteLine();
            //selection2.ForEach(card => Console.Write($"{card}, "));
            //Console.WriteLine();

            //Deck handList = d.Draw(13);
            //handList.Cards.ForEach(card => Console.Write($"{card}, "));
            //Console.WriteLine();

            //Card winningCard = new Card(Enums.CardNumber.Ace, Enums.CardSuit.Club);
            ////Enums.CardSuit trickSuit = Enums.CardSuit.Club;
            //Enums.CardSuit tarneebSuit = Enums.CardSuit.Spades;

            //List<Card> trickSuitCards = handList.Cards.Where(card => (card.Suit == winningCard.Suit) && card.Number > winningCard.Number).OrderBy(card => card.Number).ToList();
            //trickSuitCards.ForEach(card => Console.Write($"{card}, "));
            //Console.WriteLine();

            //List<Card> tarneebSuitCards = handList.Cards.Where(card => card.Suit == tarneebSuit).OrderBy(card => card.Number).ToList();
            //tarneebSuitCards.ForEach(card => Console.Write($"{card}, "));
            //Console.WriteLine();


            //Card toPick = null;

            //// If there are choices available, determine a valid card.
            //if (trickSuitCards.Count > 0)
            //{
            //    // Set default option and continue to the list if anything.
            //    toPick = trickSuitCards.First();
            //}
            //else if (tarneebSuitCards.Count > 0)
            //{
            //    toPick = tarneebSuitCards.First();
            //}
            //// Pick the lowest valued card to throw, order by the number and prioritize non-tarneeb cards
            //else
            //{
            //    toPick = handList.Cards
            //        .OrderBy(card => card.Number)
            //        .OrderBy(card => {
            //            int value = -1;
            //            if (card.Suit == tarneebSuit) value = 1;
            //            return value;
            //        })
            //        .FirstOrDefault();
            //}

            //Console.WriteLine(toPick);
            //handList.Cards
            //    .OrderBy(card => card.Number)
            //    .OrderBy(card => {
            //        int value = -1;
            //        if (card.Suit == tarneebSuit) value = 1;
            //        return value;
            //    })
            //    .ToList()
            //    .ForEach(card => Console.Write($"{card}, ")); ;


            //d.Cards = d.Cards
            //    .OrderByDescending(card => card.Number)
            //    .ToList();

            //Deck handList = d.Draw(13);
            //handList.Cards.ForEach(card => Console.Write($"{card}, "));

            //int handValue = 0;
            //handList.Cards.ForEach(card => handValue += (int) card.Number );

            //Console.WriteLine($"{ handValue }");    // 154
            //// the best hand is 154.

            d.Cards = d.Cards
                .OrderBy(card => card.Number)
                .ToList();

            Deck handList = d.Draw(13);
            handList.Cards.ForEach(card => Console.Write($"{card}, "));

            int handValue = 0;
            handList.Cards.ForEach(card => handValue += (int)card.Number);

            Console.WriteLine($"{ handValue }");    // 28
            // the worst hand is 28

            //   ((B-W)/2)+W
            // ((154-28)/2)+28
            // Average hand = 91

        }

        /// <summary>
        /// Test Player class instances and its methods.
        /// </summary>
        static void PlayerTest()
        {
            Deck deck = new Deck();
            deck.Shuffle();

            Player player1 = new Player("Player One", 1, Enums.Team.Blue, new Deck(deck.Draw(13)));
            Player player2 = new Player("Player Two", 2, Enums.Team.Red, new Deck(deck.Draw(13)));
            Player player3 = new Player("Player Three", 3, Enums.Team.Blue, new Deck(deck.Draw(13)));
            Player player4 = new Player("Player Four", 4, Enums.Team.Red, new Deck(deck.Draw(13)));

            Console.WriteLine($"{player1}");
            Console.WriteLine("Player One's cards:");
            player1.HandList.Sort();
            player1.HandList.Cards.ForEach(card => Console.WriteLine($"\t{card}"));

            Console.WriteLine($"\n{player2}");
            Console.WriteLine("Player Two's cards:");
            player2.HandList.Sort();
            player2.HandList.Cards.ForEach(card => Console.WriteLine($"\t{card}"));

            Console.WriteLine($"\n{player3}");
            Console.WriteLine("Player Three's cards:");
            player3.HandList.Sort();
            player3.HandList.Cards.ForEach(card => Console.WriteLine($"\t{card}"));

            Console.WriteLine($"\n{player4}");
            Console.WriteLine("Player Four's cards:");
            player4.HandList.Sort();
            player4.HandList.Cards.ForEach(card => Console.WriteLine($"\t{card}"));

            Console.WriteLine($"\nNumer of cards left in the deck: {deck.Cards.Count()}");
        }

        /// <summary>
        /// Test the Bid class.
        /// </summary>
        static void BidTest()
        {
            Deck deck = new Deck();
            deck.Shuffle();

            Player player1 = new Player("Player One", 1, Enums.Team.Blue, new Deck(deck.Draw(13)));
            Player player2 = new Player("Player Two", 2, Enums.Team.Blue, new Deck(deck.Draw(13)));
            Player player3 = new Player("Player Three", 3, Enums.Team.Red, new Deck(deck.Draw(13)));
            Player player4 = new Player("Player Four", 4, Enums.Team.Red, new Deck(deck.Draw(13)));

            Player[] listOfPlayers = new Player[]
            {
                player1,
                player2,
                player3,
                player4,
            };

            Bid aBid = new Bid(listOfPlayers);
        }
    }
}
