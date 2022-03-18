using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            Loaded += WorldEditor_Loaded;
        }

        private void WorldEditor_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= WorldEditor_Loaded;
            Focus();

            ((INotifyPropertyChanged)UndoRedoManager.UndoList).PropertyChanged += (_, _) => { Focus(); };
            ((INotifyPropertyChanged)UndoRedoManager.RedoList).PropertyChanged += (_, _) => { Focus(); };
        }

        private void Undo_ButtonClick(object sender, RoutedEventArgs e)
        {
            UndoRedoManager.Undo();
        }

        private void Redo_ButtonClick(object sender, RoutedEventArgs e)
        {
            UndoRedoManager.Redo();
        }

        private void Save_ButtonClick(object sender, RoutedEventArgs e)
        {
            Project.Save();
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Z && Keyboard.Modifiers == ModifierKeys.Control)
                UndoRedoManager.Undo();
            else if (e.Key == Key.Y && Keyboard.Modifiers == ModifierKeys.Control)
                UndoRedoManager.Redo();
            else if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
                Project.Save();
        }
    }
}
