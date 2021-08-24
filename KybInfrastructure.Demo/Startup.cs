using KybInfrastructure.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "KybInfrastructure.Demo",
                    Description = "A simple example of using KybInfrastructure",
                    Contact = new OpenApiContact
                    {
                        Name = "Onur Kayabasi",
                        Url = new Uri("https://github.com/onurkybsi"),
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "KybInfrastructure.Demo v1"));
            }
            app.UseServiceHelper();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
