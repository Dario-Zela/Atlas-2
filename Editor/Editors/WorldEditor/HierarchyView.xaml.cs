using System.Linq;
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
            var listView = (ListView)sender;

            var selectedEntities = listView.SelectedItems.Cast<GameEntity>().ToList();
            var previousSelection = selectedEntities.Except(e.AddedItems.Cast<GameEntity>())
                .Concat(e.RemovedItems.Cast<GameEntity>()).ToList();

            UndoRedoManager.Add(new UndoRedoAction(
                "Selection Changed",
                () =>
                {
                    listView.UnselectAll();
                    previousSelection.ForEach(e => (listView.ItemContainerGenerator.ContainerFromItem(e) as ListViewItem)!.IsSelected = true);
                    Inspector.Instance!.DataContext = listView.SelectedItem;
                },
                () =>
                {
                    listView.UnselectAll();
                    selectedEntities.ForEach(e => (listView.ItemContainerGenerator.ContainerFromItem(e) as ListViewItem)!.IsSelected = true);
                    Inspector.Instance!.DataContext = listView.SelectedItem;
                }));


            if (selectedEntities.Count == 1)
            {
                Inspector.Instance!.DataContext = listView.SelectedItems[0];
            }
            else if (selectedEntities.Any())
            {
                Inspector.Instance!.DataContext = new MSGameEntity(selectedEntities);
            }

        }
    }
}
