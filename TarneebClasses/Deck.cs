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
                {Enums.CardNumber.Ace, 13},
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
        /// Parameterized Constructor. Accepts of a Deck to have cards cloned of.
        /// </summary>
        /// <param name="aDeck">A deck to copy cards from.</param>
        public Deck(Deck aDeck)
        {
            this.Cards = new List<Card>(aDeck.Cards);
        }

        /// <summary>
        /// Parameterized Constructor. Accepts a set of list of Cards.
        /// </summary>
        /// <param name="aListOfCards">A list of cards to create a deck of.</param>
        public Deck(List<Card> aListOfCards)
        {
            this.Cards = new List<Card>(aListOfCards);
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
        /// Shuffles the cards order. Uses Random to generate a new seed for each shuffle.
        /// </summary>
        public void Shuffle()
        {
            // Seed to generate random order.
            Random randomSeed = new Random();
            // Sort according to the random order.
            this.Cards = Cards.OrderBy(card => randomSeed.Next()).ToList();
        }

        /// <summary>
        /// Sort the cards order value.
        /// </summary>
        public void Sort()
        {
            this.Cards = Cards
                .OrderBy(card => card.Value)
                .OrderBy(card => card.Suit)
                .ToList();  
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
        public Deck Draw(int numberOfCards)
        {
            // Take the number of cards from the deck.
            IEnumerable<Card> cards = Cards.Take(numberOfCards);

            // Select the cards drawn.
            List<Card> takeCards = cards as List<Card> ?? cards.ToList();
            // Remove them from the main deck.
            Cards.RemoveAll(takeCards.Contains);

            // Return the set of cards.
            return new Deck(takeCards);
        }

        /// <summary>
        /// Adds a card to the deck.
        /// </summary>
        /// <param name="aCard">Card to be added.</param>
        public void Add(Card aCard)
        {
            // Add the card to the Card list.
            this.Cards.Add(aCard);
        }

        /// <summary>
        /// Given a position in the deck returns that Card, effectively removing it from Deck.
        /// Returns null if invalid position.
        /// </summary>
        /// <param name="position">Index of the Card to return. Zero based.</param>
        /// <returns>Card at said position or null if invalid</returns>
        public Card Pick(int position)
        {
            // Card to be returned.
            Card picked;

            // Check position is reasonable.
            if (position > this.Cards.Count || position < 0)
            {
                // TODO: We could throw error if needed.
                return null;
            }

            // Grab a copy of the card at position.
            picked = this.Cards[position];
            // Remove the Card from the Card List.
            this.Cards.RemoveAt(position);

            // Return Card.
            return picked;
        }
    }
}
