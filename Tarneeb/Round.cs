﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarneebClasses
{
    /**
    * @Author  Hoang Quoc Bao Nguyen.
    * @Date    2021-01-29.
    */
    class Round 
    {
        /// <summary>
        /// Bid had been placed 
        /// </summary>
        public int bid { get; set; }

        /// <summary>
        /// Card had been played 
        /// </summary>
        public Card card { get; set; }

        /// <summary>
        /// Player who had already played a card and placed a bid 
        /// </summary>
        public Player player { get; set; }

        /// <summary>
        /// The list of card had been played
        /// </summary>
        public List<Card> CardPlayed { get; set; }

        /// <summary>
        /// The list of player, who had played
        /// </summary>
        public List<Player> PlayerPlayed { get; set; }

        // The list of bid had been placed 
        public List<int> Bid { get; set; }

        /// <summary>
        /// Parameterized constructor 
        /// </summary>
        /// <param name="card">represents the value of the card
        /// <param name="bid">represents the new bid that been placed
        public Round(int bid, Card card, Player player)
        {
            if (bid < 7)
            {
                throw new Exception("Please input a bid greater than 7!");
            }
            else
            {
                this.bid = bid;
                this.card = card;
                this.player = player;
            }
        }

        /// <summary>
        /// Method to add to the list of cards played by players during a round 
        /// </summary>
        /// <param name="card"></param>represents the value of the card  
        /// <param name="player"></param>represents the player name
        public void PlayCard(Card card, Player player)
        {
            CardPlayed.Add(card);
            PlayerPlayed.Add(player);
        }

        /// <summary>
        /// Method to add to the list of bid placed by players during a round
        /// </summary>
        /// <param name="bid"></param>represents the value of the bid 
        /// <param name="playerName"></param>represents the player name 
        public void PlaceBid(int bid)
        {
            Bid.Add(bid);
        }

        /// <summary>
        /// Determine who is the winner based on the bid 
        /// Return true/false
        /// </summary>
        /// <param name="newBid">represents the new bid placed 
        /// <returns></returns>
        public bool HighestBid(int newBid)
        {
            int lastBidItem = Bid.Count - 1;
            int bidPlaced = Bid[lastBidItem];
            newBid = this.bid;

            if (newBid > bidPlaced)
            {
                return true;
            }
            else { return false; }
        }
    }
}