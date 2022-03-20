using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Editor.Editors
{
    /// <summary>
    /// Interaction logic for WorldEditor.xaml
    /// </summary>
    public partial class WorldEditor : UserControl
    {
        public WorldEditor()
        {
            UndoRedoManager.Reset();
            InitializeComponent();

            Logger.Log(MessageType.Trace, "Test Trace");
            Logger.Log(MessageType.Info, "Test Info");
            Logger.Log(MessageType.Warning, "Test Warning");
            Logger.Log(MessageType.Error, "Test Error");

            Loaded += WorldEditor_Loaded;
        }

        private void WorldEditor_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= WorldEditor_Loaded;
            Focus();

            ((INotifyPropertyChanged?)UndoRedoManager.UndoList)!.PropertyChanged += (_, _) => { Focus(); };
            ((INotifyPropertyChanged?)UndoRedoManager.RedoList)!.PropertyChanged += (_, _) => { Focus(); };

            Undo.Command = new RelayCommand(() => { UndoRedoManager.Undo(); });
            Redo.Command = new RelayCommand(() => { UndoRedoManager.Redo(); });
            Save.Command = new RelayCommand(() => { Project.Save(); });

            EditorControl.InputBindings.Add(new KeyBinding(Undo.Command, new KeyGesture(Key.Z, ModifierKeys.Control)));
            EditorControl.InputBindings.Add(new KeyBinding(Redo.Command, new KeyGesture(Key.Y, ModifierKeys.Control)));
            EditorControl.InputBindings.Add(new KeyBinding(Save.Command, new KeyGesture(Key.S, ModifierKeys.Control)));
        }
    }
}
