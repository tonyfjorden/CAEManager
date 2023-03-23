using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Threading;
using CAEManager.Services;

namespace CAEManager.ViewModels
{
    public class MainViewModel
    {
        private readonly CAEService _caeService;

        public MainViewModel() { }

        public MainViewModel(CAEService caeService)
        {
            _caeService = caeService;

            if (!Design.IsDesignMode)
            {
                _caeService.OnReplicaAdded += (s, a) =>
                {
                    Dispatcher.UIThread.Post(() =>
                    {
                        var replicaVM = Replicas.FirstOrDefault(r => r.Id == a.Value.Id);

                        if (replicaVM == null)
                        {
                            Replicas.Add(new ContainerAppReplicaViewModel(a.Value));
                        }
                    });
                };

                _caeService.OnReplicaRemoved += (s, a) =>
                {
                    Dispatcher.UIThread.Post(() =>
                    {
                        var replicaVM = Replicas.FirstOrDefault(r => r.Id == a.Value.Id);

                        if (replicaVM != null)
                        {
                            Replicas.Remove(replicaVM);
                        }
                    });
                };

                _caeService.OnReplicaUpdated += (s, a) =>
                {
                    Dispatcher.UIThread.Post(() =>
                    {
                        var replicaVM = Replicas.FirstOrDefault(r => r.Id == a.Value.Id);

                        replicaVM?.Update(a.Value);
                    });
                };

                _caeService.OnEnvironmentAdded += (s, a) =>
                {
                    Dispatcher.UIThread.Post(() =>
                    {
                        var environment = Environments.FirstOrDefault(e => e.Id == a.Value.Id);

                        if (environment == null)
                        {
                            Environments.Add(new ContainerEnvironmentViewModel(a.Value.Id, a.Value.Name));
                        }
                    });
                };

            }
        }
        public ObservableCollection<ContainerAppReplicaViewModel> Replicas { get; init; } = new();
        public ObservableCollection<ContainerEnvironmentViewModel> Environments { get; init; } = new();
    }
}
