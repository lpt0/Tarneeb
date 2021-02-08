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

namespace Tarneeb
{
    /// <summary>
    /// Interaction logic for TitleScreen.xaml
    /// </summary>
    public partial class TitleScreen : Window
    {
        public TitleScreen()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Local play clicked, open main game window.
        /// </summary>
        private void LocalPlayClicked()
        {
            //TODO
        }

        /// <summary>
        /// Settings button clicked, open settings window.
        /// </summary>
        private void SettingsClicked()
        {
            //TODO
        }

        /// <summary>
        /// Exit button clicked, close program.
        /// </summary>
        private void ExitClicked()
        {
            this.Close(); //TODO
        }
    }
}
