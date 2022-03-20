using System;
using System.Windows;
using System.Windows.Controls;
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
            //Initialise components
            Logger.Log(MessageType.Trace, "Initialising new project");
            InitializeComponent();
        }

        //Method called when the create button is clicked
        private void Create_ButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Logger.Log(MessageType.Trace, "Create Project Button Pressed");

                //Get the view model and create it
                NewProject viewModel = (NewProject)DataContext;
                string projectPath = viewModel.CreateProject((ProjectTemplate)TemplatesView.SelectedItem) ?? throw new("Could not create new Project");

                //Get the window and set it's context to the new project
                var window = Window.GetWindow(this);
                window.DataContext = ExistingProjects.Open(new ProjectData() { ProjectName = viewModel.ProjectName, ProjectPath = projectPath });

                //Set the dialouge result to true and close the window
                window.DialogResult = true;
                window.Close();
            }
            catch (Exception ex)
            {
                Logger.Log(MessageType.Error, $"Error During Event: {ex.Message}");
            }
        }

        //Method called when the browse button is clicked
        private void Browse_ButtonClick(object sender, RoutedEventArgs e)
        {
            Logger.Log(MessageType.Trace, "Opening Browser");

            //Open a common file dialouge
            CommonOpenFileDialog openFileDialog = new CommonOpenFileDialog();

            //Make sure that the file selected is a single foulder
            openFileDialog.Multiselect = false;
            openFileDialog.IsFolderPicker = true;

            //Make the initial directory the default path of the projet
            openFileDialog.InitialDirectory = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\Atlas\Projects\";

            //If the result is valid
            if (openFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                //Change the project path to the the one selected
                ((NewProject)DataContext).ProjectPath = openFileDialog.FileName;
            }
        }
    }
}
