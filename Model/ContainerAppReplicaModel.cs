using Azure.ResourceManager.AppContainers;
using Azure.ResourceManager.AppContainers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAEManager.Model
{
    public class ContainerAppReplicaModel
    {
        private ContainerAppReplicaResource? _resource;
        private readonly ContainerAppRevisionResource _revision;
        private readonly ContainerAppResource _app;
        private readonly ContainerAppManagedEnvironmentResource _environment;

        public ContainerAppReplicaModel(ContainerAppReplicaResource replica, ContainerAppRevisionResource revision, ContainerAppResource app, ContainerAppManagedEnvironmentResource environment, long updateIndex)
        {
            _resource = replica;
            _revision = revision;
            _app = app;
            _environment = environment;
            UpdateIndex = updateIndex;
            Id = replica.Id.ToString();
            Name = replica.Data.Name;
            Environment = environment.Data.Name;
            ProvisioningState = revision.Data.ProvisioningState;
            ProvisioningError = revision.Data.ProvisioningError;
            Containers = replica.Data.Containers.Select(c => new ContainerModel { Id = c.ContainerId?.ToString() ??  string.Empty, Name = c.Name, IsReady = c.IsReady.GetValueOrDefault(), IsStarted = c.IsStarted.GetValueOrDefault() }).ToArray();
        }

        public ContainerAppReplicaModel()
        {
        }

        internal void Update(ContainerAppReplicaResource replica, long updateIndex)
        {
            _resource = replica;
            UpdateIndex = updateIndex;
        }

        public string Id { get; init; } = string.Empty;

        public string Name { get; init; } = string.Empty;
        public string Environment { get; init; } = string.Empty;
        public ContainerAppRevisionProvisioningState? ProvisioningState { get; init; }
        public string? ProvisioningError { get; init; } = string.Empty;
        public IEnumerable<ContainerModel> Containers { get; init; }

        public long UpdateIndex { get; private set; } = 0;
    }
}
