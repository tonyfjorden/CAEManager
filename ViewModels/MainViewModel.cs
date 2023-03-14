using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using CAEManager.Services;

namespace CAEManager.ViewModels
{
    public class MainViewModel
    {
        private readonly CAEService _caeService;
        private readonly Timer _periodicTimer;

        public MainViewModel() { }

        public MainViewModel(CAEService caeService)
        {
            _caeService = caeService;

            _periodicTimer = new Timer(async s =>
            {
                foreach(var replica in _caeService.ContainerAppReplicas)
                {

                    Dispatcher.UIThread.Post(() => Replicas.Add(replica));
                }

                _periodicTimer!.Change(30000, Timeout.Infinite);
            },
            null,
            0,
            Timeout.Infinite);
           
        }
        public ObservableCollection<ContainerAppReplicaViewModel> Replicas { get; init; } = new();
    }
}
