using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TarneebClasses;

namespace Tarneeb
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Executes when the program closes. Closes the database connection.
        /// </summary>
        private void OnClose(object sender, ExitEventArgs e)
        {
            Database.Close();
        }
    }
}
