using System.ComponentModel;
using System.Runtime.Serialization;

namespace Editor
{
    [DataContract(IsReference = true)]
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnProprietyChanged(string proprietyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(proprietyName));
        }
    }
}
