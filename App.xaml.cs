using System.Windows;
using NITHdmis.ErrorLogging;
using System.Windows.Threading;

namespace Netychords
{
    /// <summary>
    /// Logica di interazione per App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            string exceptionMessageText =
                $"An exception occurred: {e.Exception.Message}\r\n\r\nat: {e.Exception.StackTrace}";
            LoggingService.Log(e.Exception);
            // Create a Window to display the exception information.
            MessageBox.Show(exceptionMessageText, "Unhandled Exception", MessageBoxButton.OK);
        }
    }
}
