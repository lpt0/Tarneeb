using System;
using System.Collections.Generic;
using System.Text;

/**
 * Author: Haran
 * Date: 2021-02-08
 */
namespace TarneebClasses
{
    class GameLog
    {

        // Time | LogType | Message

        /// <summary>
        /// The types of logs.
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// Game initialization.
            /// </summary>
            INITIALIZE,

            /// <summary>
            /// A player setting their name.
            /// </summary>
            PLAYER_NAMED,

            /// <summary>
            /// A player is given a suit.
            /// </summary>
            TEAM_SET,

            /// <summary>
            /// The suit decided as the Tarneeb (trump) suit.
            /// </summary>
            TARNEEB_SUIT,

            /// <summary>
            /// A fattiyeh (bid) placed by a player.
            /// </summary>
            FATTIYEH_PLACED,

            /// <summary>
            /// The first card played during a trick round.
            /// </summary>
            LOW_SUIT,

            /// <summary>
            /// A card played during a trick.
            /// </summary>
            CARD_PLAYED,

            /// <summary>
            /// Whenever a trick is won (by playing the highest number).
            /// </summary>
            TRICK_WON,

            /// <summary>
            /// Whenever a game is won (by reaching the maximum score).
            /// </summary>
            GAME_WON
        }
    }
}
