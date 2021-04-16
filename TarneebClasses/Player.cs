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
        /**** DATA MEMBERS ****/

        /// <summary>
        /// Gets and sets a given players name
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// Gets and sets a given players ID
        /// </summary>
        public int PlayerId { get; set; }

        // Gets and sets a given players team number
        public Enums.Team TeamNumber { get; set; }

        // Gets and sets a given players hand
        public Deck HandList { get; set; }

        /**** EVENTS ****/
        /// <summary>
        /// Invoked when this player performs an action, such as playing a card.
        /// </summary>
        public event EventHandler<Events.PlayerActionEventArgs> PlayerActionEvent;

        /**** CONSTRUCTORS ****/
        public Player(string playerName, int playerId, Enums.Team teamNumber, Deck handList)
        {
            this.PlayerName = playerName;
            this.PlayerId = playerId;
            this.TeamNumber = teamNumber;
            this.HandList = handList;
        }

        /**** METHODS ****/

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

    }
}
