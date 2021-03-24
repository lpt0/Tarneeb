using System;
using System.Collections.Generic;
using System.Text;

/**
 * @author  Haran
 * @date    2021-03-23
 */
namespace TarneebClasses
{
    /// <summary>
    /// A game-specific exception has occured.
    /// This is manually raised.
    /// </summary>
    public class GameException : Exception
    {
        /// <summary>
        /// Create a new game exception.
        /// </summary>
        public GameException() : base()
        {

        }

        /// <summary>
        /// Create a new game exception with a message.
        /// </summary>
        /// <param name="message">The message to use.</param>
        public GameException(string message) : base(message)
        {

        }
    }
}
