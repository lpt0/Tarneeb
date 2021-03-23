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
using System.Windows.Shapes;
using TarneebClasses;

namespace Tarneeb
{
    /// <summary>
    /// Interaction logic for TarneebSuitWindow.xaml
    /// </summary>
    public partial class TarneebSuitWindow : Window
    {
        /// <summary>
        /// Dictionary that maps a suit name to the correct enum.
        /// </summary>
        private Dictionary<string, Enums.CardSuit> SUIT_ENUMS = new Dictionary<string, Enums.CardSuit>
        {
            { "Spades", Enums.CardSuit.Spades },
            { "Heart", Enums.CardSuit.Heart },
            { "Club", Enums.CardSuit.Club },
            { "Diamond", Enums.CardSuit.Diamond },
        };

        /// <summary>
        /// The chosen suit.
        /// </summary>
        public Enums.CardSuit Suit;

        public TarneebSuitWindow()
        {
            InitializeComponent();
        }

        private void OnSuitClicked(object sender, EventArgs e)
        {
            Button button = sender as Button;

            // Get the chosen suit based on the button name, using the dictionary
            this.Suit = SUIT_ENUMS[button.Name];

            // This window is no longer needed
            this.Close();
        }
    }
}
