using System;
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
    class Round : Card
    {
        /// <summary>
        /// New Bid that been placed by the next player
        /// </summary>
        public int newBid { get; set; }

        /// <summary>
        /// New card that been played 
        /// </summary>
        public CardValue newValue { get; set; }

        /// <summary>
        /// Suit value on the card that had been played recently
        /// </summary>
        public CardSuit newSuit { get; set; }

        /// <summary>
        /// Parameterized constructor 
        /// </summary>
        /// <param name="value">represents the value of the card 
        /// <param name="suit">represents the value of the suit on the card
        /// <param name="bid">represents the new bid that been placed
        public Round(CardValue value, CardSuit suit, int bid) : base(value, suit)
        {
            if (bid < 7)
            {
                throw new Exception("Please input a bid greater than 7!");
            }
            else
            {
                this.newValue = value;
                this.newBid = bid;
                this.newSuit = suit;
            }
        }

        /// <summary>
        /// Determine who is the winner based on the bid 
        /// Return true/false
        /// </summary>
        /// <param name="bidPlaced">represents the previous bid placed 
        /// <param name="cardPlayed">represents the card that had been played 
        /// <param name="suitPlaced">represents the suit of the card had been played
        /// <returns></returns>
        public bool isWin(int bidPlaced, CardValue cardPlayed, CardSuit suitPlaced)
        {
            cardPlayed = Value;
            suitPlaced = Suit;

            if (newBid > bidPlaced)
            {
                if (newValue > cardPlayed)
                {
                    return true;
                }
                else if (newValue == cardPlayed)
                {
                    if (newSuit > suitPlaced)
                    {
                        return true;
                    }
                    else { return false; }
                }
                else { return false; }
            }
            else { return false; }
        }
    }
}
