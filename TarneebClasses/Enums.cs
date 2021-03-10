/**
 * @author  Duy Tan Vu.
 * @date    2021-01-22.
 */

namespace TarneebClasses
{
    public static class Enums
    {
        /// <summary>
        /// The enum type of card suit with 4 constants.
        /// </summary>
        public enum CardSuit
        {
            Spades = 1,
            Heart = 2,
            Diamond = 3,
            Club = 4
        }

        /// <summary>
        /// The enum type of card number with 13 constants.
        /// </summary>
        public enum CardNumber
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
            Ace = 13,
        }

        /// <summary>
        /// The enum type of team name with 2 constants.
        /// </summary>
        public enum Team
        {
            Blue = 0,
            Red = 1
        }
    }
}
