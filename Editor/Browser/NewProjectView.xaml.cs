using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

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

        private void Create_ButtonClick(object sender, RoutedEventArgs e)
        {
            NewProject viewModel = (NewProject)DataContext;
            string projectPath = viewModel.CreateProject((ProjectTemplate)TemplatesView.SelectedItem);

            bool dialougeResult = false;
            var window = Window.GetWindow(this);

            if (!string.IsNullOrEmpty(projectPath))
            {
                dialougeResult = true;
                window.DataContext = ExistingProjects.Open(new ProjectData() { ProjectName = viewModel.ProjectName, ProjectPath = projectPath});
            }
            window.DialogResult = dialougeResult;
            window.Close();
        }

        private void Browse_ButtonClick(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog openFileDialog = new CommonOpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.IsFolderPicker = true;

            openFileDialog.InitialDirectory = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\Atlas\Projects\";

            if (openFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                ((NewProject)DataContext).ProjectPath = openFileDialog.FileName;
            }
        }
    }
}
