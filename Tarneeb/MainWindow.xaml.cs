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
        private TextBlock[] NamesInRoundHolders;

        /// <summary>
        /// An array of the wrap panels used to store each player's cards.
        /// </summary>
        private WrapPanel[] PlayerHands;

        /// <summary>
        /// Whether or not it is the player's turn.
        /// </summary>
        private Boolean IsPlayerTurn;

        /// <summary>
        /// The player's name.
        /// </summary>
        private string PlayerName;

        /// <summary>
        /// The instance of the Tarneeb game.
        /// </summary>
        private Game Game;

        /// <summary>
        /// The user's player.
        /// </summary>
        private HumanPlayer UserPlayer;

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
                promptWindow.Prompt.Content = "What is your name?";
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
            this.IsPlayerTurn = (e.Player.PlayerId == this.UserPlayer.PlayerId); // TODO: Fake player
            if (this.IsPlayerTurn)
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
                foreach (CardControl cc in this.PlayerHands[0].Children)
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
            // On each action after a new game, update all players' hands
            if (this.Game.CurrentState > Game.State.NEW_GAME)
            {
                UpdatePlayerHand(this.UserPlayer);
                foreach (CPUPlayer player in this.Game.Players.Skip(1).Take(3)) // [1:3]
                {
                    UpdatePlayerHand(player);
                }
            }

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
            this.UserPlayer.PerformAction(new PlayerActionEventArgs() { CardPlayed = card });
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
            this.UserPlayer.PerformAction(new PlayerActionEventArgs() { Bid = bid });
        }

        private void PlayerTurnCard()
        {
            // Update the player's cards
            UpdatePlayerHand(this.UserPlayer);

            // Enable card clicks again
            foreach (CardControl cc in this.PlayerHands[0].Children)
            {
                cc.Click += OnCardClicked;
            }

        }

        private void PlayerTurnTarneeb()
        {
            // Prompt the user to select the Tarneeb suit
            var tarneebWindow = new TarneebSuitWindow();
            tarneebWindow.ShowDialog();

            // Send the selected suit to the game
            this.UserPlayer.PerformAction(new PlayerActionEventArgs() { Tarneeb = tarneebWindow.Suit });
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
            if (e.Player == this.UserPlayer)
            {
                this.MyPlayerHand.Children.Remove(cc);
            }
            else
            {
                // Update the CPU's hand within the GUI
                UpdatePlayerHand(e.Player as CPUPlayer);
            }

            // Add the card to the cards played in round, and the name of the player who played it
            this.CardsInRoundHolders[e.CardsPlayedInRound].Children.Add(cc);
            this.NamesInRoundHolders[e.CardsPlayedInRound].Text = e.Player.PlayerName;
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

            // Clear the cards played in the round
            foreach (WrapPanel holder in this.CardsInRoundHolders) 
            {
                holder.Children.Clear();
            }

            // On to the next round!
        }

        private void OnBidComplete(GameActionEventArgs e)
        {
            // Update the player's hand
            //TODO

            // Update everyone else's hands
            // TODO: Prevent leaks in Game class
            //foreach (Player p in this.Game.Players)
            //{
            //    UpdateNonPlayerHand(
            //        Array.IndexOf(this.Game.Players, p),
            //        p.HandList
            //    );
            //}

            // TODO: State who won the bid in messages
            MessageBox.Show(
                $"{e.Player.PlayerName} (Team {e.Player.TeamNumber}) won the bid!\n"
                + $"{e.WinningTeam} gains {e.Score} points.\n"
                + $"{e.LosingTeam} loses {e.Score} points."
            );

            // Update scores
            this.Team0TotalScore.Text = e.TeamScores[0].ToString();
            this.Team0TrickWins.Text = "0";
            this.Team1TotalScore.Text = e.TeamScores[1].ToString();
            this.Team1TrickWins.Text = "0";

        }
        #endregion

        #region Helper functions
        #region Player hand updates
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
                cards[cardIdx] = new CardControl(fakeCard, true);
            }
            UpdatePlayerHand(Array.IndexOf(this.Game.Players, player), cards);
        }
        private void UpdatePlayerHand(int playerIdx, CardControl[] cards)
        {
            WrapPanel handPanel = PlayerHands[playerIdx];

            // Clear the existing cards
            handPanel.Children.Clear();

            // Create a CardControl with each card and add it to the panel
            foreach (CardControl cc in cards)
            {
                handPanel.Children.Add(cc);
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
            this.PlayerName = PromptPlayerName();

            // Set needed variables
            this.CardsInRoundHolders = new WrapPanel[] { this.FirstCard, this.SecondCard, this.ThirdCard, this.FourthCard };
            this.NamesInRoundHolders = new TextBlock[] { this.FirstName, this.SecondName, this.ThirdName, this.FourthName };
            this.PlayerHands = new WrapPanel[] { this.MyPlayerHand, this.LeftPlayerHand, this.TopPlayerHand, this.RightPlayerHand }; // Counter-clockwise

            // Create a new game, listen for events, and start the game
            this.Game = new Game();
            this.Game.GameActionEvent += OnGameAction;
            this.Game.PlayerTurnEvent += OnPlayerTurn;
            this.Game.NotificationEvent += OnNotification;
            this.UserPlayer = this.Game.Initialize(this.PlayerName);
            this.Logs.ItemsSource = this.Game.Logs;
            this.Game.Start();
        }

        /// <summary>
        /// Generates a hand and put them into the Player's Hand.
        /// </summary>
        private void TestCardControlPlayerHand()
        {
            // Generate 13 Cards to new Deck.
            Deck aDeck = new Deck();
            aDeck.Shuffle();
            Deck aHand = aDeck.Draw(13);
            
            // For each Card in Hand, generate CardControls
            foreach(Card aCard in aHand.Cards)
            {
                // Create CardControl with Card. 
                CardControl cc = new CardControl(aCard);
                // Attach generic Click event listener.
                cc.Click += new RoutedEventHandler(Card_Click);
                // Append the CardControl to the PlayerHand Area.
                this.MyPlayerHand.Children.Add(cc);

                // Sample place holder decks

                // Create CardControl with Card. 
                CardControl cc2 = new CardControl(aCard, true);
                // Append the CardControl to the PlayerHand Area.
                this.TopPlayerHand.Children.Add(cc2);

                // Create CardControl with Card. 
                CardControl cc3 = new CardControl(aCard, true);
                // Append the CardControl to the PlayerHand Area.
                this.LeftPlayerHand.Children.Add(cc3);

                // Create CardControl with Card. 
                CardControl cc4 = new CardControl(aCard, true);
                // Append the CardControl to the PlayerHand Area.
                this.RightPlayerHand.Children.Add(cc4);
            }

            Card aRandomCard1 = aDeck.Draw(1).Cards[0];
            CardControl aRandomCardControl1 = new CardControl(aRandomCard1);
            this.FirstCard.Children.Add(aRandomCardControl1);

            Card aRandomCard2 = aDeck.Draw(1).Cards[0];
            CardControl aRandomCardControl2 = new CardControl(aRandomCard2);
            this.SecondCard.Children.Add(aRandomCardControl2);

            Card aRandomCard3 = aDeck.Draw(1).Cards[0];
            CardControl aRandomCardControl3 = new CardControl(aRandomCard3);
            this.ThirdCard.Children.Add(aRandomCardControl3);

            Card aRandomCard4 = aDeck.Draw(1).Cards[0];
            CardControl aRandomCardControl4 = new CardControl(aRandomCard4);
            this.FourthCard.Children.Add(aRandomCardControl4);
        }

        /// <summary>
        /// Generic Click event handler for the CardControls.
        /// </summary>
        private void Card_Click(object sender, EventArgs e)
        {
            // Change sender to CardControl.
            CardControl cc = sender as CardControl;
            // Display what Card was 'Click' ed.
            MessageBox.Show(cc.Card.ToString());
        }

        /// <summary>
        /// Close the playing window and open title window when user clicks on "Settings".
        /// </summary>
        private void SettingsClicked(object sender, RoutedEventArgs e)
        {
            var isLeft = MessageBox.Show("Are you sure to leave the game?", "Warning", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

            if (isLeft == MessageBoxResult.Yes)
            {
                var newTitleScreen = new TitleScreen();
                newTitleScreen.Show();
                this.Close();
            }
        }
    }
}
