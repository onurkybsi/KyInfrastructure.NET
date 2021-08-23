using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace KybInfrastructure.Demo.Data
{
    public class ModuleDescriptor : Core.ModuleDescriptorBase<ModuleContext>
    {
        public ModuleDescriptor(ModuleContext context) : base(context) { }

        public override List<ServiceDescriptor> GetDescriptors()
        {
            return new List<ServiceDescriptor>
            {
                new ServiceDescriptor(typeof(KybInfrastructureDemoDbContext), (serviceProvider) => new KybInfrastructureDemoDbContext(),
                    ServiceLifetime.Scoped),
                ServiceDescriptor.Scoped<IUnitOfWork, UnitOfWork>()
            };
        }
    }
}
