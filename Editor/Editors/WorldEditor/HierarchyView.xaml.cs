using System.Windows.Controls;

namespace Editor.Editors
{
    /// <summary>
    /// Interaction logic for SceneBar.xaml
    /// </summary>
    public partial class HierarchyView : UserControl
    {
        public HierarchyView()
        {
            InitializeComponent();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var entity = ((ListView)sender).SelectedItem;
            Inspector.Instance!.DataContext = entity;
        }
    }
}
