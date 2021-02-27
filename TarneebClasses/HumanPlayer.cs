/**
 * Author  Duy Tan Vu.
 * Date    2021-01-22.
 */

namespace TarneebClasses
{
    /// <summary>
    /// HumanPlayer inherits from the Player Class. 
    /// </summary>
    public class HumanPlayer : Player
    {
        /// <summary>
        /// Constructor parameterized calling the base constructor.
        /// </summary>
        /// <param name="playerName">A string represents player name.</param>
        /// <param name="playerId">An int represents player ID.</param>
        /// <param name="teamNumber">An enum represents team color.</param>
        /// <param name="handList">A Deck object represents player's handlist.</param>
        public HumanPlayer(string playerName, int playerId, Enums.Team teamNumber, Deck handList) : base(playerName, playerId, teamNumber, handList)
        {
        }

        /// <summary>
        /// Set a new name for the HumanPlayer object.
        /// </summary>
        /// <param name="newName">A string containing the new player name.</param>
        public void SetName(string newName)
        {
            this.PlayerName = newName;
        }
    }
}
