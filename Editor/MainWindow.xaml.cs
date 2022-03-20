using System.Windows;

namespace Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //Before initialising, Open the Hub
            OpenHub();

            Logger.Log(MessageType.Trace, "Initialising Main Window Components");

            //Initialise Components
            InitializeComponent();
            //Add event for the main window closing
            Closing += MainWindow_Closing;
        }

        //Method called when the window is closing
        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            //Remove this event from the queue
            Closing -= MainWindow_Closing;

            Logger.Log(MessageType.Trace, "Unloading Project");

            //Unload the current project
            Project.Unload();
        }

        //Method called before window is initialised
        private void OpenHub()
        {
            Logger.Log(MessageType.Trace, "Initialising Hub");

            //Hide the main window and make a new browser
            Hide();
            var hub = new Browser.Browser();

            //If the dialoug fails shut down the application
            if (hub.ShowDialog() == false)
            {
                Logger.Log(MessageType.Info, "Closing Widow due to hub failing");

                Application.Current.Shutdown();
            }
            //Else
            else
            {
                Logger.Log(MessageType.Trace, "Unloading Project and Setting new Data Context");

                //Show the window
                Show();

                //Unload the current project and set the new data context
                Project.Unload();
                DataContext = hub.DataContext;
            }
        }
    }
}
