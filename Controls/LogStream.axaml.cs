using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using System;

namespace CAEManager.Controls
{
    public partial class LogStream : UserControl
    {
        public LogStream()
        {
            InitializeComponent();
        }

        public static readonly DirectProperty<LogStream, string> LogStreamProperty =
            AvaloniaProperty.RegisterDirect<LogStream, string>(
                nameof(LogStreamUri),
                o => o.LogStreamUri,
                (o, v) => o.LogStreamUri = v);

        private string _LogStreamUri = string.Empty;

        public string LogStreamUri
        {
            get { return _LogStreamUri; }
            set { SetAndRaise(LogStreamProperty, ref _LogStreamUri, value); }
        }
    }
}
