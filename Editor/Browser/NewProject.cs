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

        public string ProjectName
        {
            get => _projectName;
            set
            {
                if (_projectName != value)
                {
                    _projectName = value;
                    OnProprietyChanged(nameof(ProjectName));
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
                }
            }
        }

        private ObservableCollection<ProjectTemplate> _projectTemplates = new ObservableCollection<ProjectTemplate>();
        public ReadOnlyCollection<ProjectTemplate> ProjectTemplates { get; }

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
                    template.IconFilePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), "icon.png"));
                    template.Icon = File.ReadAllBytes(template.IconFilePath);
                    template.IconFilePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), template.ProjectFile));

                    _projectTemplates.Add(template);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
