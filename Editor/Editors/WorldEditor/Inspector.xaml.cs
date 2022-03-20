using System.Windows.Controls;

namespace Editor.Editors
{
    /// <summary>
    /// Interaction logic for Inspector.xaml
    /// </summary>
    public partial class Inspector : UserControl
    {
        //Static instance of inspector
        public static Inspector? Instance { get; private set; }

        public Inspector()
        {
            Logger.Log(MessageType.Trace, "Inspector Initialised");

            //Initialise it's components, set the instance to this
            //And the data context to null
            InitializeComponent();
            DataContext = null;
            Instance = this;
        }
    }
}
