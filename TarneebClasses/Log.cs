using System;

/**
 * @author  Haran
 * @date    2021-04-03
 */
namespace TarneebClasses
{
    /// <summary>
    /// A game log.
    /// </summary>
    public class Log
    {
        /// <summary>
        /// The date and time the action took place.
        /// </summary>
        public DateTime DateTime { get; private set; }

        /// <summary>
        /// The action that took place, such a card being played or a bid being placed.
        /// </summary>
        public string Action { get; private set; }

        /// <summary>
        /// The identifier for the game that the action took place.
        /// </summary>
        public int GameID { get; private set; }

        /// <summary>
        /// Create a game log, with the date and time.
        /// </summary>
        /// <param name="dateTime">The date and time the action took place.</param>
        /// <param name="gameId">The identifier for the game that the action took place.</param>
        /// <param name="action">The action that took place.</param>
        public Log (DateTime dateTime, int gameId, string action)
        {
            this.DateTime = dateTime;
            this.GameID = gameId;
            this.Action = action;
        }

        /// <summary>
        /// Create a new log.
        /// </summary>
        /// <param name="gameId">The identifier for the game that the action took place.</param>
        /// <param name="action">The action that took place.</param>
        public Log(int gameId, string action)
            : this(DateTime.Now, gameId, action)
        {
        }

        /// <summary>
        /// Get this log as a string.
        /// </summary>
        /// <returns>The action.</returns>
        public override string ToString()
        {
            return this.Action;
        }
    }
}