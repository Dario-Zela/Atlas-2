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
            //Initialise components
            Logger.Log(MessageType.Trace, "Initialising existing projects");
            InitializeComponent();
        }

        //Method called when the add button is pressed
        private void Add_ButtonClick(object sender, RoutedEventArgs e)
        {
            Logger.Log(MessageType.Trace, "Adding extra file");

            //Open the file browser with a filter on atlas files
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Atlas Files (*.at)|*.at";

            //Make sure there is no multi file selection and that the initial
            //directory the default path of the projet
            openFileDialog.Multiselect = false;
            openFileDialog.InitialDirectory = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\Atlas\Projects\";

            //If there is a valid response
            if (openFileDialog.ShowDialog() == true)
            {
                Logger.Log(MessageType.Info, "File Selected");

                //Get the filepath of the project and open it
                var filePath = openFileDialog.FileName;
                var project = Serialiser.FromFile<Project>(filePath);

                if (project != null)
                {
                    Logger.Log(MessageType.Error, "The project file wasn't readable");
                    throw new("The project file wasn't readable");
                }

                //If the to the file has changed, updated it
                if (project!.Path != Path.GetDirectoryName(filePath) + @"\")
                {
                    project!.ChangePath(Path.GetDirectoryName(filePath) + @"\");
                    Serialiser.ToFile(project, filePath);
                }

                //Then change the data context of the window to that of the project
                //Using open to update the register of known file location
                Window.GetWindow(this).DataContext = ExistingProjects.Open(new ProjectData() { ProjectName = project!.Name, ProjectPath = Path.GetDirectoryName(filePath) + @"\" });

                //Set the dialog result and close the window
                Window.GetWindow(this).DialogResult = true;
                Window.GetWindow(this).Close();
            }

            Logger.Log(MessageType.Info, "No File Selected");
        }

        //Method called whenever an open project command is initaiated
        private void OpenProjectHandler(object sender, RoutedEventArgs e)
        {
            Logger.Log(MessageType.Trace, "Opening Project");

            //If nothing is selected, do nothing
            if (ExistingProjectsView.SelectedItem == null) return;

            try
            {
                //Open the project being selected if it exists
                var project = ExistingProjects.Open((ProjectData)ExistingProjectsView.SelectedItem) ?? throw new("Couldn't open existing project");

                //Get the window and modify the data context
                var window = Window.GetWindow(this);
                window.DataContext = project;

                //Set the dialog result to true and close the window
                window.DialogResult = true;

                Logger.Log(MessageType.Info, "Opening Project Succeded");

                window.Close();
            }
            catch (Exception ex)
            {
                Logger.Log(MessageType.Info, $"Opening Project Failed: {ex.Message}");
            }
        }
    }
}
