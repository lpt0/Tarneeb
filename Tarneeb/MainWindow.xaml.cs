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
                this.PlayerHand.Children.Add(cc);

                // Sample place holder decks

                // Create CardControl with Card. 
                CardControl cc2 = new CardControl(aCard, true);
                // Append the CardControl to the PlayerHand Area.
                this.PlayerHand2.Children.Add(cc2);

                // Create CardControl with Card. 
                CardControl cc3 = new CardControl(aCard, true);
                // Append the CardControl to the PlayerHand Area.
                this.PlayerHand3.Children.Add(cc3);

                // Create CardControl with Card. 
                CardControl cc4 = new CardControl(aCard, true);
                // Append the CardControl to the PlayerHand Area.
                this.PlayerHand4.Children.Add(cc4);
            }
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
