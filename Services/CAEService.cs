using Avalonia.FreeDesktop.DBusIme;
using Azure;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.AppContainers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAEManager.Services
{
    public class CAEService
    {
        ArmClient _client;
        public CAEService() 
        {
            _client = new ArmClient(new AzureCliCredential());
        }

        public async IAsyncEnumerable<ContainerAppResource> GetAppsAsync()
        {
            var subscriptions = _client.GetSubscriptions();

            foreach (var subscription in subscriptions)
            {
                var pagable = subscription.GetContainerAppsAsync();
                await foreach (var app in pagable)
                    yield return app;
            }
        }

        public async IAsyncEnumerable<ContainerAppRevisionResource> GetActiveRevisionsAsync(ContainerAppResource containerAppResource)
        {
            var revisions = containerAppResource.GetContainerAppRevisions();

            var pages = revisions.GetAllAsync("properties/active eq true");

            await foreach (var revision in pages)
                yield return revision;
        }
    }
}