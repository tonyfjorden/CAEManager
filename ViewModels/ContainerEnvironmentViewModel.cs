using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAEManager.ViewModels
{
    public class ContainerEnvironmentViewModel
    {
        public ContainerEnvironmentViewModel(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public ContainerEnvironmentViewModel()
        {
        }

        public string Id { get; init; }
        public string Name { get; init; }
    }
}
