using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TarneebClasses.Events;

/**
 * Author: Haran
 * Date: 2021-02-08
 */
namespace TarneebClasses
{
    /// <summary>
    /// Core game logic and logging.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Enums for representing game state.
        /// </summary>
        public enum State
        {
            /// <summary>
            /// A new game has been started.
            /// </summary>
            NEW_GAME,

            /// <summary>
            /// Bids must be placed, and a Tarneeb suit decided.
            /// </summary>
            BID_STAGE,

            /// <summary>
            /// A player has bid the highest, and gets to decide the Tarneeb
            /// suit.
            /// </summary>
            BID_WON,

            /// <summary>
            /// The Tarneeb suit has been decided.
            /// </summary>
            TARNEEB_SUIT,

            /// <summary>
            /// It is a trick turn, where cards can be played.
            /// </summary>
            TRICK,

            /// <summary>
            /// A trick has been completed.
            /// </summary>
            TRICK_COMPLETE,

            /// <summary>
            /// The bid value is complete.
            /// </summary>
            BID_COMPLETE,

            /// <summary>
            /// The game is complete.
            /// </summary>
            DONE
        }

        #region Constants
        /// <summary>
        /// The number of players in a game.
        /// </summary>
        public const int NUMBER_OF_PLAYERS = 4;

        /// <summary>
        /// The number of teams.
        /// </summary>
        public const int NUMBER_OF_TEAMS = 2;

        /// <summary>
        /// The number of cards in a hand (size of deck / number of players).
        /// </summary>
        public const int HAND_SIZE = 13;
        #endregion

        #region Fields
        #region Internal fields
        /// <summary>
        /// TODO: Counts number of bids placed
        /// </summary>
        private int bidCount = 0;

        /// <summary>
        /// Hardcoded list of CPU player names.
        /// TODO: Remove in final.
        /// </summary>
        private static readonly string[] cpuNames = { "Jim", "Tim", "Bim" };

        /// <summary>
        /// The player that gets to make the next move (trick, bid, etc).
        /// </summary>
        private Player _currentPlayer;

        /// <summary>
        /// The number of cards played in the round.
        /// </summary>
        private int _cardsPlayedInRound = 0;

        /// <summary>
        /// The score for the current bid.
        /// </summary>
        private int[] bidScore = new int[NUMBER_OF_TEAMS];

        /// <summary>
        /// The overall team score, across all bids.
        /// </summary>
        private int[] teamScore = new int[NUMBER_OF_TEAMS];

        /// <summary>
        /// Internal list of bids for this game.
        /// </summary>
        private List<Bid> _bids;

        /// <summary>
        /// Internal list of rounds for this game.
        /// </summary>
        private List<Round> _rounds;

        #endregion
        #region Public fields
        /// <summary>
        /// Current game state.
        /// The state can be read by any class, but can only be changed inside of the class.
        /// </summary>
        public State CurrentState { get; private set; }

        /// <summary>
        /// The players of the current game.
        /// TODO: Fake players for network
        /// </summary>
        public Player[] Players { get; }

        /// <summary>
        /// The game's deck.
        /// </summary>
        //public Deck Deck { get; } //TODO

        /// <summary>
        /// The cards currently in play for the current trick.
        /// </summary>
        public Card[] CurrentCards { get; private set; }

        /// <summary>
        /// The players that played each card.
        /// Indices here match up with CurrentCards.
        /// </summary>
        /// <see cref="CurrentCards" />
        public Player[] CurrentPlayers { get; private set; }

        /// <summary>
        /// The number of rounds played for the current bid.
        /// </summary>
        public int TrickCounter { get; private set; }

        /// <summary>
        /// The score that needs to be reached to win.
        /// </summary>
        public int MaxScore { get; private set; }

        /// <summary>
        /// TODO
        /// </summary>
        public List<Round> Rounds { get => this._rounds; } //TODO

