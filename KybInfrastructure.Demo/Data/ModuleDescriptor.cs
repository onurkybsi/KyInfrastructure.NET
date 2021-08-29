using KybInfrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Collections.Generic;

namespace KybInfrastructure.Demo.Data
{
    public class ModuleDescriptor : Core.ModuleDescriptorBase<ModuleContext>
    {
        public ModuleDescriptor(ModuleContext context) : base(new List<ServiceDescriptor>
            {
                ServiceDescriptor.Singleton<IMongoClient>((serviceProvider) => {
                    MongoClientSettings settings = MongoClientSettings.FromConnectionString(context.MongoDbConnectionString);
                    return new MongoClient(settings);
                }),
                new ServiceDescriptor(typeof(MongoContext), (serviceProvider) => new MongoContext(serviceProvider.GetRequiredService<IMongoClient>().GetDatabase("KybInfrastructureDemoDb")),
                    ServiceLifetime.Scoped),
                ServiceDescriptor.Scoped<IUserRepository, UserRepository>(),
                new ServiceDescriptor(typeof(KybInfrastructureDemoDbContext), (serviceProvider) => new KybInfrastructureDemoDbContext(),
                    ServiceLifetime.Scoped),
                ServiceDescriptor.Scoped<IUnitOfWork, UnitOfWork>()
            }, context)
        { }
    }
}
