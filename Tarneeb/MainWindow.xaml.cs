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
using TarneebClasses;

/**
 * @author  Andrew Kuo, Duy Tan Vu
 * 
 * @date    2021-03-11
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

        /// <summary>
        /// Window Loaded handler. Runs when Mainwindow is ready.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Call CardControl Test.
            TestCardControlPlayerHand();
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
