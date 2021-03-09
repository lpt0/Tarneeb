//using System;
//using System.Collections.Generic;
//using System.Text;
//using TarneebClasses.Events;

///**
// * Author: Haran
// * Date: 2021-02-08
// */
//namespace TarneebClasses
//{
    

//    /// <summary>
//    /// Core game logic and logging.
//    /// </summary>
//    public class Game2
//    {
//        /* TODO:
//         * There are two ways (that I can think of) to perform game logic.
//         * 1.
//         *  It can be performed in a class like this. However, drawing to screen
//         *  becomes a bit more complicated.
//         *  Drawing can be done by having the GUI logic read the last game log,
//         *  and draw an action based on that.
//         * 2.
//         *  GUI logic can also handle game logic. That way, it knows exactly
//         *  what the last action was, and what needs to be displayed based on that.
//         *  The downside of this is that AI logic may become more complicated to
//         *  work on - the AI would need to read the game logs, so the GUI would
//         *  need to maintain them and have them accessible by other classes.
//         *  This could be create a circular dependency issue, since the AI is in
//         *  the TarneebClasses project, and the GUI is in a separate project that
//         *  requires the TarneebClasses project. 
//         *  If the AI were to read logs from the GUI, TarneebClasses needs access to
//         *  the GUI project, and the GUI project needs access to the TarneebClasses project.
//         *  OR, could also use events; emit an event that the GUI listens for.
//         *  But, what if too many events happen at once?
//         */

//        /// <summary>
//        /// Enums for representing game state.
//        /// </summary>
//        public enum State
//        {
//            /// <summary>
//            /// Game has been initialized.
//            /// </summary>
//            INITIALIZED,

//            /// <summary>
//            /// Bids must be placed, and a Tarneeb suit decided.
//            /// </summary>
//            BID_STAGE,

//            /// <summary>
//            /// The user must place a bid.
//            /// </summary>
//            USER_BID_STAGE,

//            /// <summary>
//            /// The next trick is being determined.
//            /// </summary>
//            NEXT_TRICK,

//            /// <summary>
//            /// A CPU trick is to be played.
//            /// </summary>
//            CPU_TRICK,

//            /// <summary>
//            /// The user must play a trick.
//            /// </summary>
//            USER_TRICK,

//            /// <summary>
//            /// A trick has been completed.
//            /// </summary>
//            TRICK_COMPLETE,

//            /// <summary>
//            /// The bid value has been reached.
//            /// </summary>
//            BID_REACHED,

//            /// <summary>
//            /// The game is complete.
//            /// </summary>
//            DONE
//        }

//        #region Fields
//        #region Internal fields
//        /// <summary>
//        /// Hardcoded list of CPU player names.
//        /// TODO: Remove in final.
//        /// </summary>
//        private string[] cpuNames = { "Jim", "Tim", "Bim" };

//        /// <summary>
//        /// Game logs; retrievable by using GetLog and SetLog.
//        /// </summary>

//        /// <summary>
//        /// The player that gets to make the next move (trick, bid, etc).
//        /// </summary>
//        private Player currentPlayer;

//        /// <summary>
//        /// The bid that is currently being played. TODO: Wording
//        /// </summary>
//        private Bid currentBid;

//        /// <summary>
//        /// The low suit for the round.
//        /// </summary>
//        private Enums.CardSuit currentLowSuit;

//        /// <summary>
//        /// The cards currently being played.
//        /// </summary>
//        private Card[] currentCards;

//        /// <summary>
//        /// The number of cards played in the round.
//        /// </summary>
//        private int cardsPlayedInRound = 0;

//        /// <summary>
//        /// The number of rounds played for the current bid.
//        /// </summary>
//        private int roundCounter = 0;

//        /// <summary>
//        /// The score for the current bid.
//        /// </summary>
//        private int[] bidScore;

//        /// <summary>
//        /// The overall team score, across all bids.
//        /// </summary>
//        private int[] teamScore;
//        #endregion
//        #region Public fields
//        /// <summary>
//        /// Current game state.
//        /// The state can be read by any class, but can only be changed inside of the class.
//        /// </summary>
//        public State GameState { get; private set; } //TODO: Move this under public fields

