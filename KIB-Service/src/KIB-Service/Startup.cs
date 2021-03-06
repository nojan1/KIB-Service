﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using KIB_Service.Repositories;
using KIB_Service.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using KIB_Service.Helpers;
using System.Data.Common;
using MySql.Data.MySqlClient;
using KIB_Service.Filters;
using KIB_Service.TournamentMatchupEngine.Interface;
using KIB_Service.TournamentMatchupEngine;

namespace KIB_Service
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
            this.env = env;
        }

        private IHostingEnvironment env;
        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc()
                    .AddMvcOptions(o => { o.Filters.Add(new GlobalAPIExceptionFilter(env)); });


            services.AddSingleton<ConnectionStringOption>(new ConnectionStringOption { ConnectionString = Configuration.GetConnectionString("DefaultConnection") });
            services.AddScoped<ITournamentRepository, TournamentRepository>();
            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<IRoundRepository, RoundRepository>();
            services.AddScoped<DBHelper>();
            services.AddScoped<IMatchupManager, MatchupManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            });

            app.UseMvc();
            
        }
    }
}
