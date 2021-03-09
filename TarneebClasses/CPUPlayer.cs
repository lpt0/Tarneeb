/**
 * Author  Duy Tan Vu.
 * Date    2021-01-22.
 */

namespace TarneebClasses
{
    /// <summary>
    /// CPU Player class inherits from Player class.
    /// </summary>
    public class CPUPlayer : Player
    {
        /// <summary>
        /// Constructor parameterized calling the base constructor.
        /// </summary>
        /// <param name="playerName">A string represents player name.</param>
        /// <param name="playerId">An int represents player ID.</param>
        /// <param name="teamNumber">An enum represents team color.</param>
        /// <param name="handList">A Deck object represents player's handlist.</param>
        public CPUPlayer(string playerName, int playerId, Enums.Team teamNumber, Deck handList) : base(playerName, playerId, teamNumber, handList)
        {
        }
    }
}
