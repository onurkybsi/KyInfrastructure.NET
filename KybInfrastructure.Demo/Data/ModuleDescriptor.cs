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
                ServiceDescriptor.Scoped<IUnitOfWork, UnitOfWork>()
            };
        }
    }
}
