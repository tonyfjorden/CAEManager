﻿using CAEManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAEManager
{
    internal class DesignData
    {
        public static MainViewModel DesignViewModel { get; } = new MainViewModel
        {
            Replicas = GenerateContainerReplicasData(),
            Environments = GenerateEnvironments()
        };

        private static ObservableCollection<ContainerEnvironmentViewModel> GenerateEnvironments()
        {
            var result = new ObservableCollection<ContainerEnvironmentViewModel>();
            result.Add(new ContainerEnvironmentViewModel
            {
                Id = "1",
                Name = "The Environment"
            });
            result.Add(new ContainerEnvironmentViewModel
            {
                Id = "2",
                Name = "The Environment2"
            });
            return result;
        }

        internal static ObservableCollection<ContainerAppReplicaViewModel> GenerateContainerReplicasData()
        {
            var result = new ObservableCollection<ContainerAppReplicaViewModel>();

            int apps = Random.Shared.Next(5, 10);
            for (int app = 0; app < apps; app++)
            {
                int revisions = Random.Shared.Next(1, 3);

                for (int revision = 0; revision < revisions; revision++)
                {
                    int replicas = Random.Shared.Next(1, 2);
                    for (int replica = 0; replica < replicas; replica++)
                    {
                        result.Add(new ContainerAppReplicaViewModel
                        {
                            Name = $"app_{app}_revision_{revision}",
                            Environment="The Environment",
                            Containers = new[]
                            {
                                new ContainerViewModel
                                {
                                    Id="1",
                                    Name="Container 1",
                                    IsReady = true,
                                    IsStarted = true
                                },
                                new ContainerViewModel
                                {
                                    Id="2",
                                    Name="Container 2",
                                    IsReady = false,
                                    IsStarted = true
                                }
                            },
                            ProvisioningState = "Running"
                        });
                    }
                }
            }

            return result;
        }
    }
}
