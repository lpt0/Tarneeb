/*
 * Author: Domenic Catalano
 * Date: January 22, 2020
 * Version: 1.0 (Jan 22, 2020)
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarneebClasses
{
    /// <summary>
    /// The player class will represent an individual player including their
    /// ID, team number, name, and hand.
    /// </summary>
    public class Player
    {
        #region Data members
        /// <summary>
        /// Gets and sets a given players name
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// Gets and sets a given players ID
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Gets and sets a given players team number
        /// </summary>
        public Enums.Team TeamNumber { get; set; }

        /// <summary>
        /// Gets and sets a given players hand
        /// </summary>
        public Deck HandList { get; set; }
        #endregion


        #region Events
        /// <summary>
        /// Invoked when this player performs an action, such as playing a card.
        /// </summary>
        public event EventHandler<Events.PlayerActionEventArgs> PlayerActionEvent;
        #endregion

        #region Constructor
        /// <summary>
        /// Create a new player.
        /// </summary>
        /// <param name="playerName">The name of the player.</param>
        /// <param name="playerId">The player's identifier.</param>
        /// <param name="teamNumber">The player's team.</param>
        /// <param name="handList">The player's hand.</param>
        public Player(string playerName, int playerId, Enums.Team teamNumber, Deck handList)
        {
            this.PlayerName = playerName;
            this.PlayerId = playerId;
            this.TeamNumber = teamNumber;
            this.HandList = handList;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get this player as a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"User Name: {PlayerName}. Player Number: {PlayerId}. Team Color: {TeamNumber}. ";
        }

        /// <summary>
        /// Raise the player action event.
        /// </summary>
        /// <param name="args">The arguments to use.</param>
        public void PerformAction(Events.PlayerActionEventArgs args)
        {
            this.PlayerActionEvent?.Invoke(this, args);
        }
        #endregion
    }
}
