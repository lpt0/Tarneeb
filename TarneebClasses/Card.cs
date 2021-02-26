/**
 * @author  Duy Tan Vu.
 * @date    2021-01-22.
 */

namespace TarneebClasses
{
    /// <summary>
    /// Card class representing individual card of a deck with number and suit.
    /// </summary>
    public class Card
    {
        /// <summary>
        /// Number attribute of type CardNumber.
        /// </summary>
        public Enums.CardNumber Number { get; set; }

        /// <summary>
        /// Suit attribute of type CardSuit.
        /// </summary>
        public Enums.CardSuit Suit { get; set; }

        /// <summary>
        /// Card attribute of type int.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Parameterized constructor.
        /// </summary>
        /// <param name="number">A CardNumber enum named number representing card number.</param>
        /// <param name="suit">A CardSuit enum named suit representing card suit</param>
        public Card(Enums.CardNumber number, Enums.CardSuit suit)
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
