using Azure.ResourceManager.AppContainers;
using Azure.ResourceManager.Resources;

namespace CAEManager.Model
{
    public class EnvironmentModel
    {
        private readonly ContainerAppManagedEnvironmentResource _resource;

        public EnvironmentModel(ContainerAppManagedEnvironmentResource resource)
        {
            _resource = resource;
        }

        public string Name => _resource.Data.Name;
        public string Id => _resource.Id.ToString();
    }
}