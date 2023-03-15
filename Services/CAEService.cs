using Avalonia.Controls.Mixins;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.AppContainers;
using Azure.ResourceManager.Resources;
using CAEManager.Model;
using CAEManager.ViewModels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace CAEManager.Services
{
    public class CAEService
    {
        private readonly ArmClient _client;

        private readonly Dictionary<string, ContainerAppReplicaModel> _containerAppReplicas = new();

        private readonly Timer _periodicTimer;

        private volatile ContainerAppReplicaModel[] _replicas = Array.Empty<ContainerAppReplicaModel>();
        private readonly List<SubscriptionResource> _subscriptions;

        private long _UpdateIndex = 0;

        public CAEService()
        {
            _client = new ArmClient(new DefaultAzureCredential());
            _subscriptions = _client.GetSubscriptions().ToList();

            _periodicTimer = new Timer(async s =>
            {
                Update(++_UpdateIndex);

                _periodicTimer!.Change(60, Timeout.Infinite);
            },
            null,
            0,
            Timeout.Infinite);
        }

        public IEnumerable<ContainerAppReplicaModel> Replicas => _replicas;

        private void Update(long updateIndex)
        {
            var managedEnvironments = _subscriptions.SelectMany(s => s.GetContainerAppManagedEnvironments()).AsParallel();

            var replicas = from app in _subscriptions.AsParallel().SelectMany(s => s.GetContainerApps()).AsParallel()
                           join environment in managedEnvironments on app.Data.ManagedEnvironmentId equals environment.Id
                           let currentEnvironment = environment
                           let currentApp = app
                           from revision in app.GetContainerAppRevisions().Where(r => r.HasData && r.Data.IsActive.GetValueOrDefault())
                           let currentRevision = revision
                           from replica in revision.GetContainerAppReplicas().Where(rep => rep.HasData)
                           select new { replica, revision = currentRevision, app = currentApp, environment = currentEnvironment };


            //var apps = _subscriptions.SelectMany(s=>s.GetContainerApps()));
            //var revisions = apps.AsParallel().SelectMany(a => a.GetContainerAppRevisions().Where(r => r.HasData && r.Data.IsActive.GetValueOrDefault()));
            //var replicas = revisions.SelectMany(r => r.GetContainerAppReplicas().Where(rep=>rep.HasData));

            var existing = new HashSet<string>();
            foreach (var replica in replicas)
            {
                var id = replica.replica.Id.ToString();
                existing.Add(id);

                if (!_containerAppReplicas.TryGetValue(id, out var containerAppReplica))
                {
                    containerAppReplica = new ContainerAppReplicaModel(replica.replica, replica.revision, replica.app, replica.environment, _UpdateIndex);
                    _containerAppReplicas[id] = containerAppReplica;
                }
                else
                    containerAppReplica.Update(replica.replica, _UpdateIndex);
            }

            if (existing.Any())
            {
                var toRemove = _containerAppReplicas.Where(r => !existing.Contains(r.Key)).Select(r => r.Key).ToArray();

                foreach (var id in toRemove)
                {
                    _containerAppReplicas.Remove(id);
                }
            }

            _replicas = _containerAppReplicas.Values.ToArray();
        }
    }
}
