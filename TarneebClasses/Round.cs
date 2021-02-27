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
    public class Round 
    {
        public Enums.CardSuit TrumpSuit { get; set; }

        public Card CardOne { get; set; }

        public Card CardTwo { get; set; }

        public Card CardThree { get; set; }

        public Card CardFour { get; set; }

        static List<Card> CardList = new List<Card>() { }; 

        public Round (Enums.CardSuit trumpSuit, Card card1, Card card2, Card card3, Card card4)
        {
            TrumpSuit = trumpSuit; 
            CardOne = card1;
            CardTwo = card2;
            CardThree = card3;
            CardFour = card4;
            List<Card> CardList = new List<Card>() { CardOne, CardTwo, CardThree, CardFour };
        }

        public int LeadTrick (Round round)
        {
            int numberOfWinTrick = 0; 
            var trumpSuitCard = CardList.Where(x => x.Suit == TrumpSuit).ToString();

            if (trumpSuitCard != "")
            {
                var highestNumber = CardList.Max(x => x.Number);
                var cardMax = CardList.Where(x => x.Number == highestNumber)
                                      .Where(y => y.Suit == TrumpSuit);
                numberOfWinTrick++;
            }
            else
            {
                var highestNumber = CardList.Max(x => x.Number);
                var cardMax = CardList.Where(x => x.Number == highestNumber);
                numberOfWinTrick++;
            }

            return numberOfWinTrick; 
        }

        public int Score (int bid, int numberOfWinTrick)
        {
            int score = 0; 
            int winScore = 0;
            int lostScore = 0;
            int totalScore = 0; 

            if (numberOfWinTrick == bid || numberOfWinTrick > bid)
            {
                winScore = numberOfWinTrick;
            }
            else
            {
                lostScore = - numberOfWinTrick; 
            }

            totalScore = winScore + lostScore;
            score = totalScore; 

            return totalScore; 
        }

        private static void main()
        {

        }
    }
}
