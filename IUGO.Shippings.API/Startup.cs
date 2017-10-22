using System;
using IUGO.Shippings.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace IUGO.Shippings.API
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
            services.AddShippingService();
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
        public static void AddShippingService(this IServiceCollection services)
        {
            services.AddTransient<IShippingService>(provider => CeateShippingService());
        }

        public static IShippingService CeateShippingService()
        {
            var uri = "fabric:/IUGOsf/IUGO.Shippings.Services";

            return ServiceProxy.Create<IShippingService>(
                new Uri(uri),
                new ServicePartitionKey(0));
        }
    }
}
