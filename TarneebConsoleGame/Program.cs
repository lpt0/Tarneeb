using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TarneebClasses;
using TarneebClasses.Events;

namespace Tarneeb.ConsoleGame
{
    /// <summary>
    /// Console game for testing Tarneeb.
    /// </summary>
    class ConsoleGame
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
                        PrintPlayerCards(consoleUser.HandList);
                        Console.Write("Enter your bid: ");
                        int bid = GetStdinInt(13);
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
                        Enums.CardSuit suit = (Enums.CardSuit)GetStdinInt(4);
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

                    //default:
                    //    throw new Exception("Unknown stage!");
                    //    break;
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
                case Game.State.BID_COMPLETE:
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
                //default:
                //    throw new Exception("Unknown state!");
                //    break;
            }
        }

        static void Main(string[] args)
        {
            PrintHeader();

            // Create a game and subscribe to events for players
            Game game = new Game();

            // Listen to events
            game.NewGameEvent += OnNewGame; // TODO: Can ignore sender arg
            //game.NewPlayerEvent += OnNewPlayer;
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
            consoleUser = game.Initialize(Console.ReadLine());

            Console.WriteLine("Your cards:");
            PrintPlayerCards(consoleUser.HandList);

            // Start game events
            game.Start();


            while (game.CurrentState != Game.State.DONE)
            {
                // spin
            }
            //TODO: Await until the game is done

            Console.WriteLine("\n\nPress any key to quit...");
            Console.ReadKey();
        }
    }
}
