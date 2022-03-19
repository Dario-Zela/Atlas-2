using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace Editor.Browser
{
    /// <summary>
    /// Interaction logic for Existing_Projects.xaml
    /// </summary>
    public partial class Existing_Projects : UserControl
    {
        public Existing_Projects()
        {
            InitializeComponent();
        }

        private void Add_ButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Atlas Files (*.at)|*.at";
            openFileDialog.Multiselect = false;
            openFileDialog.InitialDirectory = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\Atlas\Projects\";

            if (openFileDialog.ShowDialog() == true)
            {
                var filePath = openFileDialog.FileName;
                var project = Serialiser.FromFile<Project>(filePath);

                if (project!.Path != Path.GetDirectoryName(filePath) + @"\")
                {
                    project!.ChangePath(Path.GetDirectoryName(filePath) + @"\");
                    Serialiser.ToFile(project, filePath);
                }

                Window.GetWindow(this).DataContext = ExistingProjects.Open(new ProjectData() { ProjectName = project!.Name, ProjectPath = Path.GetDirectoryName(filePath) + @"\" });
                Window.GetWindow(this).DialogResult = true;
                Window.GetWindow(this).Close();
            }
        }

        private void OpenProjectHandler(object sender, RoutedEventArgs e)
        {
            var project = ExistingProjects.Open((ProjectData)ExistingProjectsView.SelectedItem);

            bool dialougeResult = false;
            var window = Window.GetWindow(this);

            if (project != null)
            {
                dialougeResult = true;
                window.DataContext = project;
            }
            window.DialogResult = dialougeResult;
            window.Close();
        }
    }
}
