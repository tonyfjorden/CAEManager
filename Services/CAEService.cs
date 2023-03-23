using Avalonia.Controls.Mixins;
using Avalonia.Threading;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.AppContainers;
using Azure.ResourceManager.Resources;
using CAEManager.Model;
using CAEManager.ViewModels;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Websocket.Client;

namespace CAEManager.Services
{
    public class CAEService
    {
        private readonly ArmClient _client;

        public event EventHandler<EventArgs<ContainerAppReplicaModel>> OnReplicaAdded;
        public event EventHandler<EventArgs<ContainerAppReplicaModel>> OnReplicaUpdated;
        public event EventHandler<EventArgs<ContainerAppReplicaModel>> OnReplicaRemoved;
        public event EventHandler<EventArgs<EnvironmentModel>> OnEnvironmentAdded;


        private readonly Dictionary<string, ContainerAppReplicaModel> _containerAppReplicas = new();

        private readonly Timer _periodicTimer;

        private volatile ContainerAppReplicaModel[] _replicas = Array.Empty<ContainerAppReplicaModel>();
        private readonly List<SubscriptionResource> _subscriptions = new List<SubscriptionResource>();
        private readonly List<EnvironmentModel> _environments = new List<EnvironmentModel>();

        private long _UpdateIndex = 0;

        public CAEService()
        {
            _client = new ArmClient(new DefaultAzureCredential());
            

            _periodicTimer = new Timer(async s =>
            {
                Update(++_UpdateIndex);

                _periodicTimer!.Change(TimeSpan.FromSeconds(10), Timeout.InfiniteTimeSpan);
            },
            null,
            0,
            Timeout.Infinite);
        }

        public IEnumerable<ContainerAppReplicaModel> Replicas => _replicas;

        private void Update(long updateIndex)
        {
            if (_subscriptions.Count == 0)
            {
                foreach (var subscription in _client.GetSubscriptions().AsParallel())
                    _subscriptions.Add(subscription);
            }

            if (_environments.Count == 0)
            {
                foreach (var environment in _subscriptions.AsParallel().SelectMany(s => s.GetContainerAppManagedEnvironments()).Select(e => new EnvironmentModel(e)))
                {
                    _environments.Add(environment);
                    OnEnvironmentAdded?.Invoke(this, new EventArgs<EnvironmentModel>(environment));
                }
            }

            var replicas = from app in _subscriptions.AsParallel().SelectMany(s => s.GetContainerApps()).AsParallel()
                           join environment in _environments.AsParallel() on app.Data.ManagedEnvironmentId equals environment.Id
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
                    containerAppReplica = new ContainerAppReplicaModel(replica.replica, replica.revision, replica.app, replica.environment, updateIndex);
                    _containerAppReplicas[id] = containerAppReplica;
                    OnReplicaAdded?.Invoke(this, new EventArgs<ContainerAppReplicaModel>(containerAppReplica));
                }
                else
                {
                    containerAppReplica.Update(replica.replica, replica.revision, replica.app, replica.environment, updateIndex);
                    OnReplicaUpdated?.Invoke(this, new EventArgs<ContainerAppReplicaModel>(containerAppReplica));
                }
            }

            if (existing.Any())
            {
                var toRemove = _containerAppReplicas.Where(r => !existing.Contains(r.Key)).Select(r => r.Key).ToArray();

                foreach (var id in toRemove)
                {
                    if (_containerAppReplicas.TryGetValue(id, out var removedReplica))
                    {
                        _containerAppReplicas.Remove(id);
                        OnReplicaRemoved?.Invoke(this, new EventArgs<ContainerAppReplicaModel>(removedReplica));
                    }
                }
            }

            _replicas = _containerAppReplicas.Values.ToArray();
        }
    }
}
