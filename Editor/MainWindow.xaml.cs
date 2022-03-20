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
            OpenHub();
            InitializeComponent();
            Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            Closing -= MainWindow_Closing;
            Project.Current?.Unload();
        }

        private void OpenHub()
        {
            Hide();
            var hub = new Browser.Browser();
            if (hub.ShowDialog() == false || hub.DataContext == null)
            {
                Application.Current.Shutdown();
            }
            else
            {
                Show();

                Project.Current?.Unload();
                DataContext = hub.DataContext;
            }
        }
    }
}
