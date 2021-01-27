using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**
 * @Author  Duy Tan Vu
 * @Date    2021-01-22
 */

namespace Tarneeb
{
    /// <summary>
    /// The enum type of card suit with four constants
    /// </summary>
    public enum CardSuit
    {
        Spades, Hearts, Diamonds, Clubs
    }

    /// <summary>
    /// Card class representing individual card of a deck with value and suit.
    /// </summary>
    class Card
    {
        /// <summary>
        /// Value attribute of type int
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Value attribute of type CardSuit
        /// </summary>
        public CardSuit Suit { get; set; }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="value">An int representing card value</param>
        /// <param name="suit">A CardSuit enum value representing card suit</param>
        public Card(int value, CardSuit suit)
        {
            // Throw a new arguement if card value is out of range
            if (value < 2 || value > 14)
            {
                throw new ArgumentException("Card value must be between 2 and 14");
            }
            else
            {
                Value = value;
                Suit = suit;
            }
        }

        /// <summary>
        /// ToString() method return a string containing card value and suit
        /// </summary>
        /// <returns>A string containing card information</returns>
        public override string ToString()
        {
            string valueString = "";
            if (Value > 10)
            {
                switch (Value)
                {
                    case 11:
                        valueString += "Jack";
                        break;
                    case 12:
                        valueString += "Queen";
                        break;
                    case 13:
                        valueString += "King";
                        break;
                    default:
                        valueString += "Ace";
                        break;
                }
            }
            else
            {
                valueString += Value.ToString();
            }

            return valueString + " of " + Suit.ToString();
        }
    }
}
