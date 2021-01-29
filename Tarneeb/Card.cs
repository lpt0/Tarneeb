using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**
 * @Author  Tan Vu.
 * @Date    2021-01-22.
 */

namespace Tarneeb
{
    /// <summary>
    /// The enum type of card suit with 4 constants.
    /// </summary>
    public enum CardSuit
    {
        Spades = 1,
        Hearts = 2,
        Diamonds = 3,
        Clubs = 4
    }

    /// <summary>
    /// The enum type of card number with 13 constants.
    /// </summary>
    public enum CardNumber
    {
        Ace = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13
    }

    /// <summary>
    /// Card class representing individual card of a deck with number and suit.
    /// </summary>
    class Card
    {
        /// <summary>
        /// Number attribute of type CardNumber.
        /// </summary>
        public CardNumber Number { get; set; }

        /// <summary>
        /// Suit attribute of type CardSuit.
        /// </summary>
        public CardSuit Suit { get; set; }

        /// <summary>
        /// Parameterized constructor.
        /// </summary>
        /// <param name="number">A CardNumber enum named number representing card number.</param>
        /// <param name="suit">A CardSuit enum named suit representing card suit</param>
        public Card(CardNumber number, CardSuit suit)
        {
            this.Number = number;
            this.Suit = suit;
        }

        /// <summary>
        /// ToString() method return a string containing card number and suit.
        /// </summary>
        /// <returns>A string containing card information.</returns>
        public override string ToString()
        {
            return $"{Number} of {Suit}";
        }
    }
}
