using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using TripPlanner.Services;
using TripPlanner.Data;
using TripPlanner.Models;
using TripPlanner.ViewModels;
using AutoMapper;

namespace TripPlanner
{
    public class Startup
    {
        private IHostingEnvironment _env;
        private IConfigurationRoot _config;

        public Startup(IHostingEnvironment env) 
        {
            _env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(_env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            _config = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_config);

            services.AddDbContext<TripPlannerContext>(options =>
                options.UseSqlite(_config.GetConnectionString("DefaultConnection")));

            services.AddTransient<TripPlannerSeedData>();

            services.AddScoped<ITripPlannerRepository, TripPlannerRepository>();
            
            if (_env.IsEnvironment("Development") || _env.IsEnvironment("Testing"))
            {
                services.AddScoped<IMailService, DebugMailService>();
            }
            else 
            {
                // use actual mail service
                services.AddScoped<IMailService, DebugMailService>();
            }

            services.AddMvc()
                .AddJsonOptions(config => {
                    config.SerializerSettings.ContractResolver 
                        = new CamelCasePropertyNamesContractResolver();
                });

            services.AddLogging();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
            ILoggerFactory loggerFactory, TripPlannerSeedData seeder)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                loggerFactory.AddDebug(LogLevel.Information);
            }
            else 
            {
                loggerFactory.AddDebug(LogLevel.Error);
            }

            // mapping
            Mapper.Initialize(config => 
            {
                config.CreateMap<TripViewModel, Trip>().ReverseMap();
            });

            app.UseStaticFiles();

            app.UseMvc(config => {
                config.MapRoute(
                    name: "Default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" }
                );
            });

            seeder.EnsureSeedData().Wait();
        }
    }
}
