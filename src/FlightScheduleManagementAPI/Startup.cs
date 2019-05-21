using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pitstop.Infrastructure.Messaging;
using Swashbuckle.AspNetCore.Swagger;
using AutoMapper;
using Pitstop.FlightScheduleManagementAPI.Repositories;
using Pitstop.FlightScheduleManagementAPI.Commands;
using Pitstop.FlightScheduleManagementAPI.Events;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Microsoft.Extensions.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Hosting.Server.Features;
using System.Linq;
using FlightScheduleManagementAPI.CommandHandlers;

namespace Pitstop.FlightScheduleManagementAPI
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // add repo classes
            var eventStoreConnectionString = _configuration.GetConnectionString("EventStoreCN");
            services.AddTransient<IFlightScheduleRepository>((sp) => 
                new SqlServerFlightScheduleRepository(eventStoreConnectionString));

            var flightScheduleManagementConnectionString = _configuration.GetConnectionString("FlightScheduleManagementCN");
            services.AddTransient<IFlightRepository>((sp) => new SqlServerFlightDataRepository(flightScheduleManagementConnectionString));

            // add messagepublisher classes
            var configSection = _configuration.GetSection("RabbitMQ");
            string host = configSection["Host"];
            string userName = configSection["UserName"];
            string password = configSection["Password"];
            services.AddTransient<IMessagePublisher>((sp) => new RabbitMQMessagePublisher(host, userName, password, "Pitstop"));

            // add commandhandlers
            services.AddCommandHandlers();

            // Add framework services.
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "FlightScheduleManagement API", Version = "v1" });
            });

            services.AddHealthChecks(checks =>
            {
                checks.WithDefaultCacheDuration(TimeSpan.FromSeconds(1));
                checks.AddSqlCheck("EventStoreCN", _configuration.GetConnectionString("EventStoreCN"));
                checks.AddSqlCheck("FlightScheduleManagementCN", _configuration.GetConnectionString("FlightScheduleManagementCN"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime, IFlightScheduleRepository flightScheduleRepo)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(_configuration)
                .Enrich.WithMachineName()
                .CreateLogger();

            app.UseMvc();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            AutomapperConfigurator.SetupAutoMapper();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FlightScheduleManagement API - v1");
            });

            // initialize database
            flightScheduleRepo.EnsureDatabase();
        }     
    }
}
