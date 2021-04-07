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

/**
 * @author  Haran
 * @date    2021-04-06
 */
namespace Tarneeb
{
    /// <summary>
    /// Interaction logic for DifficultySelectWindow.xaml
    /// </summary>
    public partial class DifficultySelectWindow : Window
    {
        /// <summary>
        /// The difficulty that was selected.
        /// </summary>
        public Game.Difficulty SelectedDifficulty { get; private set; }

        public DifficultySelectWindow()
        {
            InitializeComponent();

            // Add the difficulties to the combo box
            this.cmbDifficulty.ItemsSource = new List<Game.Difficulty>()
            {
                Game.Difficulty.EASY,
                Game.Difficulty.MEDIUM,
                Game.Difficulty.HARD
            };

            // Set a default selection
            this.cmbDifficulty.SelectedIndex = 0;
        }

        /// <summary>
        /// Executes when a difficulty is selected; enables the "OK" button
        /// </summary>
        private void OnSelection(object sender, EventArgs e)
        {
            // Only enable the "OK" button if something is selected
            if (this.cmbDifficulty.SelectedIndex != -1)
            {
                this.btnOk.IsEnabled = true;
            }
        }

        /// <summary>
        /// Executes when the "OK" button is clicked.
        /// Ensured that an option is selected in the combo box.
        /// </summary>
        private void OnClick(object sender, EventArgs e)
        {
            if (this.cmbDifficulty.SelectedIndex == -1)
            {
                MessageBox.Show(
                    this,
                    "A difficulty must be selected.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
            else
            {
                // Store the selected difficulty.
                this.SelectedDifficulty = (Game.Difficulty)this.cmbDifficulty.SelectedItem;
                this.Close();
            }
        }
    }
}
