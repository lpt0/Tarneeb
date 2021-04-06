using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using TarneebClasses;
using TarneebClasses.Events;

/**
 * @author  Andrew Kuo, Duy Tan Vu, Haran
 * 
 * @date    2021-03-11
 * @modified: 2021-03-20
 * 
 */
namespace Tarneeb
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// MainWindow Constructor.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Constants
        /// <summary>
        /// Base URI for images.
        /// </summary>
        private const string ASSET_BASE_URI = "./assets/cards";

        /// <summary>
        /// Dictionary of paths to images for each card suit.
        /// TODO: const
        /// </summary>
        private static Dictionary<Enums.CardSuit, string> SUIT_IMAGES = new Dictionary<Enums.CardSuit, string>()
        {
            { Enums.CardSuit.Spades, $"{ASSET_BASE_URI}/cardSuits/spades.png" },
            { Enums.CardSuit.Heart, $"{ASSET_BASE_URI}/cardSuits/heart.png" },
            { Enums.CardSuit.Diamond, $"{ASSET_BASE_URI}/cardSuits/diamond.png" },
            { Enums.CardSuit.Club, $"{ASSET_BASE_URI}/cardSuits/club.png" }
        };

        /// <summary>
        /// A default card, used for cards that need to be face down.
        /// TODO: Change to CardControl
        /// </summary>
        private static Card DEFAULT_CARD = new Card(Enums.CardNumber.Two, Enums.CardSuit.Spades);
        #endregion

        #region Variables
        /// <summary>
        /// An array of the wrap panels used to hold the 4 cards in play for the round.
        /// </summary>
        private WrapPanel[] CardsInRoundHolders;

        /// <summary>
        /// Same as CardsInRoundHolders, but for the name underneath the card played.
        /// </summary>
        /// <see cref="CardsInRoundHolders"/>
        private TextBlock[] _namesInRoundHolders;

        /// <summary>
        /// An array of the wrap panels used to store each player's cards.
        /// </summary>
        private WrapPanel[] _playerHands;

        /// <summary>
        /// Whether or not it is the player's turn.
        /// </summary>
        private Boolean _isPlayerTurn;

        /// <summary>
        /// The instance of the Tarneeb game.
        /// </summary>
        private Game _game;

        /// <summary>
        /// The user's player.
        /// </summary>
        private HumanPlayer _userPlayer;

        #endregion

        #region Functions
        /// <summary>
        /// Pop-up a dialog box asking the user for their name.
        /// </summary>
        /// <returns>The player's name.</returns>
        private string PromptPlayerName()
        {
            string name = "";

            // Prompt until a name is received
            do
            {
                var promptWindow = new TextInputWindow();
                promptWindow.Prompt.Content = "What is your name?\n(This can be later changed in settings.)";
                // By default, fill the input with the user's Windows username
                promptWindow.Input.Text = Environment.UserName;
                promptWindow.ShowDialog();
                name = promptWindow.Input.Text.Trim();

                if (name == "")
                {
                    MessageBox.Show("You must enter a name.");
                }
            } while (name == "");

            return name;
        }
        #endregion

        #region Event handlers
        /// <summary>
        /// Executed when it is a player's turn.
        /// If it is this player's turn, the player will be able to play their card.
        /// </summary>
        /// <param name="sender">The game object.</param>
        /// <param name="e">The event arguments.</param>
        /// <see cref="PlayerTurnEventArgs"/>
        private void OnPlayerTurn(object sender, PlayerTurnEventArgs e)
        {
            UpdateAllHands();

            this._isPlayerTurn = (e.Player.PlayerId == this._userPlayer.PlayerId); // TODO: Fake player
            if (this._isPlayerTurn)
            {
                // Perform actions
                switch (e.State)
                {
                    case Game.State.BID_STAGE:
                        this.PlayerTurnBid();
                        break;
                    case Game.State.BID_WON:
                        this.PlayerTurnTarneeb();
                        break;
                    case Game.State.TRICK:
                        this.PlayerTurnCard();
                        break;
                }
                
            }
            else
            {
                // Not the player's turn; disable the deck from being clicked
                foreach (CardControl cc in this._playerHands[0].Children)
                {
                    cc.Click -= OnCardClicked;
                }
            }
        }

        /// <summary>
        /// Executed when an action occurs in the Tarneeb game, such as a card being played.
        /// As the game validates all actions, any event raised by the game should be considered as such.
        /// </summary>
        /// <param name="sender">The game object.</param>
        /// <param name="e">The event arguments.</param>
        /// <see cref="GameActionEventArgs"/>
        private void OnGameAction(object sender, GameActionEventArgs e)
        {
            UpdateAllHands();

            // Match the state to the corresponding function
            switch (e.State)
            {
                case Game.State.BID_STAGE:
                    this.OnBidPlaced(e);
                    break;
                case Game.State.BID_WON:
                    this.OnBidWon(e);
                    this.OnTarneebSuit(e); //TODO
                    break;
                case Game.State.TARNEEB_SUIT:
                    this.OnTarneebSuit(e);
                    break;
                case Game.State.TRICK:
                    this.OnCardPlayed(e);
                    break;
                case Game.State.TRICK_COMPLETE:
                    this.OnRoundComplete(e);
                    break;
                case Game.State.BID_COMPLETE:
                    this.OnBidComplete(e);
                    break;
                case Game.State.DONE:
                    this.OnGameDone(e);
                    break; //TODO
            }
        }

        /// <summary>
        /// Executes when the game sends the player a notification.
        /// </summary>
        /// <param name="sender">The game object.</param>
        /// <param name="e">The notification details.</param>
        private void OnNotification(object sender, NotificationEventArgs e)
        {
            MessageBox.Show(e.Message);
        }

        /// <summary>
        /// Executes whenever a card control is clicked.
        /// </summary>
        /// <param name="sender">The card control.</param>
        /// <param name="e">The event arguments.</param>
        private void OnCardClicked(object sender, EventArgs e)
        {
            CardControl cc = sender as CardControl;
            Card card = cc.Card;
            this._userPlayer.PerformAction(new PlayerActionEventArgs() { CardPlayed = card });
        }
        #endregion

        /* Player actions are for when it is the player's turn, such as:
         * - the player's turn to place a bid
         * - the player's turn to play a card
         * - the player's turn to decide the Tarneeb suit
         */
        #region Player actions
        private void PlayerTurnBid()
        {
            int bid = 0;

            do
            {
                var promptWindow = new TextInputWindow();
                promptWindow.Prompt.Content = "Enter a bid (-1 to pass)";
                promptWindow.ShowDialog();
                int.TryParse(promptWindow.Input.Text, out bid);

                if (bid == 0)
                {
                    MessageBox.Show("You must enter a valid, numeric bid.");
                }
            } while (bid == 0);

            // Place the bid
            this._userPlayer.PerformAction(new PlayerActionEventArgs() { Bid = bid });
        }

        private void PlayerTurnCard()
        {
            // TODO: Enforce leading suit
            // Get the valid cards that a player can play
            List<Card> validCards = this._game.GetValidCards(this._userPlayer);

            // Create card controls for the valid cards, which should be face up
            CardControl[] cardControls = new CardControl[this._userPlayer.HandList.Cards.Count];
            int cardIdx = 0;
            for (; cardIdx < validCards.Count; cardIdx++)
            {
                Card c = validCards[cardIdx];
                CardControl cc = new CardControl(c, false);

                // Set up event handlers for the valid cards
                cc.Click += OnCardClicked;

                cardControls[cardIdx] = cc;
            }

            // Create controls for the remaining, invalid cards - all face down, with no click handler
            for (; cardIdx < cardControls.Length; cardIdx++)
            {
                cardControls[cardIdx] = new CardControl(DEFAULT_CARD, true);
                // No click handler for this card
            }

            // Update the player's cards using the array of CardControls
            UpdatePlayerHand(0, cardControls);
            //UpdatePlayerHand(this._userPlayer); // TODO
        }

        private void PlayerTurnTarneeb()
        {
            // Prompt the user to select the Tarneeb suit
            Enums.CardSuit tarneebSuit;
            
            // Loop until the suit is selected
            do
            {
                var tarneebWindow = new TarneebSuitWindow();
                tarneebWindow.ShowDialog();

                if (tarneebWindow.Suit == 0)
                {
                    MessageBox.Show("A Tarneeb suit must be selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error); ;
                }

                tarneebSuit = tarneebWindow.Suit;
            } while (tarneebSuit == 0);


            // Send the selected suit to the game
            this._userPlayer.PerformAction(new PlayerActionEventArgs() { Tarneeb = tarneebSuit });
        }
        #endregion

        // Game actions include a card being played, or a bid being placed.
        #region Game actions
        private void OnNewGame(NewGameEventArgs e)
        {

        }

        private void OnPlayerJoin(NewPlayerEventArgs e)
        {
            
        }

        /// <summary>
        /// A bid was placed.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        /// <see cref="GameActionEventArgs"/>
        private void OnBidPlaced(GameActionEventArgs e)
        {
            // -1 is a pass
            string bid = e.Bid == -1 ? "passed" : "bid " + e.Bid.ToString();
            this.Messages.Text += $"{e.Player.PlayerName} {bid}\n";
        }

        /// <summary>
        /// The highest bid has been decided.
        /// The relevant text element will be set.
        /// TODO: Bid decided instead of bid won
        /// </summary>
        /// <param name="e">The event arguments.</param>
        /// <see cref="GameActionEventArgs"/>
        private void OnBidWon(GameActionEventArgs e)
        {
            this.CurrentBid.Text = e.Bid.ToString();
        }

        /// <summary>
        /// A Tarneeb suit was decided on, and the image should be set.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        /// <see cref="GameActionEventArgs"/>
        private void OnTarneebSuit(GameActionEventArgs e)
        {
            this.TarneebSuitImage.Source = new BitmapImage(
                new Uri(SUIT_IMAGES[e.Tarneeb], UriKind.Relative)
            );
        }

        /// <summary>
        /// A valid card was played in the round.
        /// It should be added to the GUI.
        /// If it is the player's card, it should be removed from the player's hand.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        /// <see cref="GameActionEventArgs"/>
        private void OnCardPlayed(GameActionEventArgs e)
        {
            Card card = e.Card;
            CardControl cc = new CardControl(card);

            /* The game object validates the card played.
             * If the card was placed by the player, and this event
             * is raised signifying that, that means the card is valid;
             * so, it should be removed from the player's deck.
             */
            if (e.Player == this._userPlayer)
            {
                this.MyPlayerHand.Children.Remove(cc);
            }
            else
            {
                // Update the CPU's hand within the GUI
                UpdatePlayerHand(e.Player as CPUPlayer);
            }

            // Add the card to the cards played in round, and the name of the player who played it
            this.CardsInRoundHolders[e.CardsPlayedInRound].Children.Clear();
            this.CardsInRoundHolders[e.CardsPlayedInRound].Children.Add(cc);
            this._namesInRoundHolders[e.CardsPlayedInRound].Text = e.Player.PlayerName;
        }

        /// <summary>
        /// A round, or trick, has been completed.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        /// <see cref="GameActionEventArgs"/>
        private void OnRoundComplete(GameActionEventArgs e)
        {
            ClearMessages();

            // Update the trick scores
            this.Team0TrickWins.Text = e.BidScores[0].ToString();
            this.Team1TrickWins.Text = e.BidScores[1].ToString();

            MessageBox.Show($"{e.Player.PlayerName} won the trick!");
            AddMessage($"{e.Player.PlayerName} won the last trick.");

            // Clear the cards played in the round, and the names
            foreach (WrapPanel holder in this.CardsInRoundHolders) 
            {
                holder.Children.Clear();

                var blankCardImage = new Image
                {
                    Source = new BitmapImage(new Uri("./assets/cards/blankPlayingCard.png", UriKind.Relative)),
                    Height = 116,
                    Width = 100
                };

                holder.Children.Add(blankCardImage);
            }
            foreach (TextBlock textBlock in this._namesInRoundHolders)
            {
                textBlock.Text = "";
            }

            // On to the next round!
        }

        private void OnBidComplete(GameActionEventArgs e)
        {
            // Update the player's hand
            //TODO

            // Update everyone else's hands
            // TODO: Prevent leaks in Game class
            //foreach (Player p in this._game.Players)
            //{
            //    UpdateNonPlayerHand(
            //        Array.IndexOf(this._game.Players, p),
            //        p.HandList
            //    );
            //}

            // TODO: State who won the bid in messages
            MessageBox.Show(
                $"{e.WinningTeam} won the bid!\n"
                + $"{e.WinningTeam} gains {e.Score} points.\n"
                + $"{e.LosingTeam} loses {e.Score} points."
            );

            // Update scores
            this.Team0TotalScore.Text = e.TeamScores[0].ToString();
            this.Team0TrickWins.Text = "0";
            this.Team1TotalScore.Text = e.TeamScores[1].ToString();
            this.Team1TrickWins.Text = "0";

        }

        private void OnGameDone(GameActionEventArgs e)
        {
            if (e.WinningTeam == this._userPlayer.TeamNumber)
            {
                MessageBox.Show("You won!");
            }
            else
            {
                MessageBox.Show("You lost.");
            }
            throw new Exception("Not implemented");
        }
        #endregion

        #region Helper functions
        #region Player hand updates
        /// <summary>
        /// Update the user's hand in the GUI.
        /// </summary>
        /// <param name="player">The user.</param>
        private void UpdatePlayerHand(HumanPlayer player)
        {
            // Create the card controls, face up
            int numberOfCards = player.HandList.Cards.Count;
            CardControl[] cards = new CardControl[numberOfCards];
            for (int cardIdx = 0; cardIdx < numberOfCards; cardIdx++)
            {
                Card c = player.HandList.Cards[cardIdx];
                cards[cardIdx] = new CardControl(c, false);
            }
            UpdatePlayerHand(0, cards);
        }
        /// <summary>
        /// Update a CPU player's hands with face down cards.
        /// </summary>
        /// <param name="player">The CPU player.</param>
        private void UpdatePlayerHand(CPUPlayer player)
        {
            /* Create a fake array of card controls.
             * It has the same number of cards as the CPU player, but
             * with fake values, and all cards face down.
             */
            Card fakeCard = new Card(Enums.CardNumber.Two, Enums.CardSuit.Spades);
            int numberOfCards = player.HandList.Cards.Count;
            CardControl[] cards = new CardControl[numberOfCards];
            for (int cardIdx = 0; cardIdx < numberOfCards; cardIdx++)
            {
                // TODO: Enforce leading suit
                cards[cardIdx] = new CardControl(fakeCard, true);
            }
            UpdatePlayerHand(Array.IndexOf(this._game.Players, player), cards);
        }
        /// <summary>
        /// Update the player's hand at the given index, with the provided array of card controls.
        /// </summary>
        /// <param name="playerIdx">The index to update.</param>
        /// <param name="cards">The card controls to use.</param>
        private void UpdatePlayerHand(int playerIdx, CardControl[] cards)
        {
            WrapPanel handPanel = _playerHands[playerIdx];

            // Clear the existing cards
            handPanel.Children.Clear();

            // Create a CardControl with each card and add it to the panel
            foreach (CardControl cc in cards)
            {
                handPanel.Children.Add(cc);
            }
        }
        /// <summary>
        /// Update all hands in the GUI.
        /// </summary>
        private void UpdateAllHands()
        {
            // Only update cards after a new game
            if (this._game.CurrentState > Game.State.NEW_GAME)
            {
                UpdatePlayerHand(this._userPlayer);
                foreach (CPUPlayer player in this._game.Players.Skip(1).Take(3)) // [1:3]
                {
                    UpdatePlayerHand(player);
                }
            }
        }
        #endregion
        #region Messages
        private void ClearMessages()
        {
            this.Messages.Text = "";
        }
        private void AddMessage(string text)
        {
            this.Messages.Text += text + "\n";
        }
        #endregion
        #endregion

        /// <summary>
        /// Window Loaded handler. Runs when Mainwindow is ready.
        /// </summary>
        private void OnWindowLoad(object sender, RoutedEventArgs e)
        {
            // If no name is declared in settings, prompt the user for one, and save the change
            if (Properties.Settings.Default.PlayerName == "")
            {
                Properties.Settings.Default.PlayerName = PromptPlayerName();
                Properties.Settings.Default.Save();
            }

            // Set needed variables
            this.CardsInRoundHolders = new WrapPanel[] { this.FirstCard, this.SecondCard, this.ThirdCard, this.FourthCard };
            this._namesInRoundHolders = new TextBlock[] { this.FirstName, this.SecondName, this.ThirdName, this.FourthName };
            this._playerHands = new WrapPanel[] { this.MyPlayerHand, this.LeftPlayerHand, this.TopPlayerHand, this.RightPlayerHand }; // Counter-clockwise
            this.MaxScore.Text = Properties.Settings.Default.MaxScore.ToString();

            /* Create a new game
             * For the max score, use the max score from settings
             */
            this._game = new Game(Properties.Settings.Default.MaxScore);

            // Set up events
            this._game.GameActionEvent += OnGameAction;
            this._game.PlayerTurnEvent += OnPlayerTurn;
            this._game.NotificationEvent += OnNotification;

            /* Initialize game with player name from settings, 
             * and this program's app data path for the database. TODO
             */
            this._userPlayer = this._game.Initialize(
                Properties.Settings.Default.PlayerName 
            );

            // Use the log observable collection, to be able to see logs.
            this.Logs.ItemsSource = this._game.Logs;

            this.MyPlayerColor.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(_userPlayer.TeamNumber.ToString());
            this.TopPlayerColor.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(_userPlayer.TeamNumber.ToString());
            this.MyPlayerName.Text = this._userPlayer.PlayerName;
            this.TopPlayerName.Text = this._game.Players[2].PlayerName;

            var opponentTeamColour = _userPlayer.TeamNumber.ToString().Equals("Blue") ? "Red" : "Blue";
            this.LeftPlayerColor.Background = _userPlayer.TeamNumber.ToString().Equals("Blue") ? Brushes.Red : Brushes.Blue;
            this.RightPlayerColor.Background = _userPlayer.TeamNumber.ToString().Equals("Blue") ? Brushes.Red : Brushes.Blue;
            this.LeftPlayerName.Text = this._game.Players[1].PlayerName;
            this.RightPlayerName.Text = this._game.Players[3].PlayerName;

            this._game.Start();      
        }

        /// <summary>
        /// Close the playing window and open title window when user clicks on "Settings".
        /// </summary>
        private void BackClicked(object sender, RoutedEventArgs e)
        {
            var isLeft = MessageBox.Show("Do you want to leave the game?", "Warning", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

            if (isLeft == MessageBoxResult.Yes)
            {
                (new TitleScreen()).Show();
                this.Close();
            }
        }
    }
}
