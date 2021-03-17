using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics; // Process
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
        private void LocalPlayClicked(object sender, EventArgs args)
        {
            //TODO
        }

        /// <summary>
        /// Settings button clicked, open settings window.
        /// </summary>
        private void SettingsClicked(object sender, EventArgs args)
        {
            //TODO
        }

        /// <summary>
        /// Exit button clicked, close program.
        /// </summary>
        private void ExitClicked(object sender, EventArgs args)
        {
            this.Close(); //TODO
        }

        /// <summary>
        /// User manual button clicked, open the user manual.
        /// </summary>
        private void ManualClicked(object sender, EventArgs args)
        {
            // Let the default handler for HTTP URLs open the page
            Process.Start("https://gist.github.com/lpt0/4c532ed6474add1e9d32dd9af6098ca6");
        }
    }
}
