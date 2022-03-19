using System.Windows.Controls;

namespace Editor.Editors
{
    /// <summary>
    /// Interaction logic for Inspector.xaml
    /// </summary>
    public partial class Inspector : UserControl
    {
        public static Inspector? Instance { get; private set; }

        public Inspector()
        {
            InitializeComponent();
            DataContext = null;
            Instance = this;
        }
    }
}