//        /// <summary>
//        /// Whether the game has been completed or not.
//        /// </summary>
//        public Boolean IsComplete { get; private set; }

//        /// <summary>
//        /// The non-AI player. TODO
//        /// </summary>
//        public HumanPlayer User { get; }

//        /// <summary>
//        /// The players of the current game.
//        /// </summary>
//        public Player[] Players { get; }

//        /// <summary>
//        /// The game's deck.
//        /// </summary>
//        public Deck Deck { get; }

//        /// <summary>
//        /// The last card that was played.
//        /// </summary>
//        public Card LastCardPlayed { get; set; }

//        /// <summary>
//        /// TODO
//        /// </summary>
//        public List<Round> Rounds { // TODO: Tricks
//            get;
//            private set;
//        }

//        /// <summary>
//        /// TODO
//        /// TODO: Block public Add access
//        /// </summary>
//        public List<Bid> Bids { get; private set; }

//        /// <summary>
//        /// The trump suit.
//        /// </summary>
//        public Enums.CardSuit TarneebSuit { get; } //TODO: Initialize in constructor
//        #endregion
//        #endregion

//        #region Constructor
//        /// <summary>
//        /// Create a new game.
//        /// </summary>
//        /// <param name="playerName">The name of the human player.</param>
//        public Game2(String playerName)
//        {
            
//        }

//        #endregion

//        #region Event triggers
//        public event EventHandler<NewGameEventArgs> NewGameEvent;

//        // TODO: Function naming
//        private void TriggerNewGameEvent(NewGameEventArgs eventArgs)
//        {
//            // Actually fire the event
//            EventHandler<NewGameEventArgs> handler = NewGameEvent;
//            if (handler != null)
//            {
//                handler(this, eventArgs);
//            }

//        }
//        #endregion

//        public void Init()
//        {
//            // Fire the event
//            TriggerNewGameEvent(new NewGameEventArgs());
//        }

//        #region Functions
//        #region Helper functions
//        /// <summary>
//        /// Move on to the next player's trick, in counter-clockwise direction.
//        /// </summary>
//        private void nextPlayer()
//        {
//            int nextPlayerIdx = Array.IndexOf(this.Players, this.currentPlayer) - 1;
//            /* If it underflows, the next player is the last one in the array
//             * (since it is counter-clockwise).
//             */
//            if (nextPlayerIdx == -1)
//            {
//                nextPlayerIdx = 3;
//            }

//            // Move on to the next player
//            this.currentPlayer = this.Players[nextPlayerIdx];
//        }

//        /// <summary>
//        /// TODO: Rename the function
//        /// Determine if the next trick state is a User trick or a CPU trick.
//        /// </summary>
//        /// <returns>A trick state.</returns>
//        /// <see cref="State"/>
//        private State getTrickState()
//        {
//            if (this.currentPlayer is HumanPlayer)
//            {
//                // User's turn
//                return State.USER_TRICK;
//            }
//            else
//            {
//                // CPU's turn
//                return State.CPU_TRICK;
//            }
//        }
//        #endregion
//        #region Game state actions
//        /* Functions in this region correspond to actions performed at each
//         * game state. 
//         */
//        /// <summary>
//        /// Game initialization tasks.
//        /// </summary>
//        private void actionInitialize()
//        {
//            this.currentCards = new Card[4]; // TODO: Cards in round
//            this.teamScore = new int[] { 0, 0 };
//            this.bidScore = new int[] { 0, 0 };
//            this.Bids = new List<Bid>();
//            this.Rounds = new List<Round>();
//            this.GameState = State.BID_STAGE;
//        }

//        /// <summary>
//        /// Actions for bidding.
//        /// </summary>
//        private void actionBid()
//        {
//            // TODO: custom bid logic
//            this.actionUserBid();
//        }

//        /// <summary>
//        /// Actions for the user placing a bid.
//        /// </summary>
//        private void actionUserBid()
//        {
//            // TODO: Player to the RIGHT of the dealer bids first, then continues clockwise
//            // Bidding is currently handled in the bid class
//            Bid bid = new Bid();
//            bid.BidStage(this.Players[0], this.Players[1], this.Players[2], this.Players[3]);
//            this.Bids.Add(bid);

//            // Set the current bid and player
//            this.currentBid = bid;
//            this.currentPlayer = bid.WinningPlayer;
//        }

