using System;
using System.Collections.Generic;
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
        /* TODO:
         * There are two ways (that I can think of) to perform game logic.
         * 1.
         *  It can be performed in a class like this. However, drawing to screen
         *  becomes a bit more complicated.
         *  Drawing can be done by having the GUI logic read the last game log,
         *  and draw an action based on that.
         * 2.
         *  GUI logic can also handle game logic. That way, it knows exactly
         *  what the last action was, and what needs to be displayed based on that.
         *  The downside of this is that AI logic may become more complicated to
         *  work on - the AI would need to read the game logs, so the GUI would
         *  need to maintain them and have them accessible by other classes.
         *  This could be create a circular dependency issue, since the AI is in
         *  the TarneebClasses project, and the GUI is in a separate project that
         *  requires the TarneebClasses project. 
         *  If the AI were to read logs from the GUI, TarneebClasses needs access to
         *  the GUI project, and the GUI project needs access to the TarneebClasses project.
         *  OR, could also use events; emit an event that the GUI listens for.
         *  But, what if too many events happen at once?
         */

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
            /// The bid value has been reached.
            /// </summary>
            BID_REACHED,

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
        /// The score that needs to be reached to win.
        /// </summary>
        public const int MAX_SCORE = 41;
        #endregion

        #region Fields
        #region Internal fields
        /// <summary>
        /// Hardcoded list of CPU player names.
        /// TODO: Remove in final.
        /// </summary>
        private static readonly string[] cpuNames = { "Jim", "Tim", "Bim" };

        /// <summary>
        /// Game logs; retrievable by using GetLog and SetLog.
        /// </summary>
        private List<Logging.ILog> logs; //TODO: Public get and private set, and block Add access for get

        /// <summary>
        /// The player that gets to make the next move (trick, bid, etc).
        /// </summary>
        private Player currentPlayer;

        /// <summary>
        /// The number of cards played in the round.
        /// </summary>
        private int cardsPlayedInRound = 0;

        /// <summary>
        /// The score for the current bid.
        /// </summary>
        private int[] bidScore; // TODO: What is init state?

        /// <summary>
        /// The overall team score, across all bids.
        /// </summary>
        private int[] teamScore;

        #endregion
        #region Public fields
        /// <summary>
        /// Current game state.
        /// The state can be read by any class, but can only be changed inside of the class.
        /// </summary>
        public State CurrentState { get; private set; }

        /// <summary>
        /// Whether the game has been completed or not.
        /// </summary>
        public Boolean IsComplete { get; private set; }

        /// <summary>
        /// The non-AI player. TODO
        /// </summary>
        public HumanPlayer User { get; }

        /// <summary>
        /// The players of the current game.
        /// </summary>
        public Player[] Players { get; }

        /// <summary>
        /// The game's deck.
        /// </summary>
        public Deck Deck { get; }

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
        /// The current Tarneeb (trump) suit.
        /// </summary>
        public Enums.CardSuit CurrentTarneeb { get; private set; }

        /// <summary>
        /// TODO
        /// </summary>
        public List<Round> Rounds { get; }

        /// <summary>
        /// TODO
        /// TODO: Block public Add access
        /// </summary>
        public List<Bid> Bids { get; }
        #endregion
        #endregion

        #region Events
        #region Event objects
        /// <summary>
        /// Triggered when a new game is started.
        /// </summary>
        public event EventHandler<NewGameEventArgs> NewGameEvent;

        /// <summary>
        /// Triggered when a new player joins.
        /// </summary>
        public event EventHandler<NewPlayerEventArgs> NewPlayerEvent;

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

        #region Old events
        /// <summary>
        /// Triggered when a card is played.
        /// </summary>
        public event EventHandler<CardPlayedEventArgs> CardPlayedEvent;

        /// <summary>
        /// Triggered when it is a player's turn.
        /// Player classes should listen for this to be fired, then check if
        /// the `Player` field is their player.
        /// </summary>
        public event EventHandler<PlayerTrickTurnEventArgs> PlayerTrickTurnEvent;

        /// <summary>
        /// Triggered when a trick is completed.
        /// </summary>
        public event EventHandler<TrickCompleteEventArgs> TrickCompleteEvent;

        /// <summary>
        /// Triggered when a bid is reached (finished).
        /// </summary>
        public event EventHandler<BidFinishedEventArgs> BidFinishedEvent;
        #endregion
        #endregion
        #region Event triggers
        /// <summary>
        /// Fire a new game event.
        /// </summary>
        /// <param name="args">The arguments for the event.</param>
        public void FireNewGameEvent()
        {
            this.logs.Add(new Logging.NewGameLog());
            NewGameEvent?.Invoke(this, new NewGameEventArgs());
        }

        /// <summary>
        /// Fire a new player event.
        /// </summary>
        /// <param name="args">The arguments for the event.</param>
        public void FireNewPlayerEvent(NewPlayerEventArgs args)
        {
            this.logs.Add(new Logging.PlayerJoinedLog() { Player = args.Player });
            this.NewPlayerEvent?.Invoke(this, args);
        }
        #endregion
        #endregion

        #region Constructor
        /// <summary>
        /// Create a new game.
        /// </summary>
        public Game()
        {
            // Initialize all fields
            this.Deck = new Deck();
            this.Players = new Player[NUMBER_OF_PLAYERS];
            this.CurrentCards = new Card[NUMBER_OF_PLAYERS];
            this.CurrentPlayers = new Player[NUMBER_OF_PLAYERS];
            this.logs = new List<Logging.ILog>(); // TODO
            this.Bids = new List<Bid>();
            this.Rounds = new List<Round>();
            this.CurrentState = State.NEW_GAME;
            this.TrickCounter = 0;

            this.bidScore = new int[NUMBER_OF_TEAMS];
            this.teamScore = new int[NUMBER_OF_TEAMS];

            this.Deck.Shuffle();
        }
        #endregion

        #region Event listeners
        /// <summary>
        /// Executes when a player performs an action.
        /// </summary>
        /// <param name="sender">The player that performed the action.</param>
        /// <param name="args">The arguments for the action.</param>
        public void OnPlayerAction(object sender, PlayerActionEventArgs args)
        {
            // Is it that player's turn?
            if (sender == this.currentPlayer)
            {
                Bid bid = this.Bids[this.Bids.Count - 1];
                switch (this.CurrentState)
                {
                    case Game.State.BID_STAGE:
                        // Validate bid
                        // Bid class will validate bid, and move on to the next player if needed
                        Player nextPlayer = bid.Bids(this.currentPlayer, args.Bid);

                        this.logs.Add(new Logging.BidPlacedLog() { Player = this.currentPlayer, Bid = args.Bid });
                        FireGameActionEvent(new GameActionEventArgs() { Player = this.currentPlayer, Bid = args.Bid });

                        this.currentPlayer = nextPlayer;
                        break;

                    case Game.State.BID_WON:
                        // Validate Tarneeb suit
                        // Assume it's valid for local player
                        bid.DecideTarneebSuit(args.Tarneeb);
                        //TODO: Log
                        FireGameActionEvent(new GameActionEventArgs() { Player = this.currentPlayer, Tarneeb = args.Tarneeb });
                        break;

                    case Game.State.TRICK:
                        // Validate card played
                        // Local play game, assume card is valid
                        this.CurrentPlayers[cardsPlayedInRound] = this.currentPlayer as Player;
                        this.CurrentCards[cardsPlayedInRound] = args.CardPlayed;
                        cardsPlayedInRound++;

                        this.logs.Add(new Logging.CardPlayedLog() { Player = this.currentPlayer, Card = args.CardPlayed });
                        FireGameActionEvent(new GameActionEventArgs() { Player = this.currentPlayer, Card = args.CardPlayed });

                        this.currentPlayer = this.nextPlayer();
                        break;
                }
                this.Next();
            }
            else
            {
                throw new Exception("Illegal action!");
            }
        }
        #region Unused code
        public void OnPlayerPlacedBid(Player sender, PlayerPlaceBidEventArgs args)
        {
            if (sender == currentPlayer)
            {
                // Record the bid
                //TODO

                // Fire the bid placed event
            }
            else
            {
                Console.WriteLine($"{sender.PlayerName} tried to place a bid when it isn't their turn!");

                // Don't fire the event; it's not a legal bid
            }
        }

        public void OnPlayerPlayedCard(Player sender, PlayerPlayCardEventArgs args)
        {
            // Is it this player's turn?
            if (sender == currentPlayer)
            {
                // Play the card
                //TODO
            }
            else
            {
                Console.WriteLine($"{sender.PlayerName} tried to play a card, when it isn't their turn!");
            }
        }

        public void OnPlayerDecidedTarneebSuit(Player sender, PlayerDecideTarneebEventArgs args)
        {
            // Is this the winning player on the bid, and is there no trump suit?
            //TODO
            if (true)
            {

            }
        }
        #endregion
        #endregion

        #region Functions
        /// <summary>
        /// Initialize game state.
        /// </summary>
        /// <param name="playerName">The name of the user.</param>
        /// <returns>The user's created player.</returns>
        public HumanPlayer Initialize(String playerName)
        {
            int handSize = this.Deck.Cards.Count / NUMBER_OF_PLAYERS;
            FireNewGameEvent();

            // Create the user's player
            HumanPlayer user = new HumanPlayer(
                playerName,
                0,
                Enums.Team.Blue,
                this.Deck.Draw(handSize)
            );
            this.Players[0] = user;
            this.logs.Add(new Logging.PlayerJoinedLog() { Player = user });
            this.logs.Add(new Logging.InitialHandLog() { Hand = user.HandList, Player = user });
            FireGameActionEvent(new GameActionEventArgs() { Player = user });

            // Create CPU players
            for (int playerNum = 1; playerNum < NUMBER_OF_PLAYERS; playerNum++)
            {
                this.Players[playerNum] = new CPUPlayer(
                    this, // Need to pass the game so CPU knows how to listen to event
                    Game.cpuNames[playerNum - 1],
                    playerNum,
                    (Enums.Team)(playerNum % 2),
                    this.Deck.Draw(handSize)
                );
                this.logs.Add(new Logging.PlayerJoinedLog() { Player = this.Players[playerNum] });
                // don't leak hand dealt to CPU
                FireGameActionEvent(new GameActionEventArgs() { Player = this.Players[playerNum] });
            }

            // Setup listeners
            foreach (Player p in this.Players)
            {
                p.PlayerActionEvent += this.OnPlayerAction;
            }

            this.CurrentState = State.BID_STAGE;
            // TODO: Dealer?
            this.currentPlayer = this.Players[0];
            this.Bids.Add(new Bid(this.Players));

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
        /// Move on to the next action in the game.
        /// </summary>
        private void Next()
        {
            Bid currentBid = this.Bids[this.Bids.Count - 1];
            switch (this.CurrentState)
            {
                case State.NEW_GAME:
                    // Condition: all players must join
                    break;
                case State.BID_STAGE:
                    // Condition: bid must be complete
                    if (currentBid.WinningPlayer != null)
                    {
                        // Bid is done; winner picks trump suit
                        this.currentPlayer = currentBid.WinningPlayer;
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
                    if (cardsPlayedInRound == NUMBER_OF_PLAYERS)
                    {
                        this.CurrentState = State.TRICK_COMPLETE;
                        this.Next();
                    }
                    // TODO: Handle card dealing
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
                        FireGameActionEvent(new GameActionEventArgs() { Player = winner });

                        // Increment number of wins for their team
                        this.bidScore[(int)winner.TeamNumber]++;

                        // TODO: Log trick completion
                        this.logs.Add(new Logging.TrickCompletedLog() { Player = winner });

                        // The winner gets to play the next card
                        this.currentPlayer = winner;

                        // Move back to the trick stage by default
                        this.CurrentState = State.TRICK;

                        // Reset card counter
                        this.cardsPlayedInRound = 0;

                        if (this.TrickCounter == currentBid.HighestBid)
                        {
                            // Bid reached
                            this.CurrentState = State.BID_REACHED;
                            this.Next();
                        }
                    }
                    break;

                case State.BID_REACHED:
                    {
                        Enums.Team winningTeam =
                            this.teamScore[(int)Enums.Team.Blue] > this.teamScore[(int)Enums.Team.Red]
                            ? Enums.Team.Blue
                            : Enums.Team.Red;
                        //TODO: Elaborate on XOR 
                        Enums.Team losingTeam = (Enums.Team)((int)winningTeam ^ 1);
                        int score = currentBid.HighestBid;

                        // Update team scores
                        this.teamScore[(int)winningTeam] += score;
                        this.teamScore[(int)losingTeam] -= score;

                        // Reset number of bid wins
                        this.bidScore[(int)Enums.Team.Blue] = 0;
                        this.bidScore[(int)Enums.Team.Red] = 0;

                        // TODO: Log bid completion

                        // Fire game action
                        FireGameActionEvent(new GameActionEventArgs()
                        {
                            Player = this.currentPlayer,
                            Score = score,
                            WinningTeam = winningTeam,
                            LosingTeam = losingTeam
                        }
                        );


                        // Score reached?
                        if (this.teamScore[(int)Enums.Team.Blue] >= MAX_SCORE
                                || this.teamScore[(int)Enums.Team.Red] >= MAX_SCORE)
                        {
                            // Game is done.
                            this.CurrentState = State.DONE;
                        }
                        else
                        {
                            // Start a new bid; winner (who is current player) gets first bid
                            this.Bids.Add(new Bid(this.Players));
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
        private Player nextPlayer()
        {
            int nextPlayerIdx = Array.IndexOf(this.Players, this.currentPlayer) - 1;
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
        /// Raise the PlayerTurnEvent.
        /// </summary>
        private void FirePlayerTurnEvent()
        {
            // All arguments are the same
            this.PlayerTurnEvent?.Invoke(
                this,
                new PlayerTurnEventArgs() { Player = this.currentPlayer, State = this.CurrentState }
            );
        }

        private void FireGameActionEvent(GameActionEventArgs args)
        {
            // Set state, just in case TODO
            args.State = this.CurrentState;
            this.GameActionEvent?.Invoke(this, args);
        }
        #endregion
        

        /// <summary>
        /// Get a log of all of the actions for this game.
        /// </summary>
        /// <returns>The game logs.</returns>
        public List<Logging.ILog> GetLogs()
        {
            // TODO: Block Add
            return this.logs;
        }
        #endregion
    }
}
