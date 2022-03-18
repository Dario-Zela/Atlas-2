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
    public class GameEntity : ViewModelBase
    {
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

        [DataMember(Name = "Components")]
        private readonly ObservableCollection<Component> _components = new();
        private ReadOnlyObservableCollection<Component> Components { get; }

        [DataMember]
        public Scene Parent { get; private set; }

        public GameEntity(Scene parent)
        {
            Debug.Assert(parent != null);
            Parent = parent;
            _name = "Default Entity";
            Components = new(_components);
        }
    }
}
