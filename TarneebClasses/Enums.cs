/**
 * @author  Duy Tan Vu.
 * @date    2021-01-22.
 */

namespace TarneebClasses
{
    /// <summary>
    /// Enums in the game.
    /// </summary>
    public static class Enums
    {
        /// <summary>
        /// The enum type of card suit with 4 constants.
        /// </summary>
        public enum CardSuit
        {
            /// <summary>
            /// Suit of spades.
            /// </summary>
            Spades = 1,

            /// <summary>
            /// Suit of heart.
            /// </summary>
            Heart = 2,

            /// <summary>
            /// Suit of diamond.
            /// </summary>
            Diamond = 3,

            /// <summary>
            /// Suit of club.
            /// </summary>
            Club = 4
        }

        /// <summary>
        /// The enum type of card number with 13 constants.
        /// </summary>
        public enum CardNumber
        {
            /// <summary>
            /// Card is a two.
            /// </summary>
            Two = 1,

            /// <summary>
            /// Card is a three.
            /// </summary>
            Three = 2,

            /// <summary>
            /// Card is a four.
            /// </summary>
            Four = 3,

            /// <summary>
            /// Card is a five.
            /// </summary>
            Five = 4,

            /// <summary>
            /// Card is a six.
            /// </summary>
            Six = 5,

            /// <summary>
            /// Card is a seven.
            /// </summary>
            Seven = 6,

            /// <summary>
            /// Card is an eight.
            /// </summary>
            Eight = 7,

            /// <summary>
            /// Card is a nine.
            /// </summary>
            Nine = 8,

            /// <summary>
            /// Card is a ten.
            /// </summary>
            Ten = 9,

            /// <summary>
            /// Card is a jack.
            /// </summary>
            Jack = 10,

            /// <summary>
            /// Card is a queen.
            /// </summary>
            Queen = 11,

            /// <summary>
            /// Card is a king.
            /// </summary>
            King = 12,

            /// <summary>
            /// Card is an ace.
            /// </summary>
            Ace = 13,
        }

        /// <summary>
        /// The enum type of team name with 2 constants.
        /// </summary>
        public enum Team
        {
            /// <summary>
            /// Blue team.
            /// </summary>
            Blue = 0,

            /// <summary>
            /// Red team.
            /// </summary>
            Red = 1
        }
    }
}
