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
 * @author  Andrew Kuo, <APPEND_NAME>
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
            Deck aHand = (new Deck()).Draw(13);
            
            // For each Card in Hand, generate CardControls
            foreach(Card aCard in aHand.Cards)
            {
                // Create CardControl with Card. 
                CardControl cc = new CardControl(aCard);
                // Attach generic Click event listener.
                cc.Click += new RoutedEventHandler(Card_Click);
                // Append the CardControl to the PlayerHand Area.
                this.PlayerHand.Children.Add(cc);
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
    }
}
