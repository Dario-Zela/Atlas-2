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

namespace Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // TODO: Initialise Components
            UndoRedoManager.Initialise();
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
