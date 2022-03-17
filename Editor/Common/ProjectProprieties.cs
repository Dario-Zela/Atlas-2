using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Editor
{
    [DataContract]
    public class Project : ViewModelBase
    {
        public static string Extention => ".at";
        private string _name;

        [DataMember]
        public string Name 
        {
            get => _name;  
            set
            {
                if(_name != value)
                {
                    _name = value;
                    OnProprietyChanged(nameof(Name));
                }
            }
        }
        [DataMember]
        public string Path { get; private set; }
        public string FullPath => $"{Path}{Name}{Extention}";

        [DataMember(Name = "Scenes")]
        private readonly ObservableCollection<Scene> _scenes = new();
        public ReadOnlyCollection<Scene> Scenes { get; }

        public Project(string name, string path)
        {
            _name = name;
            Path = path;
            _scenes.Add(new Scene(this, "Default Scene"));
            Scenes = new ReadOnlyCollection<Scene>(_scenes);
        }
    }

    [DataContract]
    public class Scene : ViewModelBase
    {
        private string _name;

        [DataMember]
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnProprietyChanged(nameof(Name));
                }
            }
        }
        [DataMember]
        public Project Project { get; private set; }

        public Scene(Project project, string name)
        {
            Debug.Assert(project != null);
            Project = project;
            _name = name;
        }

/*        private ObservableCollection<Scene> _scenes = new ObservableCollection<Scene>();
        public ReadOnlyCollection<Scene>? Scenes { get; }*/
    }
}
