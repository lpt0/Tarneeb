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
        #region Enums
        /// <summary>
        /// The outcome of the game.
        /// </summary>
        public enum Outcome
        {
            /// <summary>
            /// The player won the game.
            /// </summary>
            WIN,

            /// <summary>
            /// The player lost the game.
            /// </summary>
            LOSS
        }

        /// <summary>
        /// Enum for representing game state.
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

        /// <summary>
        /// The difficulty level of the game.
        /// </summary>
        public enum Difficulty
        {
            /// <summary>
            /// A game composed of easy CPU players.
            /// </summary>
            EASY,

            /// <summary>
            /// A game composed of medium difficulty CPU players.
            /// </summary>
            MEDIUM,

            /// <summary>
            /// A game with hard CPU players.
            /// </summary>
            HARD
        }
        #endregion

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

        #region Variables/fields
        #region Internal variables
        /// <summary>
        /// Number of bids placed.
        /// </summary>
        private int _bidCount = 0;

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
        private readonly int[] bidScore = new int[NUMBER_OF_TEAMS];

        /// <summary>
        /// The overall team score, across all bids.
        /// </summary>
        private readonly int[] teamScore = new int[NUMBER_OF_TEAMS];

        /// <summary>
        /// Internal list of bids for this game.
        /// </summary>
        private readonly List<Bid> _bids;

        /// <summary>
        /// Internal list of rounds for this game.
        /// </summary>
        private readonly List<Round> _tricks;
        #endregion
        #region Public fields
        /// <summary>
        /// Identifier for the current game, for database insertions.
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// Current game state.
        /// The state can be read by any class, but can only be changed inside of the class.
        /// </summary>
        public State CurrentState { get; private set; }

        /// <summary>
        /// The players of the current game.
        /// </summary>
        public Player[] Players { get; }

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
        /// The difficulty level of this game.
        /// </summary>
        public Difficulty DifficultyLevel { get; private set; }

        /// <summary>
        /// Tricks/rounds in the game.
        /// </summary>
        public List<Round> Tricks { get => this._tricks; }

        /// <summary>
        /// Bids placed in this game.
        /// </summary>
        public List<Bid> Bids { get => this._bids; }

        /// <summary>
        /// Game logs.
        /// </summary>
        public ObservableCollection<Log> Logs { get; private set; }
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
        public void FireNewGameEvent()
        {
            this.AddLog($"An {this.DifficultyLevel.ToString().ToLower()} game was started.");
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
        /// <summary>
        /// A player has placed a bid; it needs to be validated and placed.
        /// </summary>
        /// <param name="args">The arguments with the player's bid.</param>
        private void OnPlayerBid(PlayerActionEventArgs args)
        {
            Bid bid = this._bids[this._bids.Count - 1];

            // Validate bid
            // Bid class will validate bid, and move on to the next player if needed
            Player nextPlayer = bid.Bids(this._currentPlayer, args.Bid);

            // If the bid was invalid, the next bidder is the same player
            // And if it is invalid, don't log the bid, and don't send the event out
            if (this._currentPlayer != nextPlayer || _bidCount >= 3) 
            {
                // Create the log
                string action = $"{this._currentPlayer.PlayerName} ";
                if (args.Bid != -1)
                {
                    action += $"has placed a bid of {args.Bid}.";
                }
                else
                {
                    action += "has passed on their bid.";
                }
                this.AddLog(action);
                FireGameActionEvent(new GameActionEventArgs() { Player = this._currentPlayer, Bid = args.Bid });
                _bidCount++;
            }

            this._currentPlayer = nextPlayer;
        }

        /// <summary>
        /// A player has decided on a Tarneeb suit. 
        /// Set the Tarneeb suit for the current bid.
        /// </summary>
        /// <param name="args">The arguments, with the chosen Tarneeb suit.</param>
        private void OnPlayerTarneeb(PlayerActionEventArgs args)
        {
            // Validate Tarneeb suit
            // Assume the suit is valid for a local game
            Bid bid = this._bids[this._bids.Count - 1];
            bid.DecideTarneebSuit(args.Tarneeb);
            this.AddLog($"{this._currentPlayer.PlayerName} has decided on {args.Tarneeb} for the trump suit.");
            FireGameActionEvent(new GameActionEventArgs() { Player = this._currentPlayer, Bid = bid.HighestBid, Tarneeb = args.Tarneeb });
        }

        /// <summary>
        /// A player has played a card during a trick.
        /// Validate whether the card can be played, and add it to the cards played.
        /// </summary>
        /// <param name="args">The arguments, with the card that was played.</param>
        private void OnPlayerTrick(PlayerActionEventArgs args)
        {
            /* Make sure the card is valid; as in, it is allowed to be played
             * (card is of leading suit, Tarneeb suit, or player has neither and can play
             * anything).
             */
            if (this.GetValidCards(this._currentPlayer).Contains(args.CardPlayed))
            {
                this.CurrentPlayers[_cardsPlayedInRound] = this._currentPlayer;
                this.CurrentCards[_cardsPlayedInRound] = args.CardPlayed;
                _cardsPlayedInRound++;

                /* Remove the card from the player's hand (again, just in case)
                 * Players may or may not have already done this, but better to do it
                 * just in case.
                 */
                this._currentPlayer.HandList.Cards.Remove(args.CardPlayed);
                this.AddLog($"{this._currentPlayer.PlayerName} has played a {args.CardPlayed}");
                FireGameActionEvent(new GameActionEventArgs() { Player = this._currentPlayer, Card = args.CardPlayed, CardsPlayedInRound = this._cardsPlayedInRound - 1 });

                this._currentPlayer = this.NextPlayer();
            }
            else
            {
                // Card was not valid; let the player know
                throw new GameException("Card is not allowed to be played.");

                // Fall-through without moving to next player
            }
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
                            this.OnPlayerTarneeb(args);
                            break;

                        case Game.State.TRICK:
                            this.OnPlayerTrick(args);
                            break;
                    }
                } 
                catch (GameException e)
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
        /// <param name="difficulty">The difficulty level of the game.</param>
        /// <param name="maxScore">The score of the game to be reached before the game is won.</param>
        public Game(int maxScore = 41, Difficulty difficulty = Difficulty.EASY)
        {
            // Initialize all fields
            this.Players = new Player[NUMBER_OF_PLAYERS];
            this.CurrentCards = new Card[NUMBER_OF_PLAYERS];
            this.CurrentPlayers = new Player[NUMBER_OF_PLAYERS];
            this.Logs = new ObservableCollection<Log>();
            this._bids = new List<Bid>();
            this._tricks = new List<Round>();
            this.CurrentState = State.NEW_GAME;
            this.TrickCounter = 0;
            this.MaxScore = maxScore;
            this.DifficultyLevel = difficulty;

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
        public HumanPlayer Initialize(string playerName)
        {
            // Set up the database, add a new game
            Database.Connect();
            Database.Initialize();
            this.ID = Database.InsertGame(DateTime.Now);

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
            string initialHandLog = "Your initial hand was:";
            foreach (Card card in user.HandList.Cards)
            {
                initialHandLog += $"\n{card}";
            }
            this.AddLog(initialHandLog);

            // Log the player join
            this.AddLog($"{user.PlayerName} has joined.");
            FireGameActionEvent(new GameActionEventArgs() { Player = user });

            // Create CPU players
            for (int playerNum = 1; playerNum < NUMBER_OF_PLAYERS; playerNum++)
            {
                CPUPlayer player;
                string cpuPlayerName = $"Player {playerNum + 1}";
                Enums.Team team = (Enums.Team)(playerNum % 2);
                Deck hand = hands[playerNum];

                // Determine what level of CPU player is needed, and create them
                switch (this.DifficultyLevel)
                {
                    case Difficulty.EASY:
                        player = new CPUPlayerEasy(
                            this, // Need to pass the game so CPU knows how to listen to event
                            cpuPlayerName,
                            playerNum,
                            team,
                            hand
                        );
                        break;
                    case Difficulty.MEDIUM:
                        player = new CPUPlayerMedium(
                            this,
                            cpuPlayerName,
                            playerNum,
                            team,
                            hand
                        );
                        break;
                    case Difficulty.HARD:
                        player = new CPUPlayerHard(
                            this,
                            cpuPlayerName,
                            playerNum,
                            team,
                            hand
                        );
                        break;
                    default: // Unknown CPU level
                        player = player = new CPUPlayer(
                            this,
                            cpuPlayerName,
                            playerNum,
                            team,
                            hand
                        );
                        break;
                }
                this.Players[playerNum] = player;

                this.AddLog($"{player.PlayerName} has joined.");
                FireGameActionEvent(new GameActionEventArgs() { Player = player});
            }

            // Setup event listeners
            foreach (Player p in this.Players)
            {
                p.PlayerActionEvent += this.OnPlayerAction;
            }

            this.CurrentState = State.BID_STAGE;

            // Pick a random player as dealer
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
                    { // Scope control
                        // Perform end of round tasks
                        Round round = new Round(
                            currentBid.TarneebSuit,
                            this.CurrentCards[0],
                            this.CurrentCards[1],
                            this.CurrentCards[2],
                            this.CurrentCards[3]
                        );
                        this.TrickCounter++;

                        /* Determine round winning card, and get the player who played that
                         * That player wins the trick.
                         */
                        Card winningCard = round.WinCard(round);
                        int winningCardIdx = Array.IndexOf(this.CurrentCards, winningCard);
                        Player winner = this.CurrentPlayers[winningCardIdx];

                        // Increment number of wins for their team
                        this.bidScore[(int)winner.TeamNumber]++;

                        // Fire off the event and log it
                        FireGameActionEvent(new GameActionEventArgs() { Player = winner, BidScores = this.bidScore });
                        this.AddLog($"{winner.PlayerName} has won the trick.");

                        // The winner gets to play the next card
                        this._currentPlayer = winner;

                        // Move back to the trick stage by default
                        this.CurrentState = State.TRICK;

                        // Reset card counter and re-initialize cards in play
                        this._cardsPlayedInRound = 0;
                        this.CurrentCards = new Card[NUMBER_OF_PLAYERS];

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
                        Enums.Team winningTeam;
                        Enums.Team playerTeam = currentBid.WinningPlayer.TeamNumber;
                        // If the player's team reached the bidded value...
                        if (this.bidScore[(int)playerTeam] >= currentBid.HighestBid)
                        {
                            // ...then they win
                            winningTeam = playerTeam;
                        }
                        // Otherwise, the team did NOT reach the bid value
                        else
                        {
                            /* ...so they lose.
                             * The XOR acts as a NOT on the player team
                             */
                            winningTeam = (Enums.Team)((int)playerTeam ^ 1);
                        }
                        // And the losing team is the opposite of the winner.
                        Enums.Team losingTeam = (Enums.Team)((int)winningTeam ^ 1);
                        int score = 13; // Score is always 13, win condition depends on if bid was reached

                        // Update team scores
                        this.teamScore[(int)winningTeam] += score;
                        this.teamScore[(int)losingTeam] -= score;

                        // Reset number of wins for the bid
                        this.bidScore[(int)Enums.Team.Blue] = 0;
                        this.bidScore[(int)Enums.Team.Red] = 0;

                        this.TrickCounter = 0;

                        this.AddLog($"{winningTeam} has won the bid with a score of {score}.");

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

                        // Did the player win?
                        if (winningTeam == this.Players[0].TeamNumber)
                        {
                            Database.InsertOutcome(DateTime.Now, this.ID, Outcome.WIN);
                        }
                        // Player lost
                        else
                        {
                            Database.InsertOutcome(DateTime.Now, this.ID, Outcome.LOSS);
                        }
                        this.AddLog($"Team {winningTeam} won the game.");

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
        /// <returns>The player who plays next.</returns>
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
        /// Get a player's teammate.
        /// </summary>
        /// <param name="player">The player to get the teammate for.</param>
        /// <returns>The teammate.</returns>
        public Player GetTeammate(Player player)
        {
            int currentPlayerIdx = Array.IndexOf(this.Players, player);
            var teammate = player;
            switch (currentPlayerIdx)
            {
                case 0:
                    teammate = this.Players[2];
                    break;
                case 1:
                    teammate = this.Players[3];
                    break;
                case 2:
                    teammate = this.Players[0];
                    break;
                case 3:
                    teammate = this.Players[1];
                    break;
            }
            return teammate;
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
                // Deal cards to the next player
                this._currentPlayer = this.NextPlayer(); 
            } while (++handsDealt != NUMBER_OF_PLAYERS);

            // Player to the right of the dealer goes first
            this._currentPlayer = this.NextPlayer(dealer); 
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
                // Sort the hand
                hands[hand].Sort();
            }

            return hands;
        }

        /// <summary>
        /// Get the valid cards that a player has.
        /// Valid cards are those that are of the leading suit or Tarneeb suit.
        /// 
        /// If the player does not have cards of either suit, all cards are valid;
        /// they just don't have a score.
        /// </summary>
        /// <param name="player">The player to check cards for.</param>
        /// <returns>The valid cards.</returns>
        public List<Card> GetValidCards(Player player)
        {
            Enums.CardSuit? leadingSuit = this.CurrentCards[0]?.Suit;
            Enums.CardSuit? tarneebSuit = this.Bids[this.Bids.Count - 1]?.TarneebSuit;

            // If there is no leading suit, that means this player decides one; so all cards are valid.
            if (leadingSuit == null)
            {
                return player.HandList.Cards;
            }
            // Otherwise, there is a leading suit, and it needs to be followed
            else
            {
                List<Card> validCards = player.HandList.Cards
                    .FindAll(card => (card.Suit == leadingSuit || card.Suit == tarneebSuit));

                // Are there no valid cards?
                if (validCards.Count == 0)
                {
                    // There are no cards with the leading or Tarneeb suit, so all cards are valid
                    validCards = player.HandList.Cards;
                }

                return validCards;
            }
        }

        /// <summary>
        /// Create a log for the given action, and add it to the list and database of logs.
        /// </summary>
        /// <param name="action">The action to create a log for and add.</param>
        private void AddLog(string action)
        {
            var log = new Log(this.ID, action);
            this.Logs.Add(log);
            Database.InsertLog(log);
        }
        #endregion
        #endregion
    }
}
