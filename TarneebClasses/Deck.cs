using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**
 * @author  Andrew Kuo
 * @date    2021-01-22
 */

namespace TarneebClasses
{
    /// <summary>
    /// Deck class represents a collection of cards. 
    /// 
    /// </summary>
    public class Deck
    {
        /// <summary>
        /// Value of each Card. Should probably involve higher class with game
        /// logic. This dictionary assigns a integer value to each CardNumber.
        /// </summary>
        public static Dictionary<Enums.CardNumber, int> CardValues =
            new Dictionary<Enums.CardNumber, int> {
                {Enums.CardNumber.Ace, 13},
                {Enums.CardNumber.Two, 1},
                {Enums.CardNumber.Three, 2},
                {Enums.CardNumber.Four, 3},
                {Enums.CardNumber.Five, 4},
                {Enums.CardNumber.Six, 5},
                {Enums.CardNumber.Seven, 6},
                {Enums.CardNumber.Eight, 7},
                {Enums.CardNumber.Nine, 8},
                {Enums.CardNumber.Ten, 9},
                {Enums.CardNumber.Jack, 10},
                {Enums.CardNumber.Queen, 11},
                {Enums.CardNumber.King, 12},
        };

        /// <summary>
        /// List of cards stored in the deck.
        /// </summary>
        public List<Card> Cards
        {
            get;
            set;
        }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public Deck()
        {
            this.Reset();
        }

        /// <summary>
        /// Generates a new standard 52 card deck.
        /// </summary>
        public void Reset()
        {
            // Fetch the number of Card suit and numbers. 
            // This could be hard coded or saved to the Enum class.
            int numberOfCardSuit =
                Enum.GetNames(typeof(Enums.CardSuit)).Length;
            int numberOfCardNumber =
                Enum.GetNames(typeof(Enums.CardNumber)).Length;

            // Generate all cards with Tarneeb values.
            this.Cards = Enumerable.Range(1, numberOfCardSuit)
                .SelectMany(suit => Enumerable.Range(1, numberOfCardNumber)
                .Select(number => new Card((Enums.CardNumber)number, (Enums.CardSuit)suit)
                {
                    Value = CardValues[(Enums.CardNumber)number]
                }))
                .ToList();
        }

        /// <summary>
        /// Shuffles the cards order.
        /// </summary>
        public void Shuffle()
        {
            this.Cards = Cards.OrderBy(card => Guid.NewGuid()).ToList();
        }

        /// <summary>
        /// Sort the cards order value.
        /// </summary>
        public void Sort()
        {
            this.Cards = Cards.OrderBy(card => card.Value).ToList();
        }

        /// <summary>
        /// Draw returns the first card in the deck.
        /// </summary>
        /// <returns>Card.</returns>
        public Card Draw()
        {
            // Grab the first card and remove it from list.
            Card card = Cards.FirstOrDefault();
            Cards.Remove(card);

            // Return the selected card.
            return card;
        }

        /// <summary>
        /// Draw returns a number of cards.
        /// </summary>
        /// <param name="numberOfCards">The number of Cards</param>
        /// <returns>A list of Cards.</returns>
        public IEnumerable<Card> Draw(int numberOfCards)
        {
            // Take the number of cards from the deck.
            IEnumerable<Card> cards = Cards.Take(numberOfCards);

            // Select the cards drawn.
            IEnumerable<Card> takeCards = cards as Card[] ?? cards.ToArray();
            // Remove them from the main deck.
            Cards.RemoveAll(takeCards.Contains);

            // Return the set of cards.
            return takeCards;
        }

    }
}
