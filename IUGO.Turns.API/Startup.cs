using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IUGO.Turns.Services.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace IUGO.Turns.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddCompanyServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }

    public static class ServiceFactory
    {
        public static void AddCompanyServices(this IServiceCollection services)
        {
            services.AddTransient<ITurnService>(provider => CreateCompanyService());
        }

        public static ITurnService CreateCompanyService()
        {
            var uri = "fabric:/IUGOsf/IUGO.Turns.Services";

            return ServiceProxy.Create<ITurnService>(
                new Uri(uri),
                new ServicePartitionKey(0));
        }
    }
}
