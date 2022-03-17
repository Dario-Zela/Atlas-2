using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Editor.Browser
{
    /// <summary>
    /// Interaction logic for NewProject.xaml
    /// </summary>
    public partial class NewProjectView : UserControl
    {
        public NewProjectView()
        {
            InitializeComponent();
        }

        private void OnCreate(object sender, RoutedEventArgs e)
        {
            NewProject viewModel = (NewProject)DataContext;
            string projectPath = viewModel.CreateProject((ProjectTemplate)TemplatesView.SelectedItem);

            bool dialougeResult = false;
            var window = Window.GetWindow(this);

            if (string.IsNullOrEmpty(projectPath))
            {
                dialougeResult = true;
            }
            window.DialogResult = dialougeResult;
            window.Close();
        }
    }
}
