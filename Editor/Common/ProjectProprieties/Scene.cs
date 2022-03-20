using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Windows.Input;

namespace Editor
{
    //Implements the view model base for a scene
    [DataContract]
    public class Scene : ViewModelBase
    {
        //Field and property for the name of the scene
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

        //The parent project
        [DataMember]
        public Project Parent { get; private set; }

        //Commands to add and remove entities from a scene
        public ICommand? AddEntityCommand { get; private set; }
        public ICommand? RemoveEntityCommand { get; private set; }

        //Wrap the internal scene to allow for undoing and redoing
        //The action
        public void AddEntity(string entityName)
        {
            //Add the entity and stores it's value
            var entity = InternalAddNewEntity(entityName);

            //Whenerver the action is undone, the entity is remove
            //And when it is redone, it is added back to it's previous location
            UndoRedoManager.Add(new UndoRedoAction(
                $"Add Entity {entityName} to Scene {Name}",
                () => { InternalRemoveEntity(entity); },
                () => { InternalInsertEntity(entity, (uint)Entities!.Count - 1); }
                ));
        }
        public void RemoveEntity(GameEntity entity)
        {
            //Get the position of the removed entity from the array
            uint pos = (uint)Entities!.IndexOf(entity);
            //And remove it
            InternalRemoveEntity(entity);

            //Whenerver the action is undone, the entity is inserted back in it's original place
            //And when it is redone, it is removed again
            UndoRedoManager.Add(new UndoRedoAction(
                $"Remove Entity {entity.Name} from Scene {Name}",
                () => { InternalInsertEntity(entity, pos); },
                () => { InternalRemoveEntity(entity); }
                ));
        }

        //Internal addition of a new entity
        private GameEntity InternalAddNewEntity(string entityName)
        {
            Logger.Log(MessageType.Trace, $"Adding the entity {entityName} to the scene {Name}");

            //Make sure the name is not invalid
            Debug.Assert(!string.IsNullOrEmpty(entityName.Trim()));

            //Make a new entity and add it to the list
            var entity = new GameEntity(this, entityName);
            _entities!.Add(entity);

            //Return the entity
            return entity;
        }
        //Internal insertion of an entity at a location
        private void InternalInsertEntity(GameEntity entity, uint pos)
        {
            Logger.Log(MessageType.Trace, $"Adding the entity {entity.Name} to the scene {Name} at position {pos}");

            //Make sure the entity is in a valid location
            Debug.Assert(_entities!.Count >= pos);

            //Insert it back to it's original place
            _entities!.Insert((int)pos, entity);
        }
        //Internal removal of an entity 
        private void InternalRemoveEntity(GameEntity entity)
        {
            Logger.Log(MessageType.Trace, $"Removing the entity {entity.Name} from the scene {Name}");

            //Make sure the entity is present in the list
            Debug.Assert(_entities!.Contains(entity));

            //Remove the entity
            _entities.Remove(entity);
        }

        //Indictes if the scene is currently active
        public bool IsActive => Parent.ActiveScene == this;

        //A store of the entities contained by the scene
        [DataMember(Name = nameof(Entities))]
        private ObservableCollection<GameEntity>? _entities = new();
        public ReadOnlyObservableCollection<GameEntity>? Entities { get; private set; }

        //The method run post deserialisation
        [OnDeserialized]
        public void OnDeserialised(StreamingContext context)
        {
            Logger.Log(MessageType.Trace, $"Deserialising the Scene {Name}");

            //Make sure the entities array isn't null
            if (_entities == null)
            {
                _entities = new();
            }

            //Generate the property and invoke the property changed event
            Entities = new(_entities);
            OnProprietyChanged(nameof(Entities));

            //Generate the commands
            AddEntityCommand = new RelayCommand<string>(entityName => { AddEntity(entityName); }, null);
            RemoveEntityCommand = new RelayCommand<GameEntity>(entity => { RemoveEntity(entity); }, null);
        }

        public Scene(Project project, string name)
        {
            //Add the parent and the name
            Parent = project;
            _name = name;

            //Run the OnDeserialised method
            OnDeserialised(new StreamingContext());
        }
    }
}
