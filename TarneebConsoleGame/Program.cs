using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TarneebClasses;
using TarneebClasses.Events;

namespace TarneebTest
{
    /// <summary>
    /// Test class for the Tarneeb classes.
    /// </summary>
    class TarneebTest
    {
        static void PrintHeader()
        {
            Console.WriteLine("+-----------------+");
            Console.WriteLine("|     TARNEEB     |");
            Console.WriteLine("+-----------------+");
        }

        static void PrintPlayerCards(Deck deck)
        {
            for (int i = 0; i < deck.Cards.Count; i++)
            {
                Console.WriteLine($"[{i}] {deck.Cards[i]}");
            }
        }

        static int GetStdinInt(int limit)
        {
            int number;
            while (!int.TryParse(Console.ReadLine(), out number) && (number > limit || number < 0)) //TODO
            {
                Console.WriteLine($"Invalid number. Must be less than {limit}. Please try again.");
            }
            return number;
        }

        private static Player consoleUser;
        private static Game game;

        // TODO Add to Player class
        /// <summary>
        /// Raised when this player places a bid.
        /// </summary>
        public static event EventHandler<PlayerPlaceBidEventArgs> PlayerPlacesBidEvent;

        /// <summary>
        /// Raised when this player plays a card.
        /// </summary>
        public static event EventHandler<PlayerPlayCardEventArgs> PlayerPlaysCardEvent;

        /// <summary>
        /// Raised when this player decides on a tarneeb suit.
        /// </summary>
        public static event EventHandler<PlayerDecideTarneebEventArgs> PlayerDecidesTarneebEvent;

        #region Old event handlers
        /// <summary>
        /// Executes when it's this player's turn to play a trick.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        static void OnPlayerTrickTurn(object sender, PlayerTrickTurnEventArgs args)
        {
            // Is it my turn?
            if (args.Player == consoleUser)
            {
                // TODO: Need to find low suit
                Console.WriteLine("Your turn!");
                PrintPlayerCards(consoleUser.HandList);
                Console.Write("Enter a number from the above list: ");
                Card playedCard = consoleUser.HandList.Pick(GetStdinInt(consoleUser.HandList.Cards.Count));

                // Raise an event so the game knows that I played a card
                PlayerPlaysCardEvent?.Invoke(consoleUser, new PlayerPlayCardEventArgs { Card = playedCard });
            }
            else
            {
                // Some other player's turn
                Console.WriteLine($"{args.Player.PlayerName}'s turn!");
            }
        }

        /// <summary>
        /// Executes when it's this player's turn to place a bid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name=""></param>
        static void OnPlayerBidTurn(object sender, PlayerBidTurnEventArgs args)
        {
            // My turn?
            if (args.Player == consoleUser)
            {
                // Place my bid
                Console.Write("Place a bid: ");
                int bid = int.Parse(Console.ReadLine());

                // Fire the event off
                PlayerPlacesBidEvent?.Invoke(consoleUser, new PlayerPlaceBidEventArgs { Bid = bid });
            } else
            {
                // Somebody else's turn
                Console.WriteLine($"{args.Player.PlayerName} is placing their bid...");
            }
        }

        static void OnCardPlayed(object sender, CardPlayedEventArgs eventArgs)
        {
            Console.WriteLine($"{eventArgs.Player.PlayerName} played {eventArgs.Card}.");
        }

        static void OnTrickCompleted(object sender, TrickCompleteEventArgs eventArgs)
        {
            Console.WriteLine($"Trick completed TODO");
        }
        #endregion

        static void OnNewPlayer(object sender, NewPlayerEventArgs args)
        {
            Console.WriteLine($"{args.Player.PlayerName} joined.");
        }

        static void OnNewGame(object sender, NewGameEventArgs eventArgs)
        {
            Console.WriteLine($"A new game was started at {DateTime.Now}.");
        }

        /// <summary>
        /// Executes when it is a player's turn.
        /// </summary>
        /// <param name="sender">The game object.</param>
        /// <param name="args">The arguments.</param>
        static void OnPlayerTurn(object sender, PlayerTurnEventArgs args)
        {
            if (args.Player == consoleUser)
            {
                switch (args.State)
                {
                    case Game.State.BID_STAGE:
                        Console.Write("Enter your bid: ");
                        int bid = 7;// GetStdinInt(4);
                        consoleUser.PerformAction(new PlayerActionEventArgs() { Bid = bid });
                        break;

                    case Game.State.BID_WON:
                        // TODO
                        Console.WriteLine("Suits:");
                        for (int i = (int)Enums.CardSuit.Spades; i <= (int)Enums.CardSuit.Club; i++)
                        {
                            Console.Write($"[{i}] {(Enums.CardSuit)i} ");
                        }
                        Console.Write("\nEnter the Tarneeb suit: ");
                        Enums.CardSuit suit = Enums.CardSuit.Diamond; // (Enums.CardSuit)int.Parse(Console.ReadLine());
                        consoleUser.PerformAction(new PlayerActionEventArgs() { Tarneeb = suit });
                        break;

                    case Game.State.TRICK:
                        Console.WriteLine("Your turn!");
                        PrintPlayerCards(consoleUser.HandList);
                        Console.Write("Pick a card: ");
                        int cardPickedIdx = int.Parse(Console.ReadLine());
                        consoleUser.PerformAction(
                            new PlayerActionEventArgs() {
                                CardPlayed = consoleUser.HandList.Pick(cardPickedIdx)
                            }
                        );
                        break;

                    default:
                        throw new Exception("Unknown stage!");
                        break;
                }
            }
        }

