using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FileHoster
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal static string ApplicationState { get; set; }
        internal static string UserName { get; set; }
        internal static bool IsSign => UserName != null;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Resource.Culture = System.Globalization.CultureInfo.CurrentUICulture;
            MainWindow window = new MainWindow() { Title = Resource.FileHosterTitle };
            window.Show();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(Resource.ErrorMessage, Resource.Message, MessageBoxButton.OK, MessageBoxImage.Hand, MessageBoxResult.OK);
        }
    }
}
