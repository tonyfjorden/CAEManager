using CAEManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAEManager
{
    internal class DesignData
    {
        public static MainViewModel DesignViewModel { get; } = new MainViewModel
        {
            Revisions = GenerateContainerRevisionData(),
        };


        internal static IEnumerable<ContainerRevisionViewModel> GenerateContainerRevisionData()
        {
            int apps = Random.Shared.Next(5, 10);
            for (int app = 0; app < apps; app++)
            {
                int revisions = Random.Shared.Next(1, 3);

                for (int revision = 0; revision < revisions; revision++)
                {
                    yield return new ContainerRevisionViewModel
                    {
                        Environment = new ContainerEnvironmentViewModel { Name = "My Environment" },
                        App = new ContainerAppViewModel { Name = $"Container App {app}" },
                        RevisionName = $"Container App {app}-revision {revision}",
                        IsActive = revision == 0,
                        Replicas = Random.Shared.Next(10),
                        State = "Provisioned",
                        TrafficWeight = (revision == 0) ? 100 : 0
                    };
                }
            };
        }
    }
}
