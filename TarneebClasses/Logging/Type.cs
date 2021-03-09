using System;
using System.Collections.Generic;
using System.Text;

/**
 * @author: Haran
 * @date: 2021-02-08
 */
namespace TarneebClasses.Logging
{
    /// <summary>
    /// The types of logs.
    /// </summary>
    public enum Type
    {
        /// <summary>
        /// Start of a new game of Tarneeb.
        /// </summary>
        NEW_GAME,

        /// <summary>
        /// A player joined.
        /// </summary>
        PLAYER_JOINED,

        /// <summary>
        /// The initial hand dealt to a player.
        /// </summary>
        INITIAL_HAND,

        /// <summary>
        /// A bid was placed.
        /// </summary>
        BID_PLACED,

        /// <summary>
        /// The suit decided as the Tarneeb (trump) suit.
        /// </summary>
        TARNEEB_SUIT,

        /// <summary>
        /// The first card played during a trick round.
        /// </summary>
        //LOW_SUIT,

        /// <summary>
        /// A card played during a trick.
        /// </summary>
        CARD_PLAYED,

        /// <summary>
        /// A card was dealt to a player.
        /// </summary>
        CARD_DEALT,

        /// <summary>
        /// Whenever a trick is won (by playing the highest number).
        /// </summary>
        TRICK_WON,

        /// <summary>
        /// A bid was completed.
        /// </summary>
        BID_COMPLETE,

        /// <summary>
        /// Whenever a game is won (by reaching the maximum score).
        /// </summary>
        GAME_WON
    }
}
