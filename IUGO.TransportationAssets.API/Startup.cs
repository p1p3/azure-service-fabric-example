﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IUGO.TransportationAssets.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace IUGO.TransportationAssets.API
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
            services.AddVehiclesServices();
            services.AddDriversServices();
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
        public static void AddVehiclesServices(this IServiceCollection services)
        {
            services.AddTransient<IVehiclesServices>(provider => CreateVehicleService());
        }

        private const string TransportationAssetsUri = "fabric:/IUGOsf/IUGO.TransportationAssets.Services";
        public static IVehiclesServices CreateVehicleService()
        {
            return ServiceProxy.Create<IVehiclesServices>(
                new Uri(TransportationAssetsUri));
        }


        public static void AddDriversServices(this IServiceCollection services)
        {
            services.AddTransient<IDriverService>(provider => CreateDriverService());
        }

        public static IDriverService CreateDriverService()
        {
            return ServiceProxy.Create<IDriverService>(
                 new Uri(TransportationAssetsUri));
        }
    }
}
