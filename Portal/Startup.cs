using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AutoMapper;

using CodingMonkeyNet.SumpPumpMonitor.Data.Entities;
using CodingMonkeyNet.SumpPumpMonitor.Data.Repositories;
using CodingMonkeyNet.SumpPumpMonitor.Portal.Services;
using CodingMonkeyNet.SumpPumpMonitor.IoT.Messages;
using CodingMonkeyNet.SumpPumpMonitor.Data.Configuration;

namespace CodingMonkeyNet.SumpPumpMonitor.Portal
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration["IoTStorageConnectionString"];
            IoTHubConfiguration iotConfig = new IoTHubConfiguration
            {
                HostName = Configuration["IoTHubHostName"],
                SharedAccessKeyName = Configuration["IoTHubSharedAccessKeyName"],
                SharedAccessKey = Configuration["IoTHubSharedAccessKey"]
            };

            // Add framework services.
            services
                .AddMvc();

            services.AddAutoMapper();
            services.AddScoped<ITableRepository<DataPointEntity>>(p => new DataPointRepository(connectionString));
            services.AddScoped<ITableRepository<AlertEntity>>(p => new AlertRepository(connectionString));
            services.AddScoped<IIoTHubSender<SumpPumpSettings>>(p => new SumpPumpService(iotConfig));
            services.AddScoped<ITwinRepository<SumpPumpSettingEntity>>(p => new SettingsRepository(iotConfig));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions {
                    HotModuleReplacement = false // Aurelia Webpack Plugin HMR currently has issues. Leave this set to false.
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
