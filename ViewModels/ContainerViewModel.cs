using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAEManager.ViewModels
{
    public class ContainerViewModel
    {
        public ContainerViewModel()
        {

        }

        public string Id { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;

        public bool IsReady { get; init; }
        public IBrush IsReadyColor => IsReady ? Brushes.Green : Brushes.Orange;
        public bool IsStarted { get; init; }
    }
}