        /// <summary>
        /// Bids placed in this game.
        /// </summary>
        public List<Bid> Bids { get => this._bids; } // TODO

        /// <summary>
        /// Game logs; TODO
        /// TODO: https://docs.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.readonlyobservablecollection-1?view=net-5.0
        /// </summary>
        public ObservableCollection<Logging.ILog> Logs { get; private set; } //TODO: Public get and private set, and block Add access for get
        #endregion
        #endregion

        #region Events
        #region Event objects
        /// <summary>
        /// Triggered when a new game is started.
        /// </summary>
        public event EventHandler<NewGameEventArgs> NewGameEvent;

        /// <summary>
        /// Raised when it is a player's turn.
        /// </summary>
        public event EventHandler<PlayerTurnEventArgs> PlayerTurnEvent;

        /// <summary>
        /// Raised when a game action has occurred.
        /// This is different from a player action, since a game action could
        /// be a card played, or it could be a team winning a trick.
        /// </summary>
        public event EventHandler<GameActionEventArgs> GameActionEvent;

        /// <summary>
        /// Raised when there are notifications.
        /// </summary>
        public event EventHandler<NotificationEventArgs> NotificationEvent;
        #endregion
        #region Event triggers
        /// <summary>
        /// Fire a new game event.
        /// </summary>
        /// <param name="args">The arguments for the event.</param>
        public void FireNewGameEvent()
        {
            this.Logs.Add(new Logging.NewGameLog());
            NewGameEvent?.Invoke(this, new NewGameEventArgs());
        }

        /// <summary>
        /// Raise the PlayerTurnEvent.
        /// </summary>
        private void FirePlayerTurnEvent()
        {
            // All arguments are the same
            this.PlayerTurnEvent?.Invoke(
                this,
                new PlayerTurnEventArgs() { Player = this._currentPlayer, State = this.CurrentState }
            );
        }

        /// <summary>
        /// Fire a game action event.
        /// Such as when a card is played, or a bid is placed.
        /// </summary>
        /// <param name="args">The event arguments.</param>
        /// <see cref="GameActionEventArgs" />
        private void FireGameActionEvent(GameActionEventArgs args)
        {
            // Set state, just in case
            args.State = this.CurrentState;
            this.GameActionEvent?.Invoke(this, args);
        }
        #endregion
        #endregion

        #region Player action functions
        private void OnPlayerBid(PlayerActionEventArgs args)
        {
            Bid bid = this._bids[this._bids.Count - 1];

            // Validate bid
            // Bid class will validate bid, and move on to the next player if needed
            Player nextPlayer = bid.Bids(this._currentPlayer, args.Bid);

            // If the bid was invalid, the next bidder is the same player
            // And if it is invalid, don't log the bid, and don't send the event out
            if (this._currentPlayer != nextPlayer || bidCount >= 3) // TODO: Need to find a new way of validation
            {
                this.Logs.Add(new Logging.BidPlacedLog() { Player = this._currentPlayer, Bid = args.Bid });
                FireGameActionEvent(new GameActionEventArgs() { Player = this._currentPlayer, Bid = args.Bid });
                bidCount++;
            }

            this._currentPlayer = nextPlayer;
        }

        private void OnPlayerTarneeb(PlayerActionEventArgs args)
        {
            // Validate Tarneeb suit
            // Assume the suit is valid for a local game
            Bid bid = this._bids[this._bids.Count - 1];
            bid.DecideTarneebSuit(args.Tarneeb);
            this.Logs.Add(new Logging.TarneebSuitLog() { Player = this._currentPlayer, Suit = args.Tarneeb });
            FireGameActionEvent(new GameActionEventArgs() { Player = this._currentPlayer, Bid = bid.HighestBid, Tarneeb = args.Tarneeb });
        }

