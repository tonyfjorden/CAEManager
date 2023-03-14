using Azure.ResourceManager.AppContainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAEManager.ViewModels
{
    public class ContainerAppViewModel
    {
        private ContainerAppResource _app;

        public ContainerAppViewModel()
        {
        }

        public ContainerAppViewModel(ContainerAppResource app)
        {
            _app = app;
        }

        public string? Name { get; init; }
    }
}
