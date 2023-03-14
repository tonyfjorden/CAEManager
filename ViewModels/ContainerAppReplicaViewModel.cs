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
            _replica = replica;
        }

        public string Id { get; private set; }

        public string Name { get; set; }

        public string Environment { get; set; }

        internal void Update(ContainerAppReplicaModel replica)
        {
            _replica = replica;
            Name = replica.Name;
        }
    }
}
