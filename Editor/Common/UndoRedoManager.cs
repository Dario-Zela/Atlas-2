using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Editor
{
    public interface IUndoRedo
    {
        public string Name { get; }

        public abstract void Redo();

        public abstract void Undo();
    }

    public class UndoRedoAction : IUndoRedo
    {
        private Action _undo;
        private Action _redo;

        public string Name { get; private set; }

        public void Redo()
        {
            _redo();
        }

        public void Undo()
        {
            _undo();
        }

        public UndoRedoAction(string name, Action undo, Action redo)
        {
            Name = name;
            _undo = undo;
            _redo = redo;
        }

        public UndoRedoAction(string name, string proprietyChanged, object propriety,
                              object oldValue, object newValue)
        : this(name,
              () => propriety.GetType().GetProperty(proprietyChanged)!.SetValue(propriety, oldValue),
              () => propriety.GetType().GetProperty(proprietyChanged)!.SetValue(propriety, newValue)
             )
        { }
    }

    public class UndoRedoActionGroup : IUndoRedo
    {
        public string Name { get; private set; }
        private List<UndoRedoAction> _actions = new();
        private Action? _postUndoRedoAction;

        public void Redo()
        {
            foreach (var action in _actions)
            {
                action.Redo();
            }
            _postUndoRedoAction?.Invoke();
        }

        public void Undo()
        {
            foreach (var action in _actions)
            {
                action.Undo();
            }
            _postUndoRedoAction?.Invoke();
        }

        public UndoRedoActionGroup(string name, string proprietyChanged, IEnumerable<object> proprieties,
                      IEnumerable<object> oldValues, object newValue, Action? postUndoRedoAction = null)
        {
            Name = name;
            _postUndoRedoAction = postUndoRedoAction;

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
        private static bool _isEditing = false;

        private static readonly ObservableCollection<IUndoRedo> _undoList = new();
        private static readonly ObservableCollection<IUndoRedo> _redoList = new();

        public static ReadOnlyObservableCollection<IUndoRedo>? UndoList { get; } = new(_undoList);
        public static ReadOnlyObservableCollection<IUndoRedo>? RedoList { get; } = new(_redoList);

        public static bool IsUndoAvailable => _undoList.Any();
        public static bool IsRedoAvailable => _redoList.Any();

        public static event EventHandler? IsUndoAvailableChanged;
        public static event EventHandler? IsRedoAvailableChanged;

        private static void OnAvailabilityChanged(EventArgs e)
        {
            EventHandler? handler = IsUndoAvailableChanged;

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

        public static void Add(IUndoRedo action)
        {
            if (!_isEditing)
            {
                Logger.Log(MessageType.Trace, action.Name);
                _undoList.Add(action);
                _redoList.Clear();
                OnAvailabilityChanged(EventArgs.Empty);
            }
        }

        public static void Reset()
        {
            if (!_isEditing)
            {
                _undoList.Clear();
                _redoList.Clear();
                OnAvailabilityChanged(EventArgs.Empty);
            }
        }

        public static void Undo()
        {
            if (!UndoList!.Any()) return;

            _isEditing = true;
            _undoList.Last().Undo();
            _isEditing = false;

            _redoList.Insert(0, _undoList.Last());
            _undoList.RemoveAt(_undoList.Count - 1);

            OnAvailabilityChanged(EventArgs.Empty);
        }

        public static void Redo()
        {
            if (!RedoList!.Any()) return;

            _isEditing = true;
            _redoList.First().Redo();
            _isEditing = false;

            _undoList.Add(_redoList.First());
            _redoList.RemoveAt(0);

            OnAvailabilityChanged(EventArgs.Empty);
        }

        static UndoRedoManager()
        {
            IsUndoAvailableChanged += (sender, e) => { return; };
            IsRedoAvailableChanged += (sender, e) => { return; };
        }
    }
}
