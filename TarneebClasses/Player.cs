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
    class Player
    {
        /**** DATA MEMBERS ****/

        private string playerName;      // A given player's name
        private int playerId;           // A given player's ID (i.e. player 1, 2, 3, 4)
        private int teamNumber;         // A given player's team number (i.e. team 1 or team 2)

        Deck handList = new Deck();     // A given player's hand of cards

        /**** CONSTRUCTORS ****/
        public Player(string playerName, int playerId, int teamNumber, Deck handList)
        {
            this.playerName = playerName;
            this.playerId = playerId;
            this.teamNumber = teamNumber;
            this.handList = handList;
        }


        /**** ACCESSORS & MUTATORS ****/

        /// <summary>
        /// Gets and sets a given player's name
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// Gets and sets a given player's ID
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Gets and sets a given player's team number
        /// </summary>
        public int TeamNumber { get; set; }

        /// <summary>
        /// Gets and sets a given players hand
        /// </summary>
        public Deck HandList { get; set; }

        /**** METHODS ****/

    }
}
