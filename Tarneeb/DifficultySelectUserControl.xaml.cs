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

namespace Tarneeb
{
    /// <summary>
    /// Interaction logic for DifficultySelectUserControl.xaml
    /// </summary>
    public partial class DifficultySelectUserControl : UserControl
    {
        public DifficultySelectUserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Determine if any button is clicked or not.
        /// </summary>
        public bool IsClicked { get; private set; } = false;

        /// <summary>
        /// The difficulty that was selected.
        /// </summary>
        public Game.Difficulty SelectedDifficulty { get; private set; }

        /// <summary>
        /// Set difficulty to easy.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Easy_Click(object sender, RoutedEventArgs e)
        {
            SelectedDifficulty = Game.Difficulty.EASY;
            IsClicked = true;
        }

        /// <summary>
        /// Set difficulty to medium.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Medium_Click(object sender, RoutedEventArgs e)
        {
            SelectedDifficulty = Game.Difficulty.MEDIUM;
            IsClicked = true;
        }

        /// <summary>
        /// Set difficulty to hard.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hard_Click(object sender, RoutedEventArgs e)
        {
            SelectedDifficulty = Game.Difficulty.HARD;
            IsClicked = true;
        }
    }
}
