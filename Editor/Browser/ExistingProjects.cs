using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace Editor.Browser
{
    [DataContract]
    public class ProjectData
    {
        //Name of the project
        [DataMember]
        public string ProjectName { get; set; } = string.Empty;

        //Location of the project
        [DataMember]
        public string ProjectPath { get; set; } = string.Empty;

        //Date last modified
        [DataMember]
        public DateTime LastModified { get; set; }

        //The full path of the project
        public string FullPath { get => $@"{ProjectPath}{ProjectName}{Project.Extention}"; }
    }

    [DataContract]
    public struct ProjectDataList
    {
        //A list of the known projects
        [DataMember]
        public List<ProjectData> Projects { get; set; }
    }

    public class ExistingProjects
    {
        //The location of the folder of the record of know projects
        private static readonly string _applicationDataPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\Atlas\";
        //The location of the record of know projects
        private static readonly string _projectDataPath = $@"{_applicationDataPath}ProjectData.xml";

        //The known projects
        private static readonly ObservableCollection<ProjectData> _projects = new();
        public static ReadOnlyObservableCollection<ProjectData> Projects { get; }

        //Reading the data from the file
        private static void ReadProjectData()
        {
            Logger.Log(MessageType.Trace, "Reading Project Data");

            //Deserialise the data lsit and clear the 
            var projects = Serialiser.FromFile<ProjectDataList>(_projectDataPath).Projects.OrderBy(x => x.LastModified);
            _projects.Clear();

            //Add each existing project to the list
            foreach (var project in projects)
            {
                if (File.Exists(project.FullPath))
                {
                    _projects.Add(project);
                }
            }
        }

        public static void Save(Project? project)
        {
            Logger.Log(MessageType.Trace, "Saving Project Data");

            //Making sure the project isn't null get the project data
            Debug.Assert(project != null);
            var projectData = _projects.FirstOrDefault(x => x.FullPath == project.FullPath);

            //Modify the last modified term
            projectData!.LastModified = DateTime.Now;

            //Save the data
            WriteProjectData();
        }

        public static Project? Open(ProjectData projectData)
        {
            Logger.Log(MessageType.Trace, "Opening Project");

            //Read the project data to make sure there were no changes in availability of files
            ReadProjectData();

            //Get the corresponding project data
            var project = _projects.FirstOrDefault(x => x.FullPath == projectData.FullPath);

            //If it is null, it's a new project,
            //So add it to the list
            if (project == null)
            {
                project = projectData;
                project.LastModified = DateTime.Now;
                _projects.Add(project);
            }

            //Update the record
            WriteProjectData();

            //Return the project
            return Project.Load(project.FullPath);
        }

        private static void WriteProjectData()
        {
            Logger.Log(MessageType.Trace, "Writing Project Data");

            //Get all of the projects in order of last modified
            var projects = _projects.OrderBy(x => x.LastModified).ToList();

            //And Write to the file
            Serialiser.ToFile(new ProjectDataList() { Projects = projects }, _projectDataPath);
        }

        static ExistingProjects()
        {
            Logger.Log(MessageType.Trace, "Creating Existing Projects");

            //Initialise the readonly list
            Projects = new(_projects);

            try
            {
                //If there is no folder, make it
                if (!Directory.Exists(_applicationDataPath)) Directory.CreateDirectory(_applicationDataPath);

                //And read the data
                ReadProjectData();

                Logger.Log(MessageType.Trace, "Existing Projects Created Successfully");
            }
            catch (Exception ex)
            {
                Logger.Log(MessageType.Error, $"Error During Contruction: {ex.Message}");
            }
        }
    }
}
