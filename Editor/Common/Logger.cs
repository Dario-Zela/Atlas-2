using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

namespace Editor
{
    //Enum declaring the message type in a 
    //Bitways ORable fashilon 
    public enum MessageType : uint
    {
        Trace = 0x01,
        Info = 0x02,
        Warning = 0x04,
        Error = 0x08
    }

    //Defines the content of a log message
    public class LogMessage
    {
        public DateTime Time { get; }
        public MessageType Type { get; }
        public string Message { get; }
        public string File { get; }
        public string Caller { get; }
        public int Line { get; }

        //Stucture of the metadata of the logMessage
        public string Metadata => $@"{File}: {Caller} ({Line})";

        public LogMessage(MessageType type, string message, string file, string caller, int line)
        {
            Time = DateTime.Now;
            Type = type;
            Message = message;
            File = Path.GetFileName(file);
            Caller = caller;
            Line = line;
        }
    }

    public static class Logger
    {
        private const int MAX_MESSAGES = 50;

        //The filter used, it is initialised with all types on except for trace
        private static uint _filter = (uint)(MessageType.Info | MessageType.Warning | MessageType.Error);

        //The store for the logged messages
        private static readonly ObservableCollection<LogMessage> _messages = new();
        public static ReadOnlyObservableCollection<LogMessage> Messages { get; } = new(_messages);

        //A viewsouce on messages to filter the ones displayed
        public static CollectionViewSource FilteredMessages { get; } = new() { Source = Messages };

        //Adds a message to the collection in the UI thread
        public static async void Log(MessageType type, string message,
            [CallerFilePath] string file = "", [CallerMemberName] string caller = "", [CallerLineNumber] int line = 0)
        {
            //The BeginInvoke method is done to allow the program to contine while logging
            await Application.Current.Dispatcher.BeginInvoke(() =>
            {
                _messages.Add(new LogMessage(type, message, file, caller, line));

                //Make sure the collection contains only the last
                //MAX_MESSAGES messages
                if (_messages.Count > MAX_MESSAGES) _messages.RemoveAt(0);
            });
        }

        //Clears the collection in the UI thread
        public static async void Clear()
        {
            //The BeginInvoke method is done to allow the program to contine while clearing
            await Application.Current.Dispatcher.BeginInvoke(() =>
            {
                _messages.Clear();
            });
        }

        //Toggles a filter on or off
        public static void ToggleMessageFilter(uint mask)
        {
            _filter ^= mask;

            //The filtered messages are refreshed to 
            //Display changes
            FilteredMessages.View.Refresh();
        }

        static Logger()
        {
            //Sets up the filter
            FilteredMessages.Filter += (sender, e) =>
            {
                //If the type is selected, allow it's display
                uint type = (uint)(e.Item as LogMessage)!.Type;
                e.Accepted = (type & _filter) != 0;
            };
        }
    }
}