//        /// <summary>
//        /// Actions performed during a CPU's turn in a round, or trick.
//        /// </summary>
//        private void actionCPUTrick()
//        {
//            // CPU plays a card
//            Card card = this.currentPlayer.HandList.Pick(0);

//            // TODO
//            Console.WriteLine($"{this.currentPlayer.PlayerName} played {card}.");

//            /* Place the card in the array of cards for this round,
//             * and add the number of cards played in this round.
//             */
//            this.currentCards[cardsPlayedInRound++] = card;

//            // Move on to the next player
//            this.nextPlayer();

//            this.GameState = State.NEXT_TRICK;
//        }

//        /// <summary>
//        /// Actions performed during a user's turn in a trick.
//        /// </summary>
//        private void actionUserTrick()
//        {
//            // TODO
//            this.GameState = State.NEXT_TRICK;
//        }

//        /// <summary>
//        /// Actions performed when a trick is completed.
//        /// A trick is complete when all players have played a card.
//        /// </summary>
//        private void actionTrickComplete()
//        {
//            // Check if the round is done (when everyone has played a card)
//            if (this.cardsPlayedInRound == this.Players.Length)
//            {
//                Round round = new Round(
//                    this.currentBid.TrumpSuit,
//                    this.currentCards[0],
//                    this.currentCards[1],
//                    this.currentCards[2],
//                    this.currentCards[3]
//                );
//                Rounds.Add(round);

//                // TODO: Determine winner of round, add to bid score
//                // TODO: Winner becomes currentPlayer
//                this.nextPlayer();

//                // Prepare for next round
//                this.roundCounter++;
//                this.cardsPlayedInRound = 0;

//                // Next stage is a trick, assuming the bid has not been reached
//                this.GameState = this.getTrickState();

//                // Check if the bid has been reached
//                this.actionBidComplete(); // TODO
//            }
//        }

//        /// <summary>
//        /// Actions performed when a bid is complete.
//        /// A bid is complete when the winning bidder's number of tricks
//        /// has been reached.
//        /// </summary>
//        private void actionBidComplete()
//        {
//            // Has the bid been reached?
//            if (this.roundCounter == this.currentBid.HighestBid)
//            {
//                //TODO: Perform logging
//                // TODO: Determine winner of bid, add appropriate scores, reset bid score
//                this.bidScore = new int[] { 0, 0 };

//                // Time for the next bid!
//                this.GameState = State.BID_STAGE;
//            }
//        }

//        #endregion

//        /// <summary>
//        /// Move to the next step of the game.
//        /// This is essentially the game "loop".
//        /// </summary>
//        /// <returns>The next game state. This is required for checking if user action is required.</returns>
//        public State Next()
//        {
//            this.actionTrickComplete();

//            // Perform an action based on current state
//            switch (this.GameState)
//            {
//                case State.INITIALIZED:
//                    // Game has just started
//                    // TODO: Determine dealer, and play deal anim
//                    this.actionInitialize();
//                    break;

//                case State.BID_STAGE:
//                // TODO: Since Bid class currently reads in Bid, this should be skipped; will be fixed later
//                case State.USER_BID_STAGE:
//                    this.actionUserBid();

//                    // Move on to the next trick
//                    //this.GameState = this.getTrickState(); // TODO
//                    this.GameState = State.NEXT_TRICK;
//                    break;

//                case State.NEXT_TRICK:
//                    this.GameState = this.getTrickState();
//                    break;

//                case State.CPU_TRICK:
//                    this.actionCPUTrick();
//                    break;
                
//                /* USER_TRICK is handled in a polymorphic function:
//                 * public State Next(Card);
//                 */

//                default:
//                    throw new Exception("Unknown state.");
//            }

//            return this.GameState;
//        }

//        /// <summary>
//        /// The user must play a card.
//        /// </summary>
//        /// <param name="userPlayedCard">The card to play.</param>
//        /// <returns>The next game state.</returns>
//        public void Next(Card userPlayedCard)
//        {
//            this.currentCards[cardsPlayedInRound++] = userPlayedCard;
//            this.nextPlayer();
//            this.GameState = State.NEXT_TRICK;
//            this.Next();
//        }
//        #endregion
//    }
//}
