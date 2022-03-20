using System.ComponentModel;
using System.Runtime.Serialization;

namespace Editor
{
    //An implementation of the INotifyProprietyChanged interface
    [DataContract(IsReference = true)]
    public class ViewModelBase : INotifyPropertyChanged
    {
        //The property changed
        public event PropertyChangedEventHandler? PropertyChanged;

        //Method called to signify a property changing
        protected void OnProprietyChanged(string proprietyName)
        {
            //Invoke the Property Changed event
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(proprietyName));
        }
    }
}
