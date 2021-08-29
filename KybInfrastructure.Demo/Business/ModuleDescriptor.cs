using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace KybInfrastructure.Demo.Business
{
    public class ModuleDescriptor : Core.ModuleDescriptorBase<ModuleContext>
    {
        public ModuleDescriptor(ModuleContext context) : base(new List<ServiceDescriptor>
            {
                ServiceDescriptor.Scoped<IUserService, UserService>(),
                ServiceDescriptor.Singleton<IProductService, ProductService>()
            }, context)
        { }
    }
}
