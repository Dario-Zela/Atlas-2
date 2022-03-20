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
            Logger.Log(MessageType.Trace, "Initialising Hierarchy View Component");

            //Initialise components
            InitializeComponent();
        }

        //Method called whenever listView changes selection
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Logger.Log(MessageType.Trace, "Changing List Selection");

            //Get the list view
            var listView = (ListView)sender;

            //Get the selected entities and the previously selected entities
            var selectedEntities = listView.SelectedItems.Cast<GameEntity>().ToList();
            var previousSelection = selectedEntities.Except(e.AddedItems.Cast<GameEntity>())
                .Concat(e.RemovedItems.Cast<GameEntity>()).ToList();

            //Add an undoRedoAction where undo selects all of the previously selected entities
            //And redo adds them back
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

            //If only one entity is selected, send a regular GameEntity
            if (selectedEntities.Count == 1)
            {
                Logger.Log(MessageType.Info, "Single Item Selected");

                Inspector.Instance!.DataContext = listView.SelectedItems[0];
            }
            //Else, if there are any selected send a multi select GameEntity
            else if (selectedEntities.Any())
            {
                Logger.Log(MessageType.Info, "Multiple Items Selected");

                Inspector.Instance!.DataContext = new MSGameEntity(selectedEntities);
            }
        }
    }
}
