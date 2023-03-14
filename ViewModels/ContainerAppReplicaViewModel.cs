using Azure.ResourceManager.AppContainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAEManager.ViewModels
{
    public class ContainerAppReplicaViewModel
    {
        private ContainerAppReplicaResource _resource;

        public ContainerAppReplicaViewModel()
        {
        }

        public ContainerAppReplicaViewModel(ContainerAppReplicaResource resource)
        {
            _resource = resource;
            Id = resource.Id.ToString();
            Name = resource.Data.Name;
        }

        public string Id { get; init; }

        public string Name { get; init; }

        internal void Update(ContainerAppReplicaResource replica)
        {
            _resource = replica;
        }
    }
}