        private void OnPlayerTrick(PlayerActionEventArgs args)
        {
            // Validate card played
            // Local play game, assume card is valid
            this.CurrentPlayers[_cardsPlayedInRound] = this._currentPlayer;
            this.CurrentCards[_cardsPlayedInRound] = args.CardPlayed;
            _cardsPlayedInRound++;

            /* Remove the card from the player's hand (again, just in case)
             * Players may or may not have already done this, but better to do it
             * just in case.
             */
            this._currentPlayer.HandList.Cards.Remove(args.CardPlayed);

            this.Logs.Add(new Logging.CardPlayedLog() { Player = this._currentPlayer, Card = args.CardPlayed });
            FireGameActionEvent(new GameActionEventArgs() { Player = this._currentPlayer, Card = args.CardPlayed, CardsPlayedInRound = this._cardsPlayedInRound - 1 });

            this._currentPlayer = this.NextPlayer();
        }

        /// <summary>
        /// Executes when a player performs an action.
        /// </summary>
        /// <param name="sender">The player that performed the action.</param>
        /// <param name="args">The arguments for the action.</param>
        private void OnPlayerAction(object sender, PlayerActionEventArgs args)
        {
            // Is it that player's turn?
            if (sender == this._currentPlayer)
            {
                try
                {
                    switch (this.CurrentState)
                    {
                        case Game.State.BID_STAGE:
                            this.OnPlayerBid(args);
                            break;

                        case Game.State.BID_WON:
                            // TODO: Tarneeb suit should be in own stage
                            this.OnPlayerTarneeb(args);
                            break;

                        case Game.State.TRICK:
                            this.OnPlayerTrick(args);
                            break;
                    }
                } 
                catch (Exception e)
                {
                    this.NotificationEvent?.Invoke(this, new NotificationEventArgs() { Message = e.Message });
                }
                this.Next();
            }
            else
            {
                throw new Exception("Illegal action!");
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Create a new game.
        /// </summary>
        public Game(int maxScore = 41)
        {
            // Initialize all fields
            this.Players = new Player[NUMBER_OF_PLAYERS];
            this.CurrentCards = new Card[NUMBER_OF_PLAYERS];
            this.CurrentPlayers = new Player[NUMBER_OF_PLAYERS];
            this.Logs = new ObservableCollection<Logging.ILog>(); // TODO
            this._bids = new List<Bid>();
            this._rounds = new List<Round>();
            this.CurrentState = State.NEW_GAME;
            this.TrickCounter = 0;
            this.MaxScore = maxScore;

            this.bidScore = new int[NUMBER_OF_TEAMS];
            this.teamScore = new int[NUMBER_OF_TEAMS];
        }
        #endregion

        

        #region Functions
        /// <summary>
        /// Initialize game state.
        /// </summary>
        /// <param name="playerName">The name of the user.</param>
        /// <returns>The user's created player.</returns>
        public HumanPlayer Initialize(String playerName)
        {
            FireNewGameEvent();

            // Create a hand for each player
            Deck[] hands = CreateHands();

            // Create the user's player
            HumanPlayer user = new HumanPlayer(
                playerName,
                0,
                Enums.Team.Blue,
                hands[0] 
            );
            this.Players[0] = user;

            // Log the player's intial hand
            this.Logs.Add(new Logging.InitialHandLog() { Hand = user.HandList, Player = user });
            this.Logs.Add(new Logging.PlayerJoinedLog() { Player = user });
            FireGameActionEvent(new GameActionEventArgs() { Player = user });

            // Create CPU players
            for (int playerNum = 1; playerNum < NUMBER_OF_PLAYERS; playerNum++)
            {
                this.Players[playerNum] = new CPUPlayer(
                    this, // Need to pass the game so CPU knows how to listen to event
                    Game.cpuNames[playerNum - 1],
                    playerNum,
                    (Enums.Team)(playerNum % 2),
                    hands[playerNum]
                );

                this.Logs.Add(new Logging.PlayerJoinedLog() { Player = this.Players[playerNum] });
                FireGameActionEvent(new GameActionEventArgs() { Player = this.Players[playerNum] });
            }

            // Setup event listeners
            foreach (Player p in this.Players)
            {
                p.PlayerActionEvent += this.OnPlayerAction;
            }

            this.CurrentState = State.BID_STAGE;
            // TODO: Dealer?
            // For now, pick a random player as dealer
            this._currentPlayer = this.Players[new Random().Next(3)];
            
            // Start a new bid
            this._bids.Add(new Bid(this.Players));


            return user;
        }
        /// <summary>
        /// Start the game.
        /// </summary>
        public void Start()
        {
            // Fire off the first event
            FirePlayerTurnEvent();
            this.Next();
        }

        /// <summary>
        /// Determine the next stage of the game.
        /// </summary>
        private void Next()
        {
            Bid currentBid = this._bids[this._bids.Count - 1];
            switch (this.CurrentState)
            {
                case State.NEW_GAME:
                    // Condition: all players must join
                    break;
                case State.BID_STAGE:
                    // Condition: bid must be complete
                    // This is met when Bid.Bids() returns null.
                    if (this._currentPlayer == null && currentBid.WinningPlayer != null)
                    {
                        // Bid is done; winner picks trump suit
                        this._currentPlayer = currentBid.WinningPlayer;
                        this.CurrentState = State.BID_WON;
                        this.Next();
                    }
                    break;
                case State.BID_WON:
                    // Condition: Trump suit is set
                    if (currentBid.TarneebSuit >= Enums.CardSuit.Spades)
                    {
                        this.CurrentState = State.TRICK;
                        // Winner goes first
                        this.Next();
                    }
                    break;
                case State.TRICK:
                    // Condition: All players must have played a card
                    if (_cardsPlayedInRound == NUMBER_OF_PLAYERS)
                    {
                        this.CurrentState = State.TRICK_COMPLETE;
                        this.Next();
                    }
                    break;
                case State.TRICK_COMPLETE:
                    //TODO: Scoping
                    {
                        // Perform end of round tasks
                        Round round = new Round(
                            currentBid.TarneebSuit,
                            this.CurrentCards[0],
                            this.CurrentCards[1],
                            this.CurrentCards[2],
                            this.CurrentCards[3]
                        );
                        this.TrickCounter++;

                        Card winningCard = round.WinCard(round); // TODO
                        int winningCardIdx = Array.IndexOf(this.CurrentCards, winningCard);
                        Player winner = this.CurrentPlayers[winningCardIdx];

                        // TODO: Get winner

                        // Increment number of wins for their team
                        this.bidScore[(int)winner.TeamNumber]++;

                        // Fire off the event and log it
                        FireGameActionEvent(new GameActionEventArgs() { Player = winner, BidScores = this.bidScore });
                        this.Logs.Add(new Logging.TrickCompletedLog() { Player = winner });

                        // The winner gets to play the next card
                        this._currentPlayer = winner;

                        // Move back to the trick stage by default
                        this.CurrentState = State.TRICK;

                        // Reset card counter
                        this._cardsPlayedInRound = 0;

                        // Always play to 13 rounds
                        if (this.TrickCounter == HAND_SIZE)
                        {
                            // Bid reached
                            this.CurrentState = State.BID_COMPLETE;
                            this.TrickCounter = 0; // Reset counter
                            this.Next();
                        }
                    }
                    break;

                case State.BID_COMPLETE:
                    {
                        //TODO: Scoring fix
                        Enums.Team winningTeam =
                            this.bidScore[(int)Enums.Team.Blue] > this.bidScore[(int)Enums.Team.Red]
                            ? Enums.Team.Blue
                            : Enums.Team.Red;
                        //TODO: Elaborate on XOR 
                        Enums.Team losingTeam = (Enums.Team)((int)winningTeam ^ 1);
                        int score = currentBid.HighestBid;

                        // Update team scores
                        this.teamScore[(int)winningTeam] += score;
                        this.teamScore[(int)losingTeam] -= score;

                        // Reset number of wins for the bid
                        this.bidScore[(int)Enums.Team.Blue] = 0;
                        this.bidScore[(int)Enums.Team.Red] = 0;

                        this.TrickCounter = 0;

                        // TODO: Log bid completion
                        this.Logs.Add(new Logging.BidCompleteLog()
                        {
                            Score = score,
                            WinningTeam = winningTeam,
                            LosingTeam = losingTeam
                        }
                        );

                        // Fire game action
                        FireGameActionEvent(new GameActionEventArgs()
                            {
                                Player = this._currentPlayer,
                                Score = score,
                                WinningTeam = winningTeam,
                                LosingTeam = losingTeam,
                                TeamScores = this.teamScore
                            }
                        );


                        // Score reached?
                        if (this.teamScore[(int)Enums.Team.Blue] >= this.MaxScore
                                || this.teamScore[(int)Enums.Team.Red] >= this.MaxScore)
                        {
                            // Game is done.
                            this.CurrentState = State.DONE;
                        }
                        else
                        {
                            // Start a new bid; winner (who is current player) gets first bid
                            this._bids.Add(new Bid(this.Players));

                            // Re-deal cards
                            this.DealCards();

                            this.CurrentState = State.BID_STAGE;
                        }
                        this.Next();
                    }
                    break;

                case State.DONE:
                    {
                        Enums.Team winningTeam =
                            this.teamScore[(int)Enums.Team.Blue] > this.teamScore[(int)Enums.Team.Red]
                            ? Enums.Team.Blue
                            : Enums.Team.Red;
                        Enums.Team losingTeam = (Enums.Team)((int)winningTeam ^ 1);

                        // TODO: Log game competion
                        FireGameActionEvent(new GameActionEventArgs()
                            {
                                WinningTeam = winningTeam,
                                LosingTeam = losingTeam,
                                TeamScores = teamScore
                            }
                        );
                    }
                    break;
            }
            FirePlayerTurnEvent();
        }
        #region Helper functions
        /// <summary>
        /// Move on to the next player's trick, in counter-clockwise direction.
        /// </summary>
        private Player NextPlayer()
        {
            return this.NextPlayer(this._currentPlayer);
        }

        /// <summary>
        /// Get the player that goes after the given player.
        /// </summary>
        /// <param name="player">The starting player.</param>
        /// <returns></returns>
        private Player NextPlayer(Player player)
        {
            int nextPlayerIdx = Array.IndexOf(this.Players, player) - 1;
            /* If it underflows, the next player is the last one in the array
             * (since it is counter-clockwise).
             */
            if (nextPlayerIdx == -1)
            {
                nextPlayerIdx = 3;
            }
            return this.Players[nextPlayerIdx];
        }

        /// <summary>
        /// (Re-)deal cards to all players.
        /// </summary>
        private void DealCards()
        {
            // Get hands for each player
            Deck[] hands = this.CreateHands();

            Player dealer = this._currentPlayer;
            int handsDealt = 0;

            // Give the hands out
            do
            {
                this._currentPlayer.HandList = hands[handsDealt];
                this._currentPlayer = this.NextPlayer(); // Deal cards to the next player
            } while (++handsDealt != NUMBER_OF_PLAYERS);

            // Player to the right of the dealer goes first
            this._currentPlayer = this.NextPlayer(dealer); //TODO: What about bid winner?
        }

        /// <summary>
        /// Split a deck and create hands for each player.
        /// </summary>
        /// <returns>An array of four decks.</returns>
        private Deck[] CreateHands()
        {
            Deck deck = new Deck();
            Deck[] hands = new Deck[NUMBER_OF_PLAYERS];
            deck.Shuffle();

            for (int hand = 0; hand < hands.Length; hand++)
            {
                hands[hand] = deck.Draw(HAND_SIZE);
            }

            return hands;
        }
        #endregion
        #endregion
    }
}
