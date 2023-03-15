using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
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

            if (!Design.IsDesignMode)
            {
                _periodicTimer = new Timer(async s =>
                {
                    foreach (var replica in _caeService.Replicas)
                    {

                        Dispatcher.UIThread.Post(() =>
                        {
                            var replicaVM = Replicas.FirstOrDefault(r => r.Id == replica.Id);

                            if (replicaVM == null)
                            {
                                Replicas.Add(new ContainerAppReplicaViewModel(replica));
                            }
                            else
                            {
                                replicaVM.Update(replica);
                            }

                        });
                    }

                    _periodicTimer!.Change(1000, Timeout.Infinite);
                },
                null,
                0,
                Timeout.Infinite);
            }
        }
        public ObservableCollection<ContainerAppReplicaViewModel> Replicas { get; init; } = new();
    }
}
