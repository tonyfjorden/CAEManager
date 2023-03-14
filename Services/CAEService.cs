using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.AppContainers;
using Azure.ResourceManager.Resources;
using CAEManager.ViewModels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CAEManager.Services
{
    public class CAEService
    {
        private readonly ArmClient _client;

        private readonly Dictionary<string, ContainerAppReplicaViewModel> _containerAppReplicas = new();

        private readonly Timer _periodicTimer;

        private volatile ContainerAppReplicaViewModel[] _car = Array.Empty<ContainerAppReplicaViewModel>();
        private List<SubscriptionResource> _subscriptions;

        public CAEService()
        {
            _client = new ArmClient(new DefaultAzureCredential());
            _subscriptions = _client.GetSubscriptions().ToList();

            _periodicTimer = new Timer(async s =>
            {
                await Update();

                _periodicTimer!.Change(60, Timeout.Infinite);
            },
            null,
            0,
            Timeout.Infinite);
        }

        public IEnumerable<ContainerAppReplicaViewModel> ContainerAppReplicas => _car;

        private async Task Update()
        {

            var apps = _subscriptions.SelectMany(s=>s.GetContainerApps());
            var revisions = apps.AsParallel().SelectMany(a => a.GetContainerAppRevisions().Where(r => r.HasData && r.Data.IsActive.GetValueOrDefault()));
            var replicas = revisions.SelectMany(r => r.GetContainerAppReplicas().Where(rep=>rep.HasData));

            var existing = new HashSet<string>();
            foreach(var replica in replicas)
            {
                var id = replica.Id.ToString();
                existing.Add(id);

                if (!_containerAppReplicas.TryGetValue(id, out var containerAppReplica))
                {
                    containerAppReplica = new ContainerAppReplicaViewModel(replica);
                    _containerAppReplicas[id] = containerAppReplica;
                }
                else
                    containerAppReplica.Update(replica);
            }

            if (existing.Any())
            {
                var toRemove = _containerAppReplicas.Where(r => !existing.Contains(r.Key)).Select(r => r.Key).ToArray();

                foreach (var id in toRemove)
                {
                    _containerAppReplicas.Remove(id);
                }
            }

            _car = _containerAppReplicas.Values.ToArray();
        }
    }
}