        /// <summary>
        /// Executes when a game action occurs.
        /// </summary>
        /// <param name="sender">The game object.</param>
        /// <param name="args">The arguments</param>
        static void OnGameAction(object sender, GameActionEventArgs args)
        {
            switch (args.State)
            {
                case Game.State.NEW_GAME:
                    Console.WriteLine($"{args.Player.PlayerName} joined.");
                    break;
                case Game.State.BID_STAGE:
                    Console.WriteLine($"{args.Player.PlayerName} bid {args.Bid}.");
                    break;
                case Game.State.BID_WON:
                    Console.WriteLine($"{args.Player.PlayerName} (team {args.Player.TeamNumber}) has bid the highest!");
                    Console.WriteLine($"The trump suit is {args.Tarneeb}.");
                    break;
                //case Game.State.TARNEEB_SUIT:
                //    Console.WriteLine($"{args.Player.PlayerName} has decided on {args.Tarneeb} as the trump suit.");
                //    break;
                case Game.State.TRICK:
                    Console.WriteLine($"{args.Player.PlayerName} played a {args.Card}.");
                    break;
                case Game.State.TRICK_COMPLETE:
                    Console.WriteLine("=========================");
                    Console.WriteLine($"{args.Player.PlayerName} has won the trick.");
                    Console.WriteLine("=========================");
                    break;
                case Game.State.BID_REACHED:
                    Console.WriteLine("=========================");
                    Console.WriteLine("The bid has been reached.");
                    Console.WriteLine($"{args.Player.PlayerName} has won the bid!");
                    Console.WriteLine($"{args.WinningTeam} gains {args.Score} points.");
                    Console.WriteLine($"{args.LosingTeam} loses {args.Score} points.");
                    Console.WriteLine("=========================");
                    break;
                case Game.State.DONE:
                    Console.WriteLine("Game complete.");
                    break;
                default:
                    throw new Exception("Unknown state!");
                    break;
            }
        }

        static void Main(string[] args)
        {
            PrintHeader();

            // Create a game and subscribe to events for players
            Game game = new Game();

            // Listen to events
            game.NewGameEvent += OnNewGame; // TODO: Can ignore sender arg
            game.NewPlayerEvent += OnNewPlayer;
            game.PlayerTurnEvent += OnPlayerTurn;
            game.GameActionEvent += OnGameAction;

            /*game.CardPlayedEvent += OnCardPlayed;
            game.TrickCompleteEvent += OnTrickComplete;*/

            Console.Write("Enter your name: ");

            // Get the player's name and create a new game.
            //Game2 game = new Game2(Console.ReadLine());

            // Hook onto the event
            //game.NewGameEvent += OnNewGame;

            // Initialize and get the player instance
            consoleUser = game.Initialize("Haran");

            // Start game events
            game.Start();

            while (game.CurrentState != Game.State.DONE)
            {
                Thread.Sleep(60);
            }
            //TODO: Await until the game is done

            #region Old Console Game
            /*do // Run until 30 points
            {
                int roundCounter = 0;

                //TODO: Log bid
                Bid bid = new Bid();
                bid.BidStage(players[0], players[1], players[2], players[3]);

                Player lastWinner = bid.WinningPlayer;
                Player currentPlayer = lastWinner;

                do // Run rounds until bid is reached
                {
                    Round currentRound;
                    Card[] playedCards = new Card[4];

                    Console.WriteLine($"==| Round {roundCounter + 1} |==");

                    for (int turn = 0; turn < 4; turn++)
                    {
                        //TODO: Logic for low suit
                        if (currentPlayer is HumanPlayer) // Human?
                        {
                            // User plays a card
                            Console.WriteLine("Your turn! Pick a card:");
                            PrintPlayerCards(player.HandList);
                            playedCards[turn] = player.HandList.Pick(GetStdinInt(player.HandList.Cards.Count));
                            //playedCards[turn] = player.HandList.Cards[]; //TODO: Add card back to deck
                            //player.HandList.Cards.Remove(playedCards[0]); // TODO: Draw(int cardIndex)
                        }
                        else
                        {
                            // CPU Player plays a card
                            //TODO: CPUPlayer and CPUPlayer.PlayCard w/ AI
                            playedCards[turn] = currentPlayer.HandList.Pick(0);
                            // TODO: Add card back to main deck
                            Console.WriteLine($"{currentPlayer.PlayerName} played {playedCards[turn]}.");
                        }
                        mainDeck.Add(playedCards[turn]); // Add the card back to the main deck

                        //TODO: Log who played what card
                        currentPlayer = NextPlayer(players, currentPlayer); // TODO: do this better; logic seems messy
                    }

                    currentRound = new Round(bid.TrumpSuit, playedCards[0], playedCards[1], playedCards[2], playedCards[3]);
                    // TODO: Who won this round? And log it
                    Console.WriteLine($"LeadTrick = {currentRound.LeadTrick(currentRound)}");
                    rounds.Add(currentRound);
                    roundCounter++;
                } while (roundCounter != bid.HighestBid);
                // TODO: Reset hand?
            } while (teamScores[0] != 30 && teamScores[1] != 30); */
            #endregion

            Console.WriteLine("\n\nPress any key to quit...");
            Console.ReadKey();
        }
    }
}
