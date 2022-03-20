using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Editor
{
    [DataContract]
    [KnownType(typeof(Transform))]
    public class GameEntity : ViewModelBase
    {
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

        public ICommand? RenameCommand { get; private set; }
        public ICommand? EnableCommand { get; private set; }

        public void Rename(string name)
        {
            UndoRedoManager.Add(new UndoRedoAction("Entity Renamed", nameof(Name), this, Name, name));
            Name = name;
        }
        public void Enable(bool status)
        {
            UndoRedoManager.Add(new UndoRedoAction("Entity Enabled/Disabled", nameof(IsEnabled), this, IsEnabled, status));
            IsEnabled = status;
        }

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

        [DataMember(Name = nameof(Components))]
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

            RenameCommand = new RelayCommand<string>(newName => Rename(newName), newName => newName != Name);
            EnableCommand = new RelayCommand<bool>(status => Enable(status), null);

            //AddEntityCommand = new RelayCommand<string>(entityName => { AddEntity(entityName); }, null);
            //RemoveEntityCommand = new RelayCommand<GameEntity>(entity => { RemoveEntity(entity); }, null);
        }

        public GameEntity(Scene parent, string name)
        {
            Debug.Assert(parent != null);
            Parent = parent;
            _name = name;
            _isEnabled = true;

            OnDeserialised(new StreamingContext());
            _components!.Add(new Transform(this));
        }
    }

    public abstract class MSEntity : ViewModelBase
    {
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

        public ICommand? RenameCommand { get; private set; }
        public ICommand? EnableCommand { get; private set; }

        public void Rename(string name)
        {
            UndoRedoManager.Add(new UndoRedoActionGroup(
                "Entities Renamed", nameof(Name), SelectedEntities, 
                SelectedEntities.Select((entity, i) => entity.Name), name, () => Refresh()));

            SelectedEntities.ForEach(SelectedEntity => SelectedEntity.Name = name);
            Name = name;
        }
        public void Enable(bool status)
        {
            UndoRedoManager.Add(new UndoRedoActionGroup(
                "Entities Enabled/Disabled", nameof(IsEnabled), 
                SelectedEntities, SelectedEntities.Select((entity, i) => entity.IsEnabled).Cast<object>(), status, () => Refresh()));

            SelectedEntities.ForEach(SelectedEntity => SelectedEntity.IsEnabled = status);
            IsEnabled = status;
        }

        [DataMember(Name = nameof(Components))]
        private readonly ObservableCollection<IMSComponent> _components = new();
        public ReadOnlyObservableCollection<IMSComponent>? Components { get; private set; }

        public List<GameEntity> SelectedEntities { get; }

        protected virtual void UpdateMSPropieties()
        {
            if (SelectedEntities.DistinctBy((entity) => { return entity.Name; }).Skip(1).Any())
            {
                Name = null;
            }
            else
            {
                Name = SelectedEntities.First().Name;
            }

            if (SelectedEntities.DistinctBy((entity) => { return entity.IsEnabled; }).Skip(1).Any())
            {
                IsEnabled = null;
            }
            else
            {
                IsEnabled = SelectedEntities.First().IsEnabled;
            }
        }

        public void Refresh()
        {
            UpdateMSPropieties();
        }

        public MSEntity(List<GameEntity> entities)
        {
            Debug.Assert(entities?.Any() == true);
            Components = new(_components);
            SelectedEntities = entities;

            RenameCommand = new RelayCommand<string>(newName => Rename(newName), newName => newName != Name);
            EnableCommand = new RelayCommand<bool>(status => Enable(status), null);
        }
    }

    public class MSGameEntity : MSEntity
    {
        public MSGameEntity(List<GameEntity> entities) : base(entities)
        {
            Refresh();
        }
    }
}
