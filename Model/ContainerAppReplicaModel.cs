using Azure.ResourceManager.AppContainers;
using Azure.ResourceManager.AppContainers.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CAEManager.Model
{
    public class ContainerAppReplicaModel
    {
        private ContainerAppReplicaResource? _resource;
        private readonly ContainerAppRevisionResource _revision;
        private readonly ContainerAppResource _app;
        private readonly EnvironmentModel _environment;

        public ContainerAppReplicaModel(ContainerAppReplicaResource replica, ContainerAppRevisionResource revision, ContainerAppResource app, EnvironmentModel environment, long updateIndex)
        {
            _resource = replica;
            _revision = revision;
            _app = app;
            _environment = environment;
            UpdateIndex = updateIndex;
            Id = replica.Id.ToString();
            Name = replica.Data.Name;
            Environment = environment.Name;
            ProvisioningState = revision.Data.ProvisioningState;
            ProvisioningError = revision.Data.ProvisioningError;
            Containers = replica.Data.Containers.Select(c => new ContainerModel(c)).ToArray();
        }

        public ContainerAppReplicaModel()
        {
        }

        internal bool Update(ContainerAppReplicaResource replica, ContainerAppRevisionResource revision, ContainerAppResource app, EnvironmentModel environment, long updateIndex)
        {
            _resource = replica;
            UpdateIndex = updateIndex;
            bool changed = false;

            this.UpdateWithChangeCheck(ref changed, revision.Data.ProvisioningState, r => r.ProvisioningState);
            this.UpdateWithChangeCheck(ref changed, revision.Data.ProvisioningError, r => r.ProvisioningError);

            var replicaContainers = replica.Data.Containers.Select(c => new ContainerModel(c)).ToArray();

            foreach (var rc in replicaContainers)
            {
                var container = Containers.FirstOrDefault(c => c.Id == rc.Id);
                if (container != null)
                {
                    container.UpdateWithChangeCheck(ref changed, rc.IsStarted, c => c.IsStarted);
                    container.UpdateWithChangeCheck(ref changed, rc.IsReady, c => c.IsReady);
                }
                
            }
            
            return changed;
        }

        

        public string Id { get; init; } = string.Empty;

        public string Name { get; init; } = string.Empty;
        public string Environment { get; init; } = string.Empty;
        public ContainerAppRevisionProvisioningState? ProvisioningState { get; init; }
        public string? ProvisioningError { get; init; } = string.Empty;
        public IEnumerable<ContainerModel> Containers { get; init; } = Enumerable.Empty<ContainerModel>();

        public long UpdateIndex { get; private set; } = 0;
    }

    public static class ObjectExtensions
    {
        public static void UpdateWithChangeCheck<T, K>(this K target, ref bool changed, T? value, Expression<Func<K, T?>> outExpr)
        {
            var expr = (MemberExpression)outExpr.Body;
            var prop = (PropertyInfo)expr.Member;

            var currentValue = (T?)prop.GetValue(target);

            if ((currentValue == null && value != null) || !currentValue!.Equals(value))
            {
                prop.SetValue(target, value, null);
                changed |= true;
            }
        }
    }
}
