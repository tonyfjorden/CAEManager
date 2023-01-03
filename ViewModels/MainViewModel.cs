using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAEManager.Services;

namespace CAEManager.ViewModels
{
    public class MainViewModel
    {
        private readonly CAEService _caeService;

        public MainViewModel() { }

        public MainViewModel(CAEService caeService)
        {
            _caeService = caeService;


        }
        public IEnumerable<ContainerRevisionViewModel> Revisions { get; init; } = Enumerable.Empty<ContainerRevisionViewModel>();
    }
}
