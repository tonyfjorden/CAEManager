using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAEManager.Model
{
    public class ContainerModel
    {
        public ContainerModel(Azure.ResourceManager.AppContainers.Models.ContainerAppReplicaContainer container)
        {
            Id = container.ContainerId;
            Name = container.Name;
            IsReady = container.IsReady;
            IsStarted = container.IsStarted;
            RestartCount = container.RestartCount;
            ExecStreamUri = new Uri(container.ExecEndpoint);
            LogStreamUri = new Uri(container.LogStreamEndpoint);
        }
        public string Id { get; init; }
        public string Name { get; init; }
        public Uri LogStreamUri { get; init; }
        public Uri ExecStreamUri { get; init; }

        public bool? IsReady { get; init; }
        public bool? IsStarted { get; init; }
        public int? RestartCount { get; init; }
    }
}
