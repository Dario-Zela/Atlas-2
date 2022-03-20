using System.Windows;
using System.Windows.Controls;

namespace Editor.Browser
{
    /// <summary>
    /// Interaction logic for Loader.xaml
    /// </summary>
    public partial class Browser : Window
    {
        public Browser()
        {
            //Initialise the window
            //Initiate the Open Project tab
            Logger.Log(MessageType.Trace, "Initialising Browser");

            InitializeComponent();
            Projects.IsSelected = true;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Swich which user control is visible
            //When the tab is changed
            if (Projects.IsSelected)
            {
                Logger.Log(MessageType.Trace, "Changing display to Existing projects");
                Existing_Projects.Visibility = Visibility.Visible;
                New_Projects.Visibility = Visibility.Hidden;
            }
            else
            {
                Logger.Log(MessageType.Trace, "Changing display to New projects");
                New_Projects.Visibility = Visibility.Visible;
                Existing_Projects.Visibility = Visibility.Hidden;
            }
        }
    }
}
