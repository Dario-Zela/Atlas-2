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

namespace Editor.Browser
{
    /// <summary>
    /// Interaction logic for Loader.xaml
    /// </summary>
    public partial class Browser : Window
    {
        public Browser()
        {
            InitializeComponent();
            Projects.IsSelected = true;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Projects.IsSelected)
            {
                Existing_Projects.Visibility = Visibility.Visible;
                New_Projects.Visibility = Visibility.Hidden;
            }
            else
            {
                New_Projects.Visibility = Visibility.Visible;
                Existing_Projects.Visibility = Visibility.Hidden;
            }
        }
    }
}
