using Azure.ResourceManager.AppContainers;
using CAEManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAEManager.ViewModels
{
    public class ContainerAppReplicaViewModel
    {
        private ContainerAppReplicaModel _replica;

        public ContainerAppReplicaViewModel()
        {
        }

        public ContainerAppReplicaViewModel(ContainerAppReplicaModel replica)
        {
            Id = replica.Id;
            Name = replica.Name;
            Environment = replica.Environment;
            Containers = replica.Containers.Select(c=>new ContainerViewModel { Id = c.Id, Name = c.Name, IsReady = c.IsReady, IsStarted = c.IsStarted}).ToArray();
            ProvisioningState = replica.ProvisioningState.ToString();
            ProvisioningError = replica.ProvisioningError;
            _replica = replica;
        }

        public string Id { get; init; }

        public string Name { get; set; }

        public string Environment { get; init; }

        public string ProvisioningState { get; set; }
        public string ProvisioningError { get; set; }

        public IEnumerable<ContainerViewModel> Containers { get; set; }

        internal void Update(ContainerAppReplicaModel replica)
        {
            _replica = replica;
            Name = replica.Name;
            Containers = replica.Containers.Select(c => new ContainerViewModel { Id = c.Id, Name = c.Name, IsReady = c.IsReady, IsStarted = c.IsStarted }).ToArray();
            ProvisioningState = replica.ProvisioningState?.ToString();
            ProvisioningError = replica.ProvisioningError;
        }
    }
}
