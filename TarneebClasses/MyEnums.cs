using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarneebClasses
{
    public static class MyEnums
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
    }
}
