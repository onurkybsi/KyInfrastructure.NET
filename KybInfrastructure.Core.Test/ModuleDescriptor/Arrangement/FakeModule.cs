using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace KybInfrastructure.Core.Test
{
    public class FakeModule : ModuleDescriptorBase<FakeModule, FakeModuleContext>
    {
        public FakeModule(FakeModuleContext moduleContext) : base(moduleContext) { }

        public override List<ServiceDescriptor> GetDescriptors()
            => new()
            {
                ServiceDescriptor.Describe(typeof(IFakeService), typeof(FakeService), ServiceLifetime.Transient)
            };
    }
}