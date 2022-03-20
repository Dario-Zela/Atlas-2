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
            Logger.Log(MessageType.Trace, "World Editor Initialised");

            //Reset the UndoRedoManager, and initialise the components
            UndoRedoManager.Reset();
            InitializeComponent();

            //Add a loading routine to the editor
            Loaded += WorldEditor_Loaded;
        }

        //When the world editor is loaded
        private void WorldEditor_Loaded(object sender, RoutedEventArgs e)
        {
            Logger.Log(MessageType.Trace, "Loading Routing Executed");

            //Remove method from loading routine
            Loaded -= WorldEditor_Loaded;

            //Focus the window and set it such that when a change in property occurs, the window gets focus
            Focus();
            ((INotifyPropertyChanged?)UndoRedoManager.UndoList)!.PropertyChanged += (_, _) => { Focus(); };
            ((INotifyPropertyChanged?)UndoRedoManager.RedoList)!.PropertyChanged += (_, _) => { Focus(); };

            //Generate the undo, redo and save commands
            Undo.Command = new RelayCommand(() => { UndoRedoManager.Undo(); });
            Redo.Command = new RelayCommand(() => { UndoRedoManager.Redo(); });
            Save.Command = new RelayCommand(() => { Project.Save(); });

            //Add the input bindings to the screen
            EditorControl.InputBindings.Add(new KeyBinding(Undo.Command, new KeyGesture(Key.Z, ModifierKeys.Control)));
            EditorControl.InputBindings.Add(new KeyBinding(Redo.Command, new KeyGesture(Key.Y, ModifierKeys.Control)));
            EditorControl.InputBindings.Add(new KeyBinding(Save.Command, new KeyGesture(Key.S, ModifierKeys.Control)));
        }
    }
}
