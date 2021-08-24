using KybInfrastructure.Core;

namespace KybInfrastructure.Demo.Data
{
    public class ModuleContext : IModuleContext
    {
        public string MongoDbConnectionString { get; set; }
    }
}
