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
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        /// <summary>
        /// Original player name when this window was opened.
        /// </summary>
        private string originalPlayerName = Properties.Settings.Default.PlayerName;

        /// <summary>
        /// Original max score when this window was opened.
        /// </summary>
        private string originalMaxScore = Properties.Settings.Default.MaxScore.ToString();

        public SettingsWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Executes when the window is loaded.
        /// Populates text inputs with current settings.
        /// </summary>
        private void OnWindowLoad(object sender, EventArgs e)
        {
            this.PlayerName.Text = originalPlayerName;
            this.MaxScore.Text = originalMaxScore;
        }

        /// <summary>
        /// Reset button clicked; reset the database to clear all logs and stats.
        /// </summary>
        private void OnResetClicked(object sender, EventArgs e)
        {
            // Call the database reset method
            Database.Reset();
            MessageBox.Show("Data has been reset.");
        }

        /// <summary>
        /// Executes when the save and exit button is clicked.
        /// Validates and save settings, then closes this window.
        /// </summary>
        private void OnSaveClicked(object sender, EventArgs e)
        {
            bool isValid = true;
            string playerName = this.PlayerName.Text.Trim();
            int maxScore;

            // Check if all fields are valid
            if (!int.TryParse(this.MaxScore.Text, out maxScore))
            {
                MessageBox.Show("Max score must be a number.");
                isValid = false;
            }

            if (playerName.Length == 0)
            {
                MessageBox.Show("Player name cannot be empty, and cannot consist of just spaces.");
                isValid = false;
            }

            // Fields are valid; write and save data, then close window
            if (isValid)
            {
                Properties.Settings.Default.PlayerName = playerName;
                Properties.Settings.Default.MaxScore = maxScore;
                Properties.Settings.Default.Save();
                this.Close();
            }
        }

        /// <summary>
        /// Executes when exit without saving is clicked.
        /// Checks if values have been changed, then closes window.
        /// </summary>
        private void OnExitClicked(object sender, EventArgs e)
        {
            bool shouldClose = true;

            // Check if values have changed; if they have, warn the user before closing
            if (this.PlayerName.Text != originalPlayerName 
                || this.MaxScore.Text != originalMaxScore)
            {
                var result = MessageBox.Show(
                    "Changes have been made. Are you sure you want to exit without saving?",
                    "Are you sure you want to exit?",
                    MessageBoxButton.YesNo
                );
                shouldClose = (result == MessageBoxResult.Yes);
            }

            // Close if it is OK to close
            if (shouldClose)
            {
                this.Close();
            }
        }
    }
}
