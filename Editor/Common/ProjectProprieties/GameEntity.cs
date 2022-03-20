using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Input;

namespace Editor
{
    //Implements the view model base for a game entity
    [DataContract]
    [KnownType(typeof(Transform))]
    public class GameEntity : ViewModelBase
    {
        //Field and property for the enable status of the entity
        private bool _isEnabled;
        [DataMember]
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    OnProprietyChanged(nameof(IsEnabled));
                }
            }
        }

        //Field and property for the name of the entity
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

        //Commands to rename and enable an entity
        public ICommand? RenameCommand { get; private set; }
        public ICommand? EnableCommand { get; private set; }

        //Wraps the set operations with a undo redo action
        public void Rename(string name)
        {
            Logger.Log(MessageType.Trace, $"Renaming Entity {Name} to {name}");

            //Add a changed value UndoRedoAction on Name
            UndoRedoManager.Add(new UndoRedoAction($"Entity {Name} Renamed to {name}", nameof(Name), this, Name, name));
            Name = name;
        }
        public void Enable(bool status)
        {
            Logger.Log(MessageType.Trace, $"Changing the enabled status of Entity {Name} to {status}");

            //Add a changed value UndoRedoAction on IsEnabled
            UndoRedoManager.Add(new UndoRedoAction($"Entity enabled status Changed to {status}", nameof(IsEnabled), this, IsEnabled, status));
            IsEnabled = status;
        }

        //Collection of components owned by the entity
        [DataMember(Name = nameof(Components))]
        private ObservableCollection<Component> _components { get; set; }
        public ReadOnlyObservableCollection<Component>? Components { get; private set; }

        //The parent scene to the object
        [DataMember]
        public Scene Parent { get; private set; }

        //The method run post deserialisation
        [OnDeserialized]
        public void OnDeserialised(StreamingContext context)
        {
            Logger.Log(MessageType.Trace, $"Deserialising the Entity {Name}");

            //Make sure the entities array isn't null
            if (_components == null)
            {
                _components = new();
            }

            //Generate the property and invoke the property changed event
            Components = new(_components);
            OnProprietyChanged(nameof(Components));

            //Generate the commands
            RenameCommand = new RelayCommand<string>(newName => Rename(newName), newName => newName != Name);
            EnableCommand = new RelayCommand<bool>(status => Enable(status), null);
        }

        public GameEntity(Scene parent, string name)
        {
            //Add the parent, name and set the enabled status to true
            Parent = parent;
            _name = name;
            _isEnabled = true;

            //Run OnDeserialsied
            OnDeserialised(new StreamingContext());

            //Add a transform component
            _components!.Add(new Transform(this));
        }
    }

    //Implements the view model base for a multi selection game entity
    public abstract class MSEntity : ViewModelBase
    {
        //Field and property for the enable status of the multi select entity
        private bool? _isEnabled;
        public bool? IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    OnProprietyChanged(nameof(IsEnabled));
                }
            }
        }

        //Field and property for the name of the multi select entity
        private string? _name;
        public string? Name
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

        //Commands to rename and enable a group of entities
        public ICommand? RenameCommand { get; private set; }
        public ICommand? EnableCommand { get; private set; }

        //Wraps the multi set operations with a undo redo action
        public void Rename(string name)
        {
            Logger.Log(MessageType.Trace, "Initialising Renaming for Multi Select Object");

            //Adds an action group to rename all selected entities,
            //with a post command to refreshes the data
            UndoRedoManager.Add(new UndoRedoActionGroup(
                $"Entities Renamed to {name}", nameof(Name), SelectedEntities,
                SelectedEntities.Select((entity, i) => entity.Name), name, () => Refresh()));

            //Changes the name of all selected entities and refreshes the data
            SelectedEntities.ForEach(SelectedEntity => SelectedEntity.Name = name);
            Refresh();
        }
        public void Enable(bool status)
        {
            Logger.Log(MessageType.Trace, "Initialising Enabling for Multi Select Object");

            //Adds an action group to set enable status of all selected entities,
            //with a post command to refreshes the data
            UndoRedoManager.Add(new UndoRedoActionGroup(
                "Entities Enabled/Disabled", nameof(IsEnabled),
                SelectedEntities, SelectedEntities.Select((entity, i) => entity.IsEnabled).Cast<object>(), status, () => Refresh()));

            //Changes the name of all selected entities and refreshes the data
            SelectedEntities.ForEach(SelectedEntity => SelectedEntity.IsEnabled = status);
            Refresh();
        }

        //Collection of components owned by the multi selection of entities
        [DataMember(Name = nameof(Components))]
        private readonly ObservableCollection<IMSComponent> _components = new();
        public ReadOnlyObservableCollection<IMSComponent>? Components { get; private set; }

        //The list of selected entities
        public List<GameEntity> SelectedEntities { get; }

        //Helper function to update an multi select property
        protected void UpdateMSProperty(string propetyName)
        {
            //Check that the property exists
            if (GetType().GetProperty(propetyName) == null)
            {
                //If it doesn't log a warning and return
                Logger.Log(MessageType.Warning, $"Field {propetyName} does not exist -- Nothing set");
                return;
            }

            //If there are more then one value
            if (SelectedEntities.DistinctBy((entity) => { return entity.GetType().GetProperty(propetyName); }).Skip(1).Any())
            {
                //Set the property to null
                GetType().GetProperty(propetyName)?.SetValue(this, null);
            }
            //Otherwise
            else
            {
                //Get the value of the property of the first element
                var value = SelectedEntities.First().GetType().GetProperty(propetyName)!.GetValue(SelectedEntities.First());

                //And set the value of that property in the multi select component to that
                GetType().GetProperty(propetyName)?.SetValue(this, value);
            }
        }

        //Virtual Update method to be filed by other all properties
        protected virtual void UpdateMSProperties()
        {
            //Update Name and IsEnabled
            UpdateMSProperty(nameof(Name));
            UpdateMSProperty(nameof(IsEnabled));
        }

        //A public method to refresh the status of the multi select property
        public void Refresh()
        {
            Logger.Log(MessageType.Trace, "Refreshing Multi Select Entity");

            UpdateMSProperties();
        }

        public MSEntity(List<GameEntity> entities)
        {
            //Check the the there are entities selected
            Debug.Assert(entities.Any() == true);

            //Generate the components list and save the selected entities
            Components = new(_components);
            SelectedEntities = entities;

            //Generate the commands
            RenameCommand = new RelayCommand<string>(newName => Rename(newName), newName => newName != Name);
            EnableCommand = new RelayCommand<bool>(status => Enable(status), null);
        }
    }

    //The implementation of MSEntity on a game entity
    public class MSGameEntity : MSEntity
    {
        public MSGameEntity(List<GameEntity> entities) : base(entities)
        {
            //Refresh the 
            Refresh();
        }
    }
}
