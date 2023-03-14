using Azure.ResourceManager.AppContainers;
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

        public ContainerAppReplicaModel(ContainerAppReplicaResource resource, ContainerAppRevisionResource revision, ContainerAppResource app, ContainerAppManagedEnvironmentResource environment, long updateIndex)
        {
            _resource = resource;
            _revision = revision;
            _app = app;
            _environment = environment;
            UpdateIndex = updateIndex;
            Id = resource.Id.ToString();
            Name = resource.Data.Name;
            Environment = environment.Data.Name;
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
        public string Environment { get; set; } = string.Empty;
        public long UpdateIndex { get; private set; } = 0;
    }
}
