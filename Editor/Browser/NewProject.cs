using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Browser
{
    [DataContract]
    public struct ProjectTemplate
    {
        [DataMember]
        public string ProjectType { get; set; }
        [DataMember]
        public string ProjectFile { get; set; }
        [DataMember]
        public List<string> Folders { get; set; }

        public byte[] Icon { get; set; }
        public string IconFilePath { get; set; }

        public string ProjectFilePath { get; set; }
    }

    public class NewProject : ViewModelBase
    {
        private readonly string _templatePath = $@"..\..\ProjectTemplates\";

        private string _projectPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\Atlas\Projects\";
        private string _projectName = "New Project";
        private bool _isValid = true;

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

        private readonly ObservableCollection<ProjectTemplate> _projectTemplates = new();
        public ReadOnlyCollection<ProjectTemplate> ProjectTemplates { get; }

        private bool ValidateProject()
        {
            var path = ProjectPath;
            if(!Path.EndsInDirectorySeparator(path))
                path += @"\";

            path += $@"{ProjectName}\";

            IsValid = false;

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
            else if (Directory.Exists(path) && Directory.EnumerateFileSystemEntries(path).Any())
            {
                return false;
            }

            IsValid = true;
            return true;
        }

        public string CreateProject(ProjectTemplate projectTemplate)
        {
            if (!ValidateProject()) 
                return string.Empty;

            var path = ProjectPath;
            if (!Path.EndsInDirectorySeparator(path))
                path += @"\";

            path += $@"{ProjectName}\";

            try
            {
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                foreach (var foulder in projectTemplate.Folders)
                {
                    Directory.CreateDirectory(Path.GetFullPath(Path.Combine(Path.GetDirectoryName(path) ?? "", foulder)));
                }

                var directory = new DirectoryInfo(path + ".Atlas");
                directory.Attributes |= FileAttributes.Hidden;

                var projectXML = File.ReadAllText(projectTemplate.ProjectFilePath);
                projectXML = String.Format(projectXML, ProjectName, ProjectPath);
                var projectPath = Path.GetFullPath(Path.Combine(path, $"{ProjectName}{Project.Extention}"));
                File.WriteAllText(projectPath, projectXML);

                return path;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return string.Empty;
            }
        }

        public NewProject()
        {
            ProjectTemplates = new ReadOnlyCollection<ProjectTemplate>(_projectTemplates);

            try
            {
                var templates = Directory.GetFiles(_templatePath, "template.xml", SearchOption.AllDirectories);
                Debug.Assert(templates.Any());

                foreach (var file in templates)
                {
                    var template = Serialiser.FromFile<ProjectTemplate>(file);
                    string directory = Path.GetDirectoryName(file)!.ToString();

                    template.IconFilePath = Path.GetFullPath(Path.Combine(directory, "icon.png"));
                    template.Icon = File.ReadAllBytes(template.IconFilePath);
                    template.ProjectFilePath = Path.GetFullPath(Path.Combine(directory, template.ProjectFile));

                    _projectTemplates.Add(template);
                }

                ValidateProject();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
