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
        [DataMember]
        public string ProjectName { get; set; } = string.Empty;
        [DataMember]
        public string ProjectPath { get; set; } = string.Empty;
        [DataMember]
        public DateTime LastModified { get; set; }

        public string FullPath { get => $@"{ProjectPath}{ProjectName}{Project.Extention}"; }
    }

    [DataContract]
    public struct ProjectDataList
    {
        [DataMember]
        public List<ProjectData> Projects { get; set; }
    }

    public class ExistingProjects
    {
        private static readonly string _applicationDataPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\Atlas\";
        private static readonly string _projectDataPath;

        private static readonly ObservableCollection<ProjectData> _projects = new();
        public static ReadOnlyObservableCollection<ProjectData> Projects { get; }

        private static void ReadProjectData()
        {
            var projects = Serialiser.FromFile<ProjectDataList>(_projectDataPath).Projects.OrderBy(x => x.LastModified);
            _projects.Clear();

            foreach (var project in projects)
            {
                if (File.Exists(project.FullPath))
                {
                    _projects.Add(project);
                }
            }
        }

        public static Project? Open(ProjectData projectData)
        {
            ReadProjectData();
            var project = _projects.FirstOrDefault(x => x.FullPath == projectData.FullPath);

            if (project == null)
            {
                project = projectData;
                project.LastModified = DateTime.Now;
                _projects.Add(project);
            }
            else
            {
                project.LastModified = DateTime.Now;
            }

            WriteProjectData();
            return Project.Load(project.FullPath);
        }

        private static void WriteProjectData()
        {
            var projects = _projects.OrderBy(x => x.LastModified).ToList();
            Serialiser.ToFile(new ProjectDataList() { Projects = projects }, _projectDataPath);
        }

        static ExistingProjects()
        {
            _projectDataPath = $@"{_applicationDataPath}ProjectData.xml";
            Projects = new(_projects);

            try
            {
                if (!Directory.Exists(_applicationDataPath)) Directory.CreateDirectory(_applicationDataPath);
                ReadProjectData();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
