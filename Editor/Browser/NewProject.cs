﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace Editor.Browser
{
    [DataContract]
    public struct ProjectTemplate
    {
        //The type of project generated by the template
        [DataMember]
        public string ProjectType { get; set; }

        //The name of the templated file
        [DataMember]
        public string ProjectFile { get; set; }

        //The name of the folders that will be generated
        [DataMember]
        public List<string> Folders { get; set; }

        //The icon used to display it on the hub
        public byte[] Icon { get; set; }
        //The location of the icon
        public string IconFilePath { get; set; }

        //The full path to the project template
        public string ProjectFilePath { get; set; }
    }

    public class NewProject : ViewModelBase
    {
        //The folder location of the template
        //TODO -- Better finding of folder
        private readonly string _templatePath = $@"..\..\ProjectTemplates\";

        //The location of the new project
        private string _projectPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\Atlas\Projects\";

        //The name of the projct
        private string _projectName = "New Project";

        //A check on the validity of the new project
        private bool _isValid = true;

        //The templates that will be visible for the user
        private readonly ObservableCollection<ProjectTemplate> _projectTemplates = new();

        //The public proprieties for the fields
        public string ProjectName
        {
            get => _projectName;
            set
            {
                if (_projectName != value)
                {
                    _projectName = value;
                    OnProprietyChanged(nameof(ProjectName));
                    ValidateProject();
                }
            }
        }
        public string ProjectPath
        {
            get => _projectPath;
            set
            {
                if (_projectPath != value)
                {
                    _projectPath = value;
                    OnProprietyChanged(nameof(ProjectPath));
                    ValidateProject();
                }
            }
        }
        public bool IsValid
        {
            get => _isValid;
            set
            {
                if (_isValid != value)
                {
                    _isValid = value;
                    OnProprietyChanged(nameof(IsValid));
                }
            }
        }
        public ReadOnlyObservableCollection<ProjectTemplate> ProjectTemplates { get; }


        //A method called whenever the project definition changes
        private bool ValidateProject()
        {
            Logger.Log(MessageType.Trace, "Validating Project");

            //Make sure the path ends in a separator
            var path = ProjectPath;
            if (!Path.EndsInDirectorySeparator(path))
                path += @"\";

            //And add the name of the project to it
            path += $@"{ProjectName}\";

            //IsValid will be initialised to false
            IsValid = false;

            //If the name or path are empty or possess invalid characters
            //The new project is not valid
            if (string.IsNullOrEmpty(ProjectName.Trim()))
            {
                return false;
            }
            else if (ProjectName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
            {
                return false;
            }
            else if (string.IsNullOrEmpty(ProjectPath.Trim()))
            {
                return false;
            }
            else if (ProjectPath.IndexOfAny(Path.GetInvalidPathChars()) != -1)
            {
                return false;
            }
            //If the project exists already, the project is also invalid
            else if (Directory.Exists(path) && Directory.EnumerateFileSystemEntries(path).Any())
            {
                return false;
            }

            Logger.Log(MessageType.Info, "Project Valid");

            //If it passes those conditions the project is valid
            IsValid = true;
            return true;
        }

        //Called when the project is created
        public string CreateProject(ProjectTemplate projectTemplate)
        {
            Logger.Log(MessageType.Trace, "Creating Project");

            //Make sure the project ends in a separator
            var path = ProjectPath;
            if (!Path.EndsInDirectorySeparator(path))
                path += @"\";

            //And add the name of the project
            path += $@"{ProjectName}\";

            try
            {
                //If the path doesn't exist already, make it
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                //Then make all of the folders that are to be constructed
                foreach (var folder in projectTemplate.Folders)
                {
                    Directory.CreateDirectory(Path.GetFullPath(Path.Combine(Path.GetDirectoryName(path) ?? "", folder)));
                }

                //Make the .Atlas foulder hidden
                var directory = new DirectoryInfo(path + ".Atlas");
                directory.Attributes |= FileAttributes.Hidden;

                //Then generate the project file
                var projectXML = File.ReadAllText(projectTemplate.ProjectFilePath);
                projectXML = String.Format(projectXML, ProjectName, path);

                //Get the full path of the project
                var projectPath = Path.GetFullPath(Path.Combine(path, $@"{ProjectName}{Project.Extention}"));
                //And save it
                File.WriteAllText(projectPath, projectXML);

                Logger.Log(MessageType.Info, "Create Project Succeded");

                //Return the path
                return path;
            }
            catch (Exception ex)
            {
                //If an error occoured return null
                Logger.Log(MessageType.Error, $"Error During Contruction: {ex.Message}");
                return string.Empty;
            }
        }

        public NewProject()
        {
            Logger.Log(MessageType.Trace, "New Project Class Constructed");

            //Set up the readonly collection
            ProjectTemplates = new(_projectTemplates);

            try
            {
                //Get all of the templates in the path making sure that at least one exists
                var templates = Directory.GetFiles(_templatePath, "template.xml", SearchOption.AllDirectories);
                Debug.Assert(templates.Any());

                //For each template location
                foreach (var file in templates)
                {
                    //Generate the project template and get the name of the directory
                    var template = Serialiser.FromFile<ProjectTemplate>(file);
                    //If it doesn't exist, throw an error
                    string directory = Path.GetDirectoryName(file) ?? throw new();

                    //Get the icon path, icon data and the path to the project template
                    template.IconFilePath = Path.GetFullPath(Path.Combine(directory, "icon.png"));
                    template.Icon = File.ReadAllBytes(template.IconFilePath);
                    template.ProjectFilePath = Path.GetFullPath(Path.Combine(directory, template.ProjectFile));

                    //Add the template to the list of templates
                    _projectTemplates.Add(template);
                }

                Logger.Log(MessageType.Trace, "New Project Class Constructed Successfully");

                //Validate the project
                ValidateProject();
            }
            catch (Exception ex)
            {
                //If there are any errors, write down the message
                Logger.Log(MessageType.Error, $"Error During Contruction: {ex.Message}");
            }
        }
    }
}
