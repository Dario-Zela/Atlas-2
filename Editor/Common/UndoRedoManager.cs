using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Editor
{
    //Implements and undoable and redoable action
    public interface IUndoRedo
    {
        //Name of of action
        public string Name { get; }

        //Undo action
        public abstract void Redo();

        //Redo action
        public abstract void Undo();
    }

    //Implements IUndoRedo for single actions
    public class UndoRedoAction : IUndoRedo
    {
        //The undo and redo actions
        private Action _undo;
        private Action _redo;

        //The name of the action
        public string Name { get; private set; }

        public void Redo()
        {
            //Redo
            Logger.Log(MessageType.Trace, $"Redoing {Name}");
            _redo();
        }

        public void Undo()
        {
            Logger.Log(MessageType.Trace, $"Undoing {Name}");
            _undo();
        }

        //Implementing UndoRedo for given actions
        public UndoRedoAction(string name, Action undo, Action redo)
        {
            Name = name;
            _undo = undo;
            _redo = redo;
        }

        //Implementing UndoRedo for a change of value of  variable
        public UndoRedoAction(string name, string proprietyChanged, object propriety,
                              object oldValue, object newValue)
        : this(name,
              () => propriety.GetType().GetProperty(proprietyChanged)!.SetValue(propriety, oldValue),
              () => propriety.GetType().GetProperty(proprietyChanged)!.SetValue(propriety, newValue)
             )
        { }
    }

    //Implements IUndoRedo for group actions
    public class UndoRedoActionGroup : IUndoRedo
    {
        //The name of the group action
        public string Name { get; private set; }

        //The list of actions in the group
        private List<UndoRedoAction> _actions = new();

        //Action to be done after executing the group
        private Action? _postUndoRedoAction;

        public void Redo()
        {
            //Redo all actions
            foreach (var action in _actions)
            {
                action.Redo();
            }

            //And do the postUnodRedoAction
            _postUndoRedoAction?.Invoke();
        }

        public void Undo()
        {
            //Undo all actions
            foreach (var action in _actions)
            {
                action.Undo();
            }

            //And do the postUnodRedoAction
            _postUndoRedoAction?.Invoke();
        }

        public UndoRedoActionGroup(string name, string proprietyChanged, IEnumerable<object> proprieties,
                      IEnumerable<object> oldValues, object newValue, Action? postUndoRedoAction = null)
        {
            //Set the name and postUndoRedo Action
            Name = name;
            _postUndoRedoAction = postUndoRedoAction;

            //For each propety add a changed value undoRedoAction
            for (int i = 0; i < proprieties.Count(); i++)
            {
                _actions.Add(new UndoRedoAction("",
                    proprietyChanged,
                    proprieties.ElementAt(i),
                    oldValues.ElementAt(i),
                    newValue));
            }
        }
    }

    public static class UndoRedoManager
    {
        //Indictates if the class is doing something and stops
        //The addition of new actions
        private static bool _isEditing = false;

        //The list of undoable and redoable actions
        private static readonly ObservableCollection<IUndoRedo> _undoList = new();
        private static readonly ObservableCollection<IUndoRedo> _redoList = new();

        //The public property to the field
        public static ReadOnlyObservableCollection<IUndoRedo>? UndoList { get; } = new(_undoList);
        public static ReadOnlyObservableCollection<IUndoRedo>? RedoList { get; } = new(_redoList);

        //Indicates if there are undoable or redoable actions
        public static bool IsUndoAvailable => _undoList.Any();
        public static bool IsRedoAvailable => _redoList.Any();

        //Hanlders for the changed events
        public static event EventHandler IsUndoAvailableChanged;
        public static event EventHandler IsRedoAvailableChanged;

        //Function that updates the availability of the two events
        private static void OnAvailabilityChanged(EventArgs e)
        {
            EventHandler handler = IsUndoAvailableChanged;

            if (handler != null)
            {
                handler(null, e);
            }

            handler = IsRedoAvailableChanged;

            if (handler != null)
            {
                handler(null, e);
            }
        }

        //Adds a new IUNdoRedo to the list
        public static void Add(IUndoRedo action)
        {
            //If it is not called during the editing process
            if (!_isEditing)
            {
                Logger.Log(MessageType.Trace, action.Name);

                //Add the action and clear the redo buffer
                _undoList.Add(action);
                _redoList.Clear();

                //Raise the changed events
                OnAvailabilityChanged(EventArgs.Empty);
            }
        }

        //Resets the buffers
        public static void Reset()
        {
            //If it was not done due to an action
            if (!_isEditing)
            {
                //Clear buffers and raise the changed events
                _undoList.Clear();
                _redoList.Clear();
                OnAvailabilityChanged(EventArgs.Empty);
            }
        }

        //Undoes the last item added
        public static void Undo()
        {
            //If there are no actions, do nothing
            if (!UndoList!.Any()) return;

            //Enable editing and Undo the last actions
            _isEditing = true;
            _undoList.Last().Undo();
            _isEditing = false;

            //Then move the last action from the end of the undo list
            //To the top of the redo list
            _redoList.Insert(0, _undoList.Last());
            _undoList.RemoveAt(_undoList.Count - 1);

            //Raise the changed availability events
            OnAvailabilityChanged(EventArgs.Empty);
        }

        //Redoes the last item added
        public static void Redo()
        {
            //If there are no actions, do nothing
            if (!RedoList!.Any()) return;

            //Enable editing and Redo the first actions
            _isEditing = true;
            _redoList.First().Redo();
            _isEditing = false;

            //Then move the first action from the top of the undo list
            //To the bottom of the redo list
            _undoList.Add(_redoList.First());
            _redoList.RemoveAt(0);

            //Raise the changed availability events
            OnAvailabilityChanged(EventArgs.Empty);
        }

        static UndoRedoManager()
        {
            //The events do not need to do anything
            IsUndoAvailableChanged += (sender, e) => { return; };
            IsRedoAvailableChanged += (sender, e) => { return; };
        }
    }
}
