using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Input;

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

        public ICommand? AddEntityCommand { get; private set; }
        public ICommand? RemoveEntityCommand { get; private set; }

        public void AddEntity(string entityName)
        {
            var entity = InternalAddNewEntity(entityName);
            UndoRedoManager.Add(new UndoRedoAction(
                "Add Entity",
                () => { InternalRemoveEntity(entity); },
                () => { InternalInsertEntity(entity, Entities!.Count - 1); }
                ));
        }
        public void RemoveEntity(GameEntity entity)
        {
            int pos = Entities!.IndexOf(entity);
            InternalRemoveEntity(entity);
            UndoRedoManager.Add(new UndoRedoAction(
                "Remove Entity",
                () => { InternalInsertEntity(entity, pos); },
                () => { InternalRemoveEntity(entity); }
                ));
        }

        private GameEntity InternalAddNewEntity(string entityName)
        {
            Debug.Assert(!string.IsNullOrEmpty(entityName.Trim()));
            var entity = new GameEntity(this, entityName);
            _entities!.Add(entity);

            return entity;
        }
        private void InternalInsertEntity(GameEntity entity, int pos = -1)
        {
            Debug.Assert(entity != null);
            _entities!.Insert(pos < 0 ? Entities!.Count : pos, entity);
        }
        private void InternalRemoveEntity(GameEntity entity)
        {
            Debug.Assert(_entities!.Contains(entity));
            _entities.Remove(entity);
        }

        public bool IsActive => Project.ActiveScene == this;

        [DataMember(Name = nameof(Entities))]
        private ObservableCollection<GameEntity>? _entities { get; set; }
        public ReadOnlyObservableCollection<GameEntity>? Entities { get; private set; }

        [OnDeserialized]
        public void OnDeserialised(StreamingContext context)
        {
            if (_entities == null)
            {
                _entities = new();
            }

            Entities = new(_entities);
            OnProprietyChanged(nameof(Entities));

            AddEntityCommand = new RelayCommand<string>(entityName => { AddEntity(entityName); }, null);
            RemoveEntityCommand = new RelayCommand<GameEntity>(entity => { RemoveEntity(entity); }, null);
        }

        public Scene(Project project, string name)
        {
            Debug.Assert(project != null);
            Project = project;
            _name = name;

            OnDeserialised(new StreamingContext());
        }

    }
}
