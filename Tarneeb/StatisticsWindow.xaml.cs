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
        /// Executes when the window is loaded, and retrieves data from the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowLoad(object sender, EventArgs e)
        {
            Database.Connect();
        }

        /// <summary>
        /// The close button was clicked, close this window.
        /// </summary>
        private void OnCloseClicked(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
