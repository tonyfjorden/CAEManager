using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAEManager.Model
{
    public class ContainerModel
    {
        public ContainerModel()
        {

        }
        public string Id { get; init; }
        public string Name { get; init; }

        public bool IsReady { get; init; }
        public bool IsStarted { get; init; }
    }
}
