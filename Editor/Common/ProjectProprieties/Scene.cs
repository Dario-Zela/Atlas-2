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

        public bool IsActive => Project.ActiveScene == this;

        [DataMember(Name = "Scenes")]
        private ObservableCollection<GameEntity> _scenes = new();
        public ReadOnlyObservableCollection<GameEntity>? Scenes { get; }

        public Scene(Project project, string name)
        {
            Debug.Assert(project != null);
            Project = project;
            _name = name;
            Scenes = new(_scenes)
        }

    }
}
