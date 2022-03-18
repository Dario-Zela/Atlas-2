using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accessibility;

namespace Editor
{
    public class UndoRedoAction
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
    }

    public static class UndoRedoManager
    {
        private static readonly ObservableCollection<UndoRedoAction> _undoList = new();
        private static readonly ObservableCollection<UndoRedoAction> _redoList = new();

        public static ReadOnlyObservableCollection<UndoRedoAction>? UndoList = null;
        public static ReadOnlyObservableCollection<UndoRedoAction>? RedoList = null;

        public static bool IsUndoAvailable => _undoList.Any();
        public static bool IsRedoAvailable => _redoList.Any();

        public static event EventHandler IsUndoAvailableChanged;
        public static event EventHandler IsRedoAvailableChanged;

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

        public static void Add(UndoRedoAction action)
        {
            _undoList.Add(action);
            _redoList.Clear();
            OnAvailabilityChanged(EventArgs.Empty);
        }

        public static void Reset()
        {
            _undoList.Clear();
            _redoList.Clear();
            OnAvailabilityChanged(EventArgs.Empty);
        }

        public static void Undo()
        {
            if (!UndoList!.Any()) return;

            _undoList.First().Undo();
            _redoList.Insert(0, _undoList.First());
            _undoList.RemoveAt(0);

            OnAvailabilityChanged(EventArgs.Empty);
        }

        public static void Redo()
        {
            if (!RedoList!.Any()) return;

            _redoList.First().Redo();
            _undoList.Insert(0, _redoList.First());
            _redoList.RemoveAt(0);

            OnAvailabilityChanged(EventArgs.Empty);
        }

        public static void Initialise()
        {
            UndoList = new(_undoList);
            RedoList = new(_redoList);
            IsUndoAvailableChanged += (sender, e) => { return; };
            IsRedoAvailableChanged += (sender, e) => { return; };
        }
    }
}
