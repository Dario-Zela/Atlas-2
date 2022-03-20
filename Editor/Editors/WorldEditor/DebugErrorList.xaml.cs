using System;
using System.Collections.Generic;
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
