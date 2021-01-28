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
    /// The enum type of card value with 13 constants.
    /// </summary>
    public enum CardValue
    {
        Two = 1,
        Three = 2,
        Four = 3,
        Five = 4,
        Six = 5,
        Seven = 6,
        Eight = 7,
        Nine = 8,
        Ten = 9,
        Jack = 10,
        Queen = 11,
        King = 12,
        Ace = 13
    }

    /// <summary>
    /// Card class representing individual card of a deck with value and suit.
    /// </summary>
    class Card
    {
        /// <summary>
        /// Value attribute of type CardValue.
        /// </summary>
        public CardValue Value { get; set; }

        /// <summary>
        /// Suit attribute of type CardSuit.
        /// </summary>
        public CardSuit Suit { get; set; }

        /// <summary>
        /// Parameterized constructor.
        /// </summary>
        /// <param name="value">An CardValue enum value representing card value.</param>
        /// <param name="suit">A CardSuit enum value representing card suit</param>
        public Card(CardValue value, CardSuit suit)
        {
            Value = value;
            Suit = suit;
        }

        /// <summary>
        /// ToString() method return a string containing card value and suit.
        /// </summary>
        /// <returns>A string containing card information.</returns>
        public override string ToString()
        {
            return Value.ToString() + " of " + Suit.ToString();
        }
    }
}
