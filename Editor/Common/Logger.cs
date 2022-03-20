using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Printing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Editor
{
    enum MessageType : uint
    {
        Trace = 0x01,
        Info = 0x02,
        Warning = 0x04,
        Error = 0x08
    }

    class LogMessage
    {
        public DateTime Time { get; }
        public MessageType Type { get; }
        public string Message { get; }
        public string File { get; }
        public string Caller { get; }
        public int Line { get; }

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

    static class Logger
    {
        private static uint _filter = (uint)(MessageType.Info | MessageType.Warning | MessageType.Error);

        private static readonly ObservableCollection<LogMessage> _messages = new();
        public static ReadOnlyObservableCollection<LogMessage> Messages { get; } = new(_messages);

        public static CollectionViewSource FilteredMessages { get; } = new() { Source = Messages };

        public static async void Log(MessageType type, string message,
            [CallerFilePath]string file = "", [CallerMemberName]string caller = "", [CallerLineNumber]int line = 0)
        {
            await Application.Current.Dispatcher.BeginInvoke(() =>
            {
                _messages.Add(new LogMessage(type, message, file, caller, line));
            });
        }

        public static async void Clear()
        {
            await Application.Current.Dispatcher.BeginInvoke(() =>
            {
                _messages.Clear();
            });
        }

        public static void ToggleMessageFilter(uint mask)
        {
            _filter ^= mask;
            FilteredMessages.View.Refresh();
        }

        static Logger()
        {
            FilteredMessages.Filter += (sender, e) =>
            {
                uint type = (uint)(e.Item as LogMessage)!.Type;
                e.Accepted = (type & _filter) != 0;
            };
        }
    }
}
