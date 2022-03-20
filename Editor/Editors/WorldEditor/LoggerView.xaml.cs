using System.Windows;
using System.Windows.Controls;

namespace Editor.Editors
{
    /// <summary>
    /// Interaction logic for LoggerView.xaml
    /// </summary>
    public partial class LoggerView : UserControl
    {
        public LoggerView()
        {
            InitializeComponent();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Logger.Clear();
        }

        private void TraceToggleButton_Click(object sender, RoutedEventArgs e)
        {
            Logger.ToggleMessageFilter((uint)MessageType.Trace);
        }

        private void InfoToggleButton_Click(object sender, RoutedEventArgs e)
        {
            Logger.ToggleMessageFilter((uint)MessageType.Info);
        }

        private void WarningToggleButton_Click(object sender, RoutedEventArgs e)
        {
            Logger.ToggleMessageFilter((uint)MessageType.Warning);
        }

        private void ErrorToggleButton_Click(object sender, RoutedEventArgs e)
        {
            Logger.ToggleMessageFilter((uint)MessageType.Error);
        }
    }
}
