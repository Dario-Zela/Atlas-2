using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Editor
{
    [DataContract]
    [KnownType(typeof(Transform))]
    public class GameEntity : ViewModelBase
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

        [DataMember(Name = "Components")]
        private ObservableCollection<Component> _components { get; set; }
        public ReadOnlyObservableCollection<Component>? Components { get; private set; }

        [DataMember]
        public Scene Parent { get; private set; }

        [OnDeserialized]
        public void OnDeserialised(StreamingContext context)
        {
            if (_components == null)
            {
                _components = new();
            }

            Components = new(_components);
            OnProprietyChanged(nameof(Components));

            //AddEntityCommand = new RelayCommand<string>(entityName => { AddEntity(entityName); }, null);
            //RemoveEntityCommand = new RelayCommand<GameEntity>(entity => { RemoveEntity(entity); }, null);
        }

        public GameEntity(Scene parent, string name)
        {
            Debug.Assert(parent != null);
            Parent = parent;
            _name = name;

            OnDeserialised(new StreamingContext());
            _components!.Add(new Transform(this));
        }
    }
}
