using System.Runtime.Serialization;

namespace CAEManager.ViewModels
{
    public class ContainerRevisionViewModel
    {
        public ContainerEnvironmentViewModel? Environment { get; set; }
        public ContainerAppViewModel? App { get; set; }
        public string? RevisionName { get; init; }
        public bool IsActive { get; init; }
        public int Replicas { get; init; }

        public string? State { get; set; }

        public int TrafficWeight { get; init; }
    }
}