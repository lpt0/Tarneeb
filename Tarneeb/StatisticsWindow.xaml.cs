using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for StatisticsWindow.xaml
    /// </summary>
    public partial class StatisticsWindow : Window
    {
        public StatisticsWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Executes when the window is loaded, and retrieves a list of games from the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowLoad(object sender, EventArgs e)
        {
            // Start a database connection, load the games, and populate the combo box
            Database.Connect();
            var games = Database.GetGames();
            this.GamesList.ItemsSource = games;

            // Select the first game by default
            this.GamesList.SelectedIndex = 0;
        }

        /// <summary>
        /// The close button was clicked, close this window.
        /// </summary>
        private void OnCloseClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// A game was selected from the combo box, retrieve the logs for it.
        /// </summary>
        private void OnGameSelected(object sender, EventArgs e)
        {
            // Make sure something is selected before trying to cast it
            if (this.GamesList.SelectedIndex != -1)
            {
                // Get the selected game as a database game entry
                var game = (DatabaseGameEntry)this.GamesList.SelectedItem;

                // Load the logs for that game, and use it for the data grid
                this.logsGrid.ItemsSource = Database.GetLogs(game.GameID);

                // Show the grid
                this.logsGrid.Visibility = Visibility.Visible;
            }

        }
    }
}
