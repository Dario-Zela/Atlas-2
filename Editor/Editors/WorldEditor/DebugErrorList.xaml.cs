using System.Windows;
using System.Windows.Controls;

namespace Editor.Editors
{
    /// <summary>
    /// Interaction logic for DebugErrorList.xaml
    /// </summary>
    public partial class DebugErrorList : UserControl
    {
        public DebugErrorList()
        {
            InitializeComponent();
            Loaded += DebugErrorList_Loaded;
        }

        private void DebugErrorList_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= DebugErrorList_Loaded;

            listBox1.ItemsSource = UndoRedoManager.UndoList;
            listBox2.ItemsSource = UndoRedoManager.RedoList;
        }
    }
}
