using KybInfrastructure.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KybInfrastructure.Demo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddModule<Data.ModuleDescriptor, Data.ModuleContext>(new Data.ModuleContext
            {
                MongoDbConnectionString = "mongodb://localhost:27017"
            });
            services.AddModule<Business.ModuleDescriptor, Business.ModuleContext>(new Business.ModuleContext());
            services.AddServiceLocator();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}